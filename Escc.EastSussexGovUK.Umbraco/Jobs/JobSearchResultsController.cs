using System;
using System.Collections.Generic;
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

            var mediaUrlTransformer = new RemoveMediaDomainUrlTransformer();
            var viewModel = new JobSearchResultsViewModelFromUmbraco(model.Content,
                new UmbracoOnAzureRelatedLinksService(mediaUrlTransformer)).BuildModel();

            // Add common properties to the model
            var modelBuilder = new BaseViewModelBuilder();
            modelBuilder.PopulateBaseViewModel(viewModel, model.Content, new ContentExperimentSettingsService(), UmbracoContext.Current.InPreviewMode);
            modelBuilder.PopulateBaseViewModelWithInheritedContent(viewModel, 
                new UmbracoLatestService(model.Content), 
                new UmbracoSocialMediaService(model.Content),
                null,
                new UmbracoWebChatSettingsService(model.Content, new UrlListReader()),
                null);


            var detailPage = model.Content.GetPropertyValue<IPublishedContent>("JobDetailPage_Content");
            var detailPageUrl = detailPage != null ? new Uri(detailPage.UrlWithDomain()) : Request.Url;

            var jobs = Task.Run(async() => await ReadJobs(model, detailPageUrl));
            var page = String.IsNullOrWhiteSpace(Request.QueryString["page"]) ? 1 : Int32.Parse(Request.QueryString["page"]);
            viewModel.Jobs = jobs.Result.ToPagedList(page, 10);

            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, new List<string>() { "latestUnpublishDate_Latest" });

            return CurrentTemplate(viewModel);
        }

        private async Task<List<Job>> ReadJobs(RenderModel model, Uri detailPageUrl)
        {
            List<Job> jobs = null;

            var query = new JobSearchQueryFactory().CreateFromQueryString(Request.QueryString);
            var cacheKey = "Jobs" + query.ToHash();

            if (HttpContext.Cache[cacheKey] == null || Request.QueryString["ForceCacheRefresh"] == "1")
            {
                var searchUrl = new TalentLinkUrl(model.Content.GetPropertyValue<string>("SearchScriptUrl_Content")).LinkUrl;
                var resultsUrl = new TalentLinkUrl(model.Content.GetPropertyValue<string>("ResultsScriptUrl_Content")).LinkUrl;

                var lookupValuesParser = new JobLookupValuesHtmlParser();
                var jobResultsParser = new JobResultsHtmlParser(detailPageUrl);
                var jobsProvider = new JobsDataFromTalentLink(searchUrl, resultsUrl, new ConfigurationProxyProvider(), lookupValuesParser, jobResultsParser);

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