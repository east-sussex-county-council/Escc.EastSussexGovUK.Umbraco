using System;
using Escc.EastSussexGovUK.Umbraco.Services;
using Umbraco.Core.Models;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    public class JobsSearchViewModelFromUmbraco : BaseViewModelFromUmbracoBuilder, IViewModelBuilder<JobsSearchViewModel>
    {
        private readonly JobsSearchViewModel _model;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobsSearchViewModel" /> class.
        /// </summary>
        /// <param name="umbracoContent">Content from Umbraco using the 'Jobs' document type.</param>
        /// <exception cref="System.ArgumentNullException">umbracoContent</exception>
        public JobsSearchViewModelFromUmbraco(IPublishedContent umbracoContent, JobsSearchViewModel model) :
            base(umbracoContent, null)
        {
            _model = model ?? new JobsSearchViewModel();
        }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <returns></returns>
        public JobsSearchViewModel BuildModel()
        {
            _model.JobsLogo = BuildImage("JobsLogo_Content");
            _model.HeaderBackgroundImageSmall = BuildImage("HeaderBackgroundImage_Content");
            _model.HeaderBackgroundImageMedium = BuildImage("HeaderBackgroundImageMedium_Content");
            _model.HeaderBackgroundImageLarge = BuildImage("HeaderBackgroundImageLarge_Content");
            _model.JobsHomePage = BuildLinkToPage("JobsHomePage_Content");
            _model.LoginPage = BuildLinkToPage("LoginPage_Content");
            _model.JobsSet = ParseJobsSet("PublicOrRedeployment_Content");

            return _model;
        }

        public void AddLookupValuesToModel(IJobsLookupValuesProvider lookupValuesDataSource, JobsSearchViewModel viewModel)
        {
            var locations = System.Threading.Tasks.Task.Run(async () => await lookupValuesDataSource.ReadLocations());
            foreach (var location in locations.Result)
            {
                viewModel.Locations.Add(location);
            }

            var jobTypes = System.Threading.Tasks.Task.Run(async () => await lookupValuesDataSource.ReadJobTypes());
            foreach (var jobType in jobTypes.Result)
            {
                viewModel.JobTypes.Add(jobType);
            }

            var salaryRanges = System.Threading.Tasks.Task.Run(async () => await lookupValuesDataSource.ReadSalaryRanges());
            foreach (var salaryRange in salaryRanges.Result)
            {
                viewModel.SalaryRanges.Add(salaryRange);
            }

            var workPatterns = System.Threading.Tasks.Task.Run(async () => await lookupValuesDataSource.ReadWorkPatterns());
            foreach (var workPattern in workPatterns.Result)
            {
                viewModel.WorkPatterns.Add(workPattern);
            }
        }

    }
}