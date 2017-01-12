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
            var expectedValues = new List<JobsLookupValue>()
            {
                new JobsLookupValue() { Id = "9802", Text = "Alfriston" },
                new JobsLookupValue() { Id = "9804", Text = "Bexhill-on-Sea" },
                new JobsLookupValue() { Id = "9808", Text = "Countywide"},
                new JobsLookupValue() { Id = "9813", Text = "Exceat, Nr Seaford"}
            };

            var parser = new JobLookupValuesHtmlParser();
            var parsedValues = parser.ParseLookupValues(Properties.Resources.SearchFieldsOuterHtml, "LOV39");

            Assert.AreEqual(expectedValues[0].Id, parsedValues[0].Id);
            Assert.AreEqual(expectedValues[0].Text, parsedValues[0].Text);
            Assert.AreEqual(expectedValues[1].Id, parsedValues[1].Id);
            Assert.AreEqual(expectedValues[1].Text, parsedValues[1].Text);
            Assert.AreEqual(expectedValues[2].Id, parsedValues[2].Id);
            Assert.AreEqual(expectedValues[2].Text, parsedValues[2].Text);
            Assert.AreEqual(expectedValues[3].Id, parsedValues[3].Id);
            Assert.AreEqual(expectedValues[3].Text, parsedValues[3].Text);
        }

        [Test]
        public void JobTypesAreSelected()
        {
            var expectedValues = new List<JobsLookupValue>()
            {
                new JobsLookupValue() { Id = "9857", Text = "Administration and Clerical" },
                new JobsLookupValue() { Id = "9858", Text = "Analyst" },
                new JobsLookupValue() { Id = "10319", Text = "Children’s - Social Work"},
                new JobsLookupValue() { Id = "10682", Text = "Teaching - Leadership"}
            };

            var parser = new JobLookupValuesHtmlParser();
            var parsedValues = parser.ParseLookupValues(Properties.Resources.SearchFieldsOuterHtml, "LOV40");

            Assert.AreEqual(expectedValues[0].Id, parsedValues[0].Id);
            Assert.AreEqual(expectedValues[0].Text, parsedValues[0].Text);
            Assert.AreEqual(expectedValues[1].Id, parsedValues[1].Id);
            Assert.AreEqual(expectedValues[1].Text, parsedValues[1].Text);
            Assert.AreEqual(expectedValues[2].Id, parsedValues[2].Id);
            Assert.AreEqual(expectedValues[2].Text, parsedValues[2].Text);
            Assert.AreEqual(expectedValues[3].Id, parsedValues[3].Id);
            Assert.AreEqual(expectedValues[3].Text, parsedValues[3].Text);
        }

        [Test]
        public void OrganisationsAreSelected()
        {
            var expectedValues = new List<JobsLookupValue>()
            {
                new JobsLookupValue() { Id = "10220", Text = "Academies" },
                new JobsLookupValue() { Id = "10218", Text = "East Sussex County Council" }
            };

            var parser = new JobLookupValuesHtmlParser();
            var parsedValues = parser.ParseLookupValues(Properties.Resources.SearchFieldsOuterHtml, "LOV52");

            Assert.AreEqual(expectedValues[0].Id, parsedValues[0].Id);
            Assert.AreEqual(expectedValues[0].Text, parsedValues[0].Text);
            Assert.AreEqual(expectedValues[1].Id, parsedValues[1].Id);
            Assert.AreEqual(expectedValues[1].Text, parsedValues[1].Text);
        }


        [Test]
        public void SalaryRangesAreSelected()
        {
            var expectedValues = new List<JobsLookupValue>()
            {
                new JobsLookupValue() { Id = "10065", Text = "£0 to £9,999" },
                new JobsLookupValue() { Id = "10066", Text = "£10,000 to £14,999" },
                new JobsLookupValue() { Id = "10347", Text = "£25,000 - £34,999" },
                new JobsLookupValue() { Id = "10349", Text = "£50,000 and over" },
                new JobsLookupValue() { Id = "10308", Text = "Teachers' Pay Scale" },
                new JobsLookupValue() { Id = "10309", Text = "Teachers' Leadership Pay Scale" }
            };

            var parser = new JobLookupValuesHtmlParser();
            var parsedValues = parser.ParseLookupValues(Properties.Resources.SearchFieldsOuterHtml, "LOV46");

            Assert.AreEqual(expectedValues[0].Id, parsedValues[0].Id);
            Assert.AreEqual(expectedValues[0].Text, parsedValues[0].Text);
            Assert.AreEqual(expectedValues[1].Id, parsedValues[1].Id);
            Assert.AreEqual(expectedValues[1].Text, parsedValues[1].Text);
            Assert.AreEqual(expectedValues[2].Id, parsedValues[2].Id);
            Assert.AreEqual(expectedValues[2].Text, parsedValues[2].Text);
            Assert.AreEqual(expectedValues[3].Id, parsedValues[3].Id);
            Assert.AreEqual(expectedValues[3].Text, parsedValues[3].Text);
            Assert.AreEqual(expectedValues[4].Id, parsedValues[4].Id);
            Assert.AreEqual(expectedValues[4].Text, parsedValues[4].Text);
            Assert.AreEqual(expectedValues[5].Id, parsedValues[5].Id);
            Assert.AreEqual(expectedValues[5].Text, parsedValues[5].Text);
        }


        [Test]
        public void WorkingHoursAreSelected()
        {
            var expectedValues = new List<JobsLookupValue>()
            {
                new JobsLookupValue() { Id = "10098", Text = "Full time" },
                new JobsLookupValue() { Id = "10099", Text = "Part time" }
            };

            var parser = new JobLookupValuesHtmlParser();
            var parsedValues = parser.ParseLookupValues(Properties.Resources.SearchFieldsOuterHtml, "LOV50");

            Assert.AreEqual(expectedValues[0].Id, parsedValues[0].Id);
            Assert.AreEqual(expectedValues[0].Text, parsedValues[0].Text);
            Assert.AreEqual(expectedValues[1].Id, parsedValues[1].Id);
            Assert.AreEqual(expectedValues[1].Text, parsedValues[1].Text);
        }
    }

}
