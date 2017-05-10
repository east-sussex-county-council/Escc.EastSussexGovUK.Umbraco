using System;
using Escc.EastSussexGovUK.Umbraco.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using Escc.EastSussexGovUK.Umbraco.UrlTransformers;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestClass]
    public class RemoveMediaDomainUrlTransformerTests
    {
        [TestMethod]
        public void RelativeMediaUrlIsUnchanged()
        {
            var url = new Uri("/media/1234/some-media-item-in-umbraco.pdf", UriKind.Relative);

            var linkTransformer = new RemoveMediaDomainUrlTransformer();
            url = linkTransformer.TransformUrl(url);

            Assert.AreEqual("/media/1234/some-media-item-in-umbraco.pdf", url.ToString());
        }

        [TestMethod]
        public void UnrelatedRelativeUrlIsNotUpdated()
        {
            var url = new Uri("/mediafiles/1234/some-other-media-item.pdf", UriKind.Relative);

            var linkTransformer = new RemoveMediaDomainUrlTransformer();
            url = linkTransformer.TransformUrl(url);

            Assert.AreEqual("/mediafiles/1234/some-other-media-item.pdf", url.ToString());
        }

        [TestMethod]
        public void AbsoluteMediaUrlIsUpdated()
        {
            var url = new Uri("https://different-site.blob.core.windows.net/media/1234/some-media-item-in-umbraco.pdf");

            var linkTransformer = new RemoveMediaDomainUrlTransformer();
            url = linkTransformer.TransformUrl(url);

            Assert.AreEqual("/media/1234/some-media-item-in-umbraco.pdf", url.ToString());
        }

        [TestMethod]
        public void UnrelatedAbsoluteUrlIsNotUpdated()
        {
            var url = new Uri("https://www.accesseastsussex.org/jobs/index.aspx");

            var linkTransformer = new RemoveMediaDomainUrlTransformer();
            url = linkTransformer.TransformUrl(url);

            Assert.AreEqual("https://www.accesseastsussex.org/jobs/index.aspx", url.ToString());
        }

        [TestCase("<p><img src=\"/media/1234/some-media-item-in-umbraco.jpg\" alt=\"Test\" /></p>", "<p><img src=\"/media/1234/some-media-item-in-umbraco.jpg\" alt=\"Test\" /></p>")]
        [TestCase("<p><a href=\"/media/1234/some-media-item-in-umbraco.pdf\">Some text</a></p>", "<p><a href=\"/media/1234/some-media-item-in-umbraco.pdf\">Some text</a></p>")]
        public void RelativeUrlIsNotUpdatedInHtml(string before, string expected)
        {
            var linkTransformer = new RemoveMediaDomainUrlTransformer();

            var after = linkTransformer.ParseAndTransformMediaUrlsInHtml(before);

            Assert.AreEqual(expected, after);
        }

        [TestCase("<p><img src=\"/mediafiles/1234/some-other-media-item.jpg\" alt=\"Test\" /></p>", "<p><img src=\"/mediafiles/1234/some-other-media-item.jpg\" alt=\"Test\" /></p>")]
        [TestCase("<p><a href=\"/mediafiles/1234/some-other-media-item.pdf\">Some text</a></p>", "<p><a href=\"/mediafiles/1234/some-other-media-item.pdf\">Some text</a></p>")]
        public void UnrelatedRelativeUrlIsNotUpdatedInHtml(string before, string expected)
        {
            var linkTransformer = new RemoveMediaDomainUrlTransformer();

            var after = linkTransformer.ParseAndTransformMediaUrlsInHtml(before);

            Assert.AreEqual(expected, after);
        }

        [TestCase("<p><img src=\"https://different-site.blob.core.windows.net/media/1234/some-media-item-in-umbraco.jpg\" alt=\"Test\" /></p>", "<p><img src=\"/media/1234/some-media-item-in-umbraco.jpg\" alt=\"Test\" /></p>")]
        [TestCase("<p><a href=\"https://different-site.blob.core.windows.net/media/1234/some-media-item-in-umbraco.pdf\">Some text</a></p>", "<p><a href=\"/media/1234/some-media-item-in-umbraco.pdf\">Some text</a></p>")]
        public void AbsoluteUrlIsUpdatedInHtml(string before, string expected)
        {
            var linkTransformer = new RemoveMediaDomainUrlTransformer();

            var after = linkTransformer.ParseAndTransformMediaUrlsInHtml(before);

            Assert.AreEqual(expected, after);
        }
    }
}
