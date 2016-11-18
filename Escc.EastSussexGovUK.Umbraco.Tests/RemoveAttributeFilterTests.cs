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
    public class RemoveAttributeFilterTests
    {
        [Test]
        public void AttributeIsRemovedFromAnyElement()
        {
            var originalHtml = "<div class=\"example\" style=\"color:red\">Example</div><span class=\"example2\" style=\"color:blue\">Example</span>";
            var expectedHtml = "<div class=\"example\">Example</div><span class=\"example2\">Example</span>";

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(originalHtml);

            var filter = new RemoveAttributeFilter("style");
            filter.Filter(htmlDocument);

            Assert.AreEqual(expectedHtml, htmlDocument.DocumentNode.OuterHtml);
        }
    }
}
