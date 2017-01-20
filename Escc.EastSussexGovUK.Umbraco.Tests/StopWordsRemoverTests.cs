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
        private static readonly string[] StopWords = new[] {"a", "an", "and", "are", "as", "at", "be", "but", "by",
            "for", "if", "in", "into", "is", "it",
            "no", "not", "of", "on", "or", "such",
            "that", "the", "their", "then", "there", "these",
            "they", "this", "to", "was", "will", "with"};

        [Test]
        public void StopWordsAreRemovedFromIndexedValues()
        {
            var stopWordsRemover = new StopWordsRemover();

            var result = stopWordsRemover.RemoveStopWords("Administration and Clerical", StopWords);

            Assert.AreEqual("Administration Clerical", result);
        }

        [Test]
        public void ReturnsEmptyStringFromNullInput()
        {
            var stopWordsRemover = new StopWordsRemover();

            var result = stopWordsRemover.RemoveStopWords(null, StopWords);

            Assert.AreEqual(String.Empty, result);
        }

        [Test]
        public void ReturnsEmptyStringFromEmptyStringInput()
        {
            var stopWordsRemover = new StopWordsRemover();

            var result = stopWordsRemover.RemoveStopWords(String.Empty, StopWords);

            Assert.AreEqual(String.Empty, result);
        }
    }
}
