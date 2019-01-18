using Escc.EastSussexGovUK.Umbraco.Api.Jobs.HtmlFormatters;
using Escc.EastSussexGovUK.Umbraco.Api.Jobs.JobTransformers;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using Escc.Html;
using HtmlAgilityPack;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Api.Tests
{
    [TestFixture]
    public class TruncateLongLinksTransformerTests
    {
        [Test]
        public void SurreyTalentLinkUrlIsTruncated()
        {
            var linkFormatter = new Mock<IHtmlLinkFormatter>();
            linkFormatter.Setup(x => x.AbbreviateUrl(Moq.It.IsAny<Uri>())).Returns("replaced link");
            var transformer = new TruncateLongLinksTransformer(linkFormatter.Object);
            var job = new Job() { AdvertHtml = new HtmlString(Properties.Resources.LongUrlAndEmailInBodyText) };

            transformer.TransformJob(job);

            Assert.IsTrue(job.AdvertHtml.ToHtmlString().Contains("replaced link"));
        }
    }
}
