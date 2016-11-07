using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Services
{
    /// <summary>
    /// Formats a reference to a font in the Google Fonts Directory into the format needed to load the font
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Services.IFontFamilyFormatter" />
    public class GoogleFontFamilyFormatter : IFontFamilyFormatter
    {
        /// <summary>
        /// Formats the font family name as an HTML string that makes the font available in the page.
        /// </summary>
        /// <param name="fontFamily">The font family name.</param>
        /// <returns></returns>
        public IHtmlString FormatAsFontReference(string fontFamily)
        {
            if (String.IsNullOrWhiteSpace(fontFamily)) return null;

            fontFamily = IsloateFontName(fontFamily);

            return new HtmlString("<link href=\"https://fonts.googleapis.com/css?family=" + fontFamily.Replace(" ", "+") + "\" rel=\"stylesheet\" />");
        }

        private static string IsloateFontName(string fontFamily)
        {
            fontFamily = fontFamily.Trim();
            
            // Users might paste the whole HTML snippet
            fontFamily = Regex.Replace(fontFamily, "^<link href=\"https://fonts.googleapis.com/css" + @"\?" + "family=([^\"]+)\" rel=\"stylesheet\">$", "$1");

            // Users might paste the whole URL
            fontFamily = Regex.Replace(fontFamily, "^https://fonts.googleapis.com/css" + @"\?" + "family=([^\"]+)$", "$1");

            // Users might post the CSS snippet
            fontFamily = Regex.Replace(fontFamily, "^font-family: '([^']+)', (sans-)?serif;$", "$1");
            return fontFamily;
        }

        /// <summary>
        /// Formats the font family name as a font family for a CSS rule.
        /// </summary>
        /// <param name="fontFamily">The font family.</param>
        /// <returns></returns>
        public IHtmlString FormatAsCssFontName(string fontFamily)
        {
            if (String.IsNullOrWhiteSpace(fontFamily)) return null;

            fontFamily = IsloateFontName(fontFamily);

            // Discard the weights if the embed snippet was used
            var colon = fontFamily.IndexOf(":", StringComparison.Ordinal);
            if (colon > -1)
            {
                fontFamily = fontFamily.Substring(0, colon);
            }

            return new HtmlString("'" + fontFamily.Replace("+", " ") + "'");
        }
    }
}