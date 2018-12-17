using System;
using System.Text.RegularExpressions;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Web.Services
{
    /// <summary>
    /// Parses the tag code supplied by Google AdWords into its component parts
    /// </summary>
    public class GoogleAdWordsTagParser
    {
        /// <summary>
        /// Parses the tag.
        /// </summary>
        /// <param name="tagCode">The tag code.</param>
        /// <returns><c>true</c> if tag parsed successfully; <c>false</c> otherwise.</returns>
        public bool ParseTag(string tagCode)
        {
            if (String.IsNullOrEmpty(tagCode)) return false;

            bool jsParsed = false, imageParsed = false;

            var cData = "/* <![CDATA[ */";
            var cDataStart = tagCode.IndexOf(cData, StringComparison.OrdinalIgnoreCase);
            var cDataEnd = tagCode.IndexOf("/* ]]", StringComparison.OrdinalIgnoreCase);
            if (cDataStart > -1 && cDataEnd > -1)
            {
                JavaScript = tagCode.Substring(cDataStart + cData.Length, cDataEnd - cDataStart - cData.Length).Trim();
                jsParsed = true;
            }

            // Parse the components of the image URL from the JavaScript rather than parsing the image URL because Umbraco 7.4.3 doesn't return
            // anything after the closing CDATA marker even when the whole tag is pasted into a textbox multiple field
            var conversionId = Regex.Match(JavaScript, @"google_conversion_id\s+=\s+([0-9]+);");
            var conversionLabel = Regex.Match(JavaScript, @"google_conversion_label\s+=\s+""([A-Za-z0-9-_]+)"";");
            if (conversionId.Success && conversionLabel.Success)
            {
                ImageUrl = new Uri($"https://www.googleadservices.com/pagead/conversion/{conversionId.Groups[1].Value}/?label={conversionLabel.Groups[1].Value}&guid=ON&script=0");
                imageParsed = true;
            }

            return (imageParsed && jsParsed);
        }

        /// <summary>
        /// Gets the JavaScript that configures the tag.
        /// </summary>
        public string JavaScript { get; private set; }

        /// <summary>
        /// Gets the image URL for the becaon used when JavaScript is not available
        /// </summary>
        public Uri ImageUrl { get; private set; }
    }
}