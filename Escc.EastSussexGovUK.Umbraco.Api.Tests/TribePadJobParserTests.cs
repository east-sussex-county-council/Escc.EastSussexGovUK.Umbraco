﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Umbraco.Api.Jobs;
using Escc.EastSussexGovUK.Umbraco.Api.Jobs.TribePad;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using Moq;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Api.Tests
{
    [TestFixture]
    public class TribePadJobParserTests
    {
        [Test]
        public async Task JobTitleParsed()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var salaryParser = new Mock<ISalaryParser>();
            var parser = new TribePadJobParser(lookupValuesProvider.Object, salaryParser.Object);

            var job = await parser.ParseJob(Properties.Resources.TribePadJobXml, "142");

            Assert.AreEqual("Job", job.JobTitle);
        }

        [Test]
        public async Task JobReferenceParsed()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var salaryParser = new Mock<ISalaryParser>();
            var parser = new TribePadJobParser(lookupValuesProvider.Object, salaryParser.Object);

            var job = await parser.ParseJob(Properties.Resources.TribePadJobXml, "142");

            Assert.AreEqual("orbis/TP/12/142", job.Reference);
        }


        [Test]
        public async Task LocationParsed()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var salaryParser = new Mock<ISalaryParser>();
            var parser = new TribePadJobParser(lookupValuesProvider.Object, salaryParser.Object);

            var job = await parser.ParseJob(Properties.Resources.TribePadJobXml, "142");

            Assert.AreEqual("Lewes", job.Locations[0]);
        }


        [Test]
        public async Task OrganisationParsed()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var salaryParser = new Mock<ISalaryParser>();
            var parser = new TribePadJobParser(lookupValuesProvider.Object, salaryParser.Object);

            var job = await parser.ParseJob(Properties.Resources.TribePadJobXml, "142");

            Assert.AreEqual("East Sussex County Council", job.Organisation);
        }


        [Test]
        public async Task DepartmentParsed()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var salaryParser = new Mock<ISalaryParser>();
            var parser = new TribePadJobParser(lookupValuesProvider.Object, salaryParser.Object);

            var job = await parser.ParseJob(Properties.Resources.TribePadJobXml, "142");

            Assert.AreEqual("Orbis (BSD)", job.Department);
        }

        
        [Test]
        public async Task ContractTypeParsed()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            lookupValuesProvider.Setup(x => x.ReadContractTypes()).Returns(Task.FromResult(new List<JobsLookupValue>()
            {
                new JobsLookupValue() { LookupValueId = "2", Text = "Permanent" }
            } as IList<JobsLookupValue>));
            var salaryParser = new Mock<ISalaryParser>();
            var parser = new TribePadJobParser(lookupValuesProvider.Object, salaryParser.Object);

            var job = await parser.ParseJob(Properties.Resources.TribePadJobXml, "142");

            Assert.AreEqual("Permanent", job.ContractType);
        }
        

        [Test]
        public async Task JobTypeParsed()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var salaryParser = new Mock<ISalaryParser>();
            var parser = new TribePadJobParser(lookupValuesProvider.Object, salaryParser.Object);

            var job = await parser.ParseJob(Properties.Resources.TribePadJobXml, "142");

            Assert.AreEqual("Administration & Clerical", job.JobType);
        }

        [Test]
        public async Task ClosingDateParsed()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var salaryParser = new Mock<ISalaryParser>();
            var parser = new TribePadJobParser(lookupValuesProvider.Object, salaryParser.Object);

            var job = await parser.ParseJob(Properties.Resources.TribePadJobXml, "142");

            Assert.AreEqual(new DateTime(2019, 02, 11), job.ClosingDate);
        }

        [Test]
        public async Task WorkPatternParsed()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            lookupValuesProvider.Setup(x => x.ReadWorkPatterns()).Returns(Task.FromResult(new List<JobsLookupValue>()
            {
                new JobsLookupValue() { FieldId = "13", LookupValueId = "16", Text = "Full Time" }
            } as IList<JobsLookupValue>));

            var salaryParser = new Mock<ISalaryParser>();
            var parser = new TribePadJobParser(lookupValuesProvider.Object, salaryParser.Object);

            var parsedJob = await parser.ParseJob(Properties.Resources.TribePadJobXml, "142");

            Assert.IsNotNull(parsedJob.WorkPattern);
            Assert.IsTrue(parsedJob.WorkPattern.IsFullTime);
            Assert.IsFalse(parsedJob.WorkPattern.IsPartTime);
        }
    }

}