using Escc.EastSussexGovUK.Umbraco.Jobs.Alerts;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestFixture]
    public class JobAlertIdEncoderTests
    {
        [Test]
        public void IdIsAddedToTheUrl()
        {
            var url = new Uri("https://www.example.org");
            var encoder = new JobAlertIdEncoder();
            var id = encoder.GenerateId(new JobAlert() { Email = "example@example.org", Criteria = "test" });

            var after = encoder.AddIdToUrl(url, id);

            Assert.IsTrue(after.ToString().Contains(id));
        }

        [Test]
        public void IdIsParsedFromTheUrl()
        {
            var url = new Uri("https://www.example.org");
            var encoder = new JobAlertIdEncoder();
            var id = encoder.GenerateId(new JobAlert() { Email = "example@example.org", Criteria = "test" });
            var urlWithId = encoder.AddIdToUrl(url, id);

            var parsedId = encoder.ParseIdFromUrl(urlWithId);

            Assert.AreEqual(id, parsedId);
        }

        [Test]
        public void IdIsParsedFromTheUrlWithQueryString()
        {
            var url = new Uri("https://www.example.org?test=test");
            var encoder = new JobAlertIdEncoder();
            var id = encoder.GenerateId(new JobAlert() { Email = "example@example.org", Criteria = "test" });
            var urlWithId = encoder.AddIdToUrl(url, id);

            var parsedId = encoder.ParseIdFromUrl(urlWithId);

            Assert.AreEqual(id, parsedId);
        }

        [Test]
        public void IdIsRemovedFromTheUrl()
        {
            var url = new Uri("https://www.example.org");
            var encoder = new JobAlertIdEncoder();
            var id = encoder.GenerateId(new JobAlert() { Email = "example@example.org", Criteria = "test" });
            var urlWithId = encoder.AddIdToUrl(url, id);

            var urlWithoutId = encoder.RemoveIdFromUrl(urlWithId);

            Assert.AreEqual(url, urlWithoutId);
        }

        [Test]
        public void GeneratedIdIsConsistent()
        {
            var encoder = new JobAlertIdEncoder();
            var alert = new JobAlert() { Email = "example@example.org", Criteria = "test" };

            var id1 = encoder.GenerateId(alert);
            var id2 = encoder.GenerateId(alert);

            Assert.AreEqual(id1, id2);
        }

        [Test]
        public void GeneratedIdVariesByEmail()
        {
            var encoder = new JobAlertIdEncoder();
            var alert1 = new JobAlert() { Email = "example@example.org", Criteria = "test" };
            var alert2 = new JobAlert() { Email = "example2@example.org", Criteria = "test" };

            var id1 = encoder.GenerateId(alert1);
            var id2 = encoder.GenerateId(alert2);

            Assert.AreNotEqual(id1, id2);
        }

        [Test]
        public void GeneratedIdVariesByCriteria()
        {
            var encoder = new JobAlertIdEncoder();
            var alert1 = new JobAlert() { Email = "example@example.org", Criteria = "test1" };
            var alert2 = new JobAlert() { Email = "example@example.org", Criteria = "test2" };

            var id1 = encoder.GenerateId(alert1);
            var id2 = encoder.GenerateId(alert2);

            Assert.AreNotEqual(id1, id2);
        }
    }
}
