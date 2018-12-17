using System;
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
using System.Net;
using Exceptionless;

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
        public override ActionResult Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var viewModel = new HomePageViewModelFromUmbraco(model.Content, new RelatedLinksService(new RemoveMediaDomainUrlTransformer(), new ElibraryUrlTransformer(), new RemoveAzureDomainUrlTransformer())).BuildModel();
            var expiryDate = new ExpiryDateFromExamine(model.Content.Id, ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"]);
            var modelBuilder = new BaseViewModelBuilder();
            modelBuilder.PopulateBaseViewModel(viewModel, model.Content, new ContentExperimentSettingsService(),
                expiryDate.ExpiryDate,
                UmbracoContext.Current.InPreviewMode);
            modelBuilder.PopulateBaseViewModelWithInheritedContent(viewModel, null, null, null, null, null, new RatingSettingsFromUmbraco(model.Content));

            try
            {
                var jobsData = new JobsLookupValuesFromApi(new Uri(ConfigurationManager.AppSettings["JobsApiBaseUrl"]), JobsSet.PublicJobs, new MemoryJobCacheStrategy(MemoryCache.Default, Request.QueryString["ForceCacheRefresh"] == "1"));
                viewModel.JobLocations = Task.Run(async () => await jobsData.ReadLocations()).Result;
                viewModel.JobTypes = Task.Run(async () => await jobsData.ReadJobTypes()).Result;
            }
            catch (Exception ex)
            {
                // Report an error fetching jobs data, but don't cause the page to fail to load
                ex.ToExceptionless().Submit();
            }

            // Jobs close at midnight, so don't cache beyond then
            var untilMidnightTonight = DateTime.Today.ToUkDateTime().AddDays(1) - DateTime.Now.ToUkDateTime();
            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, new IExpiryDateSource[] { expiryDate, new ExpiryDateFromPropertyValue(model.Content, "latestUnpublishDate_Latest") }, (int)untilMidnightTonight.TotalSeconds);

            return CurrentTemplate(viewModel);
        }
    }
}