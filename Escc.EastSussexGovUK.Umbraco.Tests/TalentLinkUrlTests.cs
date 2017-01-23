using System;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using Escc.EastSussexGovUK.Umbraco.Jobs.TalentLink;
using Escc.EastSussexGovUK.Umbraco.Models;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestFixture]
    public class TalentLinkUrlTests
    {
        private const string ExampleId = "E6FCE5A31B0846EE44861E34CABD51C247E2";
        private const string ExampleMask = "guidofawkes";
        private readonly string _scriptUrl = $"https://emea3.recruitmentplatform.com/syndicated/lay/laydisplayrapido.cfm?ID={ExampleId}&component=lay9999_src350a&LG=UK&mask={ExampleMask}&browserchk=no";

        [Test]
        public void LinkUrlIsParsedFromScriptUrl()
        {
            var parsedUrl = new TalentLinkUrl(_scriptUrl);

            Assert.AreEqual(new Uri($"https://emea3.recruitmentplatform.com/syndicated/lay/jsoutputinitrapido.cfm?ID={ExampleId}&component=lay9999_src350a&LG=UK&mask={ExampleMask}&browserchk=no"), parsedUrl.LinkUrl);
        }

        [Test]
        public void IdIsParsedFromScriptUrl()
        {
            var parsedUrl = new TalentLinkUrl(_scriptUrl);

            Assert.AreEqual(ExampleId, parsedUrl.Id);
        }

        [Test]
        public void MaskIsParsedFromScriptUrl()
        {
            var parsedUrl = new TalentLinkUrl(_scriptUrl);

            Assert.AreEqual(ExampleMask, parsedUrl.Mask);
        }

        [Test]
        [ExpectedException(typeof(UriFormatException))]
        public void RelativeUrlThrowsUriFormatException()
        {
            var relativeUrl = new Uri(_scriptUrl).PathAndQuery;
            var parsedUrl = new TalentLinkUrl(relativeUrl);
        }
    }
}
