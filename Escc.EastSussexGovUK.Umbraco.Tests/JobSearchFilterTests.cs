using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestFixture]
    class JobSearchFilterTests
    {
        [Test]
        public void KeywordsChangesHash()
        {
            var filter = new JobSearchQuery();

            var hashBefore = filter.ToHash();
            filter.Keywords = "test";
            var hashAfter = filter.ToHash();

            Assert.AreNotEqual(hashBefore, hashAfter);
        }

        [Test]
        public void LocationChangesHash()
        {
            var filter = new JobSearchQuery();

            var hashBefore = filter.ToHash();
            filter.Locations = "test";
            var hashAfter = filter.ToHash();

            Assert.AreNotEqual(hashBefore, hashAfter);
        }

        [Test]
        public void JobTypeChangesHash()
        {
            var filter = new JobSearchQuery();

            var hashBefore = filter.ToHash();
            filter.JobTypes.Add("test");
            var hashAfter = filter.ToHash();

            Assert.AreNotEqual(hashBefore, hashAfter);
        }

        [Test]
        public void WorkingHoursChangesHash()
        {
            var filter = new JobSearchQuery();

            var hashBefore = filter.ToHash();
            filter.WorkPatterns.Add("test");
            var hashAfter = filter.ToHash();

            Assert.AreNotEqual(hashBefore, hashAfter);
        }

        [Test]
        public void OrganisationChangesHash()
        {
            var filter = new JobSearchQuery();

            var hashBefore = filter.ToHash();
            filter.Organisations = "test";
            var hashAfter = filter.ToHash();

            Assert.AreNotEqual(hashBefore, hashAfter);
        }

        [Test]
        public void SalaryRangeChangesHash()
        {
            var filter = new JobSearchQuery();

            var hashBefore = filter.ToHash();
            filter.SalaryRanges = "test";
            var hashAfter = filter.ToHash();

            Assert.AreNotEqual(hashBefore, hashAfter);
        }

        [Test]
        public void JobReferenceChangesHash()
        {
            var filter = new JobSearchQuery();

            var hashBefore = filter.ToHash();
            filter.JobReference = "test";
            var hashAfter = filter.ToHash();

            Assert.AreNotEqual(hashBefore, hashAfter);
        }

        [Test]
        public void KeywordHashIsDifferentToLocation()
        {
            var keywordsFilter = new JobSearchQuery();
            keywordsFilter.Keywords = "test";
            var hashKeywords = keywordsFilter.ToHash();

            var locationFilter = new JobSearchQuery();
            locationFilter.Locations = "test";
            var hashLocation = locationFilter.ToHash();

            Assert.AreNotEqual(hashKeywords, hashLocation);
        }
    }
}
