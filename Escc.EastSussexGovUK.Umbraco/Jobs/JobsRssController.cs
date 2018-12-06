using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Escc.Dates;
using Escc.EastSussexGovUK.Umbraco.Services;
using Escc.Net;
using Escc.Umbraco.Caching;
using Escc.Umbraco.ContentExperiments;
using Examine;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Task = System.Threading.Tasks.Task;
using Escc.EastSussexGovUK.Umbraco.Jobs.Api;
using System.Configuration;
using Escc.Umbraco.Expiry;
using System.Runtime.Caching;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Controller for pages based on the 'Jobs RSS feed' Umbraco document type
    /// </summary>
    /// <seealso cref="RenderMvcController"/>
    public class JobsRssController : RenderMvcController
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
            var expiryDate = new ExpiryDateFromExamine(model.Content.Id, ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"]);
            var modelBuilder = new BaseViewModelBuilder();
            modelBuilder.PopulateBaseViewModel(viewModel, model.Content, new ContentExperimentSettingsService(),
                expiryDate.ExpiryDate,
                UmbracoContext.Current.InPreviewMode);

            viewModel.Query = new JobSearchQueryConverter().ToQuery(Request.QueryString);
            viewModel.Query.ClosingDateFrom = DateTime.Today;
            viewModel.Query.JobsSet = viewModel.JobsSet;

            var jobsProvider = new JobsDataFromApi(new Uri(ConfigurationManager.AppSettings["JobsApiBaseUrl"]), viewModel.JobsSet, viewModel.JobAdvertPage.Url, new MemoryJobCacheStrategy(MemoryCache.Default, Request.QueryString["ForceCacheRefresh"] == "1"));
            var jobs = await jobsProvider.ReadJobs(viewModel.Query);

            foreach (var job in jobs.Jobs)
            {
                viewModel.Items.Add(job);
            }

            // Jobs close at midnight, so don't cache beyond then.
            // But we reindex every 2 hours, so expire then if it's sooner.
            var untilMidnightTonight = DateTime.Today.ToUkDateTime().AddDays(1) - DateTime.Now.ToUkDateTime();
            var untilNextReindex = TimeUntilNextReindex();
            var cacheDuration = untilMidnightTonight > untilNextReindex ? untilNextReindex : untilMidnightTonight;
            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, new[] { expiryDate }, (int)cacheDuration.TotalSeconds);

            return CurrentTemplate(viewModel);
        }

        private TimeSpan TimeUntilNextReindex()
        {
            // Reindex is at 15 minutes past the even hour
            var now = DateTime.UtcNow;
            var quarterPastThisHour = now.AddMinutes((now.Minute * -1) + 15).AddSeconds(now.Second * -1);

            if (now.Hour % 2 == 0)
            {
                // even
                if (now.Minute >= 15)
                {
                    return quarterPastThisHour.AddHours(2) - now;
                }
                else
                {
                    return quarterPastThisHour - now;
                }
            }
            else
            {
                // odd
                return quarterPastThisHour.AddHours(1) - now;
            }
        }
    }
}