using System;
using System.Collections.Generic;
using Escc.EastSussexGovUK.Features;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.EastSussexGovUK.Umbraco.Services;
using Escc.Umbraco.ContentExperiments;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Core.Models;
using Umbraco.Web;
using Escc.EastSussexGovUK.Umbraco.UrlTransformers;
using Escc.EastSussexGovUK.Umbraco.Ratings;
using Escc.EastSussexGovUK.Umbraco.Skins;

namespace Escc.EastSussexGovUK.Umbraco.Location
{
    /// <summary>
    /// Controller for the 'Recycling Site' document type, which adds extra properties to the <see cref="LocationViewModel"/> returned by <see cref="LocationController"/>
    /// </summary>
    public class RecyclingSiteController : LocationController
    {
        protected override LocationViewModel MapUmbracoContentToViewModel(IPublishedContent content, ILatestService latestService, ISocialMediaService socialMediaService, IEastSussex1SpaceService eastSussex1SpaceService, IWebChatSettingsService webChatSettingsService, IRelatedLinksService relatedLinksService, IContentExperimentSettingsService contentExperimentSettingsService, IEscisService escisService, IRatingSettingsProvider ratingSettings, IMediaUrlTransformer mediaUrlTransformer, ISkinToApplyService skinService)
        {
            var model = base.MapUmbracoContentToViewModel(content, latestService, socialMediaService, eastSussex1SpaceService, webChatSettingsService, relatedLinksService, contentExperimentSettingsService, escisService, ratingSettings, mediaUrlTransformer, skinService);

            // Get the types of waste which have been selected for this recycling site
            var recycledTypes = ReadWasteTypesFromProperty(content, "wasteTypes_Content");
            if (recycledTypes != null)
            {
                ((List<string>) model.WasteTypesRecycled).AddRange(recycledTypes);
            }

            var acceptedTypes = ReadWasteTypesFromProperty(content, "acceptedWasteTypes_Content");
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

        private static IEnumerable<string> ReadWasteTypesFromProperty(IPublishedContent content, string alias)
        {
            var acceptedWasteTypes = content.GetPropertyValue<string>(alias);
            if (!String.IsNullOrEmpty(acceptedWasteTypes))
            {
                return acceptedWasteTypes.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
            }
            return null;
        }
    }
}