﻿using Escc.EastSussexGovUK.Umbraco.Errors;
using Escc.EastSussexGovUK.Umbraco.Jobs.Api;
using Escc.EastSussexGovUK.Umbraco.Web.Latest;
using Escc.EastSussexGovUK.Umbraco.Web.Services;
using Escc.Umbraco.ContentExperiments;
using Escc.Umbraco.Expiry;
using Escc.Umbraco.PropertyTypes;
using Examine;
using System;
using System.Configuration;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using Escc.EastSussexGovUK.Umbraco.Jobs.Alerts;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Mvc;
using Umbraco.Core.Logging;
using Exceptionless;

namespace Escc.EastSussexGovUK.Umbraco.Web.Jobs.Alerts
{
    /// <summary>
    /// Controller for pages based on the 'Job alerts' Umbraco document type
    /// </summary>
    /// <seealso cref="Umbraco.Web.Mvc.RenderMvcController" />
    public class JobAlertsController : RenderMvcController
    {
        /// <summary>
        /// The default action to render the front-end view
        /// </summary>
        /// <param name="model"/>
        /// <returns/>
        public async new Task<ActionResult> Index(RenderModel model)
        {
            var modelBuilder = new JobsSearchViewModelFromUmbraco(model.Content, new JobAlertsViewModel());
            var viewModel = (JobAlertsViewModel)modelBuilder.BuildModel();
            var lookupValuesDataSource = new JobsLookupValuesFromApi(new Uri(ConfigurationManager.AppSettings["JobsApiBaseUrl"]), viewModel.JobsSet, new MemoryJobCacheStrategy(MemoryCache.Default, Request.QueryString["ForceCacheRefresh"] == "1"));
            await modelBuilder.AddLookupValuesToModel(lookupValuesDataSource, viewModel);

            var converter = new JobSearchQueryConverter(ConfigurationManager.AppSettings["TranslateObsoleteJobTypes"]?.ToUpperInvariant() == "TRUE");
            var alertId = new JobAlertIdEncoder(converter).ParseIdFromUrl(Request.Url);
            if (!string.IsNullOrEmpty(alertId))
            {
                if (ConfigurationManager.ConnectionStrings["Escc.EastSussexGovUK.Umbraco.AzureStorage"] == null || String.IsNullOrEmpty(ConfigurationManager.ConnectionStrings["Escc.EastSussexGovUK.Umbraco.AzureStorage"].ConnectionString))
                {
                    var error = new ConfigurationErrorsException("The Escc.EastSussexGovUK.Umbraco.AzureStorage connection string is missing from web.config");
                    LogHelper.Error<JobAlertsController>(error.Message, error);
                    error.ToExceptionless().Submit();
                }
                else
                {
                    var alertsRepo = new AzureTableStorageAlertsRepository(converter, ConfigurationManager.ConnectionStrings["Escc.EastSussexGovUK.Umbraco.AzureStorage"].ConnectionString);
                    viewModel.Alert = alertsRepo.GetAlertById(alertId);
                    viewModel.Query = viewModel.Alert?.Query;

                    if (viewModel.Alert == null && Request.QueryString["cancelled"] != "1" && string.IsNullOrEmpty(Request.QueryString["altTemplate"]))
                    {
                        // Returning HttpNotFoundResult() ends up with a generic browser 404, 
                        // so to get our custom one we need to look it up and transfer control to it.
                        var notFoundUrl = new HttpStatusFromConfiguration().GetCustomUrlForStatusCode(404);
                        if (notFoundUrl != null && Server != null)
                        {
                            Server.TransferRequest(notFoundUrl + "?404;" + HttpUtility.UrlEncode(Request.Url.ToString()));
                        }
                    }
                }
            }

            var baseModelBuilder = new BaseViewModelBuilder(new EastSussexGovUKTemplateRequest(Request, webChatSettingsService: new UmbracoWebChatSettingsService(model.Content, new UrlListReader())));
            await baseModelBuilder.PopulateBaseViewModel(viewModel, model.Content, new ContentExperimentSettingsService(),
                new ExpiryDateFromExamine(model.Content.Id, ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"], new ExpiryDateMemoryCache(TimeSpan.FromHours(1))).ExpiryDate,
                UmbracoContext.Current.InPreviewMode);
            baseModelBuilder.PopulateBaseViewModelWithInheritedContent(viewModel,
                  new UmbracoLatestService(model.Content),
                  new UmbracoSocialMediaService(model.Content),
                  null, null);

            return CurrentTemplate(viewModel);
        }

    }
}