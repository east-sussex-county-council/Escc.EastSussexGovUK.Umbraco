using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
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
using X.PagedList;
using Task = System.Threading.Tasks.Task;

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

            var viewModel = new SearchResultsViewModelFromUmbraco(model.Content).BuildModel();

            // Add common properties to the model
            var modelBuilder = new BaseViewModelBuilder();
            modelBuilder.PopulateBaseViewModel(viewModel, model.Content, new ContentExperimentSettingsService(), UmbracoContext.Current.InPreviewMode);
            modelBuilder.PopulateBaseViewModelWithInheritedContent(viewModel, 
                new UmbracoLatestService(model.Content), 
                new UmbracoSocialMediaService(model.Content),
                null,
                new UmbracoWebChatSettingsService(model.Content, new UrlListReader()),
                null);

            if (String.IsNullOrEmpty(Request.QueryString["altTemplate"]))
            {
                viewModel.Query = new JobSearchQueryFactory().CreateFromQueryString(Request.QueryString);

                var jobsProvider = new JobsDataFromExamine(ExamineManager.Instance.SearchProviderCollection[viewModel.ExamineSearcher], viewModel.JobAdvertPage?.Url);

                var jobs = Task.Run(async () => await jobsProvider.ReadJobs(viewModel.Query)).Result;
                if (String.IsNullOrWhiteSpace(Request.QueryString["page"]))
                {
                    viewModel.Jobs = jobs.ToPagedList(1, 10);
                }
                else if (Request.QueryString["page"].ToUpperInvariant() == "ALL")
                {
                    viewModel.Jobs = jobs.ToPagedList(1, 10000);
                }
                else
                {
                    int page = 1;
                    Int32.TryParse(Request.QueryString["page"], out page);
                    viewModel.Jobs = jobs.ToPagedList(page, 10);
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

            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, new List<string>() { "latestUnpublishDate_Latest" });

            return CurrentTemplate(viewModel);
        }
    }
}