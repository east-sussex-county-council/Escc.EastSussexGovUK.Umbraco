using System;
using System.Collections.Generic;
using System.Web;

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
        public IList<string> Locations { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the salary range.
        /// </summary>
        /// <value>
        /// The salary.
        /// </value>
        public Salary Salary { get; set; } = new Salary();

        /// <summary>
        /// Gets or sets the closing date for applications.
        /// </summary>
        /// <value>
        /// The closing date.
        /// </value>
        public DateTime? ClosingDate { get; set; }
        
        /// <summary>
        /// Gets or sets the internal identifier for the jobs service.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the published job reference.
        /// </summary>
        /// <value>
        /// The reference.
        /// </value>
        public string Reference { get; set; }

        /// <summary>
        /// Gets or sets the additional information section from the advert HTML.
        /// </summary>
        /// <value>
        /// The additional information HTML.
        /// </value>
        public IHtmlString AdditionalInformationHtml { get; set; }

        /// <summary>
        /// Gets or sets the equal opportunities section from the advert HTML.
        /// </summary>
        /// <value>
        /// The equal opportunities HTML.
        /// </value>
        public IHtmlString EqualOpportunitiesHtml { get; set; }

        /// <summary>
        /// Gets or sets the full advert text.
        /// </summary>
        /// <value>
        /// The advert text.
        /// </value>
        public IHtmlString AdvertHtml { get; set; }
       
        /// <summary>
        /// Gets or sets the type or category of the job.
        /// </summary>
        /// <value>
        /// The type of the job.
        /// </value>
        public string JobType { get; set; }

        /// <summary>
        /// Gets or sets the type of the contract, eg fixed term or permanent.
        /// </summary>
        /// <value>
        /// The type of the contract.
        /// </value>
        public string ContractType { get; set; }

        /// <summary>
        /// Gets or sets the part of the organisation that's advertising the job.
        /// </summary>
        /// <value>
        /// The department.
        /// </value>
        public string Department { get; set; }

        /// <summary>
        /// Gets or sets the URL to apply for the job.
        /// </summary>
        public Uri ApplyUrl { get; set; }

        /// <summary>
        /// Gets or sets the work pattern, eg full time or part time.
        /// </summary>
        /// <value>
        /// The work pattern.
        /// </value>
        public WorkPattern WorkPattern { get; set; } = new WorkPattern();

        /// <summary>
        /// Gets or sets the date when the job was published.
        /// </summary>
        /// <value>
        /// The date published.
        /// </value>
        public DateTime? DatePublished { get; set; }

        /// <summary>
        /// Gets how many people are required to fill this job vacancy
        /// </summary>
        public int? NumberOfPositions { get; set; }
    }
}