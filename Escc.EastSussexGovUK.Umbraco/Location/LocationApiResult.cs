using System;

namespace Escc.EastSussexGovUK.Umbraco.Location
{
    /// <summary>
    /// Data from a Location template to be returned by <see cref="LocationController"/>
    /// </summary>
    public class LocationApiResult
    {
        public string LocationType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Uri Url { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Town { get; set; }
    }
}