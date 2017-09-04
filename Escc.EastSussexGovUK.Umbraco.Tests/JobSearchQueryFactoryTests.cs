using Escc.EastSussexGovUK.Umbraco.Jobs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestFixture]
    public class JobSearchQueryFactoryTests
    {
        [Test]
        public void JobKeywordsAreReadFromQuerystring()
        {
            var queryString = new NameValueCollection();
            queryString.Add("keywords", "test");
            var factory = new JobSearchQueryFactory();

            var query = factory.CreateFromQueryString(queryString);

            Assert.AreEqual("test", query.Keywords);
        }

        [Test]
        public void JobLocationsAreReadFromQuerystring()
        {
            var queryString = new NameValueCollection();
            queryString.Add("location", "test");
            var factory = new JobSearchQueryFactory();

            var query = factory.CreateFromQueryString(queryString);

            Assert.AreEqual("test", query.Locations[0]);
        }

        [Test]
        public void JobTypesAreReadFromQuerystring()
        {
            var queryString = new NameValueCollection();
            queryString.Add("type", "test");
            var factory = new JobSearchQueryFactory();

            var query = factory.CreateFromQueryString(queryString);

            Assert.AreEqual("test", query.JobTypes[0]);
        }

        [Test]
        public void JobOrganisationsAreReadFromQuerystring()
        {
            var queryString = new NameValueCollection();
            queryString.Add("org", "test");
            var factory = new JobSearchQueryFactory();

            var query = factory.CreateFromQueryString(queryString);

            Assert.AreEqual("test", query.Organisations[0]);
        }

        [Test]
        public void SalaryIsFormattedToHouseStyle()
        {
            var queryString = new NameValueCollection();
            queryString.Add("salary", "£10000+to+£14999");
            var factory = new JobSearchQueryFactory();

            var query = factory.CreateFromQueryString(queryString);

            Assert.AreEqual("£10,000 to £14,999", query.SalaryRanges[0]);
        }

        [Test]
        public void MaximumSalaryIsFormattedToHouseStyle()
        {
            var queryString = new NameValueCollection();
            queryString.Add("salary", "£50000+and+over");
            var factory = new JobSearchQueryFactory();

            var query = factory.CreateFromQueryString(queryString);

            Assert.AreEqual("£50,000 and over", query.SalaryRanges[0]);
        }

        [Test]
        public void JobReferenceIsReadFromQuerystring()
        {
            var queryString = new NameValueCollection();
            queryString.Add("ref", "test");
            var factory = new JobSearchQueryFactory();

            var query = factory.CreateFromQueryString(queryString);

            Assert.AreEqual("test", query.JobReference);
        }

        [Test]
        public void JobWorkPatternsAreReadFromQuerystring()
        {
            var queryString = new NameValueCollection();
            queryString.Add("hours", "test");
            var factory = new JobSearchQueryFactory();

            var query = factory.CreateFromQueryString(queryString);

            Assert.AreEqual("test", query.WorkPatterns[0]);
        }

        [Test]
        public void JobsSortOrderIsReadFromQuerystring()
        {
            var queryString = new NameValueCollection();
            queryString.Add("sort", "workpatternascending");
            var factory = new JobSearchQueryFactory();

            var query = factory.CreateFromQueryString(queryString);

            Assert.AreEqual(JobSearchQuery.JobsSortOrder.WorkPatternAscending, query.SortBy);
        }
    }
}
