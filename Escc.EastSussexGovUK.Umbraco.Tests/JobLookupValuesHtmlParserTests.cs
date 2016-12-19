using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using HtmlAgilityPack;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    class JobLookupValuesHtmlParserTests
    {
        [Test]
        public void JobLocationsAreSelected()
        {
            var expectedValues = new Dictionary<int, string>()
            {
                { 9802, "Alfriston" },
                { 9804, "Bexhill-on-Sea" },
                { 9808, "Countywide"},
                { 9813, "Exceat, Nr Seaford"}
            };

            var parser = new JobLookupValuesHtmlParser();
            var parsedValues = parser.ParseLookupValues(Properties.Resources.SearchFieldsOuterHtml, "LOV39");

            Assert.AreEqual(expectedValues, parsedValues);
        }

        [Test]
        public void JobTypesAreSelected()
        {
            var expectedValues = new Dictionary<int,string>()
            {
                { 9857, "Administration and Clerical" },
                { 9858, "Analyst" },
                { 10319, "Children’s - Social Work"},
                { 10682, "Teaching - Leadership"}
            };

            var parser = new JobLookupValuesHtmlParser();
            var parsedValues = parser.ParseLookupValues(Properties.Resources.SearchFieldsOuterHtml, "LOV40");

            Assert.AreEqual(expectedValues, parsedValues);
        }

        [Test]
        public void OrganisationsAreSelected()
        {
            var expectedValues = new Dictionary<int, string>()
            {
                { 10220, "Academies" },
                { 10218, "East Sussex County Council" }
            };

            var parser = new JobLookupValuesHtmlParser();
            var parsedValues = parser.ParseLookupValues(Properties.Resources.SearchFieldsOuterHtml, "LOV52");

            Assert.AreEqual(expectedValues, parsedValues);
        }


        [Test]
        public void SalaryRangesAreSelected()
        {
            var expectedValues = new Dictionary<int, string>()
            {
                { 10065, "£0 to £9,999" },
                { 10066, "£10,000 to £14,999" },
                { 10347, "£25,000 - £34,999" },
                { 10349, "£50,000 and over" },
                { 10308, "Teachers' Pay Scale" },
                { 10309, "Teachers' Leadership Pay Scale" }
            };

            var parser = new JobLookupValuesHtmlParser();
            var parsedValues = parser.ParseLookupValues(Properties.Resources.SearchFieldsOuterHtml, "LOV46");

            Assert.AreEqual(expectedValues, parsedValues);
        }


        [Test]
        public void WorkingHoursAreSelected()
        {
            var expectedValues = new Dictionary<int, string>()
            {
                { 10098, "Full time" },
                { 10099, "Part time" }
            };

            var parser = new JobLookupValuesHtmlParser();
            var parsedValues = parser.ParseLookupValues(Properties.Resources.SearchFieldsOuterHtml, "LOV50");

            Assert.AreEqual(expectedValues, parsedValues);
        }
    }

}
