
using System;
using Escc.EastSussexGovUK.Umbraco.Services;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    public class JobsHomeViewModelFromUmbraco : BaseViewModelFromUmbracoBuilder, IViewModelBuilder<JobsHomeViewModel>
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="JobsHomeViewModelFromUmbraco" /> class.
        /// </summary>
        /// <param name="umbracoContent">Content from Umbraco using the 'Jobs' document type.</param>
        /// <param name="relatedLinksService">The related links service.</param>
        /// <exception cref="System.ArgumentNullException">umbracoContent
        /// or
        /// mediaUrlTransformer</exception>
        public JobsHomeViewModelFromUmbraco(IPublishedContent umbracoContent, IRelatedLinksService relatedLinksService) :
            base(umbracoContent, relatedLinksService)
        {
        }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public JobsHomeViewModel BuildModel()
        {
            var model = new JobsHomeViewModel
            {
                JobsLogo = BuildImage("JobsLogo_Content"),
                HeaderBackgroundImageSmall = BuildImage("HeaderBackgroundImage_Content"),
                HeaderBackgroundImageMedium = BuildImage("HeaderBackgroundImageMedium_Content"),
                HeaderBackgroundImageLarge = BuildImage("HeaderBackgroundImageLarge_Content"),
                HeaderBackgroundImageCaption = UmbracoContent.GetPropertyValue<string>("HeaderBackgroundImageCaption_Content"),
                LoginPage = BuildUri("LoginPage_Content"),
                SearchPage = BuildUri("SearchPage_Content"),
                SearchResultsPage = BuildUri("SearchResultsPage_Content"),
                TileNavigation = RelatedLinksService.BuildRelatedLinksViewModelFromUmbracoContent(UmbracoContent, "TileNavigation_Content"),
                TileImages = BuildImages("TileImages_Content"),
                CampaignImage = BuildImage("CampaignImage_Content"),
                CampaignPage = BuildUri("CampaignPage_Content")
            };

            if (model.CampaignPage == null)
            {
                var campaignUrl = UmbracoContent.GetPropertyValue<string>("CampaignUrl_Content");
                if (!String.IsNullOrEmpty(campaignUrl))
                {
                    model.CampaignPage = new HtmlLink() { Url = new Uri(campaignUrl, UriKind.RelativeOrAbsolute) };
                }
            }

            // Allow a hyphen to indicate that there's no text for the link, just an image
            foreach (var link in model.TileNavigation)
            {
                if (link.Text == "-") link.Text = String.Empty;
            }

            return model;
        }

       
        private HtmlLink BuildUri(string alias)
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