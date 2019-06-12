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
using System.Text;

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
        private readonly ILocationParser _locationParser;
        private readonly Uri _applyUrl;
        private IEnumerable<JobsLookupValue> _contractTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="TribePadJobParser"/> class.
        /// </summary>
        /// <param name="lookupValuesProvider">A method of supplying lookup values for identifiers referenced by the job data</param>
        /// <param name="salaryParser">A method of parsing salary information for the job</param>
        /// <param name="workPatternParser">A method of parsing work pattern information for the job</param>
        /// <param name="locationParser">A method of parsing locations for a job</param>
        /// <param name="applyUrl">The URL to apply for a job, with {0} to represent where the job id should be used</param>
        /// <exception cref="ArgumentNullException">lookupValuesProvider or salaryParser or workPatternParser or applyUrl</exception>
        public TribePadJobParser(IJobsLookupValuesProvider lookupValuesProvider, ISalaryParser salaryParser, IWorkPatternParser workPatternParser, ILocationParser locationParser, Uri applyUrl)
        {
            _lookupValuesProvider = lookupValuesProvider ?? throw new ArgumentNullException(nameof(lookupValuesProvider));
            _salaryParser = salaryParser ?? throw new ArgumentNullException(nameof(salaryParser));
            _workPatternParser = workPatternParser ?? throw new ArgumentNullException(nameof(workPatternParser));
            _locationParser = locationParser ?? throw new ArgumentNullException(nameof(locationParser));
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
        public async Task<IList<Job>> Parse(Stream stream)
        {
            await EnsureLookupValues();

            var xml = XDocument.Load(stream);
            var jobs = new List<Job>();

            var jobsXml = xml.Root.Element("jobs").Elements("job");

            foreach (var jobXml in jobsXml)
            {
                jobs.Add(await ParseJob(jobXml));
            }

            return jobs;
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

            var canApplyForThisJob = true;
            var comparableDepartment = job.Department.ToUpperInvariant();
            if (comparableDepartment == "PARTNERSHIP")
            {
                canApplyForThisJob = false;
                if (!ParseAndMoveOrganisationFromJobTitle(job))
                {
                    job.Organisation = job.Department;
                }
                job.Department = string.Empty;
            }
            else if (comparableDepartment == "ESCC SCHOOLS" || comparableDepartment == "ESCC ACADEMIES")
            {
                if (ParseAndMoveOrganisationFromJobTitle(job))
                {
                    job.Department = string.Empty;
                }
                else
                {
                    // If it's a school job but the school name is not in the job title, it's unknown
                    job.Organisation = string.Empty;
                    job.Department = string.Empty;
                }
                canApplyForThisJob = false;
            }
            else if (comparableDepartment == "CHILDREN SERVICES")
            {
                job.Department = "Children's Services";
            }

            var locations = _locationParser.ParseLocations(jobXml.ToString());
            if (locations != null)
            {
                foreach (var location in locations)
                {
                    job.Locations.Add(location);
                }
            }

            var logo = jobXml.Element("media")?.Element("logo")?.Element("url")?.Value;
            if (!String.IsNullOrEmpty(logo))
            {
                // Ignore width and height from TribePad as they can be wrong!
                job.LogoUrl = new Uri(logo);
            }

            var advertHtml = new StringBuilder();
            advertHtml.Append(HttpUtility.HtmlDecode(jobXml.Element("package_description")?.Value));
            advertHtml.Append(HttpUtility.HtmlDecode(jobXml.Element("summary_external")?.Value));
            advertHtml.Append(HttpUtility.HtmlDecode(jobXml.Element("main_responsibilities")?.Value));

            var files = jobXml.Element("files")?.Elements("file");
            if (files != null)
            {
                if (files.Count() == 1)
                {
                    advertHtml.Append("<h2>Documents</h2><p>").AppendLinkToFile(files.First()).Append("</p>");
                }
                else
                {
                    advertHtml.Append("<h2>Documents</h2><ul>");
                    foreach (var file in files)
                    {
                        advertHtml.Append("<li>").AppendLinkToFile(file).Append("</li>");
                    }
                    advertHtml.Append("</ul>");
                }
            }

            job.AdvertHtml = new HtmlString(advertHtml.ToString());

            job.AdditionalInformationHtml = new HtmlString(HttpUtility.HtmlDecode(jobXml.Element("ideal_candidate")?.Value));
            job.EqualOpportunitiesHtml = new HtmlString(HttpUtility.HtmlDecode(jobXml.Element("about_company")?.Value));

            job.JobType = HttpUtility.HtmlDecode(jobXml.Element("category_name")?.Value)?.Replace(" & ", " and ");

            if (jobXml.Element("no_apply")?.Value == "0" && canApplyForThisJob)
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

        private static bool ParseAndMoveOrganisationFromJobTitle(Job job)
        {
            if (job.JobTitle.Contains("(") && job.JobTitle.EndsWith(")"))
            {
                var orgStarts = job.JobTitle.LastIndexOf("(");
                job.Organisation = job.JobTitle.Substring(orgStarts + 1).TrimEnd(')');
                job.JobTitle = job.JobTitle.Substring(0, orgStarts).TrimEnd();
                return true;
            }
            return false;
        }
    }

    internal static class JobParserExtensionMethods
    {
        internal static StringBuilder AppendLinkToFile(this StringBuilder advertHtml, XElement file)
        {
            return advertHtml.Append("<a href=\"").Append(file.Element("url").Value.Trim()).Append("\">").Append(HttpUtility.HtmlDecode(file.Element("description").Value.Trim())).Append("</a>");
        }
    }
}