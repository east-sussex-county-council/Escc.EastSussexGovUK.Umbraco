using Escc.EastSussexGovUK.Umbraco.Api.Jobs.Examine;
using Escc.EastSussexGovUK.Umbraco.Examine;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using Escc.EastSussexGovUK.Umbraco.Jobs.Alerts;
using Examine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Umbraco.Web.WebApi;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.Api
{
    /// <summary>
    /// Base class for web APIs targeting an individual <see cref="JobsSet"/>
    /// </summary>
    /// <seealso cref="Umbraco.Web.WebApi.UmbracoApiController" />
    public abstract class BaseJobsApiController : UmbracoApiController
    {
        /// <summary>
        /// Reads jobs matching the specified search query
        /// </summary>
        /// <param name="jobsSet">The jobs set.</param>
        /// <param name="query">The query.</param>
        /// <param name="baseUrl">The base URL.</param>
        /// <returns></returns>
        protected JobSearchResult Jobs(JobsSet jobsSet, JobSearchQuery query, Uri baseUrl)
        {
            var jobsProvider = new JobsDataFromExamine(ExamineManager.Instance.SearchProviderCollection[jobsSet + "Searcher"], 
                new QueryBuilder(new LuceneTokenisedQueryBuilder(), new KeywordsTokeniser(), new LuceneStopWordsRemover(), new WildcardSuffixFilter()), 
                new SalaryRangeLuceneQueryBuilder(), 
                new RelativeJobUrlGenerator(baseUrl));

            var jobs = Task.Run(async () => await jobsProvider.ReadJobs(query)).Result;
            foreach (var job in jobs.Jobs)
            {
                // Remove these unnecessary and large fields to significantly reduce the amount that's serialised and sent to the client
                job.AdvertHtml = null;
                job.AdditionalInformationHtml = null;
                job.EqualOpportunitiesHtml = null;
            }
            return jobs;
        }

        /// <summary>
        /// Reads jobs with incomplete data in the specified jobs set
        /// </summary>
        /// <param name="jobsSet">The jobs set.</param>
        /// <param name="baseUrl">The base URL.</param>
        /// <returns></returns>
        protected async Task<IEnumerable<Job>> ProblemJobs(JobsSet jobsSet, Uri baseUrl)
        {
            var jobsProvider = new JobsDataFromExamine(ExamineManager.Instance.SearchProviderCollection[jobsSet + "Searcher"], 
                new QueryBuilder(new LuceneTokenisedQueryBuilder(), new KeywordsTokeniser(), new LuceneStopWordsRemover(), new WildcardSuffixFilter()), 
                null, 
                new RelativeJobUrlGenerator(baseUrl));
            return await jobsProvider.ReadProblemJobs();
        }

        /// <summary>
        /// Reads full details of an individual job from the specified jobs set.
        /// </summary>
        /// <param name="jobsSet">The jobs set.</param>
        /// <param name="jobId">The job identifier.</param>
        /// <param name="baseUrl">The base URL.</param>
        /// <returns></returns>
        protected async Task<Job> Job(JobsSet jobsSet, string jobId, Uri baseUrl)
        {
            var jobsProvider = new JobsDataFromExamine(ExamineManager.Instance.SearchProviderCollection[jobsSet + "Searcher"],
                    new QueryBuilder(new LuceneTokenisedQueryBuilder(), new KeywordsTokeniser(), new LuceneStopWordsRemover(), new WildcardSuffixFilter()),
                    null,
                    new RelativeJobUrlGenerator(baseUrl));
            return await jobsProvider.ReadJob(jobId);
        }

        /// <summary>
        /// Reads the locations.
        /// </summary>
        /// <param name="jobsSet">The jobs set.</param>
        /// <returns></returns>
        protected async Task<IList<JobsLookupValue>> ReadLocations(JobsSet jobsSet)
        {
            var dataSource = new JobsLookupValuesFromExamine(ExamineManager.Instance.SearchProviderCollection[jobsSet + "LookupValuesSearcher"]);
            return await dataSource.ReadLocations();
        }

        /// <summary>
        /// Reads the job types.
        /// </summary>
        /// <param name="jobsSet">The jobs set.</param>
        /// <returns></returns>
        protected async Task<IList<JobsLookupValue>> ReadJobTypes(JobsSet jobsSet)
        {
            var dataSource = new JobsLookupValuesFromExamine(ExamineManager.Instance.SearchProviderCollection[jobsSet + "LookupValuesSearcher"]);
            return await dataSource.ReadJobTypes();
        }

        /// <summary>
        /// Reads the organisations.
        /// </summary>
        /// <param name="jobsSet">The jobs set.</param>
        /// <returns></returns>
        protected async Task<IList<JobsLookupValue>> ReadOrganisations(JobsSet jobsSet)
        {
            var dataSource = new JobsLookupValuesFromExamine(ExamineManager.Instance.SearchProviderCollection[jobsSet + "LookupValuesSearcher"]);
            return await dataSource.ReadOrganisations();
        }

        /// <summary>
        /// Reads the salary ranges.
        /// </summary>
        /// <param name="jobsSet">The jobs set.</param>
        /// <returns></returns>
        protected async Task<IList<JobsLookupValue>> ReadSalaryRanges(JobsSet jobsSet)
        {
            var dataSource = new JobsLookupValuesFromExamine(ExamineManager.Instance.SearchProviderCollection[jobsSet + "LookupValuesSearcher"]);
            return await dataSource.ReadSalaryRanges();
        }

        /// <summary>
        /// Reads the salary frequencies.
        /// </summary>
        /// <param name="jobsSet">The jobs set.</param>
        /// <returns></returns>
        protected async Task<IList<JobsLookupValue>> ReadSalaryFrequencies(JobsSet jobsSet)
        {
            var dataSource = new JobsLookupValuesFromExamine(ExamineManager.Instance.SearchProviderCollection[jobsSet + "LookupValuesSearcher"]);
            return await dataSource.ReadSalaryFrequencies();
        }

        /// <summary>
        /// Reads the work patterns.
        /// </summary>
        /// <param name="jobsSet">The jobs set.</param>
        /// <returns></returns>
        protected async Task<IList<JobsLookupValue>> ReadWorkPatterns(JobsSet jobsSet)
        {
            var dataSource = new JobsLookupValuesFromExamine(ExamineManager.Instance.SearchProviderCollection[jobsSet + "LookupValuesSearcher"]);
            return await dataSource.ReadWorkPatterns();
        }

        /// <summary>
        /// Reads the contract types.
        /// </summary>
        /// <param name="jobsSet">The jobs set.</param>
        /// <returns></returns>
        protected async Task<IList<JobsLookupValue>> ReadContractTypes(JobsSet jobsSet)
        {
            var dataSource = new JobsLookupValuesFromExamine(ExamineManager.Instance.SearchProviderCollection[jobsSet + "LookupValuesSearcher"]);
            return await dataSource.ReadContractTypes();
        }

        /// <summary>
        /// Gets the job alert settings for a <see cref="JobsSet" />
        /// </summary>
        /// <param name="jobsSet">The jobs set.</param>
        protected JobAlertSettings JobAlertSettings(JobsSet jobsSet)
        {
            return new JobAlertsSettingsFromUmbraco(Umbraco).GetJobAlertsSettings(jobsSet);
        }
    }
}