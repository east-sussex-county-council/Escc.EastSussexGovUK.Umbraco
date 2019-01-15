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
        private IEnumerable<JobsLookupValue> _workPatterns;
        private IEnumerable<JobsLookupValue> _contractTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="TribePadJobParser"/> class.
        /// </summary>
        /// <param name="lookupValuesProvider">A method of supplying lookup values for identifiers referenced by the job data</param>
        /// <param name="salaryParser">A method of parsing salary information for the job</param>
        /// <exception cref="ArgumentNullException">lookupValuesProvider</exception>
        public TribePadJobParser(IJobsLookupValuesProvider lookupValuesProvider, ISalaryParser salaryParser)
        {
            _lookupValuesProvider = lookupValuesProvider ?? throw new ArgumentNullException(nameof(lookupValuesProvider));
            _salaryParser = salaryParser ?? throw new ArgumentNullException(nameof(salaryParser));
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
            if (_workPatterns == null)
            {
                _workPatterns = await _lookupValuesProvider.ReadWorkPatterns();
            }

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
                Reference = jobXml.Element("reference")?.Value.Trim(),
                JobTitle = jobXml.Element("job_title").Value.Trim(),
                Department= jobXml.Element("business_unit").Value.Trim().Replace("ESCC: ", string.Empty),
                Organisation = jobXml.Element("region")?.Value.Trim(),
                DatePublished = new DateTime(Int32.Parse(jobXml.Element("open_date").Value.Substring(0, 4), CultureInfo.InvariantCulture),
                                                            Int32.Parse(jobXml.Element("open_date").Value.Substring(5, 2), CultureInfo.InvariantCulture),
                                                            Int32.Parse(jobXml.Element("open_date").Value.Substring(8, 2), CultureInfo.InvariantCulture)),
                ClosingDate = new DateTime(Int32.Parse(jobXml.Element("expiry_date").Value.Substring(0, 4), CultureInfo.InvariantCulture),
                                                            Int32.Parse(jobXml.Element("expiry_date").Value.Substring(5, 2), CultureInfo.InvariantCulture),
                                                            Int32.Parse(jobXml.Element("expiry_date").Value.Substring(8, 2), CultureInfo.InvariantCulture))
            };
            job.Locations.Add(jobXml.Element("location_city").Value.Trim());
            job.AdvertHtml = new HtmlString(HttpUtility.HtmlDecode(jobXml.Element("package_description")?.Value + jobXml.Element("summary_external")?.Value));
            job.AdditionalInformationHtml = new HtmlString(HttpUtility.HtmlDecode(jobXml.Element("ideal_candidate")?.Value));
            job.EqualOpportunitiesHtml = new HtmlString(HttpUtility.HtmlDecode(jobXml.Element("about_company")?.Value));
            job.JobType = HttpUtility.HtmlDecode(jobXml.Element("category_name")?.Value);
            job.Salary = await _salaryParser.ParseSalaryFromJobAdvert(jobXml.ToString());
            var salaryFrom = Int32.Parse(jobXml.Element("salary_from").Value, CultureInfo.InvariantCulture);
            var salaryTo = Int32.Parse(jobXml.Element("salary_to").Value, CultureInfo.InvariantCulture);

            job.Salary = new Salary()
            {
                MinimumSalary = salaryFrom,
                MaximumSalary = salaryTo
            };

            if (salaryFrom == 0 && salaryTo == 0)
            {
                job.Salary.SalaryRange = "Voluntary";
            }
            else if (salaryFrom == salaryTo)
            {
                job.Salary.SalaryRange = $"£{salaryFrom.ToString("n0")} per annum";
            }
            else
            {
                job.Salary.SalaryRange = $"£{salaryFrom.ToString("n0")} to £{salaryTo.ToString("n0")} per annum";
            }

            var contractTypeId = jobXml.Element("job_type")?.Value;
            if (!String.IsNullOrEmpty(contractTypeId))
            {
                job.ContractType = _contractTypes?.SingleOrDefault(x => x.LookupValueId == contractTypeId)?.Text;
            }

            var exampleWorkPattern = _workPatterns?.FirstOrDefault();
            if (exampleWorkPattern != null)
            {
                var workPatternId = jobXml.Element("custom_" + exampleWorkPattern.FieldId)?.Element("answer")?.Value;
                if (!String.IsNullOrEmpty(workPatternId))
                {
                    var workPattern = _workPatterns.SingleOrDefault(x => x.LookupValueId == workPatternId);
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