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
        public void WorkingPatternFullTimeWithHyphenIsParsed()
        {
            var parser = new TalentLinkWorkPatternParser();

            var result = parser.ParseWorkPattern(Properties.Resources.WorkPatternInBodyText3).Result;

            Assert.AreEqual(true, result.WorkPatterns.Contains(WorkPattern.FULL_TIME));
            Assert.AreEqual(false, result.WorkPatterns.Contains(WorkPattern.PART_TIME));
        }

        [Test]
        public void WorkingPatternFullTimeWithoutHyphenIsParsed()
        {
            var parser = new TalentLinkWorkPatternParser();

            var result = parser.ParseWorkPattern(Properties.Resources.WorkPatternInBodyText1).Result;

            Assert.AreEqual(true, result.WorkPatterns.Contains(WorkPattern.FULL_TIME));
            Assert.AreEqual(false, result.WorkPatterns.Contains(WorkPattern.PART_TIME));
        }

        [Test]
        public void WorkingPatternPartTimeWithHyphenIsParsed()
        {
            var parser = new TalentLinkWorkPatternParser();

            var result = parser.ParseWorkPattern(Properties.Resources.JobAdvert1Html).Result;

            Assert.AreEqual(false, result.WorkPatterns.Contains(WorkPattern.FULL_TIME));
            Assert.AreEqual(true, result.WorkPatterns.Contains(WorkPattern.PART_TIME));
        }

        [Test]
        public void WorkingPatternPartTimeWithoutHyphenIsParsed()
        {
            var parser = new TalentLinkWorkPatternParser();

            var result = parser.ParseWorkPattern(Properties.Resources.WorkPatternInBodyText7).Result;

            Assert.AreEqual(false, result.WorkPatterns.Contains(WorkPattern.FULL_TIME));
            Assert.AreEqual(true, result.WorkPatterns.Contains(WorkPattern.PART_TIME));
        }

        [Test]
        public void WorkingPatternPartTimeWithHyphenWithSurroundingTextIsParsed()
        {
            var parser = new TalentLinkWorkPatternParser();

            var result = parser.ParseWorkPattern(Properties.Resources.WorkPatternInBodyText5).Result;

            Assert.AreEqual(false, result.WorkPatterns.Contains(WorkPattern.FULL_TIME));
            Assert.AreEqual(true, result.WorkPatterns.Contains(WorkPattern.PART_TIME));
        }

        [Test]
        public void HoursOfWorkFullTimeWithoutHyphenWithSurroundingTextIsParsed()
        {
            var parser = new TalentLinkWorkPatternParser();

            var result = parser.ParseWorkPattern(Properties.Resources.WorkPatternInBodyText9).Result;

            Assert.AreEqual(true, result.WorkPatterns.Contains(WorkPattern.FULL_TIME));
            Assert.AreEqual(false, result.WorkPatterns.Contains(WorkPattern.PART_TIME));
        }

        [Test]
        public void HoursOfWorkHoursPerWeekIsParsed()
        {
            var parser = new TalentLinkWorkPatternParser();

            var result = parser.ParseWorkPattern(Properties.Resources.WorkPatternInBodyText4).Result;

            Assert.AreEqual(true, result.WorkPatterns.Contains(WorkPattern.FULL_TIME));
            Assert.AreEqual(false, result.WorkPatterns.Contains(WorkPattern.PART_TIME));
        }

        [Test]
        public void HoursOfWorkHoursPerWeekWithSurroundingTextIsParsed()
        {
            var parser = new TalentLinkWorkPatternParser();

            var result = parser.ParseWorkPattern(Properties.Resources.WorkPatternInBodyText10).Result;

            Assert.AreEqual(true, result.WorkPatterns.Contains(WorkPattern.FULL_TIME));
            Assert.AreEqual(false, result.WorkPatterns.Contains(WorkPattern.PART_TIME));
        }

        [Test]
        public void WorkingPatternHoursPerWeekRangeIsParsed()
        {
            var parser = new TalentLinkWorkPatternParser();

            var result = parser.ParseWorkPattern(Properties.Resources.WorkPatternInBodyText2).Result;

            Assert.AreEqual(true, result.WorkPatterns.Contains(WorkPattern.FULL_TIME));
            Assert.AreEqual(true, result.WorkPatterns.Contains(WorkPattern.PART_TIME));
        }

        [Test]
        public void FullOrPartTimeIsParsed()
        {
            var parser = new TalentLinkWorkPatternParser();

            var result = parser.ParseWorkPattern(Properties.Resources.WorkPatternInBodyText6).Result;

            Assert.AreEqual(true, result.WorkPatterns.Contains(WorkPattern.FULL_TIME));
            Assert.AreEqual(true, result.WorkPatterns.Contains(WorkPattern.PART_TIME));
        }

        [Test]
        public void CasualIsParsedAsPartTime()
        {
            var parser = new TalentLinkWorkPatternParser();

            var result = parser.ParseWorkPattern(Properties.Resources.WorkPatternInBodyText8).Result;

            Assert.AreEqual(false, result.WorkPatterns.Contains(WorkPattern.FULL_TIME));
            Assert.AreEqual(true, result.WorkPatterns.Contains(WorkPattern.PART_TIME));
        }
    }
}
