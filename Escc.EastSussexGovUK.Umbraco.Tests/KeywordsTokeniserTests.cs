using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Umbraco.Examine;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestFixture]
    public class KeywordsTokeniserTests
    {
        [Test]
        public void OrdinarySearchTermSplitOnSpace()
        {
            var term = "Example search term";
            var tokeniser = new KeywordsTokeniser();

            var result = tokeniser.Tokenise(term);
            
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Example", result[0]);
            Assert.AreEqual("search", result[1]);
            Assert.AreEqual("term", result[2]);
        }

        [Test]
        public void QuotedTermNotSplit()
        {
            var term = "\"Example search term\"";
            var tokeniser = new KeywordsTokeniser();

            var result = tokeniser.Tokenise(term);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(term, result[0]);
        }

        [Test]
        public void QuotedTermWithinUnquotedTermsNotSplit()
        {
            var term = "Example term \"to test\" search tokeniser";
            var tokeniser = new KeywordsTokeniser();

            var result = tokeniser.Tokenise(term);

            Assert.AreEqual(5, result.Count);
            Assert.AreEqual("Example", result[0]);
            Assert.AreEqual("term", result[1]);
            Assert.AreEqual("\"to test\"", result[2]);
            Assert.AreEqual("search", result[3]);
            Assert.AreEqual("tokeniser", result[4]);
        }

        [Test]
        public void TwoQuotedTermsWithTrailingSpace()
        {
            var term = "\"spring term\" \"summer term\" ";
            var tokeniser = new KeywordsTokeniser();

            var result = tokeniser.Tokenise(term);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("\"spring term\"", result[0]);
            Assert.AreEqual("\"summer term\"", result[1]);
        }

        [Test]
        public void InvalidCharacterRemoved()
        {
            // [ ] has a meaning in Examine syntax, so ] on its own causes a syntax error if it's not removedr
            var term = "Example search term]";
            var tokeniser = new KeywordsTokeniser();

            var result = tokeniser.Tokenise(term);

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Example", result[0]);
            Assert.AreEqual("search", result[1]);
            Assert.AreEqual("term", result[2]);
        }
    }
}
