using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Umbraco.Services;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestFixture]
    class GoogleAdWordsTagParserTests
    {
        private const string PastedString = @"<!-- Google Code for Get into teaching button Conversion Page -->
<script type=""text/javascript"">
/* <![CDATA[ */
var google_conversion_id = 981990103;
var google_conversion_language = ""en"";
var google_conversion_format = ""3"";
var google_conversion_color = ""ffffff"";
var google_conversion_label = ""iFAaCPuJ-WsQ1_Wf1AM"";
var google_remarketing_only = false;
/* ]]> */
</script>
<script type=""text/javascript"" src=""//www.googleadservices.com/pagead/conversion.js"">
</script>
<noscript>
<div style=""display:inline;"">
<img height=""1"" width=""1"" style=""border-style:none;"" alt="""" src=""//www.googleadservices.com/pagead/conversion/981990103/?label=iFAaCPuJ-WsQ1_Wf1AM&amp;guid=ON&amp;script=0""/>
</div>
</noscript>
";
        [Test]
        public void ParsesJavaScript()
        {
            var parser = new GoogleAdWordsTagParser();

            parser.ParseTag(PastedString);

            Assert.AreEqual(@"var google_conversion_id = 981990103;
var google_conversion_language = ""en"";
var google_conversion_format = ""3"";
var google_conversion_color = ""ffffff"";
var google_conversion_label = ""iFAaCPuJ-WsQ1_Wf1AM"";
var google_remarketing_only = false;", parser.JavaScript);
        }

        [Test]
        public void ParsesImageUrl()
        {
            var parser = new GoogleAdWordsTagParser();

            parser.ParseTag(PastedString);

            Assert.AreEqual(new Uri("https://www.googleadservices.com/pagead/conversion/981990103/?label=iFAaCPuJ-WsQ1_Wf1AM&guid=ON&script=0"), parser.ImageUrl);
        }
    }
}
