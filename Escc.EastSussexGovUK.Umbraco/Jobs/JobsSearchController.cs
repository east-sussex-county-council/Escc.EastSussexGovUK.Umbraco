using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Escc.Dates;
using Escc.EastSussexGovUK.Umbraco.Jobs.Examine;
using Escc.EastSussexGovUK.Umbraco.Jobs.TalentLink;
using Escc.EastSussexGovUK.Umbraco.Services;
using Escc.Net;
using Escc.Umbraco.Caching;
using Escc.Umbraco.ContentExperiments;
using Escc.Umbraco.PropertyTypes;
using Examine;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Controller for pages based on the 'Jobs search' Umbraco document type
    /// </summary>
    public class JobsSearchController : RenderMvcController
    {
        // GET: TalentLinkComponent
        public override ActionResult Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var viewModel = new SearchViewModelFromUmbraco(model.Content).BuildModel();

            var modelBuilder = new BaseViewModelBuilder();
            modelBuilder.PopulateBaseViewModel(viewModel, model.Content, new ContentExperimentSettingsService(), UmbracoContext.Current.InPreviewMode);
            modelBuilder.PopulateBaseViewModelWithInheritedContent(viewModel,
                new UmbracoLatestService(model.Content),
                new UmbracoSocialMediaService(model.Content),
                null,
                new UmbracoWebChatSettingsService(model.Content, new UrlListReader()),
                null);
            viewModel.Metadata.Description = String.Empty;

            var dataSource = new JobsLookupValuesFromExamine(ExamineManager.Instance.SearchProviderCollection[viewModel.ExamineSearcher]);

            var locations = Task.Run(async () => await dataSource.ReadLocations());
            foreach (var location in locations.Result)
            {
                viewModel.Locations.Add(location);
            }

            var jobTypes = Task.Run(async () => await dataSource.ReadJobTypes());
            foreach (var jobType in jobTypes.Result)
            {
                viewModel.JobTypes.Add(jobType);
            }

            var salaryRanges = Task.Run(async () => await dataSource.ReadSalaryRanges());
            foreach (var salaryRange in salaryRanges.Result)
            {
                viewModel.SalaryRanges.Add(salaryRange);
            }

            var workPatterns = Task.Run(async () => await dataSource.ReadWorkPatterns());
            foreach (var workPattern in workPatterns.Result)
            {
                viewModel.WorkPatterns.Add(workPattern);
            }

            // Jobs close at midnight, so don't cache beyond then
            var untilMidnightTonight = DateTime.Today.ToUkDateTime().AddDays(1) - DateTime.Now.ToUkDateTime();
            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, new List<string>() { "latestUnpublishDate_Latest" }, (int)untilMidnightTonight.TotalSeconds);

            return CurrentTemplate(viewModel);
        }
    }
}