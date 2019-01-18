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
    public class YouTubeVideoTransformerTests
    {
        [Test]
        public void YouTubeIFrameIsTransformed()
        {
            var transformer = new YouTubeVideoTransformer();
            var job = new Job() { AdvertHtml = new HtmlString(Properties.Resources.YouTubeIFrame) };

            transformer.TransformJob(job);

            Assert.IsTrue(job.AdvertHtml.ToHtmlString().Contains("<p><a class=\"embed\" href=\"https://www.youtube.com/watch?v=1JOsjWX56Kg\">Watch the video on YouTube</a></p>"));
        }

    }
}
