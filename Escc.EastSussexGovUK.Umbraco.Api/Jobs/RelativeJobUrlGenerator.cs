using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Escc.EastSussexGovUK.Umbraco.Jobs;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs
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

            // Seen in the wild: live jobs linked to on http://localhost, probably due to Azure referring to its own machines that way.
            // Ensure it can never happen by replacing that with a better default.
            if (normalisedBaseUrl.StartsWith("http://localhost"))
            {
                normalisedBaseUrl = "https://www.eastsussex.gov.uk" + normalisedBaseUrl.Substring(16);
            }

            var jobTitle = NormaliseSegment(job.JobTitle);
            var location = NormaliseSegment(string.Join("-", job.Locations.ToArray<string>()));
            var reference = NormaliseSegment(job.Reference);
            var department = NormaliseSegment(job.Department);
            if (!String.IsNullOrEmpty(department)) { department += "/"; };
            return new Uri($"{normalisedBaseUrl}/{job.Id}/{reference}/{jobTitle}/{department}{location}", UriKind.RelativeOrAbsolute);
        }

        public string NormaliseSegment(string text)
        {
            if (!String.IsNullOrEmpty(text))
            {
                text = text.ToLower(CultureInfo.CurrentCulture).Replace("&", "and").Replace(" ", "-");
                text = Regex.Replace(text, "[^a-z0-9-]", String.Empty);
                text = Regex.Replace(text, "-+", "-");
            }
            return text;
        }
    }
}