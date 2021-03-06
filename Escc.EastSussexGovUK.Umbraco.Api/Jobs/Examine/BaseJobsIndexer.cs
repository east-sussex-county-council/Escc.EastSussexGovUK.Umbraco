﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Linq;
using Escc.Dates;
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
using System.Text;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using Escc.EastSussexGovUK.Umbraco.Examine;
using Escc.EastSussexGovUK.Umbraco.Api.Jobs.JobTransformers;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.Examine
{
    /// <summary>
    /// Examine indexer for indexing jobs 
    /// </summary>
    public abstract class BaseJobsIndexer : ISimpleDataService
    {
        /// <summary>
        /// Gets or sets the method of getting job information to be indexed
        /// </summary>
        public virtual IJobsDataProvider JobsProvider { get; set; }

        /// <summary>
        ///  Gets or sets the method of removing stop words during indexing
        /// </summary>
        public virtual ISearchFilter StopWordsRemover { get; set; }

        /// <summary>
        ///  Gets or sets the method of removing HTML during indexing
        /// </summary>
        public virtual IHtmlTagSanitiser TagSanitiser { get; set; }

        /// <summary>
        /// Gets or sets transforms to apply to specific jobs during indexing
        /// </summary>
        public virtual Dictionary<IEnumerable<IJobMatcher>, IEnumerable<IJobTransformer>> JobTransformers { get; set; }

        /// <summary>
        /// Gets the index provider.
        /// </summary>
        public abstract BaseIndexProvider IndexProvider { get; }

        public IEnumerable<SimpleDataSet> GetAllData(string indexType)
        {
            // Because this calls Task<T>.Result to get the result of an async method, every await call from here down
            // has to be suffixed with .ConfigureAwait(false) to avoid deadlocks
            // https://stackoverflow.com/questions/10343632/httpclient-getasync-never-returns-when-using-await-async
            return GetAllDataAsync(indexType).Result;
        }

        public async Task<IEnumerable<SimpleDataSet>> GetAllDataAsync(string indexType)
        {
            if (JobsProvider == null) throw new InvalidOperationException("You must set the JobsProvider property before using this instance");

            var dataSets = new List<SimpleDataSet>();

            try
            {
                // Unforunately by this point the index has already been wiped, which means in the _jobsProvider has a problem
                // we can't just continue with the existing data.  If we try to check the data source in the constructor, 
                // the only way to prevent execution proceeding to wiping the index is to throw an exception, and in a cold-boot
                // scenario where indexes need to be rebuilt that crashes all Umbraco pages.
                var jobs = await JobsProvider.ReadJobs(new JobSearchQuery()).ConfigureAwait(false);

                //Looping all the raw models and adding them to the dataset
                foreach (var job in jobs.Jobs)
                {
                    try
                    {
                        var jobAdvert = await JobsProvider.ReadJob(job.Id.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
                        jobAdvert.Id = job.Id;
                        if (String.IsNullOrEmpty(jobAdvert.JobTitle)) jobAdvert.JobTitle = job.JobTitle;
                        if (jobAdvert.Locations == null || jobAdvert.Locations.Count == 0) jobAdvert.Locations = job.Locations;

                        jobAdvert.Salary.SearchRange = job.Salary.SearchRange;
                        if (String.IsNullOrEmpty(jobAdvert.Salary.SalaryRange)) jobAdvert.Salary.SalaryRange = job.Salary.SearchRange;
                        if (!jobAdvert.Salary.MinimumSalary.HasValue) jobAdvert.Salary.MinimumSalary = job.Salary.MinimumSalary;
                        if (!jobAdvert.Salary.MaximumSalary.HasValue) jobAdvert.Salary.MaximumSalary = job.Salary.MaximumSalary;

                        if (!jobAdvert.ClosingDate.HasValue) jobAdvert.ClosingDate = job.ClosingDate;

                        if (JobTransformers != null)
                        {
                            foreach (var matchers in JobTransformers.Keys)
                            {
                                var match = true;
                                foreach (var matcher in matchers)
                                {
                                    match = match && matcher.IsMatch(jobAdvert);
                                }

                                if (match)
                                {
                                    foreach (var transformer in JobTransformers[matchers])
                                    {
                                        transformer.TransformJob(jobAdvert);
                                    }
                                }
                            }
                        }

                        var simpleDataSet = CreateIndexItemFromJob(jobAdvert, indexType);
                        if (simpleDataSet != null)
                        {
                            dataSets.Add(simpleDataSet);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Catch the error here to ensure the indexer can carry on with indexing the next job
                        ex.ToExceptionless().Submit();
                        LogHelper.Error<BaseJobsIndexer>("Error indexing:", ex);
                    }
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
        public async Task UpdateIndex(Dictionary<int,DateTime?> currentIndexedJobs)
        {
            var jobsToDelete = new List<int>(currentIndexedJobs.Keys);

            // Delete and recreate any job still in the index, or add it if it's new
            var jobsData = await GetAllDataAsync("Job").ConfigureAwait(false);
            foreach (var job in jobsData)
            {
                try
                {
                    // If it's an existing job, preserve the date it was published
                    if (currentIndexedJobs.ContainsKey(job.NodeDefinition.NodeId) && currentIndexedJobs[job.NodeDefinition.NodeId] != null)
                    {
                        job.RowData["datePublished"] = ((DateTime)currentIndexedJobs[job.NodeDefinition.NodeId]).ToIso8601DateTime();
                    }

                    IndexProvider.DeleteFromIndex(job.NodeDefinition.NodeId.ToString(CultureInfo.InvariantCulture));
                    IndexProvider.ReIndexNode(job.RowData.ToExamineXml(job.NodeDefinition.NodeId, "Job"), "Job");
                    jobsToDelete.Remove(job.NodeDefinition.NodeId);
                }
                catch (Exception ex)
                {
                    ex.ToExceptionless().Submit();
                    LogHelper.Info<BaseJobsIndexer>(ex.Message + $" reindexing job {job.NodeDefinition.NodeId}");
                }
            }

            // Any jobs not found in the new data can be deleted
            foreach (var jobId in jobsToDelete)
            {
                try
                { 
                    LogHelper.Info<BaseJobsIndexer>($"Deleting job '{jobId}' from Examine index");
                    IndexProvider.DeleteFromIndex(jobId.ToString(CultureInfo.InvariantCulture));
                }
                catch (Exception ex)
                {
                    ex.ToExceptionless().Submit();
                    LogHelper.Info<BaseJobsIndexer>(ex.Message + $" deleting job {jobId}");
                }
            }
        }

        private SimpleDataSet CreateIndexItemFromJob(Job job, string indexType)
        {
            if (job.DatePublished > DateTime.UtcNow)
            {
                LogHelper.Info<BaseJobsIndexer>($"Ignoring job '{job.Id}' because it's publish date {job.DatePublished.ToIso8601DateTime()} is in the future.");
                return null;
            }

            LogHelper.Info<BaseJobsIndexer>($"Building Examine index item for job '{job.Id}'");

            var salary = job.Salary.SalaryRange;
            var salaryWithStopWords = salary;
            if (!String.IsNullOrEmpty(salary))
            {
                if (TagSanitiser != null)
                {
                    salary = TagSanitiser.StripTags(salary);
                }
                if (StopWordsRemover != null)
                {
                    salary = StopWordsRemover.Filter(salary);
                }
            }
            var simpleDataSet = new SimpleDataSet { NodeDefinition = new IndexedNode(), RowData = new Dictionary<string, string>() };

            simpleDataSet.NodeDefinition.NodeId = job.Id;
            simpleDataSet.NodeDefinition.Type = indexType;
            simpleDataSet.RowData.Add("id", job.Id.ToString(CultureInfo.InvariantCulture));
            simpleDataSet.RowData.Add("reference", job.Reference);
            simpleDataSet.RowData.Add("numberOfPositions", job.NumberOfPositions?.ToString(CultureInfo.CurrentCulture));
            simpleDataSet.RowData.Add("title", StopWordsRemover != null ? StopWordsRemover.Filter(job.JobTitle) : job.JobTitle);
            simpleDataSet.RowData.Add("titleDisplay", job.JobTitle);
            simpleDataSet.RowData.Add("logoUrl", job.LogoUrl?.ToString());
            simpleDataSet.RowData.Add("organisation", StopWordsRemover != null ? StopWordsRemover.Filter(job.Organisation) : job.Organisation);
            simpleDataSet.RowData.Add("organisationDisplay", job.Organisation);
            simpleDataSet.RowData.Add("salary", salary);
            simpleDataSet.RowData.Add("salaryDisplay", salaryWithStopWords); // so that it's not displayed with stop words removed
            simpleDataSet.RowData.Add("salaryRange", StopWordsRemover != null ? StopWordsRemover.Filter(job.Salary.SearchRange) : job.Salary.SearchRange);
            simpleDataSet.RowData.Add("salaryMin", job.Salary.MinimumSalary?.ToString("0000000.00").Replace(".", string.Empty) ?? String.Empty);
            simpleDataSet.RowData.Add("salaryMax", job.Salary.MaximumSalary?.ToString("0000000.00").Replace(".", string.Empty) ?? String.Empty);
            simpleDataSet.RowData.Add("salarySort", (job.Salary.MinimumSalary?.ToString("0000000.00").Replace(".", string.Empty) ?? String.Empty) + " " + (job.Salary.MaximumSalary?.ToString("0000000.00").Replace(".", string.Empty) ?? String.Empty) + " " + (StopWordsRemover != null ? StopWordsRemover.Filter(job.Salary.SalaryRange) : job.Salary.SalaryRange));
            simpleDataSet.RowData.Add("hourlyRate", job.Salary.MinimumHourlyRate?.ToString(CultureInfo.CurrentCulture));
            simpleDataSet.RowData.Add("hoursPerWeek", job.WorkPattern.HoursPerWeek?.ToString(CultureInfo.CurrentCulture));
            simpleDataSet.RowData.Add("jobType", StopWordsRemover != null ? StopWordsRemover.Filter(job.JobType) : job.JobType);
            simpleDataSet.RowData.Add("jobTypeDisplay", job.JobType);
            simpleDataSet.RowData.Add("contractType", StopWordsRemover != null ? StopWordsRemover.Filter(job.ContractType) : job.ContractType);
            simpleDataSet.RowData.Add("department", StopWordsRemover != null ? StopWordsRemover.Filter(job.Department) : job.Department);
            simpleDataSet.RowData.Add("departmentDisplay", job.Department);
            simpleDataSet.RowData.Add("datePublished", job.DatePublished.ToIso8601DateTime());

            if (job.ClosingDate.HasValue)
            {
                simpleDataSet.RowData.Add("closingDate", job.ClosingDate.Value.ToIso8601DateTime());
                simpleDataSet.RowData.Add("closingDateDisplay", job.ClosingDate.Value.ToIso8601DateTime());
            }
            else
            {
                // Examine queries are simpler if there is always a closing date, so set a far-future closing date to represent never,
                // and don't have a version of the closing date for display.
                simpleDataSet.RowData.Add("closingDate", DateTime.MaxValue.ToIso8601DateTime());
            }

            var workPatternList = string.Join(", ", job.WorkPattern.WorkPatterns.ToArray<string>());
            simpleDataSet.RowData.Add("workPattern", workPatternList);

            var locationsList = string.Join(", ", job.Locations.ToArray<string>());
            simpleDataSet.RowData.Add("location", StopWordsRemover != null ? StopWordsRemover.Filter(locationsList) : locationsList);
            simpleDataSet.RowData.Add("locationDisplay", locationsList); // because Somewhere-on-Sea needs to lose the "on" for searching but keep it for display


            if (job.AdvertHtml != null)
            {
                var fullText = job.AdvertHtml.ToHtmlString();
                if (TagSanitiser != null) fullText = TagSanitiser.StripTags(fullText);

                // Append other fields as keywords, otherwise a search term that's a good match will not be found if it has terms from two fields,
                // eg Job Title (full time)
                const string space = " ";
                fullText = new StringBuilder(fullText)
                                .Append(space).Append(job.Reference)
                                .Append(space).Append(job.JobTitle)
                                .Append(space).Append(job.Organisation)
                                .Append(space).Append(job.Locations)
                                .Append(space).Append(job.JobType)
                                .Append(space).Append(job.ContractType)
                                .Append(space).Append(job.Department)
                                .Append(space).Append(job.WorkPattern.ToString())
                                .ToString();

                simpleDataSet.RowData.Add("fullText", fullText);
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