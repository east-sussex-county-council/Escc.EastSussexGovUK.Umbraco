﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.HtmlFormatters
{
    /// <summary>
    /// Format HTML using the HTML Agility Pack library
    /// </summary>
    public interface IHtmlAgilityPackHtmlFormatter
    {
        /// <summary>
        /// Formats the HTML.
        /// </summary>
        /// <param name="htmlDocument">The HTML document.</param>
        void FormatHtml(HtmlDocument htmlDocument);
    }
}
