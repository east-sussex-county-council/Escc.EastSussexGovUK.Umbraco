using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Escc.Dates;
using Escc.EastSussexGovUK.Umbraco.Jobs.Examine;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.EastSussexGovUK.Umbraco.Services;
using Escc.Umbraco.Caching;
using Escc.Umbraco.ContentExperiments;
using Escc.Umbraco.PropertyTypes;
using Examine;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.UrlTransformers;

namespace Escc.EastSussexGovUK.Umbraco.HomePage
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
            var modelBuilder = new BaseViewModelBuilder();
            modelBuilder.PopulateBaseViewModel(viewModel, model.Content, new ContentExperimentSettingsService(), UmbracoContext.Current.InPreviewMode);

            var jobsData = new JobsLookupValuesFromExamine(ExamineManager.Instance.SearchProviderCollection["PublicJobsLookupValuesSearcher"]);
            viewModel.JobLocations = Task.Run(async() => await jobsData.ReadLocations()).Result;
            viewModel.JobTypes = Task.Run(async () => await jobsData.ReadJobTypes()).Result;

            // Jobs close at midnight, so don't cache beyond then
            var untilMidnightTonight = DateTime.Today.ToUkDateTime().AddDays(1) - DateTime.Now.ToUkDateTime();
            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, new List<string>() { "latestUnpublishDate_Latest" }, (int)untilMidnightTonight.TotalSeconds);

            return CurrentTemplate(viewModel);
        }
    }
}