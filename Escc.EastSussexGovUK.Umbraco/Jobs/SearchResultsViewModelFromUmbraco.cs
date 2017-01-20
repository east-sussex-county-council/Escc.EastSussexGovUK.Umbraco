using System;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.EastSussexGovUK.Umbraco.Services;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    public class SearchResultsViewModelFromUmbraco : BaseViewModelFromUmbracoBuilder, IViewModelBuilder<JobSearchResultsViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobSearchResultsViewModel" /> class.
        /// </summary>
        /// <param name="umbracoContent">Content from Umbraco using the 'Jobs' document type.</param>
        /// <exception cref="System.ArgumentNullException">umbracoContent
        /// or
        /// mediaUrlTransformer</exception>
        public SearchResultsViewModelFromUmbraco(IPublishedContent umbracoContent) :
            base(umbracoContent, null)
        {
        }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <returns></returns>
        public JobSearchResultsViewModel BuildModel()
        {
            var model = new JobSearchResultsViewModel()
            {
                JobsLogo = BuildImage("JobsLogo_Content"),
                HeaderBackgroundImage = BuildImage("HeaderBackgroundImage_Content"),
                JobsHomePage = BuildLinkToPage("JobsHomePage_Content"),
                JobDetailPage = BuildLinkToPage("JobDetailPage_Content"),
                LoginPage = BuildLinkToPage("LoginPage_Content"),
                JobAlertsPage = BuildLinkToPage("JobAlertsPage_Content"),
                JobsSearchPage = BuildLinkToPage("JobsSearchPage_Content"),
                ExamineSearcher = BuildSearcherName("PublicOrRedeployment_Content")
            };

            return model;
        }

        private string BuildSearcherName(string fieldAlias)
        {
            var index = umbraco.library.GetPreValueAsString(UmbracoContent.GetPropertyValue<int>(fieldAlias));
            if (index == "Redeployment jobs")
            {
                return "RedeploymentJobsSearcher";
            }
            else return "PublicJobsSearcher";
        }
    }
}