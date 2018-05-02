using System;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Details of the salary for a job
    /// </summary>
    public class Salary
    {
        /// <summary>
        /// Gets or sets the hourly rate in pounds.
        /// </summary>
        public decimal? HourlyRate { get; set; }
        public int? MinimumSalary { get; set; }
        public int? MaximumSalary { get; set; }

        public string SalaryRange { get; set; }

        public string SearchRange { get; set; }
    }
}