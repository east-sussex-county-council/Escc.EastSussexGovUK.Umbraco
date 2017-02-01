using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.HomePage
{
    /// <summary>
    /// Analytics tab for the home page items document type, used on its view as an RSS feed
    /// </summary>
    /// <seealso cref="Umbraco.Inception.BL.TabBase" />
    public class HomePageItemsAnalyticsTab: TabBase
    {
        [UmbracoProperty("Google Analytics campaign tracking utm_source", "CampaignTrackingSource", BuiltInUmbracoDataTypes.Textbox, sortOrder: 0)]
        public string CampaignTrackingSource { get; set; }

        [UmbracoProperty("Google Analytics campaign tracking utm_medium", "CampaignTrackingMedium", BuiltInUmbracoDataTypes.Textbox, sortOrder: 1)]
        public string CampaignTrackingMedium { get; set; }

        [UmbracoProperty("Google Analytics campaign tracking utm_campaign", "CampaignTrackingCampaign", BuiltInUmbracoDataTypes.Textbox, sortOrder: 2)]
        public string CampaignTrackingCampaign { get; set; }

        [UmbracoProperty("Google Analytics campaign tracking utm_content", "CampaignTrackingContent", BuiltInUmbracoDataTypes.Textbox, sortOrder: 3)]
        public string CampaignTrackingContent { get; set; }

        [UmbracoProperty("Filter domains to apply Google Analytics campaign tracking to", "CampaignTrackingRegex", BuiltInUmbracoDataTypes.Textbox, sortOrder: 4,
            description: @"Specify a regular expression which domains must match before campaign tracking is applied, eg ^www\.example\.org$")]
        public string CampaignTrackingRegex { get; set; }
    }
}