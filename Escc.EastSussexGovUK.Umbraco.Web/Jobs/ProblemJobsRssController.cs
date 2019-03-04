using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Escc.Dates;
using Escc.EastSussexGovUK.Umbraco.Web.Services;
using Escc.Umbraco.Caching;
using Escc.Umbraco.ContentExperiments;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.Jobs.Api;
using System.Configuration;
using Examine;
using Escc.Umbraco.Expiry;
using Escc.Net;

namespace Escc.EastSussexGovUK.Umbraco.Web.Jobs
{
    /// <summary>
    /// Controller for pages based on the 'Problem jobs RSS feed' Umbraco document type
    /// </summary>
    /// <seealso cref="RenderMvcController"/>
    public class ProblemJobsRssController : RenderMvcController
    {
        /// <summary>
        /// The default action to render the front-end view
        /// </summary>
        /// <param name="model"/>
        /// <returns/>
        public async new Task<ActionResult> Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var viewModel = new JobsRssViewModelFromUmbraco(model.Content).BuildModel();

            // Add common properties to the model
            var expiryDate = new ExpiryDateFromExamine(model.Content.Id, ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"], new ExpiryDateMemoryCache(TimeSpan.FromHours(1)));
            var modelBuilder = new BaseViewModelBuilder(null);
            await modelBuilder.PopulateBaseViewModel(viewModel, model.Content, null,
                expiryDate.ExpiryDate,
                UmbracoContext.Current.InPreviewMode);

            var jobsProvider = new JobsDataFromApi(new Uri(ConfigurationManager.AppSettings["JobsApiBaseUrl"]), viewModel.JobsSet, viewModel.JobAdvertPage.Url, new HttpClientProvider(), null);
            var jobs = await jobsProvider.ReadProblemJobs();

            foreach (var job in jobs)
            {
                viewModel.Items.Add(job);
            }

            // Jobs close at midnight, so don't cache beyond then
            var untilMidnightTonight = DateTime.Today.ToUkDateTime().AddDays(1) - DateTime.Now.ToUkDateTime();
            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, new[] { expiryDate }, (int)untilMidnightTonight.TotalSeconds);

            return CurrentTemplate(viewModel);
        }
    }
}