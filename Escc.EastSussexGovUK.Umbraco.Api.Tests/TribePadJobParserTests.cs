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
        public async Task JobTitleParsed()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var salaryParser = new Mock<ISalaryParser>();
            var workPatternParser = new Mock<IWorkPatternParser>();
            var locationParser = new Mock<ILocationParser>();
            var parser = new TribePadJobParser(lookupValuesProvider.Object, salaryParser.Object, workPatternParser.Object, locationParser.Object, new Uri("https://www.example.org"));

            var job = await parser.ParseJob(Properties.Resources.TribePadJobXml, "142");

            Assert.AreEqual("Job", job.JobTitle);
        }

        [Test]
        public async Task JobReferenceParsed()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var salaryParser = new Mock<ISalaryParser>();
            var workPatternParser = new Mock<IWorkPatternParser>();
            var locationParser = new Mock<ILocationParser>();
            var parser = new TribePadJobParser(lookupValuesProvider.Object, salaryParser.Object, workPatternParser.Object, locationParser.Object, new Uri("https://www.example.org"));

            var job = await parser.ParseJob(Properties.Resources.TribePadJobXml, "142");

            Assert.AreEqual("orbis/TP/12/142", job.Reference);
        }

        [Test]
        public async Task LogoUrlParsed()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var salaryParser = new Mock<ISalaryParser>();
            var workPatternParser = new Mock<IWorkPatternParser>();
            var locationParser = new Mock<ILocationParser>();
            var parser = new TribePadJobParser(lookupValuesProvider.Object, salaryParser.Object, workPatternParser.Object, locationParser.Object, new Uri("https://www.example.org"));

            var job = await parser.ParseJob(Properties.Resources.TribePadJobXml, "142");

            Assert.AreEqual("https://www.example.org/image.png", job.LogoUrl.ToString());
        }

        [Test]
        public async Task OrganisationParsed()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var salaryParser = new Mock<ISalaryParser>();
            var workPatternParser = new Mock<IWorkPatternParser>();
            var locationParser = new Mock<ILocationParser>();
            var parser = new TribePadJobParser(lookupValuesProvider.Object, salaryParser.Object, workPatternParser.Object, locationParser.Object, new Uri("https://www.example.org"));

            var job = await parser.ParseJob(Properties.Resources.TribePadJobXml, "142");

            Assert.AreEqual("East Sussex County Council", job.Organisation);
        }


        [Test]
        public async Task DepartmentParsed()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var salaryParser = new Mock<ISalaryParser>();
            var workPatternParser = new Mock<IWorkPatternParser>();
            var locationParser = new Mock<ILocationParser>();
            var parser = new TribePadJobParser(lookupValuesProvider.Object, salaryParser.Object, workPatternParser.Object, locationParser.Object, new Uri("https://www.example.org"));

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
            var workPatternParser = new Mock<IWorkPatternParser>();
            var locationParser = new Mock<ILocationParser>();
            var parser = new TribePadJobParser(lookupValuesProvider.Object, salaryParser.Object, workPatternParser.Object, locationParser.Object, new Uri("https://www.example.org"));

            var job = await parser.ParseJob(Properties.Resources.TribePadJobXml, "142");

            Assert.AreEqual("Permanent", job.ContractType);
        }
        

        [Test]
        public async Task JobTypeParsed()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var salaryParser = new Mock<ISalaryParser>();
            var workPatternParser = new Mock<IWorkPatternParser>();
            var locationParser = new Mock<ILocationParser>();
            var parser = new TribePadJobParser(lookupValuesProvider.Object, salaryParser.Object, workPatternParser.Object, locationParser.Object, new Uri("https://www.example.org"));

            var job = await parser.ParseJob(Properties.Resources.TribePadJobXml, "142");

            Assert.AreEqual("Administration and Clerical", job.JobType);
        }

        [Test]
        public async Task ClosingDateParsed()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var salaryParser = new Mock<ISalaryParser>();
            var workPatternParser = new Mock<IWorkPatternParser>();
            var locationParser = new Mock<ILocationParser>();
            var parser = new TribePadJobParser(lookupValuesProvider.Object, salaryParser.Object, workPatternParser.Object, locationParser.Object, new Uri("https://www.example.org"));

            var job = await parser.ParseJob(Properties.Resources.TribePadJobXml, "142");

            Assert.AreEqual(new DateTime(2019, 02, 11), job.ClosingDate);
        }

        [Test]
        public async Task NumberOfPositionsParsed()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var salaryParser = new Mock<ISalaryParser>();
            var workPatternParser = new Mock<IWorkPatternParser>();
            var locationParser = new Mock<ILocationParser>();
            var parser = new TribePadJobParser(lookupValuesProvider.Object, salaryParser.Object, workPatternParser.Object, locationParser.Object, new Uri("https://www.example.org"));

            var parsedJob = await parser.ParseJob(Properties.Resources.TribePadJobXml, "142");

            Assert.AreEqual(2, parsedJob.NumberOfPositions);
        }


        [Test]
        public async Task BusinessUnitSetToPartnershipSetsOrganisationFromTitle()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var salaryParser = new Mock<ISalaryParser>();
            var workPatternParser = new Mock<IWorkPatternParser>();
            var locationParser = new Mock<ILocationParser>();
            var parser = new TribePadJobParser(lookupValuesProvider.Object, salaryParser.Object, workPatternParser.Object, locationParser.Object, new Uri("https://www.example.org"));

            var parsedJob = await parser.ParseJob(Properties.Resources.TribePadPartnershipJobXmlWithOrganisation, "142");

            Assert.AreEqual(string.Empty, parsedJob.Department);
            Assert.AreEqual("Example Organisation", parsedJob.Organisation);
            Assert.AreEqual("Job", parsedJob.JobTitle);
        }

        [Test]
        public async Task BusinessUnitSetToPartnershipSetsOrganisationToPartnershipIfOrganisationNotInTitle()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var salaryParser = new Mock<ISalaryParser>();
            var workPatternParser = new Mock<IWorkPatternParser>();
            var locationParser = new Mock<ILocationParser>();
            var parser = new TribePadJobParser(lookupValuesProvider.Object, salaryParser.Object, workPatternParser.Object, locationParser.Object, new Uri("https://www.example.org"));

            var parsedJob = await parser.ParseJob(Properties.Resources.TribePadPartnershipJobXmlOrganisationMissing, "142");

            Assert.AreEqual(string.Empty, parsedJob.Department);
            Assert.AreEqual("Partnership", parsedJob.Organisation);
        }

        [Test]
        public async Task SingleAttachmentIsAddedToJobAdvertHtml()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var salaryParser = new Mock<ISalaryParser>();
            var workPatternParser = new Mock<IWorkPatternParser>();
            var locationParser = new Mock<ILocationParser>();
            var parser = new TribePadJobParser(lookupValuesProvider.Object, salaryParser.Object, workPatternParser.Object, locationParser.Object, new Uri("https://www.example.org"));

            var parsedJob = await parser.ParseJob(Properties.Resources.TribePadJobWith1File, "550");

            Assert.IsTrue(parsedJob.AdvertHtml.ToHtmlString().Contains("https://recruitment.orbispartnership.co.uk/members/modules/jobV2/fdownload.php?j=ca8c44cac39a5740&amp;f=63f515371f7149b8"));
        }

        [Test]
        public async Task JobIdIsAddedToSingleAttachment()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var salaryParser = new Mock<ISalaryParser>();
            var workPatternParser = new Mock<IWorkPatternParser>();
            var locationParser = new Mock<ILocationParser>();
            var parser = new TribePadJobParser(lookupValuesProvider.Object, salaryParser.Object, workPatternParser.Object, locationParser.Object, new Uri("https://www.example.org"));

            var parsedJob = await parser.ParseJob(Properties.Resources.TribePadJobWith1File, "550");

            Assert.IsTrue(parsedJob.AdvertHtml.ToHtmlString().Contains("https://recruitment.orbispartnership.co.uk/members/modules/jobV2/fdownload.php?j=ca8c44cac39a5740&amp;f=63f515371f7149b8&amp;job=550"));
        }

        [Test]
        public async Task MultipleAttachmentsAreAddedToJobAdvertHtml()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var salaryParser = new Mock<ISalaryParser>();
            var workPatternParser = new Mock<IWorkPatternParser>();
            var locationParser = new Mock<ILocationParser>();
            var parser = new TribePadJobParser(lookupValuesProvider.Object, salaryParser.Object, workPatternParser.Object, locationParser.Object, new Uri("https://www.example.org"));

            var parsedJob = await parser.ParseJob(Properties.Resources.TribePadJobWith2Files, "550");

            Assert.IsTrue(parsedJob.AdvertHtml.ToHtmlString().Contains("https://recruitment.orbispartnership.co.uk/members/modules/jobV2/fdownload.php?j=ca8c44cac39a5740&amp;f=63f515371f7149b8"));
            Assert.IsTrue(parsedJob.AdvertHtml.ToHtmlString().Contains("https://recruitment.orbispartnership.co.uk/members/modules/jobV2/fdownload.php?j=ca8c44cac39a5740&amp;f=011389ada6f8a430"));
        }


        [Test]
        public async Task JobIdIsAddedToMultipleAttachments()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            var salaryParser = new Mock<ISalaryParser>();
            var workPatternParser = new Mock<IWorkPatternParser>();
            var locationParser = new Mock<ILocationParser>();
            var parser = new TribePadJobParser(lookupValuesProvider.Object, salaryParser.Object, workPatternParser.Object, locationParser.Object, new Uri("https://www.example.org"));

            var parsedJob = await parser.ParseJob(Properties.Resources.TribePadJobWith2Files, "550");

            Assert.IsTrue(parsedJob.AdvertHtml.ToHtmlString().Contains("https://recruitment.orbispartnership.co.uk/members/modules/jobV2/fdownload.php?j=ca8c44cac39a5740&amp;f=63f515371f7149b8&amp;job=550"));
            Assert.IsTrue(parsedJob.AdvertHtml.ToHtmlString().Contains("https://recruitment.orbispartnership.co.uk/members/modules/jobV2/fdownload.php?j=ca8c44cac39a5740&amp;f=011389ada6f8a430&amp;job=550"));
        }
    }

}
