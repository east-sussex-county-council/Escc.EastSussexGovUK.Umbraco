using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.Jobs.Examine;
using Escc.EastSussexGovUK.Umbraco.Jobs.TalentLink;
using Escc.EastSussexGovUK.Umbraco.Services;
using Escc.Net;
using Escc.Umbraco.ContentExperiments;
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
            viewModel.Metadata.Description = String.Empty;

            var searchFieldsUrl = new Uri(model.Content.GetPropertyValue<string>("SearchScriptUrl_Content"));
            var dataSource = new JobsLookupValuesFromTalentLink(searchFieldsUrl, new TalentLinkLookupValuesHtmlParser(), new ConfigurationProxyProvider());
            var examineDataSource = new JobsLookupValuesFromExamine(ExamineManager.Instance.SearchProviderCollection["PublicJobsLookupValuesSearcher"]);

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

            var salaryRanges = Task.Run(async () => await examineDataSource.ReadSalaryRanges());
            foreach (var salaryRange in salaryRanges.Result)
            {
                viewModel.SalaryRanges.Add(salaryRange);
            }

            var workPatterns = Task.Run(async () => await dataSource.ReadWorkPatterns());
            foreach (var workPattern in workPatterns.Result)
            {
                viewModel.WorkPatterns.Add(workPattern);
            }

            return CurrentTemplate(viewModel);
        }
    }
}