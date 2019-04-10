using Escc.EastSussexGovUK.Umbraco.Api.Jobs;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Api.Tests
{
    [TestFixture]
    public class RelativeJobUrlGeneratorTests
    {
        [Test]
        public void SpacesAreConvertedToDashes()
        {
            var baseUrl = new Uri("https://www.example.org/job");
            var job = new Job() { Id = 12345, JobTitle = "Example job title", Department = "Department", Locations = new List<string>() { "Lewes" }, Reference = "ABC123"};
            var generator = new RelativeJobUrlGenerator(baseUrl);

            job.Url = generator.GenerateUrl(job);

            Assert.AreEqual("https://www.example.org/job/12345/ABC123/example-job-title/department/lewes", job.Url.ToString());
        }

        [Test]
        public void SlashesAreRemovedFromTheUrl()
        {
            var baseUrl = new Uri("https://www.example.org/job");
            var job = new Job() { Id = 12345, JobTitle = "Example job / Typical title", Department = "Department", Locations = new List<string>() { "Lewes" }, Reference = "ABC123" };
            var generator = new RelativeJobUrlGenerator(baseUrl);

            job.Url = generator.GenerateUrl(job);

            Assert.AreEqual("https://www.example.org/job/12345/ABC123/example-job-typical-title/department/lewes", job.Url.ToString());
        }
    }
}
