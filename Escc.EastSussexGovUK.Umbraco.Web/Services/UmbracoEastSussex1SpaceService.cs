﻿using System;
using Escc.EastSussexGovUK.Features;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Web.Services
{
    /// <summary>
    /// Service to control use of the EastSussex1Space search
    /// </summary>
    public class UmbracoEastSussex1SpaceService : IEastSussex1SpaceService
    {
        private IPublishedContent content;

        /// <summary>
        /// Creates a new instance of <see cref="UmbracoEastSussex1SpaceService"/>
        /// </summary>
        /// <param name="content"></param>
        public UmbracoEastSussex1SpaceService(IPublishedContent content)
        {
            this.content = content;
        }

        /// <summary>
        /// Determines whether to show the EastSussex1Space search based on a setting from the current page and its parents
        /// </summary>
        /// <returns></returns>
        public bool ShowSearch()
        {
            return ShowSearchBasedOnUmbracoContent(this.content);
        }

        private static bool ShowSearchBasedOnUmbracoContent(IPublishedContent contentNode)
        {
            // This is a recursive method which can reach the top of the content tree. If that happens, assume false.
            if (contentNode == null) return false;

            // Get the selected value. If none set, use INHERIT.
            var oneSpace = umbraco.library.GetPreValueAsString(contentNode.GetPropertyValue<int>("eastsussex1Space_Social_media_and_promotion"));
            oneSpace = String.IsNullOrEmpty(oneSpace) ? "INHERIT" : oneSpace.ToUpperInvariant();

            // If set to inherit, look to the parent page for the setting.
            if (oneSpace == "INHERIT") return ShowSearchBasedOnUmbracoContent(contentNode.Parent);

            // Otherwise take the setting from this page
            return (oneSpace == "SHOW");
        }
    }
}