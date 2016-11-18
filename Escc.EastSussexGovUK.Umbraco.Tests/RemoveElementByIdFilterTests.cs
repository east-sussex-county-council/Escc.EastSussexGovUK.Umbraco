using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Umbraco.ApiControllers;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using HtmlAgilityPack;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestFixture]
    public class RemoveElementByIdFilterTests
    {
        [Test]
        public void OnlyElementWithMatchingIdIsRemoved()
        {
            var originalHtml = "<div id=\"remove-me\">Example</div><span id=\"leave-me\">Example</span>";
            var expectedHtml = "<span id=\"leave-me\">Example</span>";

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(originalHtml);

            var filter = new RemoveElementByIdFilter("remove-me");
            filter.Filter(htmlDocument);

            Assert.AreEqual(expectedHtml, htmlDocument.DocumentNode.OuterHtml);
        }
    }
}
