using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Escc.EastSussexGovUK.Umbraco.Api.Jobs.HtmlFormatters;
using HtmlAgilityPack;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Api.Tests
{
    [TestFixture]
    public class RemoveUnwantedAttributesFormatterTests
    {
        [Test]
        public void UnwantedAttributeIsRemoved()
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml("<div style=\"color:red\"></div>");
            var transformer = new RemoveUnwantedAttributesFormatter(new string[] { "style" });

            transformer.FormatHtml(htmlDocument);

            Assert.AreEqual("<div></div>", htmlDocument.DocumentNode.OuterHtml);
        }


        [Test]
        public void UnrelatedAttributeRemains()
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml("<div class=\"unrelated\"></div>");
            var transformer = new RemoveUnwantedAttributesFormatter(new string[] { "style" });

            transformer.FormatHtml(htmlDocument);

            Assert.AreEqual("<div class=\"unrelated\"></div>", htmlDocument.DocumentNode.OuterHtml);
        }
    }
}
