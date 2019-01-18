
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NUnit.Framework;
using Escc.EastSussexGovUK.Umbraco.Api.Jobs.HtmlFormatters;

namespace Escc.EastSussexGovUK.Umbraco.Api.Tests
{
    [TestFixture]
    public class EmbeddedYouTubeVideosFormatterTests
    {
        [Test]
        public void ClassIsAddedToYouTubeLink()
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml("<p><a href=\"https://youtu.be/UXBTwFvB4T8\">Watch</a> our video</p>");

            new EmbeddedYouTubeVideosFormatter().FormatHtml(htmlDocument);

            Assert.AreEqual("<p><a href=\"https://youtu.be/UXBTwFvB4T8\" class=\"embed\">Watch</a> our video</p>", htmlDocument.DocumentNode.OuterHtml);
        }

        [Test]
        public void ClassIsAddedToYouTubeLinkThatAlreadyHasAClass()
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml("<p><a href=\"https://youtu.be/UXBTwFvB4T8\" class=\"test\">Watch</a> our video</p>");

            new EmbeddedYouTubeVideosFormatter().FormatHtml(htmlDocument);

            Assert.AreEqual("<p><a href=\"https://youtu.be/UXBTwFvB4T8\" class=\"test embed\">Watch</a> our video</p>", htmlDocument.DocumentNode.OuterHtml);
        }

        [Test]
        public void ClassIsNotAddedToNonYouTubeLink()
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml("<p><a href=\"https://www.example.org/youtu.be\">Watch</a> our video</p>");

            new EmbeddedYouTubeVideosFormatter().FormatHtml(htmlDocument);

            Assert.AreEqual("<p><a href=\"https://www.example.org/youtu.be\">Watch</a> our video</p>", htmlDocument.DocumentNode.OuterHtml);
        }

        [Test]
        public void YouTubeIFrameIsTransformed()
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(Properties.Resources.YouTubeIFrame);

            new EmbeddedYouTubeVideosFormatter().FormatHtml(htmlDocument);

            Assert.IsTrue(htmlDocument.DocumentNode.OuterHtml.Contains("<p><a class=\"embed\" href=\"https://www.youtube.com/watch?v=1JOsjWX56Kg\">Watch the video on YouTube</a></p>"));
        }
    }
}
