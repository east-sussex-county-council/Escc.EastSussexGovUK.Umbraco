using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestFixture]
    public class HtmlStringFormatterTests
    {
        [Test]
        public void EmptyElementWithoutAttributesIsClosed()
        {
            var html = "<div><br></div>";
            var formatter = new CloseEmptyElementsFormatter();

            html = formatter.FormatHtml(html);

            Assert.AreEqual("<div><br /></div>", html);
        }


        [Test]
        public void EmptyElementWithAttributesIsClosed()
        {
            var html = "<div><img src=\"http://example.org/example.jpg\"></div>";
            var formatter = new CloseEmptyElementsFormatter();

            html = formatter.FormatHtml(html);

            Assert.AreEqual("<div><img src=\"http://example.org/example.jpg\" /></div>", html);
        }
    }
}
