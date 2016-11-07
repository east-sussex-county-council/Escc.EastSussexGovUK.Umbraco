using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Services
{
    /// <summary>
    /// Formats a font family referenced by the user into one which can be added to the page
    /// </summary>
    public interface IFontFamilyFormatter
    {
        /// <summary>
        /// Formats the font family name as an HTML string that makes the font available in the page.
        /// </summary>
        /// <param name="fontFamily">The font family name.</param>
        /// <returns></returns>
        IHtmlString FormatAsFontReference(string fontFamily);

        /// <summary>
        /// Formats the font family name as a font family for a CSS rule.
        /// </summary>
        /// <param name="fontFamily">The font family.</param>
        /// <returns></returns>
        IHtmlString FormatAsCssFontName(string fontFamily);
    }
}