using Escc.EastSussexGovUK.Umbraco.Api.Jobs.JobTransformers;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Api.Tests
{
    [TestFixture]
    public class LocationMatcherTests
    {
        [Test]
        public void JobLocationMatched()
        {
            var job = new Job()
            {
                Locations = new List<string>() { "Crowborough" }
            };

            var matcher = new LocationMatcher("Crowborough");

            var result = matcher.IsMatch(job);

            Assert.IsTrue(result);
        }

        [Test]
        public void UnrelatedJobLocationDoesNotMatch()
        {
            var job = new Job()
            {
                Locations = new List<string>() { "Lewes" }
            };

            var matcher = new LocationMatcher("Crowborough");

            var result = matcher.IsMatch(job);

            Assert.IsFalse(result);
        }
    }
}
