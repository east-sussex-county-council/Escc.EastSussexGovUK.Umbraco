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
    public class RemoveUnwantedElementsFormatterTests
    {
        [Test]
        public void DefaultIsToLeaveChildNodes()
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml("<p>this is a <u>test</u>.</p>");

            new RemoveUnwantedElementsFormatter(new[] { "u" }).FormatHtml(htmlDocument);

            Assert.AreEqual("<p>this is a test.</p>", htmlDocument.DocumentNode.OuterHtml);
        }

        [Test]
        public void UnwantedElementIsRemovedButChildNodesRemain()
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml("<p>this is a <u>test</u>.</p>");
            var transformer = new RemoveUnwantedElementsFormatter(new[] { "u" }, false);

            transformer.FormatHtml(htmlDocument);

            Assert.AreEqual("<p>this is a test.</p>", htmlDocument.DocumentNode.OuterHtml);
        }

        [Test]
        public void UnwantedElementIsRemovedIncludingChildNodes()
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml("<p>this is a <u>test</u>.</p>");
            var transformer = new RemoveUnwantedElementsFormatter(new[] { "u" }, true);

            transformer.FormatHtml(htmlDocument);

            Assert.AreEqual("<p>this is a .</p>", htmlDocument.DocumentNode.OuterHtml);
        }

        [Test]
        public void OtherElementIsNotAffected()
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml("<p>this is a <u>test</u>.</p>");
            var transformer = new RemoveUnwantedElementsFormatter(new[] { "b" }, true);

            transformer.FormatHtml(htmlDocument);

            Assert.AreEqual("<p>this is a <u>test</u>.</p>", htmlDocument.DocumentNode.OuterHtml);
        }
    }
}
