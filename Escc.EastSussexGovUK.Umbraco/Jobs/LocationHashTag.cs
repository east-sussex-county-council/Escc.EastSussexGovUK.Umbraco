using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Humanizer;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Converts a job location into a social media hash tag
    /// </summary>
    public class LocationHashTag
    {
        private string _location;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationHashTag"/> class.
        /// </summary>
        /// <param name="location">The location.</param>
        public LocationHashTag(string location)
        {
            _location = location;
        }

        /// <summary>
        /// Gets the hash tag
        /// </summary>
        /// <returns>
        /// A social media hash tag
        /// </returns>
        public override string ToString()
        {
            if (String.IsNullOrEmpty(_location) || _location.ToUpper(CultureInfo.CurrentCulture) == "COUNTYWIDE")
            {
                return String.Empty;
            }
            var hashTag = _location;
            hashTag = Regex.Replace(hashTag, "[ |-]on[ |-][S|s]ea", String.Empty);
            hashTag = Regex.Replace(hashTag, "[^A-Za-z ]", String.Empty);
            hashTag = hashTag.Titleize();
            hashTag = hashTag.Replace(" ", String.Empty);
            return "#" + hashTag;
        }
    }
}