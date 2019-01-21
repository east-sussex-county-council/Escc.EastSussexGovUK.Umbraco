using System;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using Escc.EastSussexGovUK.Umbraco.Web.Services;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Web.Jobs
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
            _model.LoginUrl = !string.IsNullOrEmpty(UmbracoContent.GetPropertyValue<string>("loginURL")) ? new Uri(UmbracoContent.GetPropertyValue<string>("loginURL"), UriKind.RelativeOrAbsolute) : null;
            _model.SearchResultsPage = BuildLinkToPage("SearchResultsPage_Content");
            _model.JobsSet = ParseJobsSet("PublicOrRedeployment_Content");

            return _model;
        }

        public async System.Threading.Tasks.Task AddLookupValuesToModel(IJobsLookupValuesProvider lookupValuesDataSource, JobsSearchViewModel viewModel)
        {
            var locations = await lookupValuesDataSource.ReadLocations();
            foreach (var location in locations)
            {
                viewModel.Locations.Add(location);
            }

            var jobTypes = await lookupValuesDataSource.ReadJobTypes();
            foreach (var jobType in jobTypes)
            {
                viewModel.JobTypes.Add(jobType);
            }

            var salaryRanges = await lookupValuesDataSource.ReadSalaryRanges();
            foreach (var salaryRange in salaryRanges)
            {
                viewModel.SalaryRanges.Add(salaryRange);
            }

            var workPatterns = await lookupValuesDataSource.ReadWorkPatterns();
            foreach (var workPattern in workPatterns)
            {
                viewModel.WorkPatterns.Add(workPattern);
            }
        }

    }
}