using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.Services;
using Escc.Net;
using Escc.Umbraco.ContentExperiments;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Controller for pages based on the 'Jobs search' Umbraco document type
    /// </summary>
    /// <seealso cref="CoRenderMvcController/>
    public class JobsSearchController : RenderMvcController
    {
        // GET: TalentLinkComponent
        public override ActionResult Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var viewModel = new JobsSearchViewModel();

            var modelBuilder = new BaseViewModelBuilder();
            modelBuilder.PopulateBaseViewModel(viewModel, model.Content, new ContentExperimentSettingsService(), UmbracoContext.Current.InPreviewMode);
            viewModel.Metadata.Description = String.Empty;

            var searchFieldsUrl = new Uri(model.Content.GetPropertyValue<string>("SearchScriptUrl_Content"));
            var dataSource = new JobsDataFromTalentLink(searchFieldsUrl, null, new ConfigurationProxyProvider(), new JobLookupValuesHtmlParser(), null);

            var locations = Task.Run(async () => await dataSource.ReadLocations());
            foreach (var location in locations.Result.Keys)
            {
                viewModel.Locations.Add(location, locations.Result[location]);
            }

            var jobTypes = Task.Run(async () => await dataSource.ReadJobTypes());
            foreach (var jobType in jobTypes.Result.Keys)
            {
                viewModel.JobTypes.Add(jobType, jobTypes.Result[jobType]);
            }

            var salaryRanges = Task.Run(async () => await dataSource.ReadSalaryRanges());
            foreach (var salaryRange in salaryRanges.Result.Keys)
            {
                viewModel.SalaryRanges.Add(salaryRange, salaryRanges.Result[salaryRange]);
            }

            var workPatterns = Task.Run(async () => await dataSource.ReadWorkPatterns());
            foreach (var workPattern in workPatterns.Result.Keys)
            {
                viewModel.WorkPatterns.Add(workPattern, workPatterns.Result[workPattern]);
            }

            return CurrentTemplate(viewModel);
        }
    }
}