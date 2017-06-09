using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using Escc.Dates;
using Escc.EastSussexGovUK.Umbraco.Examine;
using Escc.Html;
using Examine;
using Examine.LuceneEngine;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Web;
using Umbraco.Web.Security;
using Exceptionless;
using System.Globalization;
using Examine.Providers;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Examine
{
    /// <summary>
    /// Examine indexer for indexing jobs 
    /// </summary>
    public abstract class BaseJobsIndexer : ISimpleDataService
    {
        private readonly IJobsDataProvider _jobsProvider;
        private readonly ISearchFilter _stopWordsRemover;
        private readonly IHtmlTagSanitiser _tagSanitiser;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseJobsIndexer" /> class.
        /// </summary>
        /// <param name="jobsProvider">The jobs provider.</param>
        /// <param name="stopWordsRemover">The stop words remover.</param>
        /// <param name="tagSanitiser">The tag sanitiser.</param>
        /// <exception cref="System.ArgumentNullException">stopWordsRemover</exception>
        protected BaseJobsIndexer(IJobsDataProvider jobsProvider, ISearchFilter stopWordsRemover, IHtmlTagSanitiser tagSanitiser)
        {
            if (jobsProvider == null) throw new ArgumentNullException(nameof(jobsProvider));
            _jobsProvider = jobsProvider;
            _stopWordsRemover = stopWordsRemover;
            _tagSanitiser = tagSanitiser;
        }

        /// <summary>
        /// Gets the index provider.
        /// </summary>
        public abstract BaseIndexProvider IndexProvider { get; }

        public IEnumerable<SimpleDataSet> GetAllData(string indexType)
        {
            var dataSets = new List<SimpleDataSet>();

            try
            {
                // Unforunately by this point the index has already been wiped, which means in the _jobsProvider has a problem
                // we can't just continue with the existing data.  If we try to check the data source in the constructor, 
                // the only way to prevent execution proceeding to wiping the index is to throw an exception, and in a cold-boot
                // scenario where indexes need to be rebuilt that crashes all Umbraco pages.
                var jobs = Task.Run(async () => await _jobsProvider.ReadJobs(new JobSearchQuery())).Result;

                //Looping all the raw models and adding them to the dataset
                foreach (var job in jobs)
                {
                    var jobAdvert = Task.Run(async () => await _jobsProvider.ReadJob(job.Id.ToString(CultureInfo.InvariantCulture))).Result;
                    jobAdvert.Id = job.Id;
                    if (String.IsNullOrEmpty(jobAdvert.JobTitle)) jobAdvert.JobTitle = job.JobTitle;
                    if (String.IsNullOrEmpty(jobAdvert.Location)) jobAdvert.Location = job.Location;

                    jobAdvert.Salary.SearchRange = job.Salary.SearchRange;
                    if (String.IsNullOrEmpty(jobAdvert.Salary.SalaryRange)) jobAdvert.Salary.SalaryRange = job.Salary.SearchRange;
                    if (!jobAdvert.Salary.MinimumSalary.HasValue) jobAdvert.Salary.MinimumSalary = job.Salary.MinimumSalary;
                    if (!jobAdvert.Salary.MaximumSalary.HasValue) jobAdvert.Salary.MaximumSalary = job.Salary.MaximumSalary;

                    if (!jobAdvert.ClosingDate.HasValue) jobAdvert.ClosingDate = job.ClosingDate;

                    var simpleDataSet = CreateIndexItemFromJob(jobAdvert, indexType);
                    dataSets.Add(simpleDataSet);
                }
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                LogHelper.Error<BaseJobsIndexer>("Error indexing:", ex);
            }

            LogHelper.Info<BaseJobsIndexer>($"{dataSets.Count} items built for indexing by {this.GetType().ToString()}");

            return dataSets;
        }

        /// <summary>
        /// Updates the index one job at a time, rather than wiping the index first.
        /// </summary>
        /// <param name="indexer">The indexer.</param>
        /// <param name="currentIndexedJobs">The job ids and publication dates in the current version of the index (for comparison with the new data).</param>
        public void UpdateIndex(Dictionary<int,DateTime?> currentIndexedJobs)
        {
            var jobsToDelete = new List<int>(currentIndexedJobs.Keys);

            // Delete and recreate any job still in the index, or add it if it's new
            var jobsData = GetAllData("Job");
            foreach (var job in jobsData)
            {
                // If it's an existing job, preserve the date it was published
                if (currentIndexedJobs.ContainsKey(job.NodeDefinition.NodeId) && currentIndexedJobs[job.NodeDefinition.NodeId] != null)
                {
                    job.RowData["datePublished"] = ((DateTime)currentIndexedJobs[job.NodeDefinition.NodeId]).ToIso8601DateTime();
                }

                IndexProvider.DeleteFromIndex(job.NodeDefinition.NodeId.ToString());
                IndexProvider.ReIndexNode(job.RowData.ToExamineXml(job.NodeDefinition.NodeId, "Job"), "Job");
                jobsToDelete.Remove(job.NodeDefinition.NodeId);
            }

            // Any jobs not found in the new data can be deleted
            foreach (var jobId in jobsToDelete)
            {
                LogHelper.Info<BaseJobsIndexer>($"Deleting job '{jobId}' from Examine index");
                IndexProvider.DeleteFromIndex(jobId.ToString());
            }
        }

        private SimpleDataSet CreateIndexItemFromJob(Job job, string indexType)
        {
            LogHelper.Info<BaseJobsIndexer>($"Building Examine index item for job '{job.Id}'");

            var simpleDataSet = new SimpleDataSet { NodeDefinition = new IndexedNode(), RowData = new Dictionary<string, string>() };

            simpleDataSet.NodeDefinition.NodeId = job.Id;
            simpleDataSet.NodeDefinition.Type = indexType;
            simpleDataSet.RowData.Add("id", job.Id.ToString(CultureInfo.InvariantCulture));
            simpleDataSet.RowData.Add("reference", job.Reference);
            simpleDataSet.RowData.Add("title", _stopWordsRemover != null ? _stopWordsRemover.Filter(job.JobTitle) : job.JobTitle);
            simpleDataSet.RowData.Add("titleDisplay", job.JobTitle);
            simpleDataSet.RowData.Add("organisation", _stopWordsRemover != null ? _stopWordsRemover.Filter(job.Organisation) : job.Organisation);
            simpleDataSet.RowData.Add("organisationDisplay", job.Organisation);
            simpleDataSet.RowData.Add("location", _stopWordsRemover != null ? _stopWordsRemover.Filter(job.Location) : job.Location);
            simpleDataSet.RowData.Add("locationDisplay", job.Location); // because Somewhere-on-Sea needs to lose the "on" for searching but keep it for display
            simpleDataSet.RowData.Add("salary", _tagSanitiser != null ? _tagSanitiser.StripTags(_stopWordsRemover != null ? _stopWordsRemover.Filter(job.Salary.SalaryRange) : job.Salary.SalaryRange) : _stopWordsRemover != null ? _stopWordsRemover.Filter(job.Salary.SalaryRange) : job.Salary.SalaryRange);
            simpleDataSet.RowData.Add("salaryRange", _stopWordsRemover != null ? _stopWordsRemover.Filter(job.Salary.SearchRange) : job.Salary.SearchRange);
            simpleDataSet.RowData.Add("salaryMin", job.Salary.MinimumSalary?.ToString("D7") ?? String.Empty);
            simpleDataSet.RowData.Add("salaryMax", job.Salary.MaximumSalary?.ToString("D7") ?? String.Empty);
            simpleDataSet.RowData.Add("salarySort", (job.Salary.MinimumSalary?.ToString("D7") ?? String.Empty) + " " + (job.Salary.MaximumSalary?.ToString("D7") ?? String.Empty) + " " + (_stopWordsRemover != null ? _stopWordsRemover.Filter(job.Salary.SalaryRange) : job.Salary.SalaryRange));
            simpleDataSet.RowData.Add("closingDate", job.ClosingDate.Value.ToIso8601DateTime());
            simpleDataSet.RowData.Add("closingDateDisplay", job.ClosingDate.Value.ToIso8601DateTime());
            simpleDataSet.RowData.Add("jobType", _stopWordsRemover != null ? _stopWordsRemover.Filter(job.JobType) : job.JobType);
            simpleDataSet.RowData.Add("jobTypeDisplay", job.JobType);
            simpleDataSet.RowData.Add("contractType", _stopWordsRemover != null ? _stopWordsRemover.Filter(job.ContractType) : job.ContractType);
            simpleDataSet.RowData.Add("department", _stopWordsRemover != null ? _stopWordsRemover.Filter(job.Department) : job.Department);
            simpleDataSet.RowData.Add("departmentDisplay", job.Department);
            simpleDataSet.RowData.Add("fullTime", job.WorkPattern.IsFullTime.ToString());
            simpleDataSet.RowData.Add("partTime", job.WorkPattern.IsPartTime.ToString());
            simpleDataSet.RowData.Add("workPattern", job.WorkPattern.ToString());
            simpleDataSet.RowData.Add("datePublished", DateTime.UtcNow.ToIso8601DateTime());
            if (job.AdvertHtml != null)
            {
                simpleDataSet.RowData.Add("fullText", _tagSanitiser != null ? _tagSanitiser.StripTags(job.AdvertHtml.ToHtmlString()) : job.AdvertHtml.ToHtmlString());
                simpleDataSet.RowData.Add("fullHtml", job.AdvertHtml.ToHtmlString());
            }
            if (job.AdditionalInformationHtml != null)
            {
                simpleDataSet.RowData.Add("additionalInfo", job.AdditionalInformationHtml.ToHtmlString());
            }
            if (job.EqualOpportunitiesHtml != null)
            {
                simpleDataSet.RowData.Add("equalOpportunities", job.EqualOpportunitiesHtml.ToHtmlString());
            }
            simpleDataSet.RowData.Add("applyUrl", job.ApplyUrl?.ToString());

            return simpleDataSet;
        }
    }
}