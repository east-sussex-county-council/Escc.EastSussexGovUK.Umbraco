using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Umbraco.Api.Jobs.TribePad;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using HtmlAgilityPack;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Api.Tests
{
    [TestFixture]
    public class LookupValuesFromTribePadBuiltInFieldParserTests
    {
        [Test]
        public void JobTypesAreSelected()
        {
            var expectedValues = new List<JobsLookupValue>()
            {
                new JobsLookupValue() { LookupValueId = "4", Text = "Apprenticeships & Trainee" },
                new JobsLookupValue() { LookupValueId = "7", Text = "Bereavement" },
                new JobsLookupValue() { LookupValueId = "16", Text = "Cultural Services"},
            };

            var parser = new LookupValuesFromTribePadBuiltInFieldParser();
            var parsedValues = parser.ParseLookupValues(Properties.Resources.TribePadJobOptionsXml, "job_categories");

            var result = parsedValues.First(x => x.LookupValueId == expectedValues[0].LookupValueId);
            Assert.AreEqual(expectedValues[0].Text, result.Text);

            result = parsedValues.First(x => x.LookupValueId == expectedValues[1].LookupValueId);
            Assert.AreEqual(expectedValues[1].Text, result.Text);

            result = parsedValues.First(x => x.LookupValueId == expectedValues[2].LookupValueId);
            Assert.AreEqual(expectedValues[2].Text, result.Text);
        }
    }

}
