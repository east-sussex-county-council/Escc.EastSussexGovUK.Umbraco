using Escc.EastSussexGovUK.Umbraco.Jobs;
using Escc.Html;
using HtmlAgilityPack;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Tests
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
