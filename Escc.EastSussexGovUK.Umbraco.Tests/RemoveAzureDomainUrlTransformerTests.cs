using System;
using Escc.EastSussexGovUK.Umbraco.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using Escc.EastSussexGovUK.Umbraco.UrlTransformers;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestClass]
    public class RemoveAzureDomainUrlTransformerTests
    {
        [TestMethod]
        public void RelativeUrlIsUnchanged()
        {
            var url = new Uri("/abc/123/page.html", UriKind.Relative);

            var linkTransformer = new RemoveAzureDomainUrlTransformer();
            url = linkTransformer.TransformUrl(url);

            Assert.AreEqual("/abc/123/page.html", url.ToString());
        }

        [TestMethod]
        public void AbsoluteAzureUrlIsUpdated()
        {
            var url = new Uri("https://example.azurewebsites.net/abc/123/page.html");

            var linkTransformer = new RemoveAzureDomainUrlTransformer();
            url = linkTransformer.TransformUrl(url);

            Assert.AreEqual("/abc/123/page.html", url.ToString());
        }

        [TestMethod]
        public void UnrelatedAbsoluteUrlIsNotUpdated()
        {
            var url = new Uri("https://www.accesseastsussex.org/jobs/index.aspx");

            var linkTransformer = new RemoveAzureDomainUrlTransformer();
            url = linkTransformer.TransformUrl(url);

            Assert.AreEqual("https://www.accesseastsussex.org/jobs/index.aspx", url.ToString());
        }
    }
}
