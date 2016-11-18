using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Umbraco.ApiControllers;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using HtmlAgilityPack;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestFixture]
    public class RemoveOuterHtmlFromSearchFieldsFilterTests
    {
        [Test]
        public void OuterHtmlIsRemoved()
        {
            var originalHtml = " " + Environment.NewLine +
                               "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01//EN\" \" http://www.w3.org/TR/html4/strict.dtd\"><html lang=\"en-GB\" xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:addthis=\"http://www.addthis.com/help/api-spec\" ><head><meta name=\"GENERATOR\" content=\"MrTed\"><meta name=\"TEMPLATEBASE\" content=\"Accessible HTML\"><meta name=\"LASTUPDATED\" content=\"18/08/05 10:05:33\"><meta name=\"author\" content=\"MrTed\"><meta name=\"keywords\" content=\"Jobs, Career Section, Candidate Portal\"><meta name=\"description\" content=\"Mrted Talentlink syndication components\"><meta http-equiv=\"Content-Style-Type\" content=\"text/css\"><meta http-equiv=\"Content-Script-Type\" content=\"text/javascript\"><meta name=\"language\" http-equiv=\"Content-Language\" content=\"en-GB\"><title>Jobs</title>" + Environment.NewLine +
                               "<meta HTTP-EQUIV=\"Content-Type\" content=\"text/html; charset=UTF-8\">" + Environment.NewLine +
                               "<LINK REL=\"STYLESHEET\" TYPE=\"text/css\" HREF=\"/syndicated/syd_style/PFOFK026203F3VBQB7968LOH0-36.css\">" + Environment.NewLine +
                               "</head><body class=\"bodyRapido\"><div id=\"rpd-content\">" + Environment.NewLine +
                               "<form action=\"jsoutputinitrapido.cfm\" id=\"lay9999_src350a\" method=\"get\">" + Environment.NewLine +
                               "<div id=\"keep-this-bit\"></div>" + Environment.NewLine +
                               "</form>" + Environment.NewLine +
                               "</div>" + Environment.NewLine +
                               "</body>" + Environment.NewLine +
                               "</html>" + Environment.NewLine +
                               " ";
            var expectedHtml = "<div id=\"keep-this-bit\"></div>";

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(originalHtml);

            var filter = new RemoveOuterHtmlFromSearchFieldsFilter();
            filter.Filter(htmlDocument);

            Assert.AreEqual(expectedHtml, htmlDocument.DocumentNode.OuterHtml);
        }
    }
}
