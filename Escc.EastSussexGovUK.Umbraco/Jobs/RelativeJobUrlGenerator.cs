using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Generates a job URL relative to a provided base URL
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.IJobUrlGenerator" />
    public class RelativeJobUrlGenerator : IJobUrlGenerator
    {
        private readonly Uri _baseUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelativeJobUrlGenerator"/> class.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <exception cref="System.ArgumentNullException">baseUrl</exception>
        public RelativeJobUrlGenerator(Uri baseUrl)
        {
            if (baseUrl == null) throw new ArgumentNullException(nameof(baseUrl));
            _baseUrl = baseUrl;
        }

        /// <summary>
        /// Generates a URL for a job.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns></returns>
        public Uri GenerateUrl(Job job)
        {
            var normalisedBaseUrl = _baseUrl.ToString().TrimEnd(new[] { '/' });
            var jobTitle = NormaliseSegment(job.JobTitle);
            var location = NormaliseSegment(string.Join("-", job.Locations.ToArray<string>()));
            return new Uri($"{normalisedBaseUrl}/{job.Id}/{job.Reference}/{jobTitle}/{location}", UriKind.RelativeOrAbsolute);
        }

        public string NormaliseSegment(string text)
        {
            text = text.ToLower(CultureInfo.CurrentCulture).Replace("&", "and").Replace(" ", "-");
            text = Regex.Replace(text, "[^a-z0-9-]", String.Empty);
            text = Regex.Replace(text, "-+", "-");
            return text;
        }
    }
}