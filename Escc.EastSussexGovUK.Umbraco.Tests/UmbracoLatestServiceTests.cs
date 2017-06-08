using Escc.EastSussexGovUK.Umbraco.Services;
using GDev.Umbraco.Test;
using Moq;
using NUnit.Framework;
using Umbraco.Core.Models;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestFixture]
    public class UmbracoLatestServiceTests
    {
        private Mock<IPublishedContent> _umbracoPage;

        [SetUp]
        public void Setup()
        {
            // Mock IPublishedContent using method at http://skrift.io/articles/archive/unit-testing-umbraco-with-umbraco-context-mock/
            this._umbracoPage = new Mock<IPublishedContent>();
        }

        [Test]
        public void LatestHtmlIsReadFromPage()
        {
            this._umbracoPage.Setup(x => x.GetProperty("latest_Latest").Value).Returns("<p>This is a test</p>");
            var latestService = new UmbracoLatestService(this._umbracoPage.Object);

            var settings = latestService.ReadLatestSettings();

            Assert.AreEqual("<p>This is a test</p>", settings.LatestHtml.ToString());
        }
    }
}
