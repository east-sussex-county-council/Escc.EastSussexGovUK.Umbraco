using System;
using Escc.EastSussexGovUK.Umbraco.Web.Services;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Web.Jobs
{
    /// <summary>
    /// Builds an instance of <see cref="JobSearchResultsViewModel"/> from an Umbraco page based on <see cref="JobSearchResultsDocumentType"/>
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Services.BaseViewModelFromUmbracoBuilder" />
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Services.IViewModelBuilder{Escc.EastSussexGovUK.Umbraco.Jobs.JobSearchResultsViewModel}" />
    public class JobSearchResultsViewModelFromUmbraco : BaseViewModelFromUmbracoBuilder, IViewModelBuilder<JobSearchResultsViewModel>
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
                HeaderBackgroundImageSmall = BuildImage("HeaderBackgroundImage_Content"),
                HeaderBackgroundImageMedium = BuildImage("HeaderBackgroundImageMedium_Content"),
                HeaderBackgroundImageLarge = BuildImage("HeaderBackgroundImageLarge_Content"),
                JobsHomePage = BuildLinkToPage("JobsHomePage_Content"),
                JobAdvertPage = BuildLinkToPage("JobDetailPage_Content"),
                LoginUrl = !string.IsNullOrEmpty(UmbracoContent.GetPropertyValue<string>("loginURL")) ? new Uri(UmbracoContent.GetPropertyValue<string>("loginURL"), UriKind.RelativeOrAbsolute) : null,
                JobsSearchPage = BuildLinkToPage("JobsSearchPage_Content"),
                JobsPrivacyPage = BuildLinkToPage("JobsPrivacyPage_Content"),
                JobsSet = ParseJobsSet("PublicOrRedeployment_Content")
            };

            var rss = BuildLinkToPage("JobsRssFeed_Content");
            if (rss != null)
            {
                model.RssFeedTitle = rss.Text;
                model.RssFeedUrl = rss.Url;
            }

            return model;
        }
    }
}