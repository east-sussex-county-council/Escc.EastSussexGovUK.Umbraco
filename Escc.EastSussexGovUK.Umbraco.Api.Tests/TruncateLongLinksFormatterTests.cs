using Escc.EastSussexGovUK.Umbraco.Api.Jobs.HtmlFormatters;
using Escc.Html;
using HtmlAgilityPack;
using Moq;
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
            var linkFormatter = new Mock<IHtmlLinkFormatter>();
            linkFormatter.Setup(x => x.AbbreviateUrl(Moq.It.IsAny<Uri>())).Returns("replaced link");

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(Properties.Resources.LongUrlAndEmailInBodyText);

            new TruncateLongLinksFormatter(linkFormatter.Object).FormatHtml(htmlDocument);

            Assert.IsTrue(htmlDocument.DocumentNode.OuterHtml.Contains("replaced link"));
        }

        [Test]
        public void BadLinkIsIgnored()
        {
            var linkFormatter = new Mock<IHtmlLinkFormatter>();

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(Properties.Resources.BadLinkInHtml);

            new TruncateLongLinksFormatter(linkFormatter.Object).FormatHtml(htmlDocument);

            // Assert that the link text (same as the bad URL in this example) is still there but no longer linked
            Assert.IsTrue(htmlDocument.DocumentNode.OuterHtml.Contains("http://www.easthoathlynursery.co uk"));
            Assert.IsFalse(htmlDocument.DocumentNode.OuterHtml.Contains("http://www.easthoathlynursery.co uk</a>"));
        }
    }
}
