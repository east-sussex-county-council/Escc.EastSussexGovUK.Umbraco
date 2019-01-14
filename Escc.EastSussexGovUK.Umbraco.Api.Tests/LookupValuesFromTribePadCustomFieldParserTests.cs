using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Umbraco.Api.Jobs.TalentLink;
using Escc.EastSussexGovUK.Umbraco.Api.Jobs.TribePad;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using HtmlAgilityPack;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Api.Tests
{
    [TestFixture]
    public class LookupValuesFromTribePadCustomFieldParserTests
    {
        [Test]
        public void WorkingHoursAreSelected()
        {
            var expectedValues = new List<JobsLookupValue>()
            {
                new JobsLookupValue() { LookupValueId = "16", Text = "Full Time" },
                new JobsLookupValue() { LookupValueId = "17", Text = "Part Time" }
            };

            var parser = new LookupValuesFromTribePadCustomFieldParser();
            var parsedValues = parser.ParseLookupValues(Properties.Resources.TribePadCustomFieldsXml, "Working Pattern");

            var result = parsedValues.First(x => x.LookupValueId == expectedValues[0].LookupValueId);
            Assert.AreEqual(expectedValues[0].Text, result.Text);

            result = parsedValues.First(x => x.LookupValueId == expectedValues[1].LookupValueId);
            Assert.AreEqual(expectedValues[1].Text, result.Text);
        }
    }

}
