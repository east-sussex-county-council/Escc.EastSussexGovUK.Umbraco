using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Umbraco.Api.Jobs.TalentLink;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using HtmlAgilityPack;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Api.Tests
{
    [TestFixture]
    public class TalentLinkLookupValuesHtmlParserTests
    {
        [Test]
        public void JobLocationsAreSelected()
        {
            var expectedValues = new List<JobsLookupValue>()
            {
                new JobsLookupValue() { LookupValueId = "9802", Text = "Alfriston" },
                new JobsLookupValue() { LookupValueId = "9804", Text = "Bexhill-on-Sea" },
                new JobsLookupValue() { LookupValueId = "9808", Text = "Countywide"},
                new JobsLookupValue() { LookupValueId = "9813", Text = "Exceat, Nr Seaford"}
            };

            var parser = new TalentLinkLookupValuesHtmlParser();
            var parsedValues = parser.ParseLookupValues(Properties.Resources.TalentLinkSearchFieldsOuterHtml, "LOV39");

            Assert.AreEqual(expectedValues[0].LookupValueId, parsedValues[0].LookupValueId);
            Assert.AreEqual(expectedValues[0].Text, parsedValues[0].Text);
            Assert.AreEqual(expectedValues[1].LookupValueId, parsedValues[1].LookupValueId);
            Assert.AreEqual(expectedValues[1].Text, parsedValues[1].Text);
            Assert.AreEqual(expectedValues[2].LookupValueId, parsedValues[2].LookupValueId);
            Assert.AreEqual(expectedValues[2].Text, parsedValues[2].Text);
            Assert.AreEqual(expectedValues[3].LookupValueId, parsedValues[3].LookupValueId);
            Assert.AreEqual(expectedValues[3].Text, parsedValues[3].Text);
        }

        [Test]
        public void JobTypesAreSelected()
        {
            var expectedValues = new List<JobsLookupValue>()
            {
                new JobsLookupValue() { LookupValueId = "9857", Text = "Administration and Clerical" },
                new JobsLookupValue() { LookupValueId = "9858", Text = "Analyst" },
                new JobsLookupValue() { LookupValueId = "10319", Text = "Children’s - Social Work"},
                new JobsLookupValue() { LookupValueId = "10682", Text = "Teaching - Leadership"}
            };

            var parser = new TalentLinkLookupValuesHtmlParser();
            var parsedValues = parser.ParseLookupValues(Properties.Resources.TalentLinkSearchFieldsOuterHtml, "LOV40");

            Assert.AreEqual(expectedValues[0].LookupValueId, parsedValues[0].LookupValueId);
            Assert.AreEqual(expectedValues[0].Text, parsedValues[0].Text);
            Assert.AreEqual(expectedValues[1].LookupValueId, parsedValues[1].LookupValueId);
            Assert.AreEqual(expectedValues[1].Text, parsedValues[1].Text);
            Assert.AreEqual(expectedValues[2].LookupValueId, parsedValues[2].LookupValueId);
            Assert.AreEqual(expectedValues[2].Text, parsedValues[2].Text);
            Assert.AreEqual(expectedValues[3].LookupValueId, parsedValues[3].LookupValueId);
            Assert.AreEqual(expectedValues[3].Text, parsedValues[3].Text);
        }

        [Test]
        public void OrganisationsAreSelected()
        {
            var expectedValues = new List<JobsLookupValue>()
            {
                new JobsLookupValue() { LookupValueId = "10220", Text = "Academies" },
                new JobsLookupValue() { LookupValueId = "10218", Text = "East Sussex County Council" }
            };

            var parser = new TalentLinkLookupValuesHtmlParser();
            var parsedValues = parser.ParseLookupValues(Properties.Resources.TalentLinkSearchFieldsOuterHtml, "LOV52");

            Assert.AreEqual(expectedValues[0].LookupValueId, parsedValues[0].LookupValueId);
            Assert.AreEqual(expectedValues[0].Text, parsedValues[0].Text);
            Assert.AreEqual(expectedValues[1].LookupValueId, parsedValues[1].LookupValueId);
            Assert.AreEqual(expectedValues[1].Text, parsedValues[1].Text);
        }


        [Test]
        public void SalaryRangesAreSelected()
        {
            var expectedValues = new List<JobsLookupValue>()
            {
                new JobsLookupValue() { LookupValueId = "10065", Text = "£0 to £9,999" },
                new JobsLookupValue() { LookupValueId = "10066", Text = "£10,000 to £14,999" },
                new JobsLookupValue() { LookupValueId = "10347", Text = "£25,000 - £34,999" },
                new JobsLookupValue() { LookupValueId = "10349", Text = "£50,000 and over" },
                new JobsLookupValue() { LookupValueId = "10308", Text = "Teachers' Pay Scale" },
                new JobsLookupValue() { LookupValueId = "10309", Text = "Teachers' Leadership Pay Scale" }
            };

            var parser = new TalentLinkLookupValuesHtmlParser();
            var parsedValues = parser.ParseLookupValues(Properties.Resources.TalentLinkSearchFieldsOuterHtml, "LOV46");

            Assert.AreEqual(expectedValues[0].LookupValueId, parsedValues[0].LookupValueId);
            Assert.AreEqual(expectedValues[0].Text, parsedValues[0].Text);
            Assert.AreEqual(expectedValues[1].LookupValueId, parsedValues[1].LookupValueId);
            Assert.AreEqual(expectedValues[1].Text, parsedValues[1].Text);
            Assert.AreEqual(expectedValues[2].LookupValueId, parsedValues[2].LookupValueId);
            Assert.AreEqual(expectedValues[2].Text, parsedValues[2].Text);
            Assert.AreEqual(expectedValues[3].LookupValueId, parsedValues[3].LookupValueId);
            Assert.AreEqual(expectedValues[3].Text, parsedValues[3].Text);
            Assert.AreEqual(expectedValues[4].LookupValueId, parsedValues[4].LookupValueId);
            Assert.AreEqual(expectedValues[4].Text, parsedValues[4].Text);
            Assert.AreEqual(expectedValues[5].LookupValueId, parsedValues[5].LookupValueId);
            Assert.AreEqual(expectedValues[5].Text, parsedValues[5].Text);
        }


        [Test]
        public void WorkingHoursAreSelected()
        {
            var expectedValues = new List<JobsLookupValue>()
            {
                new JobsLookupValue() { LookupValueId = "10098", Text = "Full time" },
                new JobsLookupValue() { LookupValueId = "10099", Text = "Part time" }
            };

            var parser = new TalentLinkLookupValuesHtmlParser();
            var parsedValues = parser.ParseLookupValues(Properties.Resources.TalentLinkSearchFieldsOuterHtml, "LOV50");

            Assert.AreEqual(expectedValues[0].LookupValueId, parsedValues[0].LookupValueId);
            Assert.AreEqual(expectedValues[0].Text, parsedValues[0].Text);
            Assert.AreEqual(expectedValues[1].LookupValueId, parsedValues[1].LookupValueId);
            Assert.AreEqual(expectedValues[1].Text, parsedValues[1].Text);
        }
    }

}
