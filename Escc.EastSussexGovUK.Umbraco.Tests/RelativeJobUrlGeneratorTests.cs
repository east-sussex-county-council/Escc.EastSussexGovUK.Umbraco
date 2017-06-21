using Escc.EastSussexGovUK.Umbraco.Jobs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestFixture]
    public class RelativeJobUrlGeneratorTests
    {
        [Test]
        public void SpacesAreConvertedToDashes()
        {
            var baseUrl = new Uri("https://www.example.org/job");
            var job = new Job() { Id = 12345, JobTitle = "Example job title", Location = "Lewes", Reference = "ABC123"};
            var generator = new RelativeJobUrlGenerator(baseUrl);

            job.Url = generator.GenerateUrl(job);

            Assert.AreEqual("https://www.example.org/job/12345/ABC123/example-job-title/lewes", job.Url.ToString());
        }

        [Test]
        public void SlashesAreRemovedFromTheUrl()
        {
            var baseUrl = new Uri("https://www.example.org/job");
            var job = new Job() { Id = 12345, JobTitle = "Example job / Typical title", Location = "Lewes", Reference = "ABC123" };
            var generator = new RelativeJobUrlGenerator(baseUrl);

            job.Url = generator.GenerateUrl(job);

            Assert.AreEqual("https://www.example.org/job/12345/ABC123/example-job-typical-title/lewes", job.Url.ToString());
        }
    }
}
