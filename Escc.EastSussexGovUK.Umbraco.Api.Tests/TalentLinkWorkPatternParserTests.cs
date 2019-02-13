using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Umbraco.Api.Jobs.TalentLink;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Api.Tests
{
    [TestFixture]
    public class TalentLinkWorkPatternParserTests
    {
        [Test]
        public async Task WorkingPatternFullTimeWithHyphenIsParsed()
        {
            var parser = new TalentLinkWorkPatternParser();

            var result = await parser.ParseWorkPattern(Properties.Resources.WorkPatternInBodyText3);

            Assert.AreEqual(true, result.WorkPatterns.Contains(WorkPattern.FULL_TIME));
            Assert.AreEqual(false, result.WorkPatterns.Contains(WorkPattern.PART_TIME));
        }

        [Test]
        public async Task WorkingPatternFullTimeWithoutHyphenIsParsed()
        {
            var parser = new TalentLinkWorkPatternParser();

            var result = await parser.ParseWorkPattern(Properties.Resources.WorkPatternInBodyText1);

            Assert.AreEqual(true, result.WorkPatterns.Contains(WorkPattern.FULL_TIME));
            Assert.AreEqual(false, result.WorkPatterns.Contains(WorkPattern.PART_TIME));
        }

        [Test]
        public async Task WorkingPatternPartTimeWithHyphenIsParsed()
        {
            var parser = new TalentLinkWorkPatternParser();

            var result = await parser.ParseWorkPattern(Properties.Resources.JobAdvert1Html);

            Assert.AreEqual(false, result.WorkPatterns.Contains(WorkPattern.FULL_TIME));
            Assert.AreEqual(true, result.WorkPatterns.Contains(WorkPattern.PART_TIME));
        }

        [Test]
        public async Task WorkingPatternPartTimeWithoutHyphenIsParsed()
        {
            var parser = new TalentLinkWorkPatternParser();

            var result = await parser.ParseWorkPattern(Properties.Resources.WorkPatternInBodyText7);

            Assert.AreEqual(false, result.WorkPatterns.Contains(WorkPattern.FULL_TIME));
            Assert.AreEqual(true, result.WorkPatterns.Contains(WorkPattern.PART_TIME));
        }

        [Test]
        public async Task WorkingPatternPartTimeWithHyphenWithSurroundingTextIsParsed()
        {
            var parser = new TalentLinkWorkPatternParser();

            var result = await parser.ParseWorkPattern(Properties.Resources.WorkPatternInBodyText5);

            Assert.AreEqual(false, result.WorkPatterns.Contains(WorkPattern.FULL_TIME));
            Assert.AreEqual(true, result.WorkPatterns.Contains(WorkPattern.PART_TIME));
        }

        [Test]
        public async Task HoursOfWorkFullTimeWithoutHyphenWithSurroundingTextIsParsed()
        {
            var parser = new TalentLinkWorkPatternParser();

            var result = await parser.ParseWorkPattern(Properties.Resources.WorkPatternInBodyText9);

            Assert.AreEqual(true, result.WorkPatterns.Contains(WorkPattern.FULL_TIME));
            Assert.AreEqual(false, result.WorkPatterns.Contains(WorkPattern.PART_TIME));
        }

        [Test]
        public async Task HoursOfWorkHoursPerWeekIsParsed()
        {
            var parser = new TalentLinkWorkPatternParser();

            var result = await parser.ParseWorkPattern(Properties.Resources.WorkPatternInBodyText4);

            Assert.AreEqual(true, result.WorkPatterns.Contains(WorkPattern.FULL_TIME));
            Assert.AreEqual(false, result.WorkPatterns.Contains(WorkPattern.PART_TIME));
        }

        [Test]
        public async Task HoursOfWorkHoursPerWeekWithSurroundingTextIsParsed()
        {
            var parser = new TalentLinkWorkPatternParser();

            var result = await parser.ParseWorkPattern(Properties.Resources.WorkPatternInBodyText10);

            Assert.AreEqual(true, result.WorkPatterns.Contains(WorkPattern.FULL_TIME));
            Assert.AreEqual(false, result.WorkPatterns.Contains(WorkPattern.PART_TIME));
        }

        [Test]
        public async Task WorkingPatternHoursPerWeekRangeIsParsed()
        {
            var parser = new TalentLinkWorkPatternParser();

            var result = await parser.ParseWorkPattern(Properties.Resources.WorkPatternInBodyText2);

            Assert.AreEqual(true, result.WorkPatterns.Contains(WorkPattern.FULL_TIME));
            Assert.AreEqual(true, result.WorkPatterns.Contains(WorkPattern.PART_TIME));
        }

        [Test]
        public async Task FullOrPartTimeIsParsed()
        {
            var parser = new TalentLinkWorkPatternParser();

            var result = await parser.ParseWorkPattern(Properties.Resources.WorkPatternInBodyText6);

            Assert.AreEqual(true, result.WorkPatterns.Contains(WorkPattern.FULL_TIME));
            Assert.AreEqual(true, result.WorkPatterns.Contains(WorkPattern.PART_TIME));
        }

        [Test]
        public async Task CasualIsParsedAsPartTime()
        {
            var parser = new TalentLinkWorkPatternParser();

            var result = await parser.ParseWorkPattern(Properties.Resources.WorkPatternInBodyText8);

            Assert.AreEqual(false, result.WorkPatterns.Contains(WorkPattern.FULL_TIME));
            Assert.AreEqual(true, result.WorkPatterns.Contains(WorkPattern.PART_TIME));
        }
    }
}
