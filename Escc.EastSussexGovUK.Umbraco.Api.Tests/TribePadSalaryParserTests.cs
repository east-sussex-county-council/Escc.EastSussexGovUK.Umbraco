﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Umbraco.Api.Jobs.TribePad;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using Moq;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Api.Tests
{
    [TestFixture]
    public class TribePadSalaryParserTests
    {
        [Test]
        public async Task SalaryRangeIsParsed()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var parser = new TribePadSalaryParser(lookupValuesProvider.Object);

            var salary = await parser.ParseSalaryFromJobAdvert(Properties.Resources.TribePadSalaryRange);

            Assert.AreEqual(15000, salary.MinimumSalary);
            Assert.AreEqual(19999, salary.MaximumSalary);
            Assert.AreEqual("£15,000 to £19,999 per annum", salary.SalaryRange);
        }

        [Test]
        public async Task ExactSalaryIsParsed()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var parser = new TribePadSalaryParser(lookupValuesProvider.Object);

            var salary = await parser.ParseSalaryFromJobAdvert(Properties.Resources.TribePadSalaryExact);

            Assert.AreEqual(23000, salary.MinimumSalary);
            Assert.AreEqual(23000, salary.MaximumSalary);
            Assert.AreEqual("£23,000 per annum", salary.SalaryRange);
        }

        [Test]
        public async Task VoluntaryRoleIsParsed()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var parser = new TribePadSalaryParser(lookupValuesProvider.Object);

            var salary = await parser.ParseSalaryFromJobAdvert(Properties.Resources.TribePadSalaryVoluntary);

            Assert.AreEqual(0, salary.MinimumSalary);
            Assert.AreEqual(0, salary.MaximumSalary);
            Assert.AreEqual("Voluntary", salary.SalaryRange);
        }

        [Test]
        public async Task HourlyRateIsNullIfNotFound()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var parser = new TribePadSalaryParser(lookupValuesProvider.Object);

            var salary = await parser.ParseSalaryFromJobAdvert(Properties.Resources.TribePadSalaryExact);

            Assert.IsNull(salary.HourlyRate);
        }

        [Test]
        public async Task HourlyRateExactIsParsed()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            lookupValuesProvider.Setup(x => x.ReadSalaryFrequencies()).Returns(Task.FromResult(new List<JobsLookupValue>()
            {
                new JobsLookupValue() { LookupValueId="2", Text = "Hourly" }
            } as IList<JobsLookupValue>));
            var parser = new TribePadSalaryParser(lookupValuesProvider.Object);

            var salary = await parser.ParseSalaryFromJobAdvert(Properties.Resources.TribePadSalaryHourlyExact);

            Assert.IsNull(salary.MinimumSalary);
            Assert.IsNull(salary.MaximumSalary);
            Assert.AreEqual(19.22, salary.HourlyRate);
        }
    }
}
