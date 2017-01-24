using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using Escc.Dates;
using Escc.EastSussexGovUK.Umbraco.Examine;
using Escc.Html;
using Escc.Net;
using Examine;
using Examine.LuceneEngine;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Web;
using Umbraco.Web.Security;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Examine indexer for indexing jobs 
    /// </summary>
    public abstract class BaseJobsIndexer : ISimpleDataService
    {
        private readonly IJobsDataProvider _jobsProvider;
        private readonly IStopWordsRemover _stopWordsRemover;
        
        // Lucene default stopwords according to http://stackoverflow.com/questions/17527741/what-is-the-default-list-of-stopwords-used-in-lucenes-stopfilter#17531638
        private static readonly string[] StopWords = {"a", "an", "and", "are", "as", "at", "be", "but", "by",
                                    "for", "if", "in", "into", "is", "it",
                                    "no", "not", "of", "on", "or", "such",
                                    "that", "the", "their", "then", "there", "these",
                                    "they", "this", "to", "was", "will", "with"};

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseJobsIndexer" /> class.
        /// </summary>
        /// <param name="jobsProvider">The jobs provider.</param>
        /// <param name="stopWordsRemover">The stop words remover.</param>
        /// <exception cref="System.ArgumentNullException">stopWordsRemover</exception>
        protected BaseJobsIndexer(IJobsDataProvider jobsProvider, IStopWordsRemover stopWordsRemover)
        {
            if (jobsProvider == null) throw new ArgumentNullException(nameof(jobsProvider));
            if (stopWordsRemover == null) throw new ArgumentNullException(nameof(stopWordsRemover));
            _jobsProvider = jobsProvider;
            _stopWordsRemover = stopWordsRemover;
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
            simpleDataSet.RowData.Add("title", _stopWordsRemover.RemoveStopWords(job.JobTitle, StopWords));
            simpleDataSet.RowData.Add("organisation", _stopWordsRemover.RemoveStopWords(job.Organisation, StopWords));
            simpleDataSet.RowData.Add("location", _stopWordsRemover.RemoveStopWords(job.Location, StopWords));
            simpleDataSet.RowData.Add("salary", _stopWordsRemover.RemoveStopWords(job.Salary.SalaryRange, StopWords));
            simpleDataSet.RowData.Add("salaryRange", _stopWordsRemover.RemoveStopWords(job.Salary.SearchRange, StopWords));
            simpleDataSet.RowData.Add("salaryMin", job.Salary.MinimumSalary?.ToString("D7") ?? String.Empty);
            simpleDataSet.RowData.Add("salaryMax", job.Salary.MaximumSalary?.ToString("D7") ?? String.Empty);
            simpleDataSet.RowData.Add("salarySort", (job.Salary.MinimumSalary?.ToString("D7") ?? String.Empty) + " " + (job.Salary.MaximumSalary?.ToString("D7") ?? String.Empty) + " " + _stopWordsRemover.RemoveStopWords(job.Salary.SalaryRange, StopWords));
            simpleDataSet.RowData.Add("closingDate", job.ClosingDate.Value.ToIso8601DateTime());
            simpleDataSet.RowData.Add("jobType", _stopWordsRemover.RemoveStopWords(job.JobType, StopWords));
            simpleDataSet.RowData.Add("contractType", _stopWordsRemover.RemoveStopWords(job.ContractType, StopWords));
            simpleDataSet.RowData.Add("department", _stopWordsRemover.RemoveStopWords(job.Department, StopWords));
            simpleDataSet.RowData.Add("workPattern", _stopWordsRemover.RemoveStopWords(job.WorkPattern, StopWords));
            if (job.AdvertHtml != null)
            {
                simpleDataSet.RowData.Add("fullText", new HtmlTagSantiser().StripTags(job.AdvertHtml.ToHtmlString()));
            }

            return simpleDataSet;
        }
    }
}