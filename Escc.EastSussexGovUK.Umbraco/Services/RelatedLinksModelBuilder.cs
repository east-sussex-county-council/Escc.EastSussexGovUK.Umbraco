using System.Collections.Generic;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.Umbraco.PropertyTypes;

namespace Escc.EastSussexGovUK.Umbraco.Services
{
    /// <summary>
    /// Builds related links into a series of headings and groups of links
    /// </summary>
    public class RelatedLinksModelBuilder
    {
        /// <summary>
        /// Organises related links as headings and sections, treating a link with no URL as a heading
        /// </summary>
        /// <param name="relatedLinks">The related links.</param>
        /// <returns></returns>
        public IList<LandingSectionViewModel> OrganiseAsHeadingsAndSections(IList<HtmlLink> relatedLinks)
        {
            var groups = new List<LandingSectionViewModel>();
            var relatedLinksGroup = new LandingSectionViewModel();
            foreach (var relatedLink in relatedLinks)
            {
                if (relatedLink.Url == null)
                {
                    if (groups.Count == 0 && relatedLinksGroup.Links == null)
                    {
                        relatedLinksGroup.Heading = relatedLink;
                    }
                    else
                    {
                        groups.Add(relatedLinksGroup);
                        relatedLinksGroup = new LandingSectionViewModel();
                        relatedLinksGroup.Heading = relatedLink;
                    }
                }
                else
                {
                    if (relatedLinksGroup.Links == null)
                    {
                        relatedLinksGroup.Links = new List<HtmlLink>();
                    }
                    relatedLinksGroup.Links.Add(relatedLink);
                }
            }
            if (relatedLinksGroup.Links != null)
            {
                groups.Add(relatedLinksGroup);
            }
            return groups;
        }
    }
}