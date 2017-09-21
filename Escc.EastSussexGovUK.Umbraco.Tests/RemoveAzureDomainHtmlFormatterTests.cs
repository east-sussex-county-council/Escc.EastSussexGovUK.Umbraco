using System;
using Escc.EastSussexGovUK.Umbraco.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using Escc.EastSussexGovUK.Umbraco.UrlTransformers;
using Escc.EastSussexGovUK.Umbraco.RichTextHtmlFormatters;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestClass]
    public class RemoveAzureDomainHtmlFormatterTests
    {
        [TestMethod]
        public void RelativeUrlIsUnchangedInALink()
        {
            var html = "<p><a href=\"/abc/123/page.html\">link</a><p>";

            var linkTransformer = new RemoveAzureDomainHtmlFormatter();
            html = linkTransformer.Format(html);

            Assert.AreEqual("<p><a href=\"/abc/123/page.html\">link</a><p>", html);
        }

        [TestMethod]
        public void AbsoluteAzureUrlIsUpdatedInALink()
        {
            var html = "<p><a href=\"https://example.azurewebsites.net/abc/123/page.html\">link</a>";

            var linkTransformer = new RemoveAzureDomainHtmlFormatter();
            html = linkTransformer.Format(html);

            Assert.AreEqual("<p><a href=\"/abc/123/page.html\">link</a>", html);
        }

        [TestMethod]
        public void UnrelatedAbsoluteUrlIsNotUpdatedInALink()
        {
            var html = "<p><a href=\"https://www.accesseastsussex.org/jobs/index.aspx\">link</a>";

            var linkTransformer = new RemoveAzureDomainHtmlFormatter();
            html = linkTransformer.Format(html);

            Assert.AreEqual("<p><a href=\"https://www.accesseastsussex.org/jobs/index.aspx\">link</a>", html);
        }

        [TestMethod]
        public void RelativeUrlIsUnchangedInAnImage()
        {
            var html = "<p><img src=\"/abc/123/page.html\" />";

            var linkTransformer = new RemoveAzureDomainHtmlFormatter();
            html = linkTransformer.Format(html);

            Assert.AreEqual("<p><img src=\"/abc/123/page.html\" />", html);
        }

        [TestMethod]
        public void AbsoluteAzureUrlIsUpdatedInAnImage()
        {
            var html = "<p><img src=\"https://example.azurewebsites.net/abc/123/page.html\" />";

            var linkTransformer = new RemoveAzureDomainHtmlFormatter();
            html = linkTransformer.Format(html);

            Assert.AreEqual("<p><img src=\"/abc/123/page.html\" />", html);
        }

        [TestMethod]
        public void UnrelatedAbsoluteUrlIsNotUpdatedInAnImage()
        {
            var html = "<p><img src=\"https://www.accesseastsussex.org/jobs/index.aspx\" />";

            var linkTransformer = new RemoveAzureDomainHtmlFormatter();
            html = linkTransformer.Format(html);

            Assert.AreEqual("<p><img src=\"https://www.accesseastsussex.org/jobs/index.aspx\" />", html);
        }
    }
}
