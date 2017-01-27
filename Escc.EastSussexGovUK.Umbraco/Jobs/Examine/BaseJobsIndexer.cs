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

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Examine
{
    /// <summary>
    /// Examine indexer for indexing jobs 
    /// </summary>
    public abstract class BaseJobsIndexer : ISimpleDataService
    {
        private readonly IJobsDataProvider _jobsProvider;
        private readonly IStopWordsRemover _stopWordsRemover;
        private readonly IHtmlTagSanitiser _tagSanitiser;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseJobsIndexer" /> class.
        /// </summary>
        /// <param name="jobsProvider">The jobs provider.</param>
        /// <param name="stopWordsRemover">The stop words remover.</param>
        /// <param name="tagSanitiser">The tag sanitiser.</param>
        /// <exception cref="System.ArgumentNullException">stopWordsRemover</exception>
        protected BaseJobsIndexer(IJobsDataProvider jobsProvider, IStopWordsRemover stopWordsRemover, IHtmlTagSanitiser tagSanitiser)
        {
            if (jobsProvider == null) throw new ArgumentNullException(nameof(jobsProvider));
            if (stopWordsRemover == null) throw new ArgumentNullException(nameof(stopWordsRemover));
            if (tagSanitiser == null) throw new ArgumentNullException(nameof(tagSanitiser));
            _jobsProvider = jobsProvider;
            _stopWordsRemover = stopWordsRemover;
            _tagSanitiser = tagSanitiser;
        }

        public IEnumerable<SimpleDataSet> GetAllData(string indexType)
        {
            //Ensure that an Umbraco context is available
            if (UmbracoContext.Current == null)
            {
                var dummyContext =
                    new HttpContextWrapper(
                        new HttpContext(new SimpleWorkerRequest("/", string.Empty, new StringWriter())));
                UmbracoContext.EnsureContext(
                    dummyContext,
                    ApplicationContext.Current,
                    new WebSecurity(dummyContext, ApplicationContext.Current),
                    false);
            }

            var dataSets = new List<SimpleDataSet>();

            try

            {

                var jobs = Task.Run(async () => await _jobsProvider.ReadJobs(new JobSearchQuery())).Result;

                //Looping all the raw models and adding them to the dataset
                var i = 1;
                foreach (var job in jobs)
                {
                    var jobAdvert = Task.Run(async () => await _jobsProvider.ReadJob(job.Id)).Result;
                    jobAdvert.Id = job.Id;
                    if (String.IsNullOrEmpty(jobAdvert.JobTitle)) jobAdvert.JobTitle = job.JobTitle;
                    if (String.IsNullOrEmpty(jobAdvert.Location)) jobAdvert.Location = job.Location;

                    jobAdvert.Salary.SearchRange = job.Salary.SearchRange;
                    if (String.IsNullOrEmpty(jobAdvert.Salary.SalaryRange)) jobAdvert.Salary.SalaryRange = job.Salary.SearchRange;
                    if (!jobAdvert.Salary.MinimumSalary.HasValue) jobAdvert.Salary.MinimumSalary = job.Salary.MinimumSalary;
                    if (!jobAdvert.Salary.MaximumSalary.HasValue) jobAdvert.Salary.MaximumSalary = job.Salary.MaximumSalary;

                    if (!jobAdvert.ClosingDate.HasValue) jobAdvert.ClosingDate = job.ClosingDate;

                    var simpleDataSet = CreateIndexItemFromJob(i, jobAdvert, indexType);
                    dataSets.Add(simpleDataSet);
                    i++;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error<BaseJobsIndexer>("error indexing:", ex);
            }

            return dataSets;
        }

        private SimpleDataSet CreateIndexItemFromJob(int fakeNodeId, Job job, string indexType)
        {
            var simpleDataSet = new SimpleDataSet { NodeDefinition = new IndexedNode(), RowData = new Dictionary<string, string>() };

            simpleDataSet.NodeDefinition.NodeId = fakeNodeId;
            simpleDataSet.NodeDefinition.Type = indexType;
            simpleDataSet.RowData.Add("id", job.Id);
            simpleDataSet.RowData.Add("reference", job.Reference);
            simpleDataSet.RowData.Add("title", _stopWordsRemover.RemoveStopWords(job.JobTitle));
            simpleDataSet.RowData.Add("organisation", _stopWordsRemover.RemoveStopWords(job.Organisation));
            simpleDataSet.RowData.Add("location", _stopWordsRemover.RemoveStopWords(job.Location));
            simpleDataSet.RowData.Add("locationDisplay", job.Location); // because Somewhere-on-Sea needs to lose the "on" for searching but keep it for display
            simpleDataSet.RowData.Add("salary", _tagSanitiser.StripTags(_stopWordsRemover.RemoveStopWords(job.Salary.SalaryRange)));
            simpleDataSet.RowData.Add("salaryRange", _stopWordsRemover.RemoveStopWords(job.Salary.SearchRange));
            simpleDataSet.RowData.Add("salaryMin", job.Salary.MinimumSalary?.ToString("D7") ?? String.Empty);
            simpleDataSet.RowData.Add("salaryMax", job.Salary.MaximumSalary?.ToString("D7") ?? String.Empty);
            simpleDataSet.RowData.Add("salarySort", (job.Salary.MinimumSalary?.ToString("D7") ?? String.Empty) + " " + (job.Salary.MaximumSalary?.ToString("D7") ?? String.Empty) + " " + _stopWordsRemover.RemoveStopWords(job.Salary.SalaryRange));
            simpleDataSet.RowData.Add("closingDate", job.ClosingDate.Value.ToIso8601DateTime());
            simpleDataSet.RowData.Add("jobType", _stopWordsRemover.RemoveStopWords(job.JobType));
            simpleDataSet.RowData.Add("jobTypeDisplay", job.JobType);
            simpleDataSet.RowData.Add("contractType", _stopWordsRemover.RemoveStopWords(job.ContractType));
            simpleDataSet.RowData.Add("department", _stopWordsRemover.RemoveStopWords(job.Department));
            simpleDataSet.RowData.Add("fullTime", job.WorkPattern.IsFullTime.ToString());
            simpleDataSet.RowData.Add("partTime", job.WorkPattern.IsPartTime.ToString());
            simpleDataSet.RowData.Add("workPattern", job.WorkPattern.ToString());
            if (job.AdvertHtml != null)
            {
                simpleDataSet.RowData.Add("fullText", _tagSanitiser.StripTags(job.AdvertHtml.ToHtmlString()));
                simpleDataSet.RowData.Add("fullHtml", job.AdvertHtml.ToHtmlString());
            }
            simpleDataSet.RowData.Add("applyUrl", job.ApplyUrl?.ToString());

            return simpleDataSet;
        }
    }
}