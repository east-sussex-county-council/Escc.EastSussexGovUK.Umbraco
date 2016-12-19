using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Settings to filter a list of jobs by
    /// </summary>
    public class JobSearchFilter
    {
        /// <summary>
        /// Gets the job types or categories to limit results to.
        /// </summary>
        /// <value>
        /// The job types.
        /// </value>
        public IList<string> JobTypes { get; } = new List<string>();

        /// <summary>
        /// Gets a unique hash of the filter settings
        /// </summary>
        /// <returns></returns>
        public string ToHash()
        {
            var allContent = new StringBuilder();
            foreach (var jobType in JobTypes) allContent.Append(jobType);

            HashAlgorithm algorithm = SHA1.Create();
            var bytes = Encoding.UTF8.GetBytes(allContent.ToString());
            return Encoding.UTF8.GetString(algorithm.ComputeHash(bytes));
        }
    }
}