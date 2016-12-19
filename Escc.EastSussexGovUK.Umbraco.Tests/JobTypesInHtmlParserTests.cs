using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using HtmlAgilityPack;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    class JobTypesInHtmlParserTests
    {
        [Test]
        public void JobTypesAreSelected()
        {
            var expectedJobTypes = new Dictionary<int,string>()
            {
                { 9857, "Administration and Clerical" },
                { 9858, "Analyst" },
                { 10319, "Children’s - Social Work"},
                { 10682, "Teaching - Leadership"}
            };

            var parser = new JobTypesHtmlParser();
            var parsedJobTypes = parser.ParseJobTypes(Properties.Resources.SearchFieldsOuterHtml);

            Assert.AreEqual(expectedJobTypes, parsedJobTypes);
        }
    }
}
