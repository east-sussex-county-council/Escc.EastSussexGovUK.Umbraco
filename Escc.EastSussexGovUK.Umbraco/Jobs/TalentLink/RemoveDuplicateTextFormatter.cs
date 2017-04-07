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
        private readonly string _textToMatch;

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

            html = Regex.Replace(html, @"<p>(<span>)*" + _textToMatch + "(</span>)*</p>", String.Empty);

            return html;
        }
    }
}