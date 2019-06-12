using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.TribePad
{
    /// <summary>
    /// Parses locations from XML returned by TribePad for a job
    /// </summary>
    public class TribePadLocationParser : ILocationParser
    {
        /// <summary>
        /// Parse locations from XML returned by TribePad for a job
        /// </summary>
        /// <param name="sourceData">XML for a single job, with a root element of &lt;job&gt;</param>
        public IList<string> ParseLocations(string sourceData)
        {
            var locations = new List<string>();
            if (String.IsNullOrEmpty(sourceData))
            {
                return locations;
            }

            var jobXml = XDocument.Parse(sourceData);
            var unparsedLocation = jobXml.Root.Element("location_city")?.Value;
            if (!String.IsNullOrEmpty(unparsedLocation))
            {
                unparsedLocation = HttpUtility.HtmlDecode(unparsedLocation);

                var parsedLocations = unparsedLocation.Split(',');
                foreach (var location in parsedLocations)
                {
                    locations.Add(location.Trim());
                }
            }

            return locations;
        }
    }
}