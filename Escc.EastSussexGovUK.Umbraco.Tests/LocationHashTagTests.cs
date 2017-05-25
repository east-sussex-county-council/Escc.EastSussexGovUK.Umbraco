using Escc.EastSussexGovUK.Umbraco.Jobs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestFixture]
    public class LocationHashTagTests
    {
        [TestCase("Countywide", "")]
        [TestCase("Uckfield", "#Uckfield")]
        [TestCase("Bexhill-on-Sea", "#Bexhill")]
        [TestCase("Bexhill on Sea", "#Bexhill")]
        [TestCase("Bexhill on sea", "#Bexhill")]
        [TestCase("St. Leonards on sea", "#StLeonards")]
        [TestCase("Wivelsfield Green", "#WivelsfieldGreen")]
        [TestCase("wivelsfield green", "#WivelsfieldGreen")]
        [TestCase("Cripp's Corner", "#CrippsCorner")]
        [TestCase("Old Town (Hastings)", "#OldTownHastings")]
        public void HashTagConvertedCorrectly(string before, string after)
        {
            var hasher = new LocationHashTag(before);

            var result = hasher.ToString();

            Assert.AreEqual(after, result);
        }
    }
}
