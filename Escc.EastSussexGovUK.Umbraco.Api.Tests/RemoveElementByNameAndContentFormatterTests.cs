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
    public class RemoveElementByNameAndContentFormatterTests
    {
        [Test]
        public void ElementIsRemoved()
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(Properties.Resources.JobDetailInHtml1);

            new RemoveElementByNameAndContentFormatter("h5", "Job Details").FormatHtml(htmlDocument);

            var expected = Properties.Resources.JobDetailInHtml1.Replace("<h5 class=\"JD-FieldLabel\" id=\"JDLabel-Field1\">Job Details</h5>", String.Empty);
            Assert.AreEqual(expected, htmlDocument.DocumentNode.OuterHtml);
        }
    }
}
