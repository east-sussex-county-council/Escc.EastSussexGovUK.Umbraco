using Escc.EastSussexGovUK.Umbraco.Api.Jobs.JobTransformers;
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
    public class SetJobLocationTransformerTests
    {
        [Test]
        public void LocationsAreSet()
        {
            var job = new Job()
            {
                Locations = new List<string>() { "Hastings" }
            };
            var transformer = new SetJobLocationTransformer(new[] { "Lewes", "Eastbourne" });

            transformer.TransformJob(job);

            Assert.AreEqual(2, job.Locations.Count);
            Assert.IsTrue(job.Locations.Contains("Lewes"));
            Assert.IsTrue(job.Locations.Contains("Eastbourne"));
            Assert.IsFalse(job.Locations.Contains("Hastings"));
        }
    }
}
