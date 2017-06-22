﻿using System;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.TalentLink
{
    /// <summary>
    /// The 'information for redeployees' header is consistently, incorrectly formatted as a paragraph. This formatter corrects that.
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.IHtmlStringFormatter" />
    public class RedeploymentHeaderFormatter : IHtmlStringFormatter
    {
        /// <summary>
        /// Formats the HTML.
        /// </summary>
        /// <param name="html">The HTML document.</param>
        /// <returns></returns>
        public string FormatHtml(string html)
        {
            return html?.Replace("<p>Information for redeployees</p>", "<h3>Information for redeployees</h3>");
        }
    }
}