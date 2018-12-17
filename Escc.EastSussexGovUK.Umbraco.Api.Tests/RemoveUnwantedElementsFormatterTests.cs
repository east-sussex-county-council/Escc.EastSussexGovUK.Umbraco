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
        public void UnwantedElementIsRemoved()
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml("<p>this is a <u>test</u>.</p>");

            new RemoveUnwantedElementsFormatter(new[] { "u" }).FormatHtml(htmlDocument);

            Assert.AreEqual("<p>this is a test.</p>", htmlDocument.DocumentNode.OuterHtml);
        }
    }
}
