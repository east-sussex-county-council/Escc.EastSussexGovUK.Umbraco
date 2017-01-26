using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Umbraco.Jobs.TalentLink;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestFixture]
    public class TalentLinkWorkPatternParserTests
    {
        [Test]
        public void WorkingPatternFullTimeWithHyphenIsParsed()
        {
            var parser = new TalentLinkWorkPatternParser();

            var result = parser.ParseWorkPatternFromHtml(Properties.Resources.WorkPatternInBodyText3);

            Assert.AreEqual(true, result.IsFullTime);
            Assert.AreEqual(false, result.IsPartTime);
        }

        [Test]
        public void WorkingPatternFullTimeWithoutHyphenIsParsed()
        {
            var parser = new TalentLinkWorkPatternParser();

            var result = parser.ParseWorkPatternFromHtml(Properties.Resources.WorkPatternInBodyText1);

            Assert.AreEqual(true, result.IsFullTime);
            Assert.AreEqual(false, result.IsPartTime);
        }

        [Test]
        public void WorkingPatternPartTimeWithHyphenIsParsed()
        {
            var parser = new TalentLinkWorkPatternParser();

            var result = parser.ParseWorkPatternFromHtml(Properties.Resources.JobAdvert1Html);

            Assert.AreEqual(false, result.IsFullTime);
            Assert.AreEqual(true, result.IsPartTime);
        }

        [Test]
        public void WorkingPatternPartTimeWithoutHyphenIsParsed()
        {
            var parser = new TalentLinkWorkPatternParser();

            var result = parser.ParseWorkPatternFromHtml(Properties.Resources.WorkPatternInBodyText7);

            Assert.AreEqual(false, result.IsFullTime);
            Assert.AreEqual(true, result.IsPartTime);
        }

        [Test]
        public void WorkingPatternPartTimeWithHyphenWithSurroundingTextIsParsed()
        {
            var parser = new TalentLinkWorkPatternParser();

            var result = parser.ParseWorkPatternFromHtml(Properties.Resources.WorkPatternInBodyText5);

            Assert.AreEqual(false, result.IsFullTime);
            Assert.AreEqual(true, result.IsPartTime);
        }

        [Test]
        public void HoursOfWorkFullTimeWithoutHyphenWithSurroundingTextIsParsed()
        {
            var parser = new TalentLinkWorkPatternParser();

            var result = parser.ParseWorkPatternFromHtml(Properties.Resources.WorkPatternInBodyText9);

            Assert.AreEqual(true, result.IsFullTime);
            Assert.AreEqual(false, result.IsPartTime);
        }

        [Test]
        public void HoursOfWorkHoursPerWeekIsParsed()
        {
            var parser = new TalentLinkWorkPatternParser();

            var result = parser.ParseWorkPatternFromHtml(Properties.Resources.WorkPatternInBodyText4);

            Assert.AreEqual(true, result.IsFullTime);
            Assert.AreEqual(false, result.IsPartTime);
        }

        [Test]
        public void HoursOfWorkHoursPerWeekWithSurroundingTextIsParsed()
        {
            var parser = new TalentLinkWorkPatternParser();

            var result = parser.ParseWorkPatternFromHtml(Properties.Resources.WorkPatternInBodyText10);

            Assert.AreEqual(true, result.IsFullTime);
            Assert.AreEqual(false, result.IsPartTime);
        }

        [Test]
        public void WorkingPatternHoursPerWeekRangeIsParsed()
        {
            var parser = new TalentLinkWorkPatternParser();

            var result = parser.ParseWorkPatternFromHtml(Properties.Resources.WorkPatternInBodyText2);

            Assert.AreEqual(true, result.IsFullTime);
            Assert.AreEqual(true, result.IsPartTime);
        }

        [Test]
        public void FullOrPartTimeIsParsed()
        {
            var parser = new TalentLinkWorkPatternParser();

            var result = parser.ParseWorkPatternFromHtml(Properties.Resources.WorkPatternInBodyText6);

            Assert.AreEqual(true, result.IsFullTime);
            Assert.AreEqual(true, result.IsPartTime);
        }

        [Test]
        public void CasualIsParsedAsPartTime()
        {
            var parser = new TalentLinkWorkPatternParser();

            var result = parser.ParseWorkPatternFromHtml(Properties.Resources.WorkPatternInBodyText8);

            Assert.AreEqual(false, result.IsFullTime);
            Assert.AreEqual(true, result.IsPartTime);
        }
    }
}
