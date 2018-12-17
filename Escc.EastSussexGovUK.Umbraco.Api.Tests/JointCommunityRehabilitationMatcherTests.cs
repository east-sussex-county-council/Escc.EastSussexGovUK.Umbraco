using Escc.EastSussexGovUK.Umbraco.Api.Jobs.JobTransformers;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using NUnit.Framework;
using System;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Api.Tests
{
    [TestFixture]
    public class JointCommunityRehabilitationMatcherTests
    {
        [Test]
        public void MatchJcrInTitle()
        {
            var job = new Job()
            {
                JobTitle = "JCR Support Worker"
            };

            var matcher = new JointCommunityRehabilitationMatcher();

            var result = matcher.IsMatch(job);

            Assert.IsTrue(result);
        }

        [Test]
        public void MatchJointCommunityRehabilitationInBody()
        {
            var job = new Job()
            {
                AdvertHtml = new HtmlString("<p>This role is in the Joint Community Rehabilitation (JCR) team, a partnership between East Sussex County Council and East Sussex Healthcare NHS Trust.</p>")
            };

            var matcher = new JointCommunityRehabilitationMatcher();

            var result = matcher.IsMatch(job);

            Assert.IsTrue(result);
        }

        [Test]
        public void UnrelatedJobDoesNotMatch()
        {
            var job = new Job()
            {
                JobTitle = "Social worker",
                AdvertHtml = new HtmlString("<p>Health and Social Care Connect (HSCC) is the East Sussex countywide hub for both professionals and the public to access adult community health and social care services.</p>")
            };

            var matcher = new JointCommunityRehabilitationMatcher();

            var result = matcher.IsMatch(job);

            Assert.IsFalse(result);
        }
    }
}
