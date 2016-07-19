using System.Collections.Generic;
using System.Linq;
using EsccWebTeam.EastSussexGovUK.MasterPages.Controls;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Views.Layouts
{
    /// <summary>
    /// Gets data about the current context from the Umbraco content tree
    /// </summary>
    public class UmbracoBreadcrumbProvider : IBreadcrumbProvider
    {
        /// <summary>
        /// Gets data about the current context from the Umbraco content tree
        /// </summary>
        public IDictionary<string, string> BuildTrail()
        {
            var breadcrumb = new Dictionary<string, string>();
            var umbraco = new UmbracoHelper(UmbracoContext.Current);
            var isTopLevelSection = false;

            // Start with the parent node of the current node, which is effectively its folder,
            // unless we're only one folder deep because then we want to have the main section 
            // highlighted.
            var node = umbraco.TypedContent(UmbracoContext.Current.PageId);
            if (node.Parent != null && node.Parent.Parent == null)
            {
                isTopLevelSection = true;
            }
            else
            {
                node = node.Parent;
            }

            // Add data for each ancestor node in the hierarchy
            while (node != null)
            {
                breadcrumb.Add(node.Name,  isTopLevelSection ? string.Empty : umbraco.NiceUrlWithDomain(node.Id));
                node = node.Parent;
                if (isTopLevelSection) isTopLevelSection = false;
            }

            // Reverse it so we start from the root, and return
            var reversedKeys = breadcrumb.Keys.Reverse();
            return reversedKeys.ToDictionary(key => key, key => breadcrumb[key]);
        }
    }
}