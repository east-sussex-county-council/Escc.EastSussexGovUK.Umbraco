using Escc.EastSussexGovUK.Umbraco.Jobs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestFixture]
    public class JobSearchQueryTests
    {
        [Test]
        public void NoFilterReturnsAllJobs()
        {
            var query = new JobSearchQuery();

            var result = query.ToString();

            Assert.AreEqual("All jobs", result);
        }

        [Test]
        public void AllFiltersSingleValues()
        {
            // Format is [Work pattern] [type] jobs [in location] [paying salary range] [advertised by organisation] [and matching keywords/reference]
            var query = new JobSearchQuery();
            query.WorkPatterns.Add("part time");
            query.ContractTypes.Add("Permanent");
            query.JobTypes.Add("Care and social work");
            query.Locations.Add("Eastbourne");
            query.SalaryRanges.Add("£20000 - £30000");
            query.Organisations.Add("East Sussex County Council");
            query.Keywords = "test query";

            var result = query.ToString();

            Assert.AreEqual("Part time permanent care and social work jobs in Eastbourne paying £20000 - £30000 advertised by East Sussex County Council and matching 'test query'", result);
        }

        [Test]
        public void AllFiltersMultipleValues()
        {
            // Format is [Work pattern] [contract type] [type] jobs [in location] [paying salary range] [advertised by organisation] [and matching keywords/reference]
            var query = new JobSearchQuery();
            query.WorkPatterns.Add("Part time");
            query.WorkPatterns.Add("Full time");
            query.ContractTypes.Add("Permanent");
            query.ContractTypes.Add("Fixed term");
            query.JobTypes.Add("Care and social work");
            query.JobTypes.Add("Personnel and HR");
            query.Locations.Add("Eastbourne");
            query.Locations.Add("Lewes");
            query.SalaryRanges.Add("£20000 - £30000");
            query.SalaryRanges.Add("£40000 - £50000");
            query.Organisations.Add("East Sussex County Council");
            query.Organisations.Add("Sussex Downs College");
            query.Keywords = "test query";
            query.JobReference = "ABC1234";

            var result = query.ToString();

            Assert.AreEqual("Part time and full time permanent and fixed term care and social work and personnel and HR jobs in Eastbourne or Lewes paying £20000 - £30000 or £40000 - £50000 advertised by East Sussex County Council or Sussex Downs College and matching 'test query' or 'ABC1234'", result);
        }

        [Test]
        public void MoreThanTwoLocations()
        {
            var query = new JobSearchQuery();
            query.Locations.Add("Eastbourne");
            query.Locations.Add("Lewes");
            query.Locations.Add("Seaford");

            var result = query.ToString();

            Assert.AreEqual("Jobs in Eastbourne, Lewes or Seaford", result);
        }
    }
}
