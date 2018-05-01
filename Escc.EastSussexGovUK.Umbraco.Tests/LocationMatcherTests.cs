using Escc.EastSussexGovUK.Umbraco.Jobs;
using Escc.EastSussexGovUK.Umbraco.Jobs.JobTransformers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Tests
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
