using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Umbraco.Api.Jobs.HtmlFormatters;
using HtmlAgilityPack;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Api.Tests
{
    [TestFixture]
    public class FakeListFormatterTests
    {
        private const string HTML_BEFORE = "<div>";
        private const string PARAGRAPH = "<p>example paragraph</p>";
        private const string FAKE_LIST_ITEM = "<p>- example list item</p>";
        private const string LIST_ITEM = "<li>example list item</li>";
        private const string HTML_AFTER = "</div>";

        [Test]
        public void NoListNoChange()
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(HTML_BEFORE + PARAGRAPH + PARAGRAPH + PARAGRAPH + HTML_AFTER);
            var formatter = new FakeListFormatter();

            formatter.FormatHtml(htmlDocument);

            Assert.AreEqual(HTML_BEFORE + PARAGRAPH + PARAGRAPH + PARAGRAPH + HTML_AFTER, htmlDocument.DocumentNode.OuterHtml);
        }

        [Test]
        public void OneListItemIsNotAList()
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(HTML_BEFORE + FAKE_LIST_ITEM + HTML_AFTER);
            var formatter = new FakeListFormatter();

            formatter.FormatHtml(htmlDocument);

            Assert.AreEqual(HTML_BEFORE + FAKE_LIST_ITEM + HTML_AFTER, htmlDocument.DocumentNode.OuterHtml);
        }

        [Test]
        public void TwoSeparateListItemsAreNotAList()
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(HTML_BEFORE + FAKE_LIST_ITEM + PARAGRAPH + FAKE_LIST_ITEM + HTML_AFTER);
            var formatter = new FakeListFormatter();

            formatter.FormatHtml(htmlDocument);

            Assert.AreEqual(HTML_BEFORE + FAKE_LIST_ITEM + PARAGRAPH + FAKE_LIST_ITEM + HTML_AFTER, htmlDocument.DocumentNode.OuterHtml);
        }

        [Test]
        public void TwoConsecutiveListItemsAreAList()
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(HTML_BEFORE + FAKE_LIST_ITEM + FAKE_LIST_ITEM + HTML_AFTER);
            var formatter = new FakeListFormatter();

            formatter.FormatHtml(htmlDocument);

            Assert.AreEqual(HTML_BEFORE + "<ul>" + LIST_ITEM + LIST_ITEM + "</ul>" + HTML_AFTER, htmlDocument.DocumentNode.OuterHtml);
        }

        [Test]
        public void SurroundingParagraphsAreUnchanged()
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(PARAGRAPH + FAKE_LIST_ITEM + FAKE_LIST_ITEM + FAKE_LIST_ITEM + PARAGRAPH);
            var formatter = new FakeListFormatter();

            formatter.FormatHtml(htmlDocument);

            Assert.AreEqual(PARAGRAPH + "<ul>" + LIST_ITEM + LIST_ITEM + LIST_ITEM + "</ul>" + PARAGRAPH, htmlDocument.DocumentNode.OuterHtml);
        }

        [Test]
        public void MultipleListsAreTransformed()
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(  PARAGRAPH + 
                                    FAKE_LIST_ITEM + FAKE_LIST_ITEM + FAKE_LIST_ITEM + 
                                    PARAGRAPH + 
                                    FAKE_LIST_ITEM + FAKE_LIST_ITEM + FAKE_LIST_ITEM + 
                                    PARAGRAPH);
            var formatter = new FakeListFormatter();

            formatter.FormatHtml(htmlDocument);

            Assert.AreEqual(PARAGRAPH +
                            "<ul>" + LIST_ITEM + LIST_ITEM + LIST_ITEM + "</ul>" +
                            PARAGRAPH +
                            "<ul>" + LIST_ITEM + LIST_ITEM + LIST_ITEM + "</ul>" +
                            PARAGRAPH,
                            htmlDocument.DocumentNode.OuterHtml);
        }
    }
}
