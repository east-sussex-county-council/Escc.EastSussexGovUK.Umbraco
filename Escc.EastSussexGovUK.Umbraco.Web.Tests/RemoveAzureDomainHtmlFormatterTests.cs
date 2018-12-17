using System;
using Escc.EastSussexGovUK.Umbraco.Web.Services;
using NUnit.Framework;
using Escc.EastSussexGovUK.Umbraco.Web.RichTextHtmlFormatters;

namespace Escc.EastSussexGovUK.Umbraco.Web.Tests
{
    [TestFixture]
    public class RemoveAzureDomainHtmlFormatterTests
    {
        [Test]
        public void RelativeUrlIsUnchangedInALink()
        {
            var html = "<p><a href=\"/abc/123/page.html\">link</a><p>";

            var linkTransformer = new RemoveAzureDomainHtmlFormatter();
            html = linkTransformer.Format(html);

            Assert.AreEqual("<p><a href=\"/abc/123/page.html\">link</a><p>", html);
        }

        [Test]
        public void AbsoluteAzureUrlIsUpdatedInALink()
        {
            var html = "<p><a href=\"https://example.azurewebsites.net/abc/123/page.html\">link</a>";

            var linkTransformer = new RemoveAzureDomainHtmlFormatter();
            html = linkTransformer.Format(html);

            Assert.AreEqual("<p><a href=\"/abc/123/page.html\">link</a>", html);
        }

        [Test]
        public void UnrelatedAbsoluteUrlIsNotUpdatedInALink()
        {
            var html = "<p><a href=\"https://www.accesseastsussex.org/jobs/index.aspx\">link</a>";

            var linkTransformer = new RemoveAzureDomainHtmlFormatter();
            html = linkTransformer.Format(html);

            Assert.AreEqual("<p><a href=\"https://www.accesseastsussex.org/jobs/index.aspx\">link</a>", html);
        }

        [Test]
        public void RelativeUrlIsUnchangedInAnImage()
        {
            var html = "<p><img src=\"/abc/123/page.html\" />";

            var linkTransformer = new RemoveAzureDomainHtmlFormatter();
            html = linkTransformer.Format(html);

            Assert.AreEqual("<p><img src=\"/abc/123/page.html\" />", html);
        }

        [Test]
        public void AbsoluteAzureUrlIsUpdatedInAnImage()
        {
            var html = "<p><img src=\"https://example.azurewebsites.net/abc/123/page.html\" />";

            var linkTransformer = new RemoveAzureDomainHtmlFormatter();
            html = linkTransformer.Format(html);

            Assert.AreEqual("<p><img src=\"/abc/123/page.html\" />", html);
        }

        [Test]
        public void UnrelatedAbsoluteUrlIsNotUpdatedInAnImage()
        {
            var html = "<p><img src=\"https://www.accesseastsussex.org/jobs/index.aspx\" />";

            var linkTransformer = new RemoveAzureDomainHtmlFormatter();
            html = linkTransformer.Format(html);

            Assert.AreEqual("<p><img src=\"https://www.accesseastsussex.org/jobs/index.aspx\" />", html);
        }
    }
}
