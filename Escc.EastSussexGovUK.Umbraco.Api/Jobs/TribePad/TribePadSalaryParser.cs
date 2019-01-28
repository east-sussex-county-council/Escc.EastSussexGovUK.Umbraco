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
        private IList<JobsLookupValue> _salaryFrequencies;
        private IList<JobsLookupValue> _payGrades;
        private string _payGradeFieldName;

        public TribePadSalaryParser(IJobsLookupValuesProvider lookupValuesProvider)
        {
            _lookupValuesProvider = lookupValuesProvider ?? throw new ArgumentNullException(nameof(lookupValuesProvider));
        }

        /// <summary>
        /// Parse salary information from the XML of a single TribePad job advert
        /// </summary>
        /// <param name="sourceData">XML for a single job, with a root element of &lt;job&gt;</param>
        /// <returns></returns>
        public async Task<Salary> ParseSalary(string sourceData)
        {
            await EnsureLookups();

            var jobXml = XDocument.Parse(sourceData);

            var salary = new Salary();

            var hourlyFrequencyId = _salaryFrequencies?.SingleOrDefault(x => x.Text == "Hourly")?.LookupValueId;
            var frequencyId = jobXml.Root.Element("salary_frequency")?.Value;

            if (frequencyId == hourlyFrequencyId)
            {
                salary.MinimumHourlyRate = Decimal.Parse(jobXml.Root.Element("salary_from").Value);
                salary.MaximumHourlyRate = Decimal.Parse(jobXml.Root.Element("salary_to").Value);

                SetSalaryFromData(jobXml, salary, 
                    x => x.MinimumHourlyRate == 0 && x.MaximumHourlyRate == 0,
                    x => (x.MinimumHourlyRate == x.MaximumHourlyRate || x.MaximumHourlyRate < x.MinimumHourlyRate),
                    x => $"£{x.MinimumHourlyRate?.ToString("n2")} per annum",
                    x => $"£{x.MinimumHourlyRate?.ToString("n2")} to £{x.MaximumHourlyRate?.ToString("n2")} per hour"
                    );
            }
            else
            {
                salary.MinimumSalary = Int32.Parse(jobXml.Root.Element("salary_from").Value, CultureInfo.InvariantCulture);
                salary.MaximumSalary = Int32.Parse(jobXml.Root.Element("salary_to").Value, CultureInfo.InvariantCulture);

                SetSalaryFromData(jobXml, salary,
                   x => x.MinimumSalary == 0 && x.MaximumSalary == 0,
                   x => (x.MinimumSalary == x.MaximumSalary || x.MaximumSalary < x.MinimumSalary),
                   x => $"£{x.MinimumSalary?.ToString("n0")} per annum",
                   x => $"£{x.MinimumSalary?.ToString("n0")} to £{x.MaximumSalary?.ToString("n0")} per annum"
                   );
            }

            return salary;
        }

        private void SetSalaryFromData(XDocument jobXml, Salary salary, 
            Func<Salary, bool> salaryIsZero, Func<Salary, bool> maxPayIsLessThanOrEqualToMinPay,
            Func<Salary, string> salaryExactValue, Func<Salary, string> salaryRange)
        {
            if (salaryIsZero(salary))
            {
                // 0 can mean 0 was entered, or nothing was entered
                var displayText = jobXml.Root.Element("salaryText")?.Value;
                if (!String.IsNullOrEmpty(displayText))
                {
                    salary.MinimumHourlyRate = null;
                    salary.MaximumHourlyRate = null;
                    salary.MinimumSalary = null;
                    salary.MaximumSalary = null;
                    salary.SalaryRange = displayText;
                }
                else
                {
                    // If no salaryText, is there a pay grade?
                    JobsLookupValue payGrade = null;
                    if (!String.IsNullOrEmpty(_payGradeFieldName))
                    {
                        payGrade = _payGrades?.SingleOrDefault(x => x.LookupValueId == jobXml.Root.Element(_payGradeFieldName)?.Element("answer")?.Value);
                    }

                    // If not, it's voluntary
                    if (payGrade != null)
                    {
                        salary.MinimumHourlyRate = null;
                        salary.MaximumHourlyRate = null;
                        salary.MinimumSalary = null;
                        salary.MaximumSalary = null;
                        salary.SalaryRange = payGrade.Text;
                    }
                    else
                    {
                        salary.SalaryRange = "Voluntary";
                    }
                }
            }
            else if (maxPayIsLessThanOrEqualToMinPay(salary))
            {
                salary.SalaryRange = salaryExactValue(salary);
            }
            else
            {
                salary.SalaryRange = salaryRange(salary);
            }
        }

        private async Task EnsureLookups()
        {
            if (_salaryFrequencies == null)
            {
                _salaryFrequencies = await _lookupValuesProvider.ReadSalaryFrequencies();
                if (_salaryFrequencies == null) _salaryFrequencies = new List<JobsLookupValue>();
            }
            if (_payGrades == null)
            {
                _payGrades = await _lookupValuesProvider.ReadPayGrades();
                if (_payGrades == null) _payGrades = new List<JobsLookupValue>();

                if (_payGrades.Any())
                {
                    _payGradeFieldName = "custom_" + _payGrades[0].FieldId;
                }
            }
        }
    }
}