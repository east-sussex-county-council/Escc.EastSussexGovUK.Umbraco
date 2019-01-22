using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Umbraco.Api.Jobs.TribePad;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using Moq;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Api.Tests
{
    [TestFixture]
    public class TribePadWorkPatternParserTests
    {
        [Test]
        public async Task FullTimeWorkPatternParsed()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            lookupValuesProvider.Setup(x => x.ReadWorkPatterns()).Returns(Task.FromResult(new List<JobsLookupValue>()
            {
                new JobsLookupValue() { FieldId = "13", LookupValueId = "16", Text = "Full Time" }
            } as IList<JobsLookupValue>));

            var parser = new TribePadWorkPatternParser(lookupValuesProvider.Object);

            var result = await parser.ParseWorkPattern(Properties.Resources.TribePadWorkPatternFullTime);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.WorkPatterns.Contains(WorkPattern.FULL_TIME));
            Assert.IsFalse(result.WorkPatterns.Contains(WorkPattern.PART_TIME));
        }

        [Test]
        public async Task WorkPatternMissingHoursIndicateFullTimeParsed()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var parser = new TribePadWorkPatternParser(lookupValuesProvider.Object);

            var result = await parser.ParseWorkPattern(Properties.Resources.TribePadWorkPatternMissing37Hours);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.WorkPatterns.Contains(WorkPattern.FULL_TIME));
            Assert.IsFalse(result.WorkPatterns.Contains(WorkPattern.PART_TIME));
        }

        [Test]
        public async Task HoursPerWeekParsed()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var parser = new TribePadWorkPatternParser(lookupValuesProvider.Object);

            var result = await parser.ParseWorkPattern(Properties.Resources.TribePadWorkPatternFullTime);

            Assert.IsNotNull(result);
            Assert.AreEqual(37, result.HoursPerWeek);
        }
    }
}
