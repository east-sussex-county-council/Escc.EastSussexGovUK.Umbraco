using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.EastSussexGovUK.Umbraco.Services;
using Escc.Net;
using Escc.Umbraco.Caching;
using Escc.Umbraco.ContentExperiments;
using Escc.Umbraco.PropertyTypes;
using Exceptionless.Extensions;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using X.PagedList;
using Task = System.Threading.Tasks.Task;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Controller for pages based on the 'Job search results' Umbraco document type
    /// </summary>
    /// <seealso cref="Umbraco.Web.Mvc.RenderMvcController" />
    public class JobSearchResultsController : RenderMvcController
    {
        /// <summary>
        /// The default action to render the front-end view
        /// </summary>
        /// <param name="model"/>
        /// <returns/>
        public override ActionResult Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var viewModel = new JobSearchResultsViewModelFromUmbraco(model.Content).BuildModel();

            // Add common properties to the model
            var modelBuilder = new BaseViewModelBuilder();
            modelBuilder.PopulateBaseViewModel(viewModel, model.Content, new ContentExperimentSettingsService(), UmbracoContext.Current.InPreviewMode);
            modelBuilder.PopulateBaseViewModelWithInheritedContent(viewModel, 
                new UmbracoLatestService(model.Content), 
                new UmbracoSocialMediaService(model.Content),
                null,
                new UmbracoWebChatSettingsService(model.Content, new UrlListReader()),
                null);


            viewModel.Query = new JobSearchQueryFactory().CreateFromQueryString(Request.QueryString);

            var searchUrl = new TalentLinkUrl(model.Content.GetPropertyValue<string>("SearchScriptUrl_Content")).LinkUrl;
            var resultsUrl = new TalentLinkUrl(model.Content.GetPropertyValue<string>("ResultsScriptUrl_Content")).LinkUrl;

            var lookupValuesParser = new JobLookupValuesHtmlParser();
            var jobResultsParser = new JobResultsHtmlParser(viewModel.JobDetailPage?.Url);
            var jobsProvider = new JobsDataFromTalentLink(searchUrl, resultsUrl, new ConfigurationProxyProvider(), lookupValuesParser, jobResultsParser);

            var jobs = Task.Run(async() => await ReadJobs(jobsProvider, viewModel.Query)).Result;
            var page = String.IsNullOrWhiteSpace(Request.QueryString["page"]) ? 1 : Int32.Parse(Request.QueryString["page"]);
            viewModel.Jobs = jobs.ToPagedList(page, 10);

            // Update query to use text rather than ids for search terms before passing it to the view
            var locations = Task.Run(async () => await jobsProvider.ReadLocations()).Result;
            var jobTypes = Task.Run(async () => await jobsProvider.ReadJobTypes()).Result;
            var organisations = Task.Run(async () => await jobsProvider.ReadOrganisations()).Result;
            var salaryRanges = Task.Run(async () => await jobsProvider.ReadSalaryRanges()).Result;
            var workPatterns = Task.Run(async () => await jobsProvider.ReadWorkPatterns()).Result;
            ReplaceLookupIdsWithValues(viewModel.Query.Locations, locations);
            ReplaceLookupIdsWithValues(viewModel.Query.JobTypes, jobTypes);
            ReplaceLookupIdsWithValues(viewModel.Query.Organisations, organisations);
            ReplaceLookupIdsWithValues(viewModel.Query.SalaryRanges, salaryRanges);
            ReplaceLookupIdsWithValues(viewModel.Query.WorkPatterns, workPatterns);

            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, new List<string>() { "latestUnpublishDate_Latest" });

            return CurrentTemplate(viewModel);
        }

        private static void ReplaceLookupIdsWithValues(IList<string> ids, IList<JobsLookupValue> lookupValues)
        {
            for (var i = 0; i < ids.Count; i++)
            {
                var match = lookupValues.FirstOrDefault(lookup => lookup.Id == ids[i]);
                ids[i] = match?.Text;
            }
        }

        private async Task<List<Job>> ReadJobs(IJobsDataProvider jobsProvider, JobSearchQuery query)
        {
            List<Job> jobs;

            var cacheKey = "Jobs" + query.ToHash();

            if (HttpContext.Cache[cacheKey] == null || Request.QueryString["ForceCacheRefresh"] == "1")
            {
                jobs = await jobsProvider.ReadJobs(query);

                HttpContext.Cache.Insert(cacheKey, jobs, null, DateTime.Now.AddHours(1), Cache.NoSlidingExpiration);
            }
            else
            {
                jobs = (List<Job>)HttpContext.Cache[cacheKey];
            }
            return jobs;
        }
    }
}