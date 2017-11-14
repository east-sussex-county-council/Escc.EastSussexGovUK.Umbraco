using Escc.EastSussexGovUK.Umbraco.Errors;
using Escc.EastSussexGovUK.Umbraco.Jobs.Api;
using Escc.EastSussexGovUK.Umbraco.Jobs.Examine;
using Escc.EastSussexGovUK.Umbraco.Services;
using Escc.Umbraco.ContentExperiments;
using Escc.Umbraco.PropertyTypes;
using Examine;
using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Alerts
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
        public override ActionResult Index(RenderModel model)
        {
            var modelBuilder = new JobsSearchViewModelFromUmbraco(model.Content, new JobAlertsViewModel());
            var viewModel = (JobAlertsViewModel)modelBuilder.BuildModel();
            var lookupValuesDataSource = new JobsLookupValuesFromApi(new Uri(ConfigurationManager.AppSettings["JobsApiBaseUrl"]), viewModel.JobsSet);
            modelBuilder.AddLookupValuesToModel(lookupValuesDataSource, viewModel);

            var converter = new JobSearchQueryConverter();
            var alertId = new JobAlertIdEncoder(converter).ParseIdFromUrl(Request.Url);
            if (!string.IsNullOrEmpty(alertId))
            {
                var alertsRepo = new AzureTableStorageAlertsRepository(converter);
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

            var baseModelBuilder = new BaseViewModelBuilder();
            baseModelBuilder.PopulateBaseViewModel(viewModel, model.Content, new ContentExperimentSettingsService(), UmbracoContext.Current.InPreviewMode);
            baseModelBuilder.PopulateBaseViewModelWithInheritedContent(viewModel,
                  new UmbracoLatestService(model.Content),
                  new UmbracoSocialMediaService(model.Content),
                  null,
                  new UmbracoWebChatSettingsService(model.Content, new UrlListReader()),
                  null);

            return CurrentTemplate(viewModel);
        }

    }
}