
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
    public class RemoveElementsWithNoContentFormatterTests
    {
        [Test]
        public void ElementWithNoContentIsRemoved()
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml("<p>this is a <strong> </strong>test.</p>");

            new RemoveElementsWithNoContentFormatter(new[] { "strong" }).FormatHtml(htmlDocument);

            Assert.AreEqual("<p>this is a test.</p>", htmlDocument.DocumentNode.OuterHtml);
        }

        [Test]
        public void ElementWithContentIsNotRemoved()
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml("<p>this is a <strong>test</strong>.</p>");

            new RemoveElementsWithNoContentFormatter(new[] { "strong" }).FormatHtml(htmlDocument);

            Assert.AreEqual("<p>this is a <strong>test</strong>.</p>", htmlDocument.DocumentNode.OuterHtml);
        }
    }
}
