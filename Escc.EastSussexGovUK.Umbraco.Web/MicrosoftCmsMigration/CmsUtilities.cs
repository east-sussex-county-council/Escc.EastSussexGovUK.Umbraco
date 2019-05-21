using System;
using System.Text;
using System.Text.RegularExpressions;
using Escc.EastSussexGovUK.Umbraco.Web.MicrosoftCmsMigration.Placeholders;
using Humanizer;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Web.MicrosoftCmsMigration
{
    /// <summary>
    /// Utility methods for working with Umbraco which are ported from Microsoft CMS. Do not use for new templates.
    /// </summary>
    public static class CmsUtilities
    {
        /// <summary>
        /// Gets the value of an Umbraco property which used to be a Microsoft CMS custom property
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static CustomProperty GetCustomProperty(string propertyName)
        {
            if (UmbracoContext.Current == null || !UmbracoContext.Current.PageId.HasValue) return null;

            var content = UmbracoContext.Current.ContentCache.GetById(UmbracoContext.Current.PageId.Value);
            if (content == null) return null;

            return new CustomProperty() { Value = content.GetPropertyValue<string>(propertyName) };

        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings")]
        public static string CorrectPublishedUrl(string cmsPath)
        {
            if (cmsPath == null) return null;

            if (Regex.IsMatch(cmsPath, "^http://[a-z]+/", RegexOptions.IgnoreCase)) cmsPath = cmsPath.Substring(6); // turn "http://yourcouncil/" into "/yourcouncil"
            if (Regex.IsMatch(cmsPath, "^https://[a-z]+/", RegexOptions.IgnoreCase)) cmsPath = cmsPath.Substring(7); // turn "https://yourcouncil/" into "/yourcouncil"
            return cmsPath;
        }

        /// <summary>
        /// Where an entire placeholder should be an unordered list of links, this prevents the author from having to remember.
        /// </summary>
        /// <param name="html">XHTML to be converted to an unordered list. Elements should already be converted to lowercase.</param>
        /// <param name="listClass">The list class.</param>
        /// <returns>Modifed XHTML</returns>
        public static string ShouldBeUnorderedList(string html, string listClass = "")
        {
            MatchCollection matches = Regex.Matches(html, "(?<tag><a [^>]*>)(?<linktext>.*?)</a>", RegexOptions.Singleline & RegexOptions.IgnoreCase);
            if (matches.Count == 0) return String.Empty; // we want a list of links so, no links, no list

            StringBuilder list = new StringBuilder("<ul");
            if (!String.IsNullOrEmpty(listClass)) list.Append(" class=\"").Append(listClass).Append("\"");
            list.Append(">");
            list.Append(Environment.NewLine);

            foreach (Match m in matches)
            {
                list.Append("<li>").Append(m.Groups["tag"].Value).Append(m.Groups["linktext"].Value.Transform(To.SentenceCase)).Append("</a></li>").Append(Environment.NewLine);
            }

            list.Append("</ul>");
            return list.ToString();
        }

        /// <summary>
        /// Shortcut to getting the placeholders of the current posting
        /// </summary>
        public static PlaceholderCollection Placeholders
        {
            get
            {
                var collection = new PlaceholderCollection();

                if (UmbracoContext.Current == null || !UmbracoContext.Current.PageId.HasValue) return collection;

                var content = UmbracoContext.Current.ContentCache.GetById(UmbracoContext.Current.PageId.Value);
                if (content == null) return collection;

                foreach (var publishedProperty in content.Properties)
                {
                    var placeholder = new Placeholder();
                    placeholder.Value = publishedProperty.Value;
                    var tabNameStarts = publishedProperty.PropertyTypeAlias.LastIndexOf("_", StringComparison.Ordinal);
                    var aliasWithoutTab = (tabNameStarts > -1) ? publishedProperty.PropertyTypeAlias.Substring(0,tabNameStarts) : publishedProperty.PropertyTypeAlias;
                    collection.Add(aliasWithoutTab, placeholder);
                }
                
                return collection;
            }
        }
    }
}