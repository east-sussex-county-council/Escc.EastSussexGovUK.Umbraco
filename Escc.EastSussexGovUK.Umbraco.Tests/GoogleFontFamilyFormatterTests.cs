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
    class GoogleFontFamilyFormatterTests
    {
        [TestCase("<link href=\"https://fonts.googleapis.com/css?family=Cormorant+Garamond:400,400i\" rel=\"stylesheet\">", "<link href=\"https://fonts.googleapis.com/css?family=Cormorant+Garamond:400,400i\" rel=\"stylesheet\" />")]
        [TestCase("https://fonts.googleapis.com/css?family=Cormorant+Garamond:400,400i", "<link href=\"https://fonts.googleapis.com/css?family=Cormorant+Garamond:400,400i\" rel=\"stylesheet\" />")]
        [TestCase("Cormorant+Garamond:400,400i", "<link href=\"https://fonts.googleapis.com/css?family=Cormorant+Garamond:400,400i\" rel=\"stylesheet\" />")]
        [TestCase("Cormorant+Garamond", "<link href=\"https://fonts.googleapis.com/css?family=Cormorant+Garamond\" rel=\"stylesheet\" />")]
        [TestCase("font-family: 'Cormorant Garamond', serif;", "<link href=\"https://fonts.googleapis.com/css?family=Cormorant+Garamond\" rel=\"stylesheet\" />")]
        public void PastedSnippetIsConvertedToHtmlEmbed(string pastedText, string expected)
        {
            var result = new GoogleFontFamilyFormatter().FormatAsFontReference(pastedText);

            Assert.AreEqual(expected, result);
        }

        [TestCase("<link href=\"https://fonts.googleapis.com/css?family=Cormorant+Garamond:400,400i\" rel=\"stylesheet\">", "'Cormorant Garamond'")]
        [TestCase("https://fonts.googleapis.com/css?family=Cormorant+Garamond:400,400i", "'Cormorant Garamond'")]
        [TestCase("Cormorant+Garamond:400,400i", "'Cormorant Garamond'")]
        [TestCase("Cormorant+Garamond", "'Cormorant Garamond'")]
        [TestCase("font-family: 'Cormorant Garamond', serif;", "'Cormorant Garamond'")]
        public void PastedSnippetIsConvertedToCssFontFamily(string pastedText, string expected)
        {
            var result = new GoogleFontFamilyFormatter().FormatAsCssFontName(pastedText);

            Assert.AreEqual(expected, result);
        }
    }
}
