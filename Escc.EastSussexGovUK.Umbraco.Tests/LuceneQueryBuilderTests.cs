using System;
using Escc.EastSussexGovUK.Umbraco.Examine;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestFixture]
    public class LuceneQueryBuilderTests
    {
        [Test]
        public void NoKeywordsNoQuery()
        {
            var builder = new LuceneTokenisedQueryBuilder();

            var query = builder.AnyOfTheseTermsInThisField(new string[0], "test", true);

            Assert.AreEqual(String.Empty, query);
        }

        [Test]
        public void OneKeyword()
        {
            var builder = new LuceneTokenisedQueryBuilder();

            var query = builder.AnyOfTheseTermsInThisField(new string[] {"search"}, "test", true);

            Assert.AreEqual(" +(test:search)", query);
        }


        [Test]
        public void TwoKeywords()
        {
            var builder = new LuceneTokenisedQueryBuilder();

            var query = builder.AnyOfTheseTermsInThisField(new string[] { "search", "\"in quotes\"" }, "test", true);

            Assert.AreEqual(" +(test:search test:\"in quotes\")", query);
        }

        [Test]
        public void TwoKeywordsTwoFieldsAnyMatch()
        {
            var builder = new LuceneTokenisedQueryBuilder();

            var query = builder.AnyOfTheseTermsInAnyOfTheseFields(new string[] { "search", "\"in quotes\"" }, new string[] {"test", "test2" }, true);

            Assert.AreEqual(" +((test:search test:\"in quotes\") (test2:search test2:\"in quotes\"))", query);
        }

        [Test]
        public void TwoKeywordsTwoFieldsAllMatch()
        {
            var builder = new LuceneTokenisedQueryBuilder();

            var query = builder.AllOfTheseTermsInAnyOfTheseFields(new string[] { "search", "\"in quotes\"" }, new string[] { "test", "test2" }, true);

            Assert.AreEqual(" +((+test:search +test:\"in quotes\") (+test2:search +test2:\"in quotes\"))", query);
        }
    }
}
