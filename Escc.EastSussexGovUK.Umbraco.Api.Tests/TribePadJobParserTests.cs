using System;
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
        public void JobTitleParsed()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var parser = new TribePadJobParser(lookupValuesProvider.Object);

            var job = parser.ParseJob(Properties.Resources.TribePadJobXml, "142");

            Assert.AreEqual("Job", job.JobTitle);
        }

        [Test]
        public void JobReferenceParsed()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var parser = new TribePadJobParser(lookupValuesProvider.Object);

            var job = parser.ParseJob(Properties.Resources.TribePadJobXml, "142");

            Assert.AreEqual("orbis/TP/12/142", job.Reference);
        }


        [Test]
        public void LocationParsed()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var parser = new TribePadJobParser(lookupValuesProvider.Object);

            var job = parser.ParseJob(Properties.Resources.TribePadJobXml, "142");

            Assert.AreEqual("Lewes", job.Locations[0]);
        }


        [Test]
        public void OrganisationParsed()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var parser = new TribePadJobParser(lookupValuesProvider.Object);

            var job = parser.ParseJob(Properties.Resources.TribePadJobXml, "142");

            Assert.AreEqual("East Sussex County Council", job.Organisation);
        }


        [Test]
        public void DepartmentParsed()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var parser = new TribePadJobParser(lookupValuesProvider.Object);

            var job = parser.ParseJob(Properties.Resources.TribePadJobXml, "142");

            Assert.AreEqual("Orbis (BSD)", job.Department);
        }

        /*
        [Test]
        public void ContractTypeParsed()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var parser = new TribePadJobParser(lookupValuesProvider.Object);

            var job = parser.ParseJob(Properties.Resources.TribePadJobXml, "142");

            Assert.AreEqual("Casual", job.ContractType);
        }
        */

        [Test]
        public void JobTypeParsed()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var parser = new TribePadJobParser(lookupValuesProvider.Object);

            var job = parser.ParseJob(Properties.Resources.TribePadJobXml, "142");

            Assert.AreEqual("Administration & Clerical", job.JobType);
        }

        [Test]
        public void ClosingDateParsed()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var parser = new TribePadJobParser(lookupValuesProvider.Object);

            var job = parser.ParseJob(Properties.Resources.TribePadJobXml, "142");

            Assert.AreEqual(new DateTime(2019, 02, 11), job.ClosingDate);
        }

        [Test]
        public void WorkPatternParsed()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            lookupValuesProvider.Setup(x => x.ReadWorkPatterns()).Returns(Task.FromResult(new List<JobsLookupValue>()
            {
                new JobsLookupValue() { FieldId = "13", LookupValueId = "16", Text = "Full Time" }
            } as IList<JobsLookupValue>));

            var parser = new TribePadJobParser(lookupValuesProvider.Object);
            var parsedJob = parser.ParseJob(Properties.Resources.TribePadJobXml, "142");

            Assert.IsNotNull(parsedJob.WorkPattern);
            Assert.IsTrue(parsedJob.WorkPattern.IsFullTime);
            Assert.IsFalse(parsedJob.WorkPattern.IsPartTime);
        }
    }

}
