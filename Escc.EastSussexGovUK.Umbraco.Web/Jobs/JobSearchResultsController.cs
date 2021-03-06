﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Escc.Dates;
using Escc.EastSussexGovUK.Umbraco.Web.Services;
using Escc.Umbraco.Caching;
using Escc.Umbraco.ContentExperiments;
using Escc.Umbraco.PropertyTypes;
using Examine;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using System.Configuration;
using Escc.Umbraco.Expiry;
using System.Runtime.Caching;
using Escc.EastSussexGovUK.Umbraco.Web.Latest;
using Escc.EastSussexGovUK.Umbraco.Jobs.Api;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Mvc;
using Escc.Net;

namespace Escc.EastSussexGovUK.Umbraco.Web.Jobs
{
    /// <summary>
    /// Controller for pages based on the 'Job search results' Umbraco document type
    /// </summary>
    public class JobSearchResultsController : RenderMvcController
    {
        /// <summary>
        /// The default action to render the front-end view
        /// </summary>
        /// <param name="model"/>
        /// <returns/>
        public async new Task<ActionResult> Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var viewModel = new JobSearchResultsViewModelFromUmbraco(model.Content).BuildModel();

            // Add common properties to the model
            var expiryDate = new ExpiryDateFromExamine(model.Content.Id, ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"], new ExpiryDateMemoryCache(TimeSpan.FromHours(1)));
            var modelBuilder = new BaseViewModelBuilder(new EastSussexGovUKTemplateRequest(Request, webChatSettingsService: new UmbracoWebChatSettingsService(model.Content, new UrlListReader())));
            await modelBuilder.PopulateBaseViewModel(viewModel, model.Content, new ContentExperimentSettingsService(),
                expiryDate.ExpiryDate,
                UmbracoContext.Current.InPreviewMode);
            modelBuilder.PopulateBaseViewModelWithInheritedContent(viewModel, 
                new UmbracoLatestService(model.Content), 
                new UmbracoSocialMediaService(model.Content),
                null, null, null);

            if (String.IsNullOrEmpty(Request.QueryString["altTemplate"]))
            {
                viewModel.Query = new JobSearchQueryConverter(ConfigurationManager.AppSettings["TranslateObsoleteJobTypes"]?.ToUpperInvariant() == "TRUE").ToQuery(Request.QueryString);
                viewModel.Query.ClosingDateFrom = DateTime.Today;
                viewModel.Query.JobsSet = viewModel.JobsSet;
                if (Request.QueryString["page"]?.ToUpperInvariant() != "ALL")
                {
                    viewModel.Query.PageSize = viewModel.Paging.PageSize;
                    viewModel.Query.CurrentPage = viewModel.Paging.CurrentPage;
                }
                viewModel.Metadata.Title = viewModel.Query.ToString();

                var jobsProvider = new JobsDataFromApi(
                    new Uri(ConfigurationManager.AppSettings["JobsApiBaseUrl"]), 
                    viewModel.JobsSet, 
                    viewModel.JobAdvertPage?.Url, 
                    new HttpClientProvider(),
                    new MemoryJobCacheStrategy(MemoryCache.Default, Request.QueryString["ForceCacheRefresh"] == "1")
                    );

                var jobs = await jobsProvider.ReadJobs(viewModel.Query);
                viewModel.Jobs = jobs.Jobs;
                viewModel.Paging.TotalResults = jobs.TotalJobs;
                if (Request.QueryString["page"]?.ToUpperInvariant() == "ALL")
                {
                    viewModel.Paging.PageSize = viewModel.Paging.TotalResults;
                }

                if (viewModel.RssFeedUrl != null)
                {
                    var queryString = HttpUtility.ParseQueryString(Request.Url.Query);
                    queryString.Remove("page");
                    if (queryString.HasKeys())
                    {
                        viewModel.RssFeedTitle = "RSS feed for " + viewModel.Metadata.Title;
                        if (!String.IsNullOrEmpty(viewModel.Metadata.TitlePattern))
                        {
                            viewModel.RssFeedTitle = string.Format(viewModel.Metadata.TitlePattern, viewModel.RssFeedTitle);
                        }
                        viewModel.RssFeedUrl = new Uri(viewModel.RssFeedUrl + "?" + queryString, UriKind.RelativeOrAbsolute);
                    }
                }
            }

            // Jobs close at midnight, so don't cache beyond then
            var untilMidnightTonight = DateTime.Today.ToUkDateTime().AddDays(1) - DateTime.Now.ToUkDateTime();
            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, new IExpiryDateSource[] { expiryDate, new ExpiryDateFromPropertyValue(model.Content, "latestUnpublishDate_Latest") }, (int)untilMidnightTonight.TotalSeconds);

            return CurrentTemplate(viewModel);
        }
    }
}