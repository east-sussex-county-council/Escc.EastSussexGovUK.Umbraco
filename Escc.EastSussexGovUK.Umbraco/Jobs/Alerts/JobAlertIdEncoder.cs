using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Alerts
{
    /// <summary>
    /// Generate job alert IDs and add/remove them from URLs
    /// </summary>
    public class JobAlertIdEncoder
    {
        /// <summary>
        /// Generates a new unique identifier based on the content of a job alert.
        /// </summary>
        /// <param name="alert">The alert.</param>
        /// <returns></returns>
        public string GenerateId(JobAlert alert)
        {
            if (alert == null) throw new ArgumentNullException(nameof(alert));
            if (String.IsNullOrEmpty(alert.Email)) throw new ArgumentException("The alert must include an email address to generate a unique ID", nameof(alert));

            HashAlgorithm algorithm = SHA1.Create();
            var bytes = Encoding.ASCII.GetBytes(alert.Email + alert.Criteria);
            var hash = algorithm.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString().ToLowerInvariant();
        }

        /// <summary>
        /// Parses the identifier from URL where it has been encoded by <see cref="AddIdToUrl(Uri, string)"/>.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public string ParseIdFromUrl(Uri url)
        {
            if (!url.IsAbsoluteUri) throw new ArgumentException("URL must be absolute", nameof(url));

            var fullUrl = url.AbsolutePath;
            var idMatch = Regex.Match(fullUrl, "/([0-9a-f]{40})$");
            if (idMatch.Success)
            {
                return idMatch.Groups[1].Value;
            }
            else return String.Empty;
        }

        /// <summary>
        /// Removes the identifier from URL, returning the remaining URL
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public Uri RemoveIdFromUrl(Uri url)
        {
            if (!url.IsAbsoluteUri) throw new ArgumentException("URL must be absolute", nameof(url));

            var fullUrl = url.Scheme + "://" + url.Authority + url.AbsolutePath.TrimEnd('/');
            var idMatch = Regex.Match(fullUrl, "/[0-9a-f]{40}$");
            if (idMatch.Success)
            {
                var updatedUrl = fullUrl.Substring(0, idMatch.Groups[0].Index + 1);
                if (!String.IsNullOrEmpty(url.Query))
                {
                    updatedUrl += "?" + url.Query;
                }
                return new Uri(updatedUrl, UriKind.RelativeOrAbsolute);
            }
            return url;
        }

        /// <summary>
        /// Adds the identifier to an absolute URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="alertId">The alert identifier.</param>
        /// <returns></returns>
        public Uri AddIdToUrl(Uri url, string alertId)
        {
            if (!url.IsAbsoluteUri) throw new ArgumentException("URL must be absolute", nameof(url));

            var fullUrl = url.Scheme + "://" + url.Authority + url.AbsolutePath.TrimEnd('/');
            fullUrl = fullUrl + "/" + alertId;
            if (!String.IsNullOrEmpty(url.Query))
            {
                fullUrl += "?" + url.Query;
            }
            return new Uri(fullUrl);
        }
    }
}