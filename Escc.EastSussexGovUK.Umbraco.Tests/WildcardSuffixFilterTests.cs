using Escc.EastSussexGovUK.Umbraco.Examine;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestFixture]
    public class WildcardSuffixFilterTests
    {
        [Test]
        public void HypenatedWordHasOneWildcardAtTheEnd()
        {
            var word = "co-ordinator";
            var filter = new WildcardSuffixFilter();

            var filtered = filter.Filter(word);

            Assert.AreEqual("co-ordinator*", filtered);
        }

        [Test]
        public void WordInBracketsHasOneWildcardAtTheEnd()
        {
            var word = "Head Teacher (PNR01596)";
            var filter = new WildcardSuffixFilter();

            var filtered = filter.Filter(word);

            Assert.AreEqual("Head* Teacher* (PNR01596*)", filtered);
        }
    }
}
