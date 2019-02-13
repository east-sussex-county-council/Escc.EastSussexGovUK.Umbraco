﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
using Escc.EastSussexGovUK.Umbraco.UrlTransformers;
using Escc.EastSussexGovUK.Umbraco.Web.Ratings;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using System.Configuration;
using Escc.Umbraco.Expiry;
using System.Runtime.Caching;
using Escc.EastSussexGovUK.Umbraco.Jobs.Api;
using Exceptionless;
using Escc.EastSussexGovUK.Mvc;
using Escc.RubbishAndRecycling.SiteFinder.Website;
using Escc.Net.Configuration;

namespace Escc.EastSussexGovUK.Umbraco.Web.HomePage
{
    /// <summary>
    /// The controller for the Home Page document type
    /// </summary>
    public class HomePageController : RenderMvcController
    {
        /// <summary>
        /// The default action to render the front-end view
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">model</exception>
        public async new Task<ActionResult> Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var viewModel = new HomePageViewModelFromUmbraco(model.Content, new RelatedLinksService(new RemoveMediaDomainUrlTransformer(), new ElibraryUrlTransformer(), new RemoveAzureDomainUrlTransformer())).BuildModel();
            var expiryDate = new ExpiryDateFromExamine(model.Content.Id, ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"], new ExpiryDateMemoryCache(TimeSpan.FromHours(1)));

            var modelBuilder = new BaseViewModelBuilder(new EastSussexGovUKTemplateRequest(Request));
            await modelBuilder.PopulateBaseViewModel(viewModel, model.Content, new ContentExperimentSettingsService(),
                expiryDate.ExpiryDate,
                UmbracoContext.Current.InPreviewMode);
            await modelBuilder.PopulateBaseViewModelWithInheritedContent(viewModel, null, null, null, null, null, new RatingSettingsFromUmbraco(model.Content));

            try
            {
                var forceCacheRefresh = Request.QueryString["ForceCacheRefresh"] == "1";

                var jobsData = new JobsLookupValuesFromApi(new Uri(ConfigurationManager.AppSettings["JobsApiBaseUrl"]), JobsSet.PublicJobs, new MemoryJobCacheStrategy(MemoryCache.Default, forceCacheRefresh));
                var locationsTask = jobsData.ReadLocations();
                var jobTypesTask = jobsData.ReadJobTypes();

                var wasteTypesDataSource = new UmbracoWasteTypesDataSource(new Uri(Request.Url, ConfigurationManager.AppSettings["WasteTypesDataUrl"]), new ConfigurationProxyProvider(), forceCacheRefresh ? null : new ApplicationCacheStrategy<List<string>>(TimeSpan.FromDays(1)));
                var wasteTypesTask = wasteTypesDataSource.LoadWasteTypes();

                await Task.WhenAll(locationsTask, jobTypesTask, wasteTypesTask);

                viewModel.JobLocations = locationsTask.Result;
                viewModel.JobTypes = jobTypesTask.Result;
                viewModel.RecyclingSiteSearch.WasteTypes = wasteTypesTask.Result;
            }
            catch (Exception ex)
            {
                // Report an error fetching jobs or waste site data, but don't cause the page to fail to load
                ex.ToExceptionless().Submit();
            }

            // Jobs close at midnight, so don't cache beyond then
            var untilMidnightTonight = DateTime.Today.ToUkDateTime().AddDays(1) - DateTime.Now.ToUkDateTime();
            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, new IExpiryDateSource[] { expiryDate, new ExpiryDateFromPropertyValue(model.Content, "latestUnpublishDate_Latest") }, (int)untilMidnightTonight.TotalSeconds);

            return CurrentTemplate(viewModel);
        }
    }
}