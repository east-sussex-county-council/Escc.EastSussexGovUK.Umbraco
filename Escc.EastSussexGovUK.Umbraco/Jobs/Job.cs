using System;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// A job advertised on the jobs pages
    /// </summary>
    public class Job
    {
        /// <summary>
        /// Gets or sets the URL to view details of the job
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public Uri Url { get; set; }
        
        /// <summary>
        /// Gets or sets the job title.
        /// </summary>
        /// <value>
        /// The job title.
        /// </value>
        public string JobTitle { get; set; }
        
        /// <summary>
        /// Gets or sets the organisation advertising the job.
        /// </summary>
        /// <value>
        /// The organisation.
        /// </value>
        public string Organisation { get; set; }
        
        /// <summary>
        /// Gets or sets the location where the job is based.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public string Location { get; set; }
        
        /// <summary>
        /// Gets or sets the salary range.
        /// </summary>
        /// <value>
        /// The salary.
        /// </value>
        public string Salary { get; set; }
        
        /// <summary>
        /// Gets or sets the closing date for applications.
        /// </summary>
        /// <value>
        /// The closing date.
        /// </value>
        public DateTime ClosingDate { get; set; }
        
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }
    }
}