using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using Escc.EastSussexGovUK.Features;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Web.Services
{
    /// <summary>
    /// Service for determining which social media widgets to display
    /// </summary>
    public class UmbracoSocialMediaService : ISocialMediaService
    {
        private IPublishedContent content;

        /// <summary>
        /// Creates a new instance of <see cref="UmbracoSocialMediaService"/>
        /// </summary>
        /// <param name="content">The content.</param>
        public UmbracoSocialMediaService(IPublishedContent content)
        {
            this.content = content;
        }

        /// <summary>
        /// Reads properties from the Umbraco document type to determine which social media widgets to display
        /// </summary>
        /// <returns></returns>
        public SocialMediaSettings ReadSocialMediaSettings()
        {
            var model = new SocialMediaSettings();
            if (this.content == null) return model;

            AddFacebookPropertiesToModelFromUmbracoContent(this.content, model);
            AddTwitterPropertiesToModelFromUmbracoContent(this.content, model);
            SetSocialMediaOrderFromUmbracoContent(this.content, model);

            return model;
        }

        /// <summary>
        /// Gets the order to display social media widgets in
        /// </summary>
        /// <param name="content"></param>
        /// <param name="model"></param>
        private static void SetSocialMediaOrderFromUmbracoContent(IPublishedContent content, SocialMediaSettings model)
        {
            var selectedOption = umbraco.library.GetPreValueAsString(content.GetPropertyValue<int>("socialMediaOrder_Social_media_and_promotion"));
            if (!String.IsNullOrEmpty(selectedOption))
            {
                var list = new List<string>();
                list.AddRange(selectedOption.ToUpperInvariant().Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries));
                model.SocialMediaOrder = list;
            }
            else
            {
                model.SocialMediaOrder = new List<string>() { "TWITTER", "FACEBOOK" };
            }
        }

        /// <summary>
        /// Recursive method to get Twitter widget script from current page or inherited from a parent page
        /// </summary>
        /// <param name="content"></param>
        /// <param name="model"></param>
        private static void AddTwitterPropertiesToModelFromUmbracoContent(IPublishedContent content, SocialMediaSettings model)
        {
            if (content == null) return;
            if (InheritSocialMedia(content, model, "twitterInherit_Social_media_and_promotion", AddTwitterPropertiesToModelFromUmbracoContent)) return;

            // Read Twitter options from current page
            model.TwitterAccount = content.GetPropertyValue<string>("twitterAccount_Social_media_and_promotion");

            // Normalise account name
            model.TwitterAccount = model.TwitterAccount.TrimStart('@');
        }

        /// <summary>
        /// Recursive method to get Facebook Like Box settings from current page or inherited from a parent page
        /// </summary>
        /// <param name="content"></param>
        /// <param name="model"></param>
        private static void AddFacebookPropertiesToModelFromUmbracoContent(IPublishedContent content, SocialMediaSettings model)
        {
            if (content == null) return;
            if (InheritSocialMedia(content, model, "facebookInherit_Social_media_and_promotion", AddFacebookPropertiesToModelFromUmbracoContent)) return;

            // Read Facebook options from current page
            try
            {
                var facebook = content.GetPropertyValue<string>("facebookPageUrl_Social_media_and_promotion");
                model.FacebookPageUrl = String.IsNullOrWhiteSpace(facebook) ? null : new Uri(facebook);
            }
            catch (UriFormatException)
            {
            }

            var options = new List<string>(content.GetPropertyValue<IEnumerable<string>>("facebookWidgetSettings_Social_media_and_promotion"));
            for (var i = 0; i < options.Count; i++) options[i] = options[i].ToUpperInvariant();
            model.FacebookShowFaces = options.Contains("SHOW FACES");
            model.FacebookShowFeed = options.Contains("SHOW FEED");
        }

        /// <summary>
        /// Inherit social media from parent pages if set to inherit
        /// </summary>
        /// <param name="content">Page to read data from</param>
        /// <param name="model">Model to build up</param>
        /// <param name="inheritPropertyAlias">Property alias of the 'inherit' checkbox</param>
        /// <param name="propertyReader">Recursive method to read the relevant properties into the model</param>
        /// <returns></returns>
        private static bool InheritSocialMedia(IPublishedContent content, SocialMediaSettings model, string inheritPropertyAlias, Action<IPublishedContent, SocialMediaSettings> propertyReader)
        {
            if (content == null) throw new ArgumentNullException("content");

            var inherit = (!content.HasProperty(inheritPropertyAlias) || content.GetPropertyValue<bool>(inheritPropertyAlias));

            if (inherit)
            {
                propertyReader(content.Parent, model);
                return true;
            }
            return false;
        }
    }
}