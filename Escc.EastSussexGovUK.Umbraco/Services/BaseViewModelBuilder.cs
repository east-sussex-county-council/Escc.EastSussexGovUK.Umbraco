using System;
using System.Globalization;
using Escc.Dates;
using Escc.EastSussexGovUK.Features;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.EastSussexGovUK.Umbraco.Views.Layouts;
using Escc.Umbraco.ContentExperiments;
using Umbraco.Core.Models;
using Umbraco.Web;
using Escc.EastSussexGovUK.Umbraco.Ratings;
using Escc.EastSussexGovUK.Umbraco.Skins;
using Escc.Umbraco.Caching;

namespace Escc.EastSussexGovUK.Umbraco.Services
{
    /// <summary>
    /// Helper service to build the common properties of an Umbraco page model
    /// </summary>
    public class BaseViewModelBuilder
    {
        /// <summary>
        /// Populates the properties of <see cref="BaseViewModel" />
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="content">The content.</param>
        /// <param name="contentExperimentSettingsService">The content experiment settings service.</param>
        /// <param name="expiryDate">The expiry date of the page.</param>
        /// <param name="inUmbracoPreviewMode">if set to <c>true</c> [in umbraco preview mode].</param>
        /// <param name="skinService">The skin service.</param>
        /// <exception cref="ArgumentNullException">model
        /// or
        /// content</exception>
        /// <exception cref="System.ArgumentNullException">model
        /// or
        /// content</exception>
        public void PopulateBaseViewModel(BaseViewModel model, IPublishedContent content, IContentExperimentSettingsService contentExperimentSettingsService, DateTime? expiryDate, bool inUmbracoPreviewMode, ISkinToApplyService skinService=null)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (content == null) throw new ArgumentNullException(nameof(content));

            model.BreadcrumbProvider = new UmbracoBreadcrumbProvider();

            if (String.IsNullOrEmpty(model.Metadata.Title))
            {
                model.Metadata.Title = content.Name;
                model.Metadata.Title = new RemoveUmbracoNumericSuffixFilter().Apply(model.Metadata.Title);
            }
            model.Metadata.PageUrl = new Uri(content.UrlAbsolute());
            if (String.IsNullOrEmpty(model.Metadata.Description))
            {
                model.Metadata.Description = content.GetPropertyValue<string>("pageDescription") ?? String.Empty;
            }
            model.PageType = content.DocumentTypeAlias;
            model.Metadata.SystemId = content.Id.ToString(CultureInfo.InvariantCulture);
            model.Metadata.DateCreated = content.CreateDate.ToIso8601Date();
            model.Metadata.DateModified = content.UpdateDate.ToIso8601Date();

            if (expiryDate.HasValue && expiryDate != DateTime.MinValue && expiryDate != DateTime.MaxValue)
            {
                model.Metadata.DateReview = expiryDate.ToIso8601Date();
            }

            model.IsPublicView = !inUmbracoPreviewMode && model.Metadata.PageUrl.Host.ToUpperInvariant() != "LOCALHOST";
            if (contentExperimentSettingsService != null) { model.ContentExperimentPageSettings = contentExperimentSettingsService.LookupSettingsForPage(content.Id); }

            if (skinService != null)
            {
                model.SkinToApply = skinService.LookupSkinForPage(content);
            }
        }

        /// <summary>
        /// Populates the properties of <see cref="BaseViewModel" /> which can be inherited from parent nodes
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="latestService">The latest service.</param>
        /// <param name="socialMediaService">The social media service.</param>
        /// <param name="eastSussex1SpaceService">The east sussex1 space service.</param>
        /// <param name="webChatSettingsService">The web chat settings service.</param>
        /// <param name="escisService">The ESCIS service.</param>
        /// <exception cref="System.ArgumentNullException">model</exception>
        public void PopulateBaseViewModelWithInheritedContent(BaseViewModel model, ILatestService latestService, ISocialMediaService socialMediaService, IEastSussex1SpaceService eastSussex1SpaceService, IWebChatSettingsService webChatSettingsService, IEscisService escisService, IRatingSettingsProvider ratingSettings=null)
        {
            if (model == null) throw new ArgumentNullException("model");
            
            if (latestService != null) model.Latest = latestService.ReadLatestSettings().LatestHtml;
            if (eastSussex1SpaceService != null) model.ShowEastSussex1SpaceWidget = eastSussex1SpaceService.ShowSearch();
            if (escisService != null) model.ShowEscisWidget = escisService.ShowSearch();
            if (socialMediaService != null) model.SocialMedia = socialMediaService.ReadSocialMediaSettings();
            if (webChatSettingsService!= null) model.WebChat = webChatSettingsService.ReadWebChatSettings();
            if (ratingSettings != null) model.RatingSettings = ratingSettings.ReadRatingSettings();
        }
    }
}