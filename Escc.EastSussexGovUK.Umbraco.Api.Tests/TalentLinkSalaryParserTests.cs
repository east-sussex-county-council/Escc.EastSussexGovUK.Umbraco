using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Umbraco.Api.Jobs.TalentLink;
using HtmlAgilityPack;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Api.Tests
{
    [TestFixture]
    public class TalentLinkSalaryParserTests
    {
        [Test]
        public async Task TwoNumbersGbpYearIsParsed()
        {
            var parseThis = "16875 - 17891 GBP Year";

            var parser = new TalentLinkSalaryFromDescriptionParser();
            var result = await parser.ParseSalary(parseThis);

            Assert.AreEqual(16875, result.MinimumSalary);
            Assert.AreEqual(17891, result.MaximumSalary);
        }

        [Test]
        public async Task OneNumberGbpYearIsParsed()
        {
            var parseThis = "29033  GBP Year";

            var parser = new TalentLinkSalaryFromDescriptionParser();
            var result = await parser.ParseSalary(parseThis);

            Assert.AreEqual(29033, result.MinimumSalary);
            Assert.AreEqual(29033, result.MaximumSalary);
        }

        [Test]
        public async Task PrefixThenOneNumberGbpYearIsParsed()
        {
            var parseThis = "To 29033  GBP Year";

            var parser = new TalentLinkSalaryFromDescriptionParser();
            var result = await parser.ParseSalary(parseThis);

            Assert.AreEqual(29033, result.MinimumSalary);
            Assert.AreEqual(29033, result.MaximumSalary);
        }

        [Test]
        public async Task SalaryWithPenceIsParsed()
        {
            var parseThis = "6752.5  - 6752.5  GBP  Year";

            var parser = new TalentLinkSalaryFromDescriptionParser();
            var result = await parser.ParseSalary(parseThis);

            Assert.AreEqual(6752, result.MinimumSalary);
            Assert.AreEqual(6752, result.MaximumSalary);
        }

        [TestCase("£20,000 to £24,999", 20000, 24999)]
        [TestCase("£25,000 - £34,999", 25000, 34999)]
        public async Task TwoNumbersWithCurrencyIsParsed(string parseThis, int minimum, int maximum)
        {
            var parser = new TalentLinkSalaryFromDescriptionParser();
            var result = await parser.ParseSalary(parseThis);

            Assert.AreEqual(minimum, result.MinimumSalary);
            Assert.AreEqual(maximum, result.MaximumSalary);
        }

        [Test]
        public async Task TwoNumbersAndOverIsParsed()
        {
            var parseThis = "£50,000 and over";

            var parser = new TalentLinkSalaryFromDescriptionParser();
            var result = await parser.ParseSalary(parseThis);

            Assert.AreEqual(50000, result.MinimumSalary);
            Assert.AreEqual(null, result.MaximumSalary);
        }

        [Test]
        public async Task UnrecognisedTextAssumedToBeScaleName()
        {
            var parseThis = "Teachers' Pay Scale";

            var parser = new TalentLinkSalaryFromDescriptionParser();
            var result = await parser.ParseSalary(parseThis);

            Assert.AreEqual("Teachers' Pay Scale", result.SalaryRange);
        }

        [Test]
        public async Task SalaryPrefixInBodyTextWithSpacesInTheNumbersIsParsed()
        {
            var parser = new TalentLinkSalaryFromHtmlParser(new TalentLinkSalaryFromDescriptionParser());
            var result = await parser.ParseSalary(Properties.Resources.TalentLinkSalaryInBodyText1);

            Assert.AreEqual(38984, result.MinimumSalary);
            Assert.AreEqual(43022, result.MaximumSalary);
        }

        [Test]
        public async Task SalaryPrefixInBodyTextWithNoNumbersButNumbersWithinTheSameParentElementIsParsedAsTextOnly()
        {
            var parser = new TalentLinkSalaryFromHtmlParser(new TalentLinkSalaryFromDescriptionParser());
            var result = await parser.ParseSalary(Properties.Resources.TalentLinkSalaryInBodyText3);

            Assert.AreEqual("To be negotiated", result.SalaryRange);
            Assert.AreEqual(null, result.MinimumSalary);
            Assert.AreEqual(null, result.MaximumSalary);
        }

        [Test]
        public async Task SalaryPrefixInBodyTextWithNoNumbersIsParsedAsTextOnly()
        {
            var parser = new TalentLinkSalaryFromHtmlParser(new TalentLinkSalaryFromDescriptionParser());
            var result = await parser.ParseSalary(Properties.Resources.TalentLinkSalaryInBodyText4);

            Assert.AreEqual("Dependant on experience, knowledge and qualifications", result.SalaryRange);
            Assert.AreEqual(null, result.MinimumSalary);
            Assert.AreEqual(null, result.MaximumSalary);
        }

        [Test]
        public async Task SalaryInBodyTextFollowedByPerAnnumIsParsed()
        {
            var parser = new TalentLinkSalaryFromHtmlParser(new TalentLinkSalaryFromDescriptionParser());
            var result = await parser.ParseSalary(Properties.Resources.TalentLinkSalaryInBodyText2);

            Assert.AreEqual(16692, result.MinimumSalary);
            Assert.AreEqual(17808, result.MaximumSalary);
        }

        [Test]
        public async Task SalaryInBodyTextFollowedByPerHourIsParsedAsText()
        {
            var parser = new TalentLinkSalaryFromHtmlParser(new TalentLinkSalaryFromDescriptionParser());
            var result = await parser.ParseSalary(Properties.Resources.TalentLinkSalaryInBodyText5);

            Assert.AreEqual("£11.79 - £12.51 per hour", result.SalaryRange);
            Assert.IsNull(result.MinimumSalary);
            Assert.IsNull(result.MaximumSalary);
        }

        [Test]
        public async Task SalaryInBodyTextFollowedByNotesIsParsedWithoutNotes()
        {
            var parser = new TalentLinkSalaryFromHtmlParser(new TalentLinkSalaryFromDescriptionParser());
            var result = await parser.ParseSalary(Properties.Resources.TalentLinkSalaryInBodyText6);

            Assert.AreEqual("£22,912 rising to £33,487 per annum (Gildredge House Main Pay Scale points M1-M6, 2018-19)", result.SalaryRange);
        }

        [Test]
        public async Task HourlyRateIsNullIfNotFound()
        {
            var parser = new TalentLinkSalaryFromHtmlParser(new TalentLinkSalaryFromDescriptionParser());
            var result = await parser.ParseSalary(Properties.Resources.TalentLinkSalaryInBodyText6);

            Assert.IsNull(result.MinimumHourlyRate);
        }

        [Test]
        public async Task HourlyRateInBodyTextIsParsed()
        {
            var parser = new TalentLinkSalaryFromHtmlParser(new TalentLinkSalaryFromDescriptionParser());
            var result = await parser.ParseSalary(Properties.Resources.HourlyRateInBodyText1);

            Assert.AreEqual(9.68, result.MinimumHourlyRate);
        }
    }
}
