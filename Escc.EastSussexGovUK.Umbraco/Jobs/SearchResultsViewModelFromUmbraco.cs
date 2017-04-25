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
                HeaderBackgroundImageSmall = BuildImage("HeaderBackgroundImage_Content"),
                HeaderBackgroundImageMedium = BuildImage("HeaderBackgroundImageMedium_Content"),
                HeaderBackgroundImageLarge = BuildImage("HeaderBackgroundImageLarge_Content"),
                JobsHomePage = BuildLinkToPage("JobsHomePage_Content"),
                JobAdvertPage = BuildLinkToPage("JobDetailPage_Content"),
                LoginPage = BuildLinkToPage("LoginPage_Content"),
                JobAlertsPage = BuildLinkToPage("JobAlertsPage_Content"),
                JobsSearchPage = BuildLinkToPage("JobsSearchPage_Content"),
                ExamineSearcher = BuildJobsSearcherName("PublicOrRedeployment_Content"),
                ExamineLookupValuesSearcher = BuildLookupValuesSearcherName("PublicOrRedeployment_Content")
            };

            var rss = BuildLinkToPage("JobsRssFeed_Content");
            if (rss != null)
            {
                model.Metadata.RssFeedTitle = rss.Text;
                model.Metadata.RssFeedUrl = rss.Url.ToString();
            }

            return model;
        }
    }
}