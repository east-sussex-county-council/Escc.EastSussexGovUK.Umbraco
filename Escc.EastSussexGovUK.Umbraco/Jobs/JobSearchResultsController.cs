using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Web.Mvc;
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

                var searchUrl = new TalentLinkUrl(model.Content.GetPropertyValue<string>("SearchScriptUrl_Content")).LinkUrl;

                var lookupValuesParser = new JobLookupValuesHtmlParser();
                var jobsProvider = new JobsDataFromExamine(ExamineManager.Instance.SearchProviderCollection[viewModel.ExamineSearcher], viewModel.JobDetailPage?.Url);

                // Update query to use text rather than ids for search terms before passing it to the view
                var lookupsProvider = new JobsDataFromTalentLink(searchUrl, null, null, new ConfigurationProxyProvider(), lookupValuesParser, null, null);
                var locations = Task.Run(async () => await lookupsProvider.ReadLocations()).Result;
                var jobTypes = Task.Run(async () => await lookupsProvider.ReadJobTypes()).Result;
                var organisations = Task.Run(async () => await lookupsProvider.ReadOrganisations()).Result;
                var workPatterns = Task.Run(async () => await lookupsProvider.ReadWorkPatterns()).Result;
                ReplaceLookupIdsWithValues(viewModel.Query.Locations, locations);
                ReplaceLookupIdsWithValues(viewModel.Query.JobTypes, jobTypes);
                ReplaceLookupIdsWithValues(viewModel.Query.Organisations, organisations);
                ReplaceLookupIdsWithValues(viewModel.Query.WorkPatterns, workPatterns);

                var jobs = Task.Run(async () => await jobsProvider.ReadJobs(viewModel.Query)).Result;
                var page = String.IsNullOrWhiteSpace(Request.QueryString["page"]) ? 1 : Int32.Parse(Request.QueryString["page"]);
                viewModel.Jobs = jobs.ToPagedList(page, 10);
            }

            // new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, new List<string>() { "latestUnpublishDate_Latest" });

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
    }
}