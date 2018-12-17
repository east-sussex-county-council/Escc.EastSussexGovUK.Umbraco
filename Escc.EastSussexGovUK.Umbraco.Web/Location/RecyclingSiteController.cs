using System;
using System.Collections.Generic;
using Escc.EastSussexGovUK.Features;
using latest = Escc.EastSussexGovUK.Umbraco.Web.Latest;
using Escc.Umbraco.ContentExperiments;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Core.Models;
using Umbraco.Web;
using Escc.EastSussexGovUK.Umbraco.UrlTransformers;
using Escc.EastSussexGovUK.Umbraco.Web.Ratings;
using Escc.EastSussexGovUK.Umbraco.Web.Skins;

namespace Escc.EastSussexGovUK.Umbraco.Web.Location
{
    /// <summary>
    /// Controller for the 'Recycling Site' document type, which adds extra properties to the <see cref="LocationViewModel"/> returned by <see cref="LocationController"/>
    /// </summary>
    public class RecyclingSiteController : LocationController
    {
        protected override LocationViewModel MapUmbracoContentToViewModel(IPublishedContent content, DateTime? expiryDate, latest.ILatestService latestService, ISocialMediaService socialMediaService, IEastSussex1SpaceService eastSussex1SpaceService, IWebChatSettingsService webChatSettingsService, IRelatedLinksService relatedLinksService, IContentExperimentSettingsService contentExperimentSettingsService, IEscisService escisService, IRatingSettingsProvider ratingSettings, IMediaUrlTransformer mediaUrlTransformer, ISkinToApplyService skinService)
        {
            var model = base.MapUmbracoContentToViewModel(content, expiryDate, latestService, socialMediaService, eastSussex1SpaceService, webChatSettingsService, relatedLinksService, contentExperimentSettingsService, escisService, ratingSettings, mediaUrlTransformer, skinService);

            // Get the types of waste which have been selected for this recycling site
            var recycledTypes = content.GetPropertyValue<IEnumerable<string>>("wasteTypes_Content");
            if (recycledTypes != null)
            {
                ((List<string>) model.WasteTypesRecycled).AddRange(recycledTypes);
            }

            var acceptedTypes = content.GetPropertyValue<IEnumerable<string>>("acceptedWasteTypes_Content");
            if (acceptedTypes != null)
            {
                ((List<string>) model.WasteTypesAccepted).AddRange(acceptedTypes);
            }

            // Get the authority responsible for this site
            var preValueId = content.GetPropertyValue<int>("responsibleAuthority_Content");
            if (preValueId > 0)
            {
                model.ResponsibleAuthority = umbraco.library.GetPreValueAsString(preValueId);
            }

            return model;
        }
    }
}