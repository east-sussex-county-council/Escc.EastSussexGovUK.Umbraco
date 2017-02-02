using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Umbraco.Examine;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestFixture]
    public class StopWordsRemoverTests
    {
        [Test]
        public void StopWordsAreRemovedFromIndexedValues()
        {
            var stopWordsRemover = new LuceneStopWordsRemover();

            var result = stopWordsRemover.Filter("Administration and Clerical");

            Assert.AreEqual("Administration Clerical", result);
        }

        [Test]
        public void ReturnsEmptyStringFromNullInput()
        {
            var stopWordsRemover = new LuceneStopWordsRemover();

            var result = stopWordsRemover.Filter(null);

            Assert.AreEqual(String.Empty, result);
        }

        [Test]
        public void ReturnsEmptyStringFromEmptyStringInput()
        {
            var stopWordsRemover = new LuceneStopWordsRemover();

            var result = stopWordsRemover.Filter(String.Empty);

            Assert.AreEqual(String.Empty, result);
        }
    }
}
