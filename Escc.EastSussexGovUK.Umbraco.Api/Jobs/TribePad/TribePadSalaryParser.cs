using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Escc.EastSussexGovUK.Umbraco.Jobs;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.TribePad
{
    /// <summary>
    /// Parse salary information from the XML of a single TribePad job advert
    /// </summary>
    public class TribePadSalaryParser : ISalaryParser
    {
        private readonly IJobsLookupValuesProvider _lookupValuesProvider;

        public TribePadSalaryParser(IJobsLookupValuesProvider lookupValuesProvider)
        {
            _lookupValuesProvider = lookupValuesProvider ?? throw new ArgumentNullException(nameof(lookupValuesProvider));
        }

        public Salary ParseSalaryFromDescription(string salaryDescription)
        {
            return new Salary();
        }

        public async Task<Salary> ParseSalaryFromJobAdvert(string sourceData)
        {
            var jobXml = XDocument.Parse(sourceData);

            var salary = new Salary();

            var salaryFrequencies = await _lookupValuesProvider.ReadSalaryFrequencies();
            var hourlyFrequencyId = salaryFrequencies?.SingleOrDefault(x => x.Text == "Hourly").LookupValueId;
            var frequencyId = jobXml.Root.Element("salary_frequency")?.Value;

            if (frequencyId == hourlyFrequencyId)
            {
                salary.HourlyRate = Decimal.Parse(jobXml.Root.Element("salary_from").Value);
            }
            else
            {
                salary.MinimumSalary = Int32.Parse(jobXml.Root.Element("salary_from").Value, CultureInfo.InvariantCulture);
                salary.MaximumSalary = Int32.Parse(jobXml.Root.Element("salary_to").Value, CultureInfo.InvariantCulture);
                if (salary.MinimumSalary == 0 && salary.MaximumSalary == 0)
                {
                    salary.SalaryRange = "Voluntary";
                }
                else if (salary.MinimumSalary == salary.MaximumSalary)
                {
                    salary.SalaryRange = $"£{salary.MinimumSalary?.ToString("n0")} per annum";
                }
                else
                {
                    salary.SalaryRange = $"£{salary.MinimumSalary?.ToString("n0")} to £{salary.MaximumSalary?.ToString("n0")} per annum";
                }
            }

            return salary;
        }
    }
}