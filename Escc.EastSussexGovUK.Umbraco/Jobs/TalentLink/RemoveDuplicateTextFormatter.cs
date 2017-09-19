using System;
using System.Text.RegularExpressions;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.TalentLink
{
    /// <summary>
    /// Removes text from the advert HTML if we can supply duplicate text from another field
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.IHtmlStringFormatter" />
    public class RemoveDuplicateTextFormatter : IHtmlStringFormatter
    {
        private string _textToMatch;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveDuplicateTextFormatter"/> class.
        /// </summary>
        /// <param name="textToMatch">The text to match.</param>
        public RemoveDuplicateTextFormatter(string textToMatch)
        {
            _textToMatch = textToMatch;
        }

        /// <summary>
        /// Formats the HTML.
        /// </summary>
        /// <param name="html">The HTML document.</param>
        /// <returns></returns>
        public string FormatHtml(string html)
        {
            if (String.IsNullOrEmpty(html) || String.IsNullOrEmpty(_textToMatch)) return html;

            // Accept any kind of white space when matching a space in the search string
            _textToMatch = _textToMatch.Replace(" ", @"\s+");

            html = Regex.Replace(html, @"<p>(<span>)*" + _textToMatch + "(</span>)*</p>", String.Empty);
            html = Regex.Replace(html, @"<p>\s*" + _textToMatch + @"\s*<br />", "<p>");
            html = Regex.Replace(html, @"<br />\s*" + _textToMatch + @"\s*<br />", "<br />");
            html = Regex.Replace(html, @"<br />\s*" + _textToMatch + @"\s*</p>", "</p>");

            return html;
        }
    }
}