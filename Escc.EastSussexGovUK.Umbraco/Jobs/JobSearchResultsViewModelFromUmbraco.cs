using System;
using AST.AzureBlobStorage.Helper;
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
        /// Initializes a new instance of the <see cref="JobSearchResultsViewModelFromUmbraco" /> class.
        /// </summary>
        /// <param name="umbracoContent">Content from Umbraco using the 'Jobs' document type.</param>
        /// <param name="mediaUrlTransformer">The media URL transformer.</param>
        /// <param name="relatedLinksService">The related links service.</param>
        /// <exception cref="System.ArgumentNullException">umbracoContent
        /// or
        /// mediaUrlTransformer</exception>
        public JobSearchResultsViewModelFromUmbraco(IPublishedContent umbracoContent, AzureMediaUrlTransformer mediaUrlTransformer, IRelatedLinksService relatedLinksService) :
            base(umbracoContent, mediaUrlTransformer, relatedLinksService)
        {
        }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public JobSearchResultsViewModel BuildModel()
        {
            var model = new JobSearchResultsViewModel()
            {
                JobsLogo = BuildImage("JobsLogo_Content"),
                HeaderBackgroundImage = BuildImage("HeaderBackgroundImage_Content"),
                LoginPage = BuildLinkToPage("LoginPage_Content"),
                JobAlertsPage = BuildLinkToPage("JobAlertsPage_Content"),
                ResultsUrl = BuildUri("ResultsScriptUrl_Content"),
                ButtonNavigation = RelatedLinksService.BuildRelatedLinksViewModelFromUmbracoContent(UmbracoContent, "ButtonNavigation_Content"),
                ButtonImages = BuildImages("ButtonImages_Content")
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

        private HtmlLink BuildLinkToPage(string alias)
        {
            var linkedPage = UmbracoContent.GetPropertyValue<IPublishedContent>(alias);
            if (linkedPage != null)
            {
                return new HtmlLink()
                {
                    Text = linkedPage.Name,
                    Url = new Uri(linkedPage.UrlAbsolute())
                };
            }
            return null;
        }
    }
}