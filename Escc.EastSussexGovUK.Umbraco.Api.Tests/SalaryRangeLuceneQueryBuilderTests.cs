using Escc.EastSussexGovUK.Umbraco.Api.Jobs.Examine;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Api.Tests
{
    [TestFixture]
    public class SalaryRangeLuceneQueryBuilderTests
    {
        [Test]
        public void RangeBetweenTwoNumbersGeneratesQueryThatIncludesJobsOverlappingTheLowerAndUpperBoundary()
        {
            var range = "£25,000 to £34,999";
            var builder = new SalaryRangeLuceneQueryBuilder();

            var query = builder.SalaryIsWithinAnyOfTheseRanges(new List<string>() { range });

            Assert.AreEqual(" +(+((+(+salaryMin:[002500000 TO 999999999] +salaryMin:[000000000 TO 003499999])) (+(+salaryMax:[002500000 TO 999999999] +salaryMax:[000000000 TO 003499999])) (+(+salaryMin:[000000000 TO 002500099] +salaryMax:[003499900 TO 999999999]))))", query);
        }

        [Test]
        public void RangeWithMinimumSalaryOnlyGeneratesQueryThatIncludesJobsOverlappingTheLowerBoundary()
        {
            var range = "£50,000 and over";
            var builder = new SalaryRangeLuceneQueryBuilder();

            var query = builder.SalaryIsWithinAnyOfTheseRanges(new List<string>() { range });

            Assert.AreEqual(" +(+((+(+salaryMin:[005000000 TO 999999999] +salaryMin:[000000000 TO 999999999])) (+(+salaryMax:[005000000 TO 999999999] +salaryMax:[000000000 TO 999999999]))))", query);
        }

        [Test]
        public void MultipleRangesGeneratesQueryThatIncludesJobsOverlappingTheLowerAndUpperBoundaryOfEitherRange()
        {
            var ranges = new List<string>() { "£25,000 to £34,999", "£50,000 and over" };
            var builder = new SalaryRangeLuceneQueryBuilder();

            var query = builder.SalaryIsWithinAnyOfTheseRanges(ranges);

            Assert.AreEqual(" +(+((+(+salaryMin:[002500000 TO 999999999] +salaryMin:[000000000 TO 003499999])) (+(+salaryMax:[002500000 TO 999999999] +salaryMax:[000000000 TO 003499999])) (+(+salaryMin:[000000000 TO 002500099] +salaryMax:[003499900 TO 999999999])) (+(+salaryMin:[005000000 TO 999999999] +salaryMin:[000000000 TO 999999999])) (+(+salaryMax:[005000000 TO 999999999] +salaryMax:[000000000 TO 999999999]))))", query);
        }
    }
}
