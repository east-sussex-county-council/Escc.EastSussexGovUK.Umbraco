using Escc.EastSussexGovUK.Umbraco.Api.Jobs.HtmlFormatters;
using Escc.Html;
using HtmlAgilityPack;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Api.Tests
{
    [TestFixture]
    public class TruncateLongLinksFormatterTests
    {
        [Test]
        public void SurreyTalentLinkUrlIsTruncated()
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(Properties.Resources.LongUrlAndEmailInBodyText);

            new TruncateLongLinksFormatter(new MockLinkFormatter()).FormatHtml(htmlDocument);

            Assert.IsTrue(htmlDocument.DocumentNode.OuterHtml.Contains("replaced link"));
        }
    }
}
