using System;
using Escc.EastSussexGovUK.Umbraco.Examine;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestFixture]
    public class LuceneTokenisedQueryBuilderTests
    {
        [Test]
        public void NoKeywordsNoQuery()
        {
            var builder = new LuceneTokenisedQueryBuilder();

            var query = builder.AnyOfTheseTermsInThisField(new string[0], new SearchField() {FieldName = "test" }, true);

            Assert.AreEqual(String.Empty, query);
        }

        [Test]
        public void OneKeyword()
        {
            var builder = new LuceneTokenisedQueryBuilder();

            var query = builder.AnyOfTheseTermsInThisField(new string[] {"search"}, new SearchField() { FieldName = "test" }, true);

            Assert.AreEqual(" +(test:search)", query);
        }


        [Test]
        public void TwoKeywords()
        {
            var builder = new LuceneTokenisedQueryBuilder();

            var query = builder.AnyOfTheseTermsInThisField(new string[] { "search", "\"in quotes\"" }, new SearchField() { FieldName = "test" }, true);

            Assert.AreEqual(" +(test:search test:\"in quotes\")", query);
        }

        [Test]
        public void TwoKeywordsTwoFieldsAnyMatch()
        {
            var builder = new LuceneTokenisedQueryBuilder();

            var query = builder.AnyOfTheseTermsInAnyOfTheseFields(new string[] { "search", "\"in quotes\"" }, new SearchField[] { new SearchField() { FieldName = "test" }, new SearchField() { FieldName = "test2" } }, true);

            Assert.AreEqual(" +((test:search test:\"in quotes\") (test2:search test2:\"in quotes\"))", query);
        }

        [Test]
        public void TwoKeywordsTwoFieldsAllMatch()
        {
            var builder = new LuceneTokenisedQueryBuilder();

            var query = builder.AllOfTheseTermsInAnyOfTheseFields(new string[] { "search", "\"in quotes\"" }, new SearchField[] { new SearchField() { FieldName = "test"}, new SearchField() { FieldName = "test2" } }, true);

            Assert.AreEqual(" +((+test:search +test:\"in quotes\") (+test2:search +test2:\"in quotes\"))", query);
        }

        [Test]
        public void TwoKeywordsTwoFieldsOneBoostedAllMatch()
        {
            var builder = new LuceneTokenisedQueryBuilder();

            var query = builder.AllOfTheseTermsInAnyOfTheseFields(new string[] { "search", "\"in quotes\"" }, new SearchField[] { new SearchField() { FieldName = "test", Boost = 2.5 }, new SearchField() { FieldName = "test2" } }, true);

            Assert.AreEqual(" +((+test:search^2.5 +test:\"in quotes\"^2.5) (+test2:search +test2:\"in quotes\"))", query);
        }

        [Test]
        public void KeywordStartingWithIsInvalid()
        {
            var builder = new LuceneTokenisedQueryBuilder();

            var query = builder.AllOfTheseTermsInAnyOfTheseFields(new string[] { "-search" }, new SearchField[] { new SearchField() { FieldName = "test" } }, true);

            Assert.AreEqual(" +((+test:search))", query);
        }

        [Test]
        public void HyphenInKeywordIsRemoved()
        {
            var builder = new LuceneTokenisedQueryBuilder();

            var query = builder.AllOfTheseTermsInAnyOfTheseFields(new string[] { "part-time" }, new SearchField[] { new SearchField() { FieldName = "test" } }, true);

            Assert.AreEqual(" +((+test:part time))", query);
        }

        [Test]
        public void HyphenOnlyAsKeywordIsRemoved()
        {
            var builder = new LuceneTokenisedQueryBuilder();

            var query = builder.AllOfTheseTermsInAnyOfTheseFields(new string[] { "-" }, new SearchField[] { new SearchField() { FieldName = "test" } }, true);

            Assert.AreEqual(String.Empty, query);
        }
    }
}
