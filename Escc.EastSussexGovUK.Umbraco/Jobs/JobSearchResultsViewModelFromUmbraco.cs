using System;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.EastSussexGovUK.Umbraco.Services;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    public class JobSearchResultsViewModelFromUmbraco : BaseJobsViewModelFromUmbracoBuilder, IViewModelBuilder<JobSearchResultsViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobSearchResultsViewModel" /> class.
        /// </summary>
        /// <param name="umbracoContent">Content from Umbraco using the 'Jobs' document type.</param>
        /// <exception cref="System.ArgumentNullException">umbracoContent
        /// or
        /// mediaUrlTransformer</exception>
        public JobSearchResultsViewModelFromUmbraco(IPublishedContent umbracoContent) :
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
                ResultsUrl = BuildUri("ResultsScriptUrl_Content")
            };

            return model;
        }

        private TalentLinkUrl BuildUri(string alias)
        {
            var url = UmbracoContent.GetPropertyValue<string>(alias);
            if (!String.IsNullOrEmpty(url))
            {
                return new TalentLinkUrl(url);
            }
            return null;
        }
    }
}