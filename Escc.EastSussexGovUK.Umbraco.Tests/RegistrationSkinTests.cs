﻿using System;
using Escc.EastSussexGovUK.Umbraco.Skins.Registration;
using Escc.EastSussexGovUK.Views;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestFixture]
    class MarriageSkinTests
    {
        [Test]
        public void SkinIsAppliedToMarriageUrl()
        {
            var marriageUrl = new Uri("https://www.eastsussex.gov.uk/community/registration/registeramarriage/example.html");
            var skin = new RegistrationSkin(marriageUrl);

            var required = skin.IsRequired();

            Assert.AreEqual(required, true);
        }

        [Test]
        public void SkinIsNotAppliedToOtherUrls()
        {
            var otherUrl = new Uri("https://www.eastsussex.gov.uk/community/somethingelse/example.html");
            var skin = new RegistrationSkin(otherUrl);

            var required = skin.IsRequired();

            Assert.AreEqual(required, false);
        }
    }
}
