using Escc.Elibrary;
using Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;

namespace Escc.EastSussexGovUK.Umbraco.RichTextHtmlFormatters
{
    /// <summary>
    /// Recognises generic elibrary links and transforms them into links to the current elibrary
    /// </summary>
    /// <seealso cref="Escc.Umbraco.PropertyEditors.RichTextPropertyEditor.IRichTextHtmlFormatter" />
    public class ElibraryLinkFormatter : IRichTextHtmlFormatter
    {
        /// <summary>
        /// Formats the specified HTML.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns></returns>
        public string Format(string html)
        {
            return new ElibraryProxyLinkConverter(new SpydusUrlBuilder()).ParseAndRewriteElibraryLinks(html);
        }
    }
}
