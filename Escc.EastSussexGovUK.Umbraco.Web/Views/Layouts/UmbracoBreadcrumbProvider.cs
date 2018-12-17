using System.Collections.Generic;
using System.Linq;
using Escc.EastSussexGovUK.Features;
using Umbraco.Web;
using System;
using Exceptionless;

namespace Escc.EastSussexGovUK.Umbraco.Web.Views.Layouts
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
            if (UmbracoContext.Current.PageId.HasValue)
            {
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
                    try
                    {
                        // If there are child pages with the same name as the parent, keep the breadcrumb trail tidy by skipping the child
                        if (!breadcrumb.ContainsKey(node.Name))
                        {
                            breadcrumb.Add(node.Name, isTopLevelSection ? string.Empty : umbraco.NiceUrlWithDomain(node.Id));
                        }
                    }
                    catch (ArgumentException ex)
                    {
                        // Report it, but we don't want to page to crash due to an error in the breadcrumb
                        ex.Data.Add("Adding node name", node.Name);
                        ex.Data.Add("Adding node URL", isTopLevelSection ? string.Empty : umbraco.NiceUrlWithDomain(node.Id));
                        var keyCount = 1;
                        foreach (var key in breadcrumb.Keys)
                        {
                            ex.Data.Add($"Already added node {keyCount} name", key);
                            ex.Data.Add($"Already added node {keyCount} URL", breadcrumb[key]);
                            keyCount++;
                        }
                        ex.ToExceptionless().Submit();
                    }
                    node = node.Parent;
                    if (isTopLevelSection) isTopLevelSection = false;
                }
            }

            // Reverse it so we start from the root, and return
            var reversedKeys = breadcrumb.Keys.Reverse();
            return reversedKeys.ToDictionary(key => key, key => breadcrumb[key]);
        }
    }
}