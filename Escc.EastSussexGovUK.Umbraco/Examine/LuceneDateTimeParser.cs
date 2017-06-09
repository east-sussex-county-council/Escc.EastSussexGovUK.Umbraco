using System;
using System.Globalization;

namespace Escc.EastSussexGovUK.Umbraco.Examine
{
    /// <summary>
    /// Parse a date time from the format stored in a Lucene index where the field type is set to DATETIME
    /// </summary>
    public class LuceneDateTimeParser
    {
        /// <summary>
        /// Parses the specified <see cref="DateTime" /> from Lucene field data
        /// </summary>
        /// <param name="luceneDateTime">The Lucene date time.</param>
        /// <returns>A parsed <see cref="DateTime"/>, or null if it could not be parsed</returns>
        public DateTime? Parse(string luceneDateTime)
        {
            if (String.IsNullOrEmpty(luceneDateTime) || luceneDateTime.Length < 14) return null;

            try
            {
                return new DateTime(Int32.Parse(luceneDateTime.Substring(0, 4), CultureInfo.InvariantCulture),
                                    Int32.Parse(luceneDateTime.Substring(4, 2), CultureInfo.InvariantCulture),
                                    Int32.Parse(luceneDateTime.Substring(6, 2), CultureInfo.InvariantCulture),
                                    Int32.Parse(luceneDateTime.Substring(8, 2), CultureInfo.InvariantCulture),
                                    Int32.Parse(luceneDateTime.Substring(10, 2), CultureInfo.InvariantCulture),
                                    Int32.Parse(luceneDateTime.Substring(12, 2), CultureInfo.InvariantCulture));

            }
            catch (FormatException ex)
            {
                return null;
            }
        }
    }
}