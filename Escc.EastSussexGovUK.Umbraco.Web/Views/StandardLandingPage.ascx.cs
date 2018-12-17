using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Escc.EastSussexGovUK.Umbraco.Web.MicrosoftCmsMigration;
using Escc.EastSussexGovUK.Umbraco.Web.MicrosoftCmsMigration.Placeholders;
using Exceptionless;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Web.Views
{
    public partial class StandardLandingPage : ViewUserControl<MicrosoftCmsViewModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Only show placeholders if they're in use
            HideUnusedSections();
            var visibleSections = GetVisibleSections();

            // Add classes for sorting into two or three columns
            AddClassesForColumns(visibleSections);

            // Format list of links
            var descriptionsProp = CmsUtilities.GetCustomProperty("Descriptions");
            var descriptionsValue = (descriptionsProp != null) ? descriptionsProp.Value : String.Empty;
            var page = UmbracoContext.Current.ContentCache.GetById(UmbracoContext.Current.PageId.Value);
            var useLinks = DisplayAsListsOfLinks(descriptionsValue, page.Level);

            // Look for default content
            if (this.section1.Visible && !this.desc01.HasContent) PopulateDefaultContent(listTitle01, this.section1Content, useLinks, this.from1);
            if (this.section2.Visible && !this.desc02.HasContent) PopulateDefaultContent(listTitle02, this.section2Content, useLinks, this.from2);
            if (this.section3.Visible && !this.desc03.HasContent) PopulateDefaultContent(listTitle03, this.section3Content, useLinks, this.from3);
            if (this.section4.Visible && !this.desc04.HasContent) PopulateDefaultContent(listTitle04, this.section4Content, useLinks, this.from4);
            if (this.section5.Visible && !this.desc05.HasContent) PopulateDefaultContent(listTitle05, this.section5Content, useLinks, this.from5);
            if (this.section6.Visible && !this.desc06.HasContent) PopulateDefaultContent(listTitle06, this.section6Content, useLinks, this.from6);
            if (this.section7.Visible && !this.desc07.HasContent) PopulateDefaultContent(listTitle07, this.section7Content, useLinks, this.from7);
            if (this.section8.Visible && !this.desc08.HasContent) PopulateDefaultContent(listTitle08, this.section8Content, useLinks, this.from8);
            if (this.section9.Visible && !this.desc09.HasContent) PopulateDefaultContent(listTitle09, this.section9Content, useLinks, this.from9);
            if (this.section10.Visible && !this.desc10.HasContent) PopulateDefaultContent(listTitle10, this.section10Content, useLinks, this.from10);
            if (this.section11.Visible && !this.desc11.HasContent) PopulateDefaultContent(listTitle11, this.section11Content, useLinks, this.from11);
            if (this.section12.Visible && !this.desc12.HasContent) PopulateDefaultContent(listTitle12, this.section12Content, useLinks, this.from12);
            if (this.section13.Visible && !this.desc13.HasContent) PopulateDefaultContent(listTitle13, this.section13Content, useLinks, this.from13);
            if (this.section14.Visible && !this.desc14.HasContent) PopulateDefaultContent(listTitle14, this.section14Content, useLinks, this.from14);
            if (this.section15.Visible && !this.desc15.HasContent) PopulateDefaultContent(listTitle15, this.section15Content, useLinks, this.from15);
        }

        private List<HtmlGenericControl> GetVisibleSections()
        {
            var visibleSections = new List<HtmlGenericControl>();
            if (this.section1.Visible) visibleSections.Add(section1);
            if (this.section2.Visible) visibleSections.Add(section2);
            if (this.section3.Visible) visibleSections.Add(section3);
            if (this.section4.Visible) visibleSections.Add(section4);
            if (this.section5.Visible) visibleSections.Add(section5);
            if (this.section6.Visible) visibleSections.Add(section6);
            if (this.section7.Visible) visibleSections.Add(section7);
            if (this.section8.Visible) visibleSections.Add(section8);
            if (this.section9.Visible) visibleSections.Add(section9);
            if (this.section10.Visible) visibleSections.Add(section10);
            if (this.section11.Visible) visibleSections.Add(section11);
            if (this.section12.Visible) visibleSections.Add(section12);
            if (this.section13.Visible) visibleSections.Add(section13);
            if (this.section14.Visible) visibleSections.Add(section14);
            if (this.section15.Visible) visibleSections.Add(section15);
            return visibleSections;
        }

        private void HideUnusedSections()
        {
            this.section1.Visible = listTitle01.HasContent;
            this.section2.Visible = listTitle02.HasContent;
            this.section3.Visible = listTitle03.HasContent;
            this.section4.Visible = listTitle04.HasContent;
            this.section5.Visible = listTitle05.HasContent;
            this.section6.Visible = listTitle06.HasContent;
            this.section7.Visible = listTitle07.HasContent;
            this.section8.Visible = listTitle08.HasContent;
            this.section9.Visible = listTitle09.HasContent;
            this.section10.Visible = listTitle10.HasContent;
            this.section11.Visible = listTitle11.HasContent;
            this.section12.Visible = listTitle12.HasContent;
            this.section13.Visible = listTitle13.HasContent;
            this.section14.Visible = listTitle14.HasContent;
            this.section15.Visible = listTitle15.HasContent;
        }

        private void AddClassesForColumns(List<HtmlGenericControl> visibleSections)
        {
            // Switch to 3 column view if editor selected it, or based on number of boxes
            var classForAllSections = "two-col";

            var columnsProp = CmsUtilities.GetCustomProperty("Columns");
            if (columnsProp != null)
            {
                if ((columnsProp.Value == "3 columns") ||
                    ((String.IsNullOrEmpty(columnsProp.Value) || columnsProp.Value == "Auto") && visibleSections.Count > 8))
                {
                    classForAllSections = "three-col";
                }
            }
            classForAllSections += " landing-section ";

            bool odd = true;
            var offsetPair = 1;
            int len = visibleSections.Count;

            for (var i = 0; i < len; i++)
            {
                // Add odd/even classes to support two column layout
                var classForThisSection = odd ? "odd" : "even";
                odd = !odd;

                // Add group1,2,3 class to support three column layout
                classForThisSection += " group" + ((i%3) + 1);

                // Add offset pair classes to support alternate styles down 2 columns (divide sections into pairs, offset by 1)
                if (odd) offsetPair = (offsetPair == 1) ? 2 : 1;
                classForThisSection += " offset-pair" + offsetPair.ToString(CultureInfo.CurrentCulture);
                
                // Put the class name on a container element for each item
                visibleSections[i].Attributes["class"] = classForAllSections + classForThisSection;
            }
        }

        /// <summary>
        /// Decide whether to show a list of links or the page description
        /// </summary>
        /// <returns></returns>
        internal static bool DisplayAsListsOfLinks(string descriptionsPropertyValue, int informationArchitectureLevel)
        {
            var useLinks = false;
            if (descriptionsPropertyValue == "Links")
            {
                useLinks = true;
            }
            else if (descriptionsPropertyValue == "Descriptions")
            {
                useLinks = false;
            }
            else
            {
                // Default behaviour is different in top-level channels
                useLinks = (informationArchitectureLevel <= 2);
            }
            return useLinks;
        }

        private void PopulateDefaultContent(RichHtmlPlaceholderControl titlePlaceholder, PlaceHolder sectionContent, bool useLinks, Control fromHint)
        {
            // 1. Parse a URL out of the title placeholder
            var match = Regex.Match(titlePlaceholder.Html, "href=\"(?<URL>[^\"]+)\"[^>]*>(?<Subject>.*?)</a>");
            if (!match.Success) return;

            // 2. Get that URL as a CMS posting
            var path = match.Groups["URL"].Value;

            // Reject anchor links because it's the current page, and because Umbraco throws an exception looking it up
            if (path.StartsWith("#", StringComparison.Ordinal)) return; 

            var cmsPath = path;
            if (cmsPath.StartsWith("http://", StringComparison.OrdinalIgnoreCase)) cmsPath = cmsPath.Substring(6); // turn "http://yourcouncil/" into "/yourcouncil"
            if (cmsPath.StartsWith("http://", StringComparison.OrdinalIgnoreCase)) cmsPath = cmsPath.Substring(6); // turn "http://yourcouncil/" into "/yourcouncil"
            if (!cmsPath.StartsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    cmsPath = Page.ResolveUrl(cmsPath);
                }
                catch (HttpException ex)
                {
                    // Catch an invalid URL which could not be resolved, such as ../ trying to go beyond the root folder.
                    // Report the error to be fixed, but we don't want the whole page to fail.
                    ex.Data.Add("URL which failed", cmsPath);
                    ex.ToExceptionless().Submit();
                    return;
                }
            }

            // Remove domain if it looks like the same site, as Umbraco throws an exception when looking it up
            var pathAsUri = new Uri(cmsPath, UriKind.RelativeOrAbsolute);
            if (pathAsUri.IsAbsoluteUri && 
                (pathAsUri.Host.EndsWith(".eastsussex.gov.uk") || pathAsUri.Host.EndsWith(".azurewebsites.net"))
                )
            {
                cmsPath = pathAsUri.AbsolutePath;
            }

            var from = String.Empty;
            var posting = UmbracoContext.Current.ContentCache.GetByRoute(cmsPath);
            if (posting != null)
            {
                from = "from linked page";
            }
            else
            {
                // If the link was the default posting in a Microsoft CMS channel, we need to remove the page name and get the 
                // 'channel' name to match the way it was imported as an Umbraco page
                cmsPath = cmsPath.TrimEnd('/');
                var pos = cmsPath.LastIndexOf("/", StringComparison.Ordinal);
                if (pos > 0)
                {
                    cmsPath = cmsPath.Substring(0, pos);

                    posting = UmbracoContext.Current.ContentCache.GetByRoute(cmsPath);
                    if (posting != null)
                    {
                        from = "from parent of linked page";
                    }
                }
            }

            if (posting != null)
            {

                // 4. Get the content from the destination page and display it
                if (useLinks)
                {
                    // 4a. If destination is a landing page or landing page with box, find first 5 links
                    var links = new List<string>(5);
                    if (posting.DocumentTypeAlias == "standardLandingPage")
                    {
                        var i = 1;
                        const int max = 15;
                        while (links.Count < 5 && i <= max)
                        {
                            var link = posting.GetPropertyValue<string>("phDefListTitle" + i.ToString("00", CultureInfo.InvariantCulture) + "_Content").Trim();
                            if (!String.IsNullOrEmpty(link))
                            {
                                // Strip any extra tags around the link (typically <p></p>)
                                var pos = link.IndexOf("<a ", StringComparison.OrdinalIgnoreCase);
                                if (pos > 0) link = link.Substring(pos);

                                pos = link.LastIndexOf("</a>", StringComparison.OrdinalIgnoreCase);
                                if (pos > -1) link = link.Substring(0, pos+4);

                                links.Add(link);
                            }
                            i++;
                        }
                    }

                    // 5a. If we found some links, display all 4, or first 3 with "more" link
                    if (links.Count > 0)
                    {
                        var list = new StringBuilder("<ul>");
                        for (var i = 0; i < links.Count; i++)
                        {
                            if (i == 3 && links.Count != 4) break;
                            list.Append("<li>").Append(links[i]).Append("</li>");
                        }
                        if (links.Count == 5) list.Append("<li><a href=\"").Append(CmsUtilities.CorrectPublishedUrl(path)).Append("\">More on " + match.Groups["Subject"].Value + "</a></li>");
                        list.Append("</ul>");
                        sectionContent.Controls.Add(new LiteralControl(list.ToString()));

                        from = "First few links " + from;
                    }
                    else
                    {
                        useLinks = false;
                    }

                }

                if (!useLinks)
                {
                    // 4b. Otherwise use the page description
                    sectionContent.Controls.Add(new LiteralControl("<p class=\"medium large\">" + HttpUtility.HtmlEncode(posting.GetPropertyValue<string>("pageDescription")) + "</p>"));

                    from = "Description " + from;
                }
            }

            if (!String.IsNullOrEmpty(from))
            {
                fromHint.Visible = true;
                fromHint.Controls.Clear();
                fromHint.Controls.Add(new LiteralControl(from));
            }
            else
            {
                fromHint.Visible = false;
            }
        }
    }
}