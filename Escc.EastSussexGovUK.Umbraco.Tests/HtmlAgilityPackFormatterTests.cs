using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using HtmlAgilityPack;
using NUnit.Framework;
using Escc.EastSussexGovUK.Umbraco.Jobs.HtmlFormatters;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestFixture]
    public class HtmlAgilityPackFormatterTests
    {
        [Test]
        public void UnwantedAttributeIsRemoved()
        {
            var html = new HtmlDocument();
            html.LoadHtml("<div style=\"color:red\"></div>");
            var formatter = new RemoveUnwantedAttributesFormatter(new string[] {"style"});

            formatter.FormatHtml(html);

            Assert.AreEqual("<div></div>", html.DocumentNode.OuterHtml);
        }

        [Test]
        public void ElementNameIsChanged()
        {
            var html = new HtmlDocument();
            html.LoadHtml("<h5 class=\"an-attribute\">Wrong heading level</h5>");
            var formatter = new ReplaceElementNameFormatter("h5", "h2");

            formatter.FormatHtml(html);

            Assert.AreEqual("<h2 class=\"an-attribute\">Wrong heading level</h2>", html.DocumentNode.OuterHtml);
        }
    }
}
