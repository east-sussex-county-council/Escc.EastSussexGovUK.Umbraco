using Escc.EastSussexGovUK.Umbraco.Jobs.Examine;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Tests
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

            Assert.AreEqual(" +(+((+(+salaryMin:[0025000 TO 9999999] +salaryMin:[0000000 TO 0034999])) (+(+salaryMax:[0025000 TO 9999999] +salaryMax:[0000000 TO 0034999])) (+(+salaryMin:[0000000 TO 0025000] +salaryMax:[0034999 TO 9999999]))))", query);
        }

        [Test]
        public void RangeWithMinimumSalaryOnlyGeneratesQueryThatIncludesJobsOverlappingTheLowerBoundary()
        {
            var range = "£50,000 and over";
            var builder = new SalaryRangeLuceneQueryBuilder();

            var query = builder.SalaryIsWithinAnyOfTheseRanges(new List<string>() { range });

            Assert.AreEqual(" +(+((+(+salaryMin:[0050000 TO 9999999] +salaryMin:[0000000 TO 9999999])) (+(+salaryMax:[0050000 TO 9999999] +salaryMax:[0000000 TO 9999999]))))", query);
        }

        [Test]
        public void MultipleRangesGeneratesQueryThatIncludesJobsOverlappingTheLowerAndUpperBoundaryOfEitherRange()
        {
            var ranges = new List<string>() { "£25,000 to £34,999", "£50,000 and over" };
            var builder = new SalaryRangeLuceneQueryBuilder();

            var query = builder.SalaryIsWithinAnyOfTheseRanges(ranges);

            Assert.AreEqual(" +(+((+(+salaryMin:[0025000 TO 9999999] +salaryMin:[0000000 TO 0034999])) (+(+salaryMax:[0025000 TO 9999999] +salaryMax:[0000000 TO 0034999])) (+(+salaryMin:[0000000 TO 0025000] +salaryMax:[0034999 TO 9999999])) (+(+salaryMin:[0050000 TO 9999999] +salaryMin:[0000000 TO 9999999])) (+(+salaryMax:[0050000 TO 9999999] +salaryMax:[0000000 TO 9999999]))))", query);
        }
    }
}
