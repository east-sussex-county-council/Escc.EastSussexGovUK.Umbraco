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
            htmlDocument.LoadHtml(Properties.Resources.ClosingDateInBody1);

            new RemoveElementByNameAndContentFormatter("span", "Closing date: Sunday 23 April 2017").FormatHtml(htmlDocument);

            var expected = Properties.Resources.ClosingDateInBody1.Replace("<span>Closing date: Sunday 23 April 2017</span>", String.Empty);
            Assert.AreEqual(expected, htmlDocument.DocumentNode.OuterHtml);
        }
    }
}
