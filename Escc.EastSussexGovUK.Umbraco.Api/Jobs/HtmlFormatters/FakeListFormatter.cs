using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.HtmlFormatters
{
    /// <summary>
    /// Converts fake lists make with hyphens into real HTML lists
    /// </summary>
    public class FakeListFormatter : IHtmlAgilityPackHtmlFormatter
    {
        public void FormatHtml(HtmlDocument htmlDocument)
        {
            if (htmlDocument == null) return;

            FormatHtmlListWithBullet(htmlDocument, "- ");
            FormatHtmlListWithBullet(htmlDocument, "* ");
        }

        private static void FormatHtmlListWithBullet(HtmlDocument htmlDocument, string bullet)
        {
            while (FormatHtmlList(htmlDocument, bullet)) { };

            var nodesWithTemporaryAttribute = htmlDocument.DocumentNode.SelectNodes("//p[@data-ignore-list-item]");
            if (nodesWithTemporaryAttribute != null)
            {
                foreach (var node in nodesWithTemporaryAttribute)
                {
                    node.Attributes.Remove("data-ignore-list-item");
                }
            }
        }

        private static bool FormatHtmlList(HtmlDocument htmlDocument, string bullet)
        {
            var firstListItem = htmlDocument.DocumentNode.SelectSingleNode($"//p[starts-with(.,'{bullet}') and not(@data-ignore-list-item)]");
            if (firstListItem != null)
            {
                var list = new List<HtmlNode>() { firstListItem };

                var nextSibling = firstListItem.NextSibling;
                while (nextSibling != null)
                {
                    nextSibling = TryToAddNextSibling(nextSibling, bullet, list);
                }

                if (list.Count == 1)
                {
                    // No list, but mark it as such so that we don't try to process it again
                    firstListItem.SetAttributeValue("data-ignore-list-item", "false");
                }
                else
                {
                    // Found a list - change the element names and surround them with unordered list elements
                    var listElement = htmlDocument.CreateElement("ul");
                    firstListItem.ParentNode.InsertBefore(listElement, firstListItem);
                    foreach (var listItem in list)
                    {
                        listItem.Name = "li";
                        listItem.InnerHtml = listItem.InnerHtml.Substring(bullet.Length);
                        listItem.Remove();
                        listElement.AppendChild(listItem);
                    }
                }

                return true;
            }

            return false;
        }

        private static HtmlNode TryToAddNextSibling(HtmlNode nextSibling, string bullet, List<HtmlNode> list)
        {
            if (nextSibling.NodeType == HtmlNodeType.Element)
            {
                if (nextSibling.Name == "p" && nextSibling.InnerHtml.StartsWith(bullet))
                {
                    list.Add(nextSibling);
                }
                else return null;
            }

            return nextSibling.NextSibling;
        }
    }
}