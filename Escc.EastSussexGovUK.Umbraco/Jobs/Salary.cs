using System;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    public class Salary
    {
        public int? MinimumSalary { get; set; }
        public int? MaximumSalary { get; set; }

        public string SalaryRange { get; set; }

        public string SearchRange { get; set; }
    }
}