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
        /// Gets or sets the keywords to search by, using an OR operator between words
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// Gets or sets the location where the job is based
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public string Location { get; set; }

        /// <summary>
        /// Gets the job types or categories to limit results to.
        /// </summary>
        /// <value>
        /// The job types.
        /// </value>
        public IList<string> JobTypes { get; } = new List<string>();

        /// <summary>
        /// Gets the working hours, eg full-time or part-time
        /// </summary>
        /// <value>
        /// The working hours.
        /// </value>
        public IList<string> WorkingHours { get; } = new List<string>();

        /// <summary>
        /// Gets or sets the organisation advertising the job
        /// </summary>
        /// <value>
        /// The organisation.
        /// </value>
        public string Organisation { get; set; }

        /// <summary>
        /// Gets or sets the salary band.
        /// </summary>
        /// <value>
        /// The salary band.
        /// </value>
        public string SalaryRange { get; set; }

        /// <summary>
        /// Gets or sets the job reference.
        /// </summary>
        /// <value>
        /// The job reference.
        /// </value>
        public string JobReference { get; set; }

        /// <summary>
        /// Gets a unique hash of the filter settings
        /// </summary>
        /// <returns></returns>
        public string ToHash()
        {
            var allContent = new StringBuilder();
            allContent.Append("keywords").Append(Keywords);
            allContent.Append("location").Append(Location);
            allContent.Append("organisation").Append(Organisation);
            allContent.Append("salary").Append(SalaryRange);
            allContent.Append("ref").Append(JobReference);
            allContent.Append("types");
            foreach (var jobType in JobTypes) allContent.Append(jobType);
            allContent.Append("hours");
            foreach (var hours in WorkingHours) allContent.Append(hours);

            HashAlgorithm algorithm = SHA1.Create();
            var bytes = Encoding.UTF8.GetBytes(allContent.ToString());
            return Encoding.UTF8.GetString(algorithm.ComputeHash(bytes));
        }
    }
}