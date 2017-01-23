using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using Escc.EastSussexGovUK.Umbraco.Jobs.TalentLink;
using HtmlAgilityPack;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestFixture]
    public class SalaryParserTests
    {
        [Test]
        public void TwoNumbersGbpYearIsParsed()
        {
            var parseThis = "16875 - 17891 GBP Year";

            var parser = new TalentLinkSalaryParser();
            var result = parser.ParseSalaryFromDescription(parseThis);

            Assert.AreEqual(16875, result.MinimumSalary);
            Assert.AreEqual(17891, result.MaximumSalary);
        }

        [Test]
        public void OneNumberGbpYearIsParsed()
        {
            var parseThis = "29033  GBP Year";

            var parser = new TalentLinkSalaryParser();
            var result = parser.ParseSalaryFromDescription(parseThis);

            Assert.AreEqual(29033, result.MinimumSalary);
            Assert.AreEqual(29033, result.MaximumSalary);
        }

        [Test]
        public void PrefixThenOneNumberGbpYearIsParsed()
        {
            var parseThis = "To 29033  GBP Year";

            var parser = new TalentLinkSalaryParser();
            var result = parser.ParseSalaryFromDescription(parseThis);

            Assert.AreEqual(29033, result.MinimumSalary);
            Assert.AreEqual(29033, result.MaximumSalary);
        }

        [TestCase("£20,000 to £24,999", 20000, 24999)]
        [TestCase("£25,000 - £34,999", 25000, 34999)]
        public void TwoNumbersWithCurrencyIsParsed(string parseThis, int minimum, int maximum)
        {
            var parser = new TalentLinkSalaryParser();
            var result = parser.ParseSalaryFromDescription(parseThis);

            Assert.AreEqual(minimum, result.MinimumSalary);
            Assert.AreEqual(maximum, result.MaximumSalary);
        }

        [Test]
        public void TwoNumbersAndOverIsParsed()
        {
            var parseThis = "£50,000 and over";

            var parser = new TalentLinkSalaryParser();
            var result = parser.ParseSalaryFromDescription(parseThis);

            Assert.AreEqual(50000, result.MinimumSalary);
            Assert.AreEqual(null, result.MaximumSalary);
        }

        [Test]
        public void UnrecognisedTextAssumedToBeScaleName()
        {
            var parseThis = "Teachers' Pay Scale";

            var parser = new TalentLinkSalaryParser();
            var result = parser.ParseSalaryFromDescription(parseThis);

            Assert.AreEqual("Teachers' Pay Scale", result.SalaryRange);
        }

        [Test]
        public void SalaryPrefixInBodyTextWithSpacesInTheNumbersIsParsed()
        {
            var parseThis = new HtmlDocument();
            parseThis.LoadHtml(Properties.Resources.SalaryInBodyText1);

            var parser = new TalentLinkSalaryParser();
            var result = parser.ParseSalaryFromHtml(parseThis);

            Assert.AreEqual(38984, result.MinimumSalary);
            Assert.AreEqual(43022, result.MaximumSalary);
        }

        [Test]
        public void SalaryPrefixInBodyTextWithNoNumbersIsNotParsed()
        {
            var parseThis = new HtmlDocument();
            parseThis.LoadHtml(Properties.Resources.SalaryInBodyText3);

            var parser = new TalentLinkSalaryParser();
            var result = parser.ParseSalaryFromHtml(parseThis);

            Assert.AreEqual(null, result.SalaryRange);
            Assert.AreEqual(null, result.MinimumSalary);
            Assert.AreEqual(null, result.MaximumSalary);
        }

        [Test]
        public void SalaryInBodyTextFollowedByPerAnnumIsParsed()
        {
            var parseThis = new HtmlDocument();
            parseThis.LoadHtml(Properties.Resources.SalaryInBodyText2);

            var parser = new TalentLinkSalaryParser();
            var result = parser.ParseSalaryFromHtml(parseThis);

            Assert.AreEqual(16692, result.MinimumSalary);
            Assert.AreEqual(17808, result.MaximumSalary);
        }
    }
}
