using Escc.EastSussexGovUK.Umbraco.Services;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestFixture]
    public class RemoveUmbracoNumericSuffixFilterTests
    {
        [Test]
        public void UmbracoSuffixIsRemoved()
        {
            const string before = "test page (1)";
            const string after = "test page";

            var result = new RemoveUmbracoNumericSuffixFilter().Apply(before);

            Assert.AreEqual(result, after);
        }

        [Test]
        [TestCase("test page (2015)")]
        [TestCase("test page (section 1)")]
        public void OtherSuffixesAreNotRemoved(string before)
        {
            var result = new RemoveUmbracoNumericSuffixFilter().Apply(before);

            Assert.AreEqual(result, before);
        }
    }
}
