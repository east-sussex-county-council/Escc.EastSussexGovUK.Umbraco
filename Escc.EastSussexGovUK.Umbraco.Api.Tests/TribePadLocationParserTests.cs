using Escc.EastSussexGovUK.Umbraco.Api.Jobs.TribePad;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Api.Tests
{
    [TestFixture]
    public class TribePadLocationParserTests
    {
        [Test]
        public void SingleLocationIsParsed()
        {
            var parser = new TribePadLocationParser();

            var locations = parser.ParseLocations(Properties.Resources.TribePadJobSingleLocation);

            Assert.AreEqual(1, locations.Count);
            Assert.AreEqual("Lewes", locations[0]);
        }

        [Test]
        public void MultipleLocationsAreParsed()
        {
            var parser = new TribePadLocationParser();

            var locations = parser.ParseLocations(Properties.Resources.TribePadJobMultipleLocationsSeparatedByCommaSpace);

            Assert.AreEqual(4, locations.Count);
            Assert.AreEqual("Lewes", locations[0]);
            Assert.AreEqual("Uckfield", locations[1]);
            Assert.AreEqual("Crowborough", locations[2]);
            Assert.AreEqual("Forest Row", locations[3]);
        }
    }
}
