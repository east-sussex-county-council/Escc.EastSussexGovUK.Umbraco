using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Escc.EastSussexGovUK.Umbraco.Api.Jobs.JobTransformers;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Api.Tests
{
    [TestFixture]
    public class RemoveUnwantedAttributesTransformerTests
    {
        [Test]
        public void UnwantedAttributeIsRemoved()
        {
            var job = new Job() { AdvertHtml = new HtmlString("<div style=\"color:red\"></div>") };
            var transformer = new RemoveUnwantedAttributesTransformer(new string[] { "style" });

            transformer.TransformJob(job);

            Assert.AreEqual("<div></div>", job.AdvertHtml.ToHtmlString());
        }
    }
}
