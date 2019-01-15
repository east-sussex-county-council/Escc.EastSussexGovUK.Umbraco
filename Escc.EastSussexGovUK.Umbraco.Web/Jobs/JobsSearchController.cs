using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Escc.Dates;
using Escc.EastSussexGovUK.Umbraco.Web.Services;
using Escc.Net;
using Escc.Umbraco.Caching;
using Escc.Umbraco.ContentExperiments;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.Jobs.Api;
using System.Configuration;
using Examine;
using Escc.Umbraco.Expiry;
using System.Runtime.Caching;
using Escc.EastSussexGovUK.Umbraco.Web.Latest;
using Escc.EastSussexGovUK.Umbraco.Jobs;

namespace Escc.EastSussexGovUK.Umbraco.Web.Jobs
{
    /// <summary>
    /// Controller for pages based on the 'Jobs search' Umbraco document type
    /// </summary>
    public class JobsSearchController : RenderMvcController
    {
        // GET: TalentLinkComponent
        public async new Task<ActionResult> Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var modelBuilder = new JobsSearchViewModelFromUmbraco(model.Content, new JobsSearchViewModel());
            var viewModel = modelBuilder.BuildModel();
            var lookupValuesDataSource = new JobsLookupValuesFromApi(new Uri(ConfigurationManager.AppSettings["JobsApiBaseUrl"]), viewModel.JobsSet, new MemoryJobCacheStrategy(MemoryCache.Default, Request.QueryString["ForceCacheRefresh"] == "1"));
            await modelBuilder.AddLookupValuesToModel(lookupValuesDataSource, viewModel);

            var expiryDate = new ExpiryDateFromExamine(model.Content.Id, ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"]);
            var baseModelBuilder = new BaseViewModelBuilder();
            baseModelBuilder.PopulateBaseViewModel(viewModel, model.Content, new ContentExperimentSettingsService(),
                expiryDate.ExpiryDate,
                UmbracoContext.Current.InPreviewMode);
            baseModelBuilder.PopulateBaseViewModelWithInheritedContent(viewModel,
                new UmbracoLatestService(model.Content),
                new UmbracoSocialMediaService(model.Content),
                null,
                new UmbracoWebChatSettingsService(model.Content, new UrlListReader()),
                null);
            viewModel.Metadata.Description = String.Empty;

            // Jobs close at midnight, so don't cache beyond then
            var untilMidnightTonight = DateTime.Today.ToUkDateTime().AddDays(1) - DateTime.Now.ToUkDateTime();
            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, new IExpiryDateSource[] { expiryDate, new ExpiryDateFromPropertyValue(model.Content, "latestUnpublishDate_Latest") }, (int)untilMidnightTonight.TotalSeconds);

            return CurrentTemplate(viewModel);
        }
    }
}