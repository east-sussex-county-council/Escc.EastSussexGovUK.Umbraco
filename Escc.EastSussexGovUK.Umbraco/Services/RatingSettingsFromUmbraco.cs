using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Escc.EastSussexGovUK.Umbraco.Models;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Services
{
    /// <summary>
    /// Reads settings for a page rating service from the Umbraco page
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Services.IRatingSettingsProvider" />
    public class RatingSettingsFromUmbraco : IRatingSettingsProvider
    {
        private readonly IPublishedContent _umbracoContent;

        /// <summary>
        /// Initializes a new instance of the <see cref="RatingSettingsFromUmbraco"/> class.
        /// </summary>
        /// <param name="umbracoContent">Content of the Umbraco page.</param>
        /// <exception cref="ArgumentNullException">umbracoContent</exception>
        public RatingSettingsFromUmbraco(IPublishedContent umbracoContent)
        {
            if (umbracoContent == null) throw new ArgumentNullException(nameof(umbracoContent));
            _umbracoContent = umbracoContent;
        }

        /// <summary>
        /// Reads the rating settings.
        /// </summary>
        /// <returns></returns>
        public RatingSettings ReadRatingSettings()
        {
            var rating = new RatingSettings();
            try
            {
                var poor = _umbracoContent.GetPropertyValue<string>("RatingUrlPoor_Social_media_and_promotion");
                rating.PoorUrl = String.IsNullOrWhiteSpace(poor) ? null : new Uri(poor);

                var adequate = _umbracoContent.GetPropertyValue<string>("RatingUrlAdequate_Social_media_and_promotion");
                rating.AdequateUrl = String.IsNullOrWhiteSpace(adequate) ? null : new Uri(adequate);

                var good = _umbracoContent.GetPropertyValue<string>("RatingUrlGood_Social_media_and_promotion");
                rating.GoodUrl = String.IsNullOrWhiteSpace(good) ? null : new Uri(good);

                var excellent = _umbracoContent.GetPropertyValue<string>("RatingUrlExcellent_Social_media_and_promotion");
                rating.ExcellentUrl = String.IsNullOrWhiteSpace(excellent) ? null : new Uri(excellent);
            }
            catch (UriFormatException)
            {
            }

            return rating;
        }
    }
}