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
            while (FormatHtmlMultiParagraphList(htmlDocument, bullet)) { };

            FormatHtmlSingleParagraphLists(htmlDocument, bullet);
        }

        private static void FormatHtmlSingleParagraphLists(HtmlDocument htmlDocument, string bullet)
        {
            var potentialSingleParaList = htmlDocument.DocumentNode.SelectNodes("//p[@potential-single-paragraph-list]");
            if (potentialSingleParaList != null)
            {
                foreach (var node in potentialSingleParaList)
                {
                    node.Attributes.Remove("potential-single-paragraph-list");
                    FormatHtmlSingleParagraphList(node, bullet);
                }
            }
        }

        private static void FormatHtmlSingleParagraphList(HtmlNode node, string bullet)
        {
            // If the paragraph starts with the bullet, contains at least one <br />, 
            // and every <br /> is followed by the bullet, then it's a list
            if (!node.InnerHtml.StartsWith(bullet)) return;

            var lineBreaks = node.SelectNodes("./br");
            if (lineBreaks == null || lineBreaks.Count == 0) return;
            foreach (var lineBreak in lineBreaks)
            {
                if (lineBreak.NextSibling == null) return;
                if (lineBreak.NextSibling.NodeType != HtmlNodeType.Text) return;
                if (!lineBreak.NextSibling.InnerHtml.StartsWith(bullet) &&
                    !lineBreak.NextSibling.InnerHtml.StartsWith(Environment.NewLine + bullet)) return;
            }

            // If we're still going, it's a list.
            // Do a string replace instead of updating each lineBreak, because that doesn't update node.OuterHtml
            node.Name = "ul";
            node.InnerHtml = "<li>" + node.InnerHtml.Substring(bullet.Length)
                .Replace("<br>" + Environment.NewLine + bullet, "</li><li>")
                .Replace("<br>" + bullet, "</li><li>")
                + "</li>";
        }

        private static bool FormatHtmlMultiParagraphList(HtmlDocument htmlDocument, string bullet)
        {
            var firstListItem = htmlDocument.DocumentNode.SelectSingleNode($"//p[starts-with(.,'{bullet}') and not(@potential-single-paragraph-list)]");
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
                    // No multi-paragraph list, but mark it as such so that we don't try to check again
                    firstListItem.SetAttributeValue("potential-single-paragraph-list", "false");
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