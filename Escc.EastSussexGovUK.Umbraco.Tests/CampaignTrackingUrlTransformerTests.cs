using System;
using NUnit.Framework;
using Escc.EastSussexGovUK.Umbraco.UrlTransformers;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestFixture]
    public class CampaignTrackingUrlTransformerTests
    {
        [Test]
        public void DomainSpecifiedIsTransformed()
        {
            var transformer = new CampaignTrackingUrlTransformer("new-source", "new-medium", "new-campaign", "new-content", @"^test\.eastsussex\.gov\.uk$");
            var url = new Uri("https://test.eastsussex.gov.uk/example/page");

            var transformedUrl = transformer.TransformUrl(url);

            Assert.AreEqual(new Uri("https://test.eastsussex.gov.uk/example/page?utm_source=new-source&utm_medium=new-medium&utm_content=new-content&utm_campaign=new-campaign"), transformedUrl);
        }

        [Test]
        public void WildcardMatchesAnySubdomain()
        {
            var transformer = new CampaignTrackingUrlTransformer("new-source", "new-medium", "new-campaign", "new-content", @"^[a-z]+\.eastsussex\.gov\.uk$");
            var url = new Uri("https://test.eastsussex.gov.uk/example/page");

            var transformedUrl = transformer.TransformUrl(url);

            Assert.AreEqual(new Uri("https://test.eastsussex.gov.uk/example/page?utm_source=new-source&utm_medium=new-medium&utm_content=new-content&utm_campaign=new-campaign"), transformedUrl);
        }

        [Test]
        public void DomainNotSpecifiedIsNotTransformed()
        {
            var transformer = new CampaignTrackingUrlTransformer("new-source", "new-medium", "new-campaign", "new-content", @"^test\.eastsussex\.gov\.uk$");
            var url = new Uri("https://subdomain.example.org/example/page");

            var transformedUrl = transformer.TransformUrl(url);

            Assert.AreEqual(url, transformedUrl);
        }

        [Test]
        public void ExistingMediumIsTransformed()
        {
            var transformer = new CampaignTrackingUrlTransformer("new-source", "new-medium", "new-campaign", "new-content", @"^test\.eastsussex\.gov\.uk$");
            var url = new Uri("https://test.eastsussex.gov.uk/example/page?utm_source=existing-source&utm_medium=existing-medium&utm_content=existing-content&utm_campaign=existing-campaign");

            var transformedUrl = transformer.TransformUrl(url);

            Assert.IsTrue(transformedUrl.ToString().Contains("utm_medium=new-medium"));
        }

        [Test]
        public void ExistingCampaignIsUnchanged()
        {
            var transformer = new CampaignTrackingUrlTransformer("new-source", "new-medium", "new-campaign", "new-content", @"^test\.eastsussex\.gov\.uk$");
            var url = new Uri("https://test.eastsussex.gov.uk/example/page?utm_source=existing-source&utm_medium=existing-medium&utm_content=existing-content&utm_campaign=existing-campaign");

            var transformedUrl = transformer.TransformUrl(url);

            Assert.IsTrue(transformedUrl.ToString().EndsWith("utm_campaign=existing-campaign"));
        }

        [Test]
        public void ExistingQueryStringIsUnchanged()
        {
            var transformer = new CampaignTrackingUrlTransformer("new-source", "new-medium", "new-campaign", "new-content", @"^test\.eastsussex\.gov\.uk$");
            var url = new Uri("https://test.eastsussex.gov.uk/example/page?custom=keep-me&utm_source=existing-source&utm_medium=existing-medium&utm_content=existing-content&utm_campaign=existing-campaign");

            var transformedUrl = transformer.TransformUrl(url);

            Assert.IsTrue(transformedUrl.ToString().Contains("custom=keep-me"));
        }

        [Test]
        public void UrlWithoutCampaignHasCampaignAdded()
        {
            var transformer = new CampaignTrackingUrlTransformer("new-source", "new-medium", "new-campaign", "new-content", @"^test\.eastsussex\.gov\.uk$");
            var url = new Uri("https://test.eastsussex.gov.uk/example/page");

            var transformedUrl = transformer.TransformUrl(url);

            Assert.AreEqual(new Uri("https://test.eastsussex.gov.uk/example/page?utm_source=new-source&utm_medium=new-medium&utm_content=new-content&utm_campaign=new-campaign"), transformedUrl);
        }
    }
}
