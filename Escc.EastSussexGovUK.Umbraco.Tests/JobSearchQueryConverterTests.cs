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
    public class JobSearchQueryConverterTests
    {
        [Test]
        public void KeywordsArePopulatedFromCollection()
        {
            var collection = new NameValueCollection();
            collection["keywords"] = "this is a test";
            var converter = new JobSearchQueryConverter();

            var query = converter.ToQuery(collection);

            Assert.AreEqual("this is a test", query.Keywords);
        }

        [Test]
        public void KeywordsArePopulatedFromQuery()
        {
            var query = new JobSearchQuery() { Keywords = "this is a test" };
            var converter = new JobSearchQueryConverter();

            var collection = converter.ToCollection(query);

            Assert.AreEqual("this is a test", collection["keywords"]);
        }

        [Test]
        public void ReferenceIsPopulatedFromCollection()
        {
            var collection = new NameValueCollection();
            collection["ref"] = "ABC1234";
            var converter = new JobSearchQueryConverter();

            var query = converter.ToQuery(collection);

            Assert.AreEqual("ABC1234", query.JobReference);
        }

        [Test]
        public void ReferenceIsPopulatedFromQuery()
        {
            var query = new JobSearchQuery() { JobReference = "ABC1234" };
            var converter = new JobSearchQueryConverter();

            var collection = converter.ToCollection(query);

            Assert.AreEqual("ABC1234", collection["ref"]);
        }

        [Test]
        public void JobTypeIsPopulatedFromCollection()
        {
            var collection = new NameValueCollection();
            collection["jobtypes"] = "Example job type";
            var converter = new JobSearchQueryConverter();

            var query = converter.ToQuery(collection);

            Assert.AreEqual("Example job type", query.JobTypes[0]);
        }

        [Test]
        public void JobTypeIsPopulatedFromQuery()
        {
            var query = new JobSearchQuery();
            query.JobTypes.Add("Example job type");
            var converter = new JobSearchQueryConverter();

            var collection = converter.ToCollection(query);

            Assert.AreEqual("Example job type", collection["jobtypes"]);
        }

        [Test]
        public void LocationIsPopulatedFromCollection()
        {
            var collection = new NameValueCollection();
            collection["locations"] = "Example location";
            var converter = new JobSearchQueryConverter();

            var query = converter.ToQuery(collection);

            Assert.AreEqual("Example location", query.Locations[0]);
        }

        [Test]
        public void LocationIsPopulatedFromQuery()
        {
            var query = new JobSearchQuery();
            query.Locations.Add("Example location");
            var converter = new JobSearchQueryConverter();

            var collection = converter.ToCollection(query);

            Assert.AreEqual("Example location", collection["locations"]);
        }

        [Test]
        public void OrganisationIsPopulatedFromCollection()
        {
            var collection = new NameValueCollection();
            collection["org"] = "Example organisation";
            var converter = new JobSearchQueryConverter();

            var query = converter.ToQuery(collection);

            Assert.AreEqual("Example organisation", query.Organisations[0]);
        }

        [Test]
        public void OrganisationIsPopulatedFromQuery()
        {
            var query = new JobSearchQuery();
            query.Organisations.Add("Example organisation");
            var converter = new JobSearchQueryConverter();

            var collection = converter.ToCollection(query);

            Assert.AreEqual("Example organisation", collection["org"]);
        }

        [Test]
        public void SalaryRangeIsPopulatedFromCollection()
        {
            var collection = new NameValueCollection();
            collection["salaryranges"] = "£10000 to £20000";
            var converter = new JobSearchQueryConverter();

            var query = converter.ToQuery(collection);

            Assert.AreEqual("£10,000 to £20,000", query.SalaryRanges[0]);
        }

        [Test]
        public void SalaryRangeIsPopulatedFromQuery()
        {
            var query = new JobSearchQuery();
            query.SalaryRanges.Add("£10000 to £20000");
            var converter = new JobSearchQueryConverter();

            var collection = converter.ToCollection(query);

            Assert.AreEqual("£10000 to £20000", collection["salaryranges"]);
        }

        [Test]
        public void MaximumSalaryRangeIsPopulatedFromQuery()
        {
            var query = new JobSearchQuery();
            query.SalaryRanges.Add("£50000 and over");
            var converter = new JobSearchQueryConverter();

            var collection = converter.ToCollection(query);

            Assert.AreEqual("£50000 and over", collection["salaryranges"]);
        }

        [Test]
        public void ContractTypeIsPopulatedFromCollection()
        {
            var collection = new NameValueCollection();
            collection["contracttypes"] = "Contract type";
            var converter = new JobSearchQueryConverter();

            var query = converter.ToQuery(collection);

            Assert.AreEqual("Contract type", query.ContractTypes[0]);
        }

        [Test]
        public void ContractTypeIsPopulatedFromQuery()
        {
            var query = new JobSearchQuery();
            query.ContractTypes.Add("Contract type");
            var converter = new JobSearchQueryConverter();

            var collection = converter.ToCollection(query);

            Assert.AreEqual("Contract type", collection["contracttypes"]);
        }

        [Test]
        public void WorkingPatternIsPopulatedFromCollection()
        {
            var collection = new NameValueCollection();
            collection["workpatterns"] = "Working pattern";
            var converter = new JobSearchQueryConverter();

            var query = converter.ToQuery(collection);

            Assert.AreEqual("Working pattern", query.WorkPatterns[0]);
        }

        [Test]
        public void WorkingPatternIsPopulatedFromQuery()
        {
            var query = new JobSearchQuery();
            query.WorkPatterns.Add("Working pattern");
            var converter = new JobSearchQueryConverter();

            var collection = converter.ToCollection(query);

            Assert.AreEqual("Working pattern", collection["workpatterns"]);
        }
    }
}
