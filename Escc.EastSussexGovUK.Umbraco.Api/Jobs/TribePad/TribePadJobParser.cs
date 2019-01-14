using System;
using System.Globalization;
using System.Linq;
using System.Web;
using Exceptionless;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using System.Xml.Linq;
using System.IO;
using System.Collections.Generic;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.TribePad
{
    /// <summary>
    /// Parses a job from the XML returned by TribePad
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.IJobAdvertParser" />
    public class TribePadJobParser : IJobAdvertParser, IJobResultsParser
    {
        private readonly IJobsLookupValuesProvider _lookupValuesProvider;
        private IEnumerable<JobsLookupValue> _workPatterns;

        /// <summary>
        /// Initializes a new instance of the <see cref="TribePadJobParser"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">lookupValuesProvider</exception>
        public TribePadJobParser(IJobsLookupValuesProvider lookupValuesProvider)
        {
            _lookupValuesProvider = lookupValuesProvider ?? throw new ArgumentNullException(nameof(lookupValuesProvider));
        }

        /// <summary>
        /// Parses a job.
        /// </summary>
        /// <param name="sourceData">The source data for the job.</param>
        /// <param name="jobId">The job identifier.</param>
        /// <returns></returns>
        public Job ParseJob(string sourceData, string jobId)
        {
            try
            {
                if (_workPatterns == null)
                {
                    _workPatterns = _lookupValuesProvider.ReadWorkPatterns().Result;
                }

                var xml = XDocument.Parse(sourceData);
                var jobXml = xml.Root.Element("job");

                return ParseJob(jobXml, _workPatterns);
            }
            catch (Exception exception)
            {
                exception.Data.Add("Job ID", jobId);
                exception.ToExceptionless().Submit();
                return null;
            }
        }

        /// <summary>
        /// Parses jobs from the HTML stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public JobsParseResult Parse(Stream stream)
        {
            if (_workPatterns == null)
            {
                _workPatterns = _lookupValuesProvider.ReadWorkPatterns().Result;
            }

            var xml = XDocument.Load(stream);
            var parseResult = new JobsParseResult();

            var jobsXml = xml.Root.Element("jobs").Elements("job");

            foreach (var jobXml in jobsXml)
            {
                parseResult.Jobs.Add(ParseJob(jobXml,_workPatterns));
            }

            return parseResult;
        }

        private static Job ParseJob(XElement jobXml, IEnumerable<JobsLookupValue> workPatterns)
        {
            var job = new Job()
            {
                Id = Int32.Parse(jobXml.Element("job_id").Value.Trim(), CultureInfo.InvariantCulture),
                Reference = jobXml.Element("reference")?.Value.Trim(),
                JobTitle = jobXml.Element("job_title").Value.Trim(),
                Department= jobXml.Element("business_unit").Value.Trim().Replace("ESCC: ", string.Empty),
                Organisation = jobXml.Element("region")?.Value.Trim(),
                Salary = new Salary()
                {
                    MinimumSalary = Int32.Parse(jobXml.Element("salary_from").Value, CultureInfo.InvariantCulture),
                    MaximumSalary = Int32.Parse(jobXml.Element("salary_to").Value, CultureInfo.InvariantCulture)
                },
                ClosingDate = new DateTime(Int32.Parse(jobXml.Element("expiry_date").Value.Substring(0, 4), CultureInfo.InvariantCulture),
                                                            Int32.Parse(jobXml.Element("expiry_date").Value.Substring(5, 2), CultureInfo.InvariantCulture),
                                                            Int32.Parse(jobXml.Element("expiry_date").Value.Substring(8, 2), CultureInfo.InvariantCulture))
            };
            job.Locations.Add(jobXml.Element("location_city").Value.Trim());
            job.AdvertHtml = new HtmlString(HttpUtility.HtmlDecode(jobXml.Element("package_description")?.Value + jobXml.Element("summary_external")?.Value));
            job.AdditionalInformationHtml = new HtmlString(HttpUtility.HtmlDecode(jobXml.Element("ideal_candidate")?.Value));
            job.EqualOpportunitiesHtml = new HtmlString(HttpUtility.HtmlDecode(jobXml.Element("about_company")?.Value));
            job.JobType = HttpUtility.HtmlDecode(jobXml.Element("category_name")?.Value);

            var exampleWorkPattern = workPatterns?.FirstOrDefault();
            if (exampleWorkPattern != null)
            {
                var workPatternId = jobXml.Element("custom_" + exampleWorkPattern.FieldId)?.Element("answer")?.Value;
                if (!String.IsNullOrEmpty(workPatternId))
                {
                    var workPattern = workPatterns.SingleOrDefault(x => x.LookupValueId == workPatternId);
                    if (workPattern != null)
                    {
                        var workPatternComparable = workPattern.Text.ToUpperInvariant();
                        job.WorkPattern = new WorkPattern()
                        {
                            IsFullTime = (workPatternComparable == "FULL TIME"),
                            IsPartTime = (workPatternComparable == "PART TIME")
                        };
                    }
                }
            }

            return job;
        }
    }
}