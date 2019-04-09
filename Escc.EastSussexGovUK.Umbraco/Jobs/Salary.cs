using System;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Details of the salary for a job
    /// </summary>
    public class Salary
    {
        /// <summary>
        /// Gets or sets the minimum hourly rate in pounds.
        /// </summary>
        public decimal? MinimumHourlyRate { get; set; }

        /// <summary>
        /// Gets or sets the maximum hourly rate in pounds.
        /// </summary>
        public decimal? MaximumHourlyRate { get; set; }

        /// <summary>
        /// Gets or sets the minimum annual salary in pounds.
        /// </summary>
        public decimal? MinimumSalary { get; set; }

        /// <summary>
        /// Gets or sets the maximum annual salary in pounds.
        /// </summary>
        public decimal? MaximumSalary { get; set; }

        /// <summary>
        /// Gets or sets a description of the salary based on <see cref="MinimumHourlyRate"/>, <see cref="MaximumHourlyRate"/>, <see cref="MinimumSalary"/> and <see cref="MaximumSalary"/>
        /// </summary>
        public string SalaryRange { get; set; }

        public string SearchRange { get; set; }
    }
}