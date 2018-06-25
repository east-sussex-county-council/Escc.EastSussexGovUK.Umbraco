using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using Escc.Dates;
using Escc.EastSussexGovUK.Umbraco.Examine;
using Escc.EastSussexGovUK.Umbraco.Jobs.Examine;
using Escc.EastSussexGovUK.Umbraco.Jobs.TalentLink;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.EastSussexGovUK.Umbraco.Services;
using Escc.Net;
using Escc.Umbraco.Caching;
using Escc.Umbraco.ContentExperiments;
using Escc.Umbraco.PropertyTypes;
using Examine;
using Exceptionless.Extensions;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Task = System.Threading.Tasks.Task;
using Escc.EastSussexGovUK.Umbraco.Jobs.Api;
using System.Configuration;
using Escc.Umbraco.Expiry;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
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
        public override ActionResult Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var viewModel = new JobSearchResultsViewModelFromUmbraco(model.Content).BuildModel();

            // Add common properties to the model
            var expiryDate = new ExpiryDateFromExamine(model.Content.Id, ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"]);
            var modelBuilder = new BaseViewModelBuilder();
            modelBuilder.PopulateBaseViewModel(viewModel, model.Content, new ContentExperimentSettingsService(),
                expiryDate.ExpiryDate,
                UmbracoContext.Current.InPreviewMode);
            modelBuilder.PopulateBaseViewModelWithInheritedContent(viewModel, 
                new UmbracoLatestService(model.Content), 
                new UmbracoSocialMediaService(model.Content),
                null,
                new UmbracoWebChatSettingsService(model.Content, new UrlListReader()),
                null);

            if (String.IsNullOrEmpty(Request.QueryString["altTemplate"]))
            {
                viewModel.Query = new JobSearchQueryConverter().ToQuery(Request.QueryString);
                viewModel.Query.ClosingDateFrom = DateTime.Today;
                viewModel.Query.JobsSet = viewModel.JobsSet;
                if (Request.QueryString["page"]?.ToUpperInvariant() != "ALL")
                {
                    viewModel.Query.PageSize = viewModel.Paging.PageSize;
                    viewModel.Query.CurrentPage = viewModel.Paging.CurrentPage;
                }
                viewModel.Metadata.Title = viewModel.Query.ToString();

                var jobsProvider = new JobsDataFromApi(new Uri(ConfigurationManager.AppSettings["JobsApiBaseUrl"]), viewModel.JobsSet, viewModel.JobAdvertPage?.Url, new MemoryJobCacheStrategy(HttpContext.Cache, Request.QueryString["ForceCacheRefresh"] == "1"));

                var jobs = Task.Run(async () => await jobsProvider.ReadJobs(viewModel.Query)).Result;
                viewModel.Jobs = jobs.Jobs;
                viewModel.Paging.TotalResults = jobs.TotalJobs;
                if (Request.QueryString["page"]?.ToUpperInvariant() == "ALL")
                {
                    viewModel.Paging.PageSize = viewModel.Paging.TotalResults;
                }

                if (!String.IsNullOrEmpty(viewModel.Metadata.RssFeedUrl))
                {
                    var queryString = HttpUtility.ParseQueryString(Request.Url.Query);
                    queryString.Remove("page");
                    if (queryString.HasKeys())
                    {
                        viewModel.Metadata.RssFeedUrl = viewModel.Metadata.RssFeedUrl + "?" + queryString;
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