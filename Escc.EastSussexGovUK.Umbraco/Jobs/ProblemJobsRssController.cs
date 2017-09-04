using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Caching;
using Escc.Dates;
using Escc.EastSussexGovUK.Umbraco.Examine;
using Escc.EastSussexGovUK.Umbraco.Jobs.Examine;
using Escc.EastSussexGovUK.Umbraco.Services;
using Escc.Net;
using Escc.Umbraco.Caching;
using Escc.Umbraco.ContentExperiments;
using Escc.Umbraco.PropertyTypes;
using Examine;
using Exceptionless.Extensions;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Task = System.Threading.Tasks.Task;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
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
            var modelBuilder = new BaseViewModelBuilder();
            modelBuilder.PopulateBaseViewModel(viewModel, model.Content, new ContentExperimentSettingsService(), UmbracoContext.Current.InPreviewMode);

            var jobsProvider = new JobsDataFromExamine(ExamineManager.Instance.SearchProviderCollection[viewModel.ExamineSearcher], new QueryBuilder(new LuceneTokenisedQueryBuilder(), new KeywordsTokeniser(), new LuceneStopWordsRemover(), new WildcardSuffixFilter()), null, new RelativeJobUrlGenerator(viewModel.JobAdvertPage.Url));
            var jobs = await jobsProvider.ReadProblemJobs();

            foreach (var job in jobs)
            {
                viewModel.Items.Add(job);
            }

            // Jobs close at midnight, so don't cache beyond then
            var untilMidnightTonight = DateTime.Today.ToUkDateTime().AddDays(1) - DateTime.Now.ToUkDateTime();
            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, null, (int)untilMidnightTonight.TotalSeconds);

            return CurrentTemplate(viewModel);
        }
    }
}