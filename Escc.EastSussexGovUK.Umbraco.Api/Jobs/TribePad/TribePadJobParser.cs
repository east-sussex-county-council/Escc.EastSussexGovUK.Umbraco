using System;
using System.Globalization;
using System.Linq;
using System.Web;
using Exceptionless;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using System.Xml.Linq;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.TribePad
{
    /// <summary>
    /// Parses a job from the XML returned by TribePad
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.IJobAdvertParser" />
    public class TribePadJobParser : IJobAdvertParser, IJobResultsParser
    {
        private readonly IJobsLookupValuesProvider _lookupValuesProvider;
        private readonly ISalaryParser _salaryParser;
        private readonly IWorkPatternParser _workPatternParser;
        private readonly Uri _applyUrl;
        private IEnumerable<JobsLookupValue> _contractTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="TribePadJobParser"/> class.
        /// </summary>
        /// <param name="lookupValuesProvider">A method of supplying lookup values for identifiers referenced by the job data</param>
        /// <param name="salaryParser">A method of parsing salary information for the job</param>
        /// <param name="workPatternParser">A method of parsing work pattern information for the job</param>
        /// <param name="applyUrl">The URL to apply for a job, with {0} to represent where the job id should be used</param>
        /// <exception cref="ArgumentNullException">lookupValuesProvider or salaryParser or workPatternParser or applyUrl</exception>
        public TribePadJobParser(IJobsLookupValuesProvider lookupValuesProvider, ISalaryParser salaryParser, IWorkPatternParser workPatternParser, Uri applyUrl)
        {
            _lookupValuesProvider = lookupValuesProvider ?? throw new ArgumentNullException(nameof(lookupValuesProvider));
            _salaryParser = salaryParser ?? throw new ArgumentNullException(nameof(salaryParser));
            _workPatternParser = workPatternParser ?? throw new ArgumentNullException(nameof(workPatternParser));
            _applyUrl = applyUrl ?? throw new ArgumentNullException(nameof(applyUrl));
        }

        /// <summary>
        /// Parses a job.
        /// </summary>
        /// <param name="sourceData">The source data for the job.</param>
        /// <param name="jobId">The job identifier.</param>
        /// <returns></returns>
        public async Task<Job> ParseJob(string sourceData, string jobId)
        {
            try
            {
                await EnsureLookupValues();

                var xml = XDocument.Parse(sourceData);
                var jobXml = xml.Root.Element("job");

                return await ParseJob(jobXml);
            }
            catch (Exception exception)
            {
                exception.Data.Add("Job ID", jobId);
                exception.ToExceptionless().Submit();
                return null;
            }
        }

        private async Task EnsureLookupValues()
        {
            if (_contractTypes == null)
            {
                _contractTypes = await _lookupValuesProvider.ReadContractTypes();
            }
        }

        /// <summary>
        /// Parses jobs from the HTML stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public async Task<JobsParseResult> Parse(Stream stream)
        {
            await EnsureLookupValues();

            var xml = XDocument.Load(stream);
            var parseResult = new JobsParseResult();

            var jobsXml = xml.Root.Element("jobs").Elements("job");

            foreach (var jobXml in jobsXml)
            {
                parseResult.Jobs.Add(await ParseJob(jobXml));
            }

            return parseResult;
        }

        private async Task<Job> ParseJob(XElement jobXml)
        {
            var job = new Job()
            {
                Id = Int32.Parse(jobXml.Element("job_id").Value.Trim(), CultureInfo.InvariantCulture),
                Reference = HttpUtility.HtmlDecode(jobXml.Element("reference")?.Value).Trim(),
                NumberOfPositions = Int32.Parse(jobXml.Element("no_of_positions").Value, CultureInfo.InvariantCulture),
                JobTitle = HttpUtility.HtmlDecode(jobXml.Element("job_title").Value).Trim(),
                Department= HttpUtility.HtmlDecode(jobXml.Element("business_unit").Value).Trim().Replace("ESCC: ", string.Empty),
                Organisation = HttpUtility.HtmlDecode(jobXml.Element("region").Value).Trim(),
                DatePublished = new DateTime(Int32.Parse(jobXml.Element("open_date").Value.Substring(0, 4), CultureInfo.InvariantCulture),
                                                            Int32.Parse(jobXml.Element("open_date").Value.Substring(5, 2), CultureInfo.InvariantCulture),
                                                            Int32.Parse(jobXml.Element("open_date").Value.Substring(8, 2), CultureInfo.InvariantCulture)),
                ClosingDate = new DateTime(Int32.Parse(jobXml.Element("expiry_date").Value.Substring(0, 4), CultureInfo.InvariantCulture),
                                                            Int32.Parse(jobXml.Element("expiry_date").Value.Substring(5, 2), CultureInfo.InvariantCulture),
                                                            Int32.Parse(jobXml.Element("expiry_date").Value.Substring(8, 2), CultureInfo.InvariantCulture))
            };

            if (job.Department.ToUpperInvariant() == "PARTNERSHIP")
            {
                job.Organisation = job.Department;
                job.Department = string.Empty;
            }

            job.Locations.Add(HttpUtility.HtmlDecode(jobXml.Element("location_city").Value).Trim());
            job.AdvertHtml = new HtmlString(HttpUtility.HtmlDecode(jobXml.Element("package_description")?.Value + jobXml.Element("summary_external")?.Value));
            job.AdditionalInformationHtml = new HtmlString(HttpUtility.HtmlDecode(jobXml.Element("ideal_candidate")?.Value));
            job.EqualOpportunitiesHtml = new HtmlString(HttpUtility.HtmlDecode(jobXml.Element("about_company")?.Value));
            job.JobType = HttpUtility.HtmlDecode(jobXml.Element("category_name")?.Value).Replace(" & ", " and ");

            if (jobXml.Element("no_apply")?.Value == "0")
            {
                job.ApplyUrl = new Uri(String.Format(CultureInfo.InvariantCulture, _applyUrl.ToString(), job.Id), UriKind.RelativeOrAbsolute);
            }

            job.Salary = await _salaryParser.ParseSalary(jobXml.ToString());
            job.WorkPattern = await _workPatternParser.ParseWorkPattern(jobXml.ToString());

            var contractTypeId = jobXml.Element("job_type")?.Value;
            if (!String.IsNullOrEmpty(contractTypeId))
            {
                job.ContractType = _contractTypes?.SingleOrDefault(x => x.LookupValueId == contractTypeId)?.Text;
            }

            return job;
        }
    }
}