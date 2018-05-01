using Escc.EastSussexGovUK.Umbraco.Jobs;
using Escc.EastSussexGovUK.Umbraco.Jobs.JobTransformers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Tests
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
