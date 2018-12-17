using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Escc.EastSussexGovUK.Umbraco.Web.Models;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Core;

namespace Escc.EastSussexGovUK.Umbraco.Web.Ratings
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
                var relationToRating = ApplicationContext.Current.Services.RelationService.GetByChildId(_umbracoContent.Id).FirstOrDefault(r => r.RelationType.Alias == RatingRelationType.RelationTypeAlias);
                if (relationToRating == null) return null;

                var settingsPage = UmbracoContext.Current.ContentCache.GetById(relationToRating.ParentId);
                if (settingsPage == null) return null;

                var poor = settingsPage.GetPropertyValue<string>("RatingUrlPoor_Content");
                rating.PoorUrl = String.IsNullOrWhiteSpace(poor) ? null : new Uri(poor);

                var adequate = settingsPage.GetPropertyValue<string>("RatingUrlAdequate_Content");
                rating.AdequateUrl = String.IsNullOrWhiteSpace(adequate) ? null : new Uri(adequate);

                var good = settingsPage.GetPropertyValue<string>("RatingUrlGood_Content");
                rating.GoodUrl = String.IsNullOrWhiteSpace(good) ? null : new Uri(good);

                var excellent = settingsPage.GetPropertyValue<string>("RatingUrlExcellent_Content");
                rating.ExcellentUrl = String.IsNullOrWhiteSpace(excellent) ? null : new Uri(excellent);
            }
            catch (UriFormatException)
            {
            }

            return rating;
        }
    }
}