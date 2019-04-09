using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Escc.Dates;
using Escc.EastSussexGovUK.Umbraco.Api.Jobs.Examine;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using Moq;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Api.Tests
{
    [TestFixture]
    public class BaseJobsIndexerTests
    {

        [Test]
        public async Task JobIdIsIndexed()
        {
            var job = new Job { Id = 123 };

            var jobsProvider = new Mock<IJobsDataProvider>();
            jobsProvider.Setup(x => x.ReadJobs(It.IsAny<JobSearchQuery>())).Returns(Task.FromResult(new JobSearchResult() { Jobs = new List<Job>() { job } }));
            jobsProvider.Setup(x => x.ReadJob(It.IsAny<string>())).Returns(Task.FromResult(job));

            var indexer = new Mock<BaseJobsIndexer>();
            indexer.CallBase = true;
            indexer.SetupGet(x => x.JobsProvider).Returns(jobsProvider.Object);

            var dataSets = await indexer.Object.GetAllDataAsync("Jobs");

            Assert.AreEqual(1, dataSets.Count());
            Assert.AreEqual(job.Id.ToString(), dataSets.First().RowData["id"]);
        }

        [Test]
        public async Task JobReferenceIsIndexed()
        {
            var job = new Job { Reference = "123" };

            var jobsProvider = new Mock<IJobsDataProvider>();
            jobsProvider.Setup(x => x.ReadJobs(It.IsAny<JobSearchQuery>())).Returns(Task.FromResult(new JobSearchResult() { Jobs = new List<Job>() { job } }));
            jobsProvider.Setup(x => x.ReadJob(It.IsAny<string>())).Returns(Task.FromResult(job));

            var indexer = new Mock<BaseJobsIndexer>();
            indexer.CallBase = true;
            indexer.SetupGet(x => x.JobsProvider).Returns(jobsProvider.Object);

            var dataSets = await indexer.Object.GetAllDataAsync("Jobs");

            Assert.AreEqual(1, dataSets.Count());
            Assert.AreEqual(job.Reference, dataSets.First().RowData["reference"]);
        }

        [Test]
        public async Task NumberOfPositionsIsIndexed()
        {
            var job = new Job { NumberOfPositions = 20 };

            var jobsProvider = new Mock<IJobsDataProvider>();
            jobsProvider.Setup(x => x.ReadJobs(It.IsAny<JobSearchQuery>())).Returns(Task.FromResult(new JobSearchResult() { Jobs = new List<Job>() { job } }));
            jobsProvider.Setup(x => x.ReadJob(It.IsAny<string>())).Returns(Task.FromResult(job));

            var indexer = new Mock<BaseJobsIndexer>();
            indexer.CallBase = true;
            indexer.SetupGet(x => x.JobsProvider).Returns(jobsProvider.Object);

            var dataSets = await indexer.Object.GetAllDataAsync("Jobs");

            Assert.AreEqual(1, dataSets.Count());
            Assert.AreEqual(job.NumberOfPositions.ToString(), dataSets.First().RowData["numberOfPositions"]);
        }

        [Test]
        public async Task LogoUrlIsIndexed()
        {
            var job = new Job { LogoUrl = new Uri("https://www.example.org") };

            var jobsProvider = new Mock<IJobsDataProvider>();
            jobsProvider.Setup(x => x.ReadJobs(It.IsAny<JobSearchQuery>())).Returns(Task.FromResult(new JobSearchResult() { Jobs = new List<Job>() { job } }));
            jobsProvider.Setup(x => x.ReadJob(It.IsAny<string>())).Returns(Task.FromResult(job));

            var indexer = new Mock<BaseJobsIndexer>();
            indexer.CallBase = true;
            indexer.SetupGet(x => x.JobsProvider).Returns(jobsProvider.Object);

            var dataSets = await indexer.Object.GetAllDataAsync("Jobs");

            Assert.AreEqual(1, dataSets.Count());
            Assert.AreEqual(job.LogoUrl.ToString(), dataSets.First().RowData["logoUrl"]);
        }

        [Test]
        public async Task JobTitleIsIndexed()
        {
            var job = new Job { JobTitle = "Example" };

            var jobsProvider = new Mock<IJobsDataProvider>();
            jobsProvider.Setup(x => x.ReadJobs(It.IsAny<JobSearchQuery>())).Returns(Task.FromResult(new JobSearchResult() { Jobs = new List<Job>() { job } }));
            jobsProvider.Setup(x => x.ReadJob(It.IsAny<string>())).Returns(Task.FromResult(job));

            var indexer = new Mock<BaseJobsIndexer>();
            indexer.CallBase = true;
            indexer.SetupGet(x => x.JobsProvider).Returns(jobsProvider.Object);

            var dataSets = await indexer.Object.GetAllDataAsync("Jobs");

            Assert.AreEqual(1, dataSets.Count());
            Assert.AreEqual(job.JobTitle, dataSets.First().RowData["title"]);
        }

        [Test]
        public async Task SalaryFromWithPenceIsIndexed()
        {
            var job = new Job();
            job.Salary.MinimumSalary = 20000.5m;

            var jobsProvider = new Mock<IJobsDataProvider>();
            jobsProvider.Setup(x => x.ReadJobs(It.IsAny<JobSearchQuery>())).Returns(Task.FromResult(new JobSearchResult() { Jobs = new List<Job>() { job } }));
            jobsProvider.Setup(x => x.ReadJob(It.IsAny<string>())).Returns(Task.FromResult(job));

            var indexer = new Mock<BaseJobsIndexer>();
            indexer.CallBase = true;
            indexer.SetupGet(x => x.JobsProvider).Returns(jobsProvider.Object);

            var dataSets = await indexer.Object.GetAllDataAsync("Jobs");

            Assert.AreEqual(1, dataSets.Count());
            Assert.AreEqual("002000050", dataSets.First().RowData["salaryMin"]);
        }


        [Test]
        public async Task SalaryToWithPenceIsIndexed()
        {
            var job = new Job();
            job.Salary.MaximumSalary = 20000.5m;

            var jobsProvider = new Mock<IJobsDataProvider>();
            jobsProvider.Setup(x => x.ReadJobs(It.IsAny<JobSearchQuery>())).Returns(Task.FromResult(new JobSearchResult() { Jobs = new List<Job>() { job } }));
            jobsProvider.Setup(x => x.ReadJob(It.IsAny<string>())).Returns(Task.FromResult(job));

            var indexer = new Mock<BaseJobsIndexer>();
            indexer.CallBase = true;
            indexer.SetupGet(x => x.JobsProvider).Returns(jobsProvider.Object);

            var dataSets = await indexer.Object.GetAllDataAsync("Jobs");

            Assert.AreEqual(1, dataSets.Count());
            Assert.AreEqual("002000050", dataSets.First().RowData["salaryMax"]);
        }

        [Test]
        public async Task SalaryRangeIsIndexed()
        {
            var job = new Job();
            job.Salary.SalaryRange = "Example";

            var jobsProvider = new Mock<IJobsDataProvider>();
            jobsProvider.Setup(x => x.ReadJobs(It.IsAny<JobSearchQuery>())).Returns(Task.FromResult(new JobSearchResult() { Jobs = new List<Job>() { job } }));
            jobsProvider.Setup(x => x.ReadJob(It.IsAny<string>())).Returns(Task.FromResult(job));

            var indexer = new Mock<BaseJobsIndexer>();
            indexer.CallBase = true;
            indexer.SetupGet(x => x.JobsProvider).Returns(jobsProvider.Object);

            var dataSets = await indexer.Object.GetAllDataAsync("Jobs");

            Assert.AreEqual(1, dataSets.Count());
            Assert.AreEqual(job.Salary.SalaryRange, dataSets.First().RowData["salary"]);
        }

        [Test]
        public async Task HoursPerWeekIsIndexed()
        {
            var job = new Job();
            job.WorkPattern.HoursPerWeek = 20;

            var jobsProvider = new Mock<IJobsDataProvider>();
            jobsProvider.Setup(x => x.ReadJobs(It.IsAny<JobSearchQuery>())).Returns(Task.FromResult(new JobSearchResult() { Jobs = new List<Job>() { job } }));
            jobsProvider.Setup(x => x.ReadJob(It.IsAny<string>())).Returns(Task.FromResult(job));

            var indexer = new Mock<BaseJobsIndexer>();
            indexer.CallBase = true;
            indexer.SetupGet(x => x.JobsProvider).Returns(jobsProvider.Object);

            var dataSets = await indexer.Object.GetAllDataAsync("Jobs");

            Assert.AreEqual(1, dataSets.Count());
            Assert.AreEqual(job.WorkPattern.HoursPerWeek.ToString(), dataSets.First().RowData["hoursPerWeek"]);
        }

        [Test]
        public async Task OrganisationIsIndexed()
        {
            var job = new Job { Organisation = "Example" };

            var jobsProvider = new Mock<IJobsDataProvider>();
            jobsProvider.Setup(x => x.ReadJobs(It.IsAny<JobSearchQuery>())).Returns(Task.FromResult(new JobSearchResult() { Jobs = new List<Job>() { job } }));
            jobsProvider.Setup(x => x.ReadJob(It.IsAny<string>())).Returns(Task.FromResult(job));

            var indexer = new Mock<BaseJobsIndexer>();
            indexer.CallBase = true;
            indexer.SetupGet(x => x.JobsProvider).Returns(jobsProvider.Object);

            var dataSets = await indexer.Object.GetAllDataAsync("Jobs");

            Assert.AreEqual(1, dataSets.Count());
            Assert.AreEqual(job.Organisation, dataSets.First().RowData["organisation"]);
        }

        [Test]
        public async Task JobTypeIsIndexed()
        {
            var job = new Job { JobType = "Example" };

            var jobsProvider = new Mock<IJobsDataProvider>();
            jobsProvider.Setup(x => x.ReadJobs(It.IsAny<JobSearchQuery>())).Returns(Task.FromResult(new JobSearchResult() { Jobs = new List<Job>() { job } }));
            jobsProvider.Setup(x => x.ReadJob(It.IsAny<string>())).Returns(Task.FromResult(job));

            var indexer = new Mock<BaseJobsIndexer>();
            indexer.CallBase = true;
            indexer.SetupGet(x => x.JobsProvider).Returns(jobsProvider.Object);

            var dataSets = await indexer.Object.GetAllDataAsync("Jobs");

            Assert.AreEqual(1, dataSets.Count());
            Assert.AreEqual(job.JobType, dataSets.First().RowData["jobType"]);
        }

        [Test]
        public async Task ContractTypeIsIndexed()
        {
            var job = new Job { ContractType = "Example" };

            var jobsProvider = new Mock<IJobsDataProvider>();
            jobsProvider.Setup(x => x.ReadJobs(It.IsAny<JobSearchQuery>())).Returns(Task.FromResult(new JobSearchResult() { Jobs = new List<Job>() { job } }));
            jobsProvider.Setup(x => x.ReadJob(It.IsAny<string>())).Returns(Task.FromResult(job));

            var indexer = new Mock<BaseJobsIndexer>();
            indexer.CallBase = true;
            indexer.SetupGet(x => x.JobsProvider).Returns(jobsProvider.Object);

            var dataSets = await indexer.Object.GetAllDataAsync("Jobs");

            Assert.AreEqual(1, dataSets.Count());
            Assert.AreEqual(job.ContractType, dataSets.First().RowData["contractType"]);
        }

        [Test]
        public async Task DepartmentIsIndexed()
        {
            var job = new Job { Department = "Example" };

            var jobsProvider = new Mock<IJobsDataProvider>();
            jobsProvider.Setup(x => x.ReadJobs(It.IsAny<JobSearchQuery>())).Returns(Task.FromResult(new JobSearchResult() { Jobs = new List<Job>() { job } }));
            jobsProvider.Setup(x => x.ReadJob(It.IsAny<string>())).Returns(Task.FromResult(job));

            var indexer = new Mock<BaseJobsIndexer>();
            indexer.CallBase = true;
            indexer.SetupGet(x => x.JobsProvider).Returns(jobsProvider.Object);

            var dataSets = await indexer.Object.GetAllDataAsync("Jobs");

            Assert.AreEqual(1, dataSets.Count());
            Assert.AreEqual(job.Department, dataSets.First().RowData["department"]);
        }

        [Test]
        public async Task DatePublishedIsIndexed()
        {
            var job = new Job { DatePublished = new DateTime(2019, 1, 29) };

            var jobsProvider = new Mock<IJobsDataProvider>();
            jobsProvider.Setup(x => x.ReadJobs(It.IsAny<JobSearchQuery>())).Returns(Task.FromResult(new JobSearchResult() { Jobs = new List<Job>() { job } }));
            jobsProvider.Setup(x => x.ReadJob(It.IsAny<string>())).Returns(Task.FromResult(job));

            var indexer = new Mock<BaseJobsIndexer>();
            indexer.CallBase = true;
            indexer.SetupGet(x => x.JobsProvider).Returns(jobsProvider.Object);

            var dataSets = await indexer.Object.GetAllDataAsync("Jobs");

            Assert.AreEqual(1, dataSets.Count());
            Assert.AreEqual(job.DatePublished.ToIso8601DateTime(), dataSets.First().RowData["datePublished"]);
        }

        [Test]
        public async Task ClosingDateIsIndexed()
        {
            var job = new Job { ClosingDate = new DateTime(2019, 1, 29) };

            var jobsProvider = new Mock<IJobsDataProvider>();
            jobsProvider.Setup(x => x.ReadJobs(It.IsAny<JobSearchQuery>())).Returns(Task.FromResult(new JobSearchResult() { Jobs = new List<Job>() { job } }));
            jobsProvider.Setup(x => x.ReadJob(It.IsAny<string>())).Returns(Task.FromResult(job));

            var indexer = new Mock<BaseJobsIndexer>();
            indexer.CallBase = true;
            indexer.SetupGet(x => x.JobsProvider).Returns(jobsProvider.Object);

            var dataSets = await indexer.Object.GetAllDataAsync("Jobs");

            Assert.AreEqual(1, dataSets.Count());
            Assert.AreEqual(job.ClosingDate.ToIso8601DateTime(), dataSets.First().RowData["closingDate"]);
        }

        [Test]
        public async Task LocationIsIndexed()
        {
            var job = new Job();
            job.Locations.Add("Example");

            var jobsProvider = new Mock<IJobsDataProvider>();
            jobsProvider.Setup(x => x.ReadJobs(It.IsAny<JobSearchQuery>())).Returns(Task.FromResult(new JobSearchResult() { Jobs = new List<Job>() { job } }));
            jobsProvider.Setup(x => x.ReadJob(It.IsAny<string>())).Returns(Task.FromResult(job));

            var indexer = new Mock<BaseJobsIndexer>();
            indexer.CallBase = true;
            indexer.SetupGet(x => x.JobsProvider).Returns(jobsProvider.Object);

            var dataSets = await indexer.Object.GetAllDataAsync("Jobs");

            Assert.AreEqual(1, dataSets.Count());
            Assert.AreEqual(job.Locations[0], dataSets.First().RowData["location"]);
        }

        [Test]
        public async Task WorkPatternIsIndexed()
        {
            var job = new Job();
            job.WorkPattern.WorkPatterns.Add("Example");

            var jobsProvider = new Mock<IJobsDataProvider>();
            jobsProvider.Setup(x => x.ReadJobs(It.IsAny<JobSearchQuery>())).Returns(Task.FromResult(new JobSearchResult() { Jobs = new List<Job>() { job } }));
            jobsProvider.Setup(x => x.ReadJob(It.IsAny<string>())).Returns(Task.FromResult(job));

            var indexer = new Mock<BaseJobsIndexer>();
            indexer.CallBase = true;
            indexer.SetupGet(x => x.JobsProvider).Returns(jobsProvider.Object);

            var dataSets = await indexer.Object.GetAllDataAsync("Jobs");

            Assert.AreEqual(1, dataSets.Count());
            Assert.AreEqual(job.WorkPattern.WorkPatterns[0], dataSets.First().RowData["workPattern"]);
        }

        [Test]
        public async Task AdvertHtmlIsIndexed()
        {
            var job = new Job() { AdvertHtml = new HtmlString("<p>Example</p>") };

            var jobsProvider = new Mock<IJobsDataProvider>();
            jobsProvider.Setup(x => x.ReadJobs(It.IsAny<JobSearchQuery>())).Returns(Task.FromResult(new JobSearchResult() { Jobs = new List<Job>() { job } }));
            jobsProvider.Setup(x => x.ReadJob(It.IsAny<string>())).Returns(Task.FromResult(job));

            var indexer = new Mock<BaseJobsIndexer>();
            indexer.CallBase = true;
            indexer.SetupGet(x => x.JobsProvider).Returns(jobsProvider.Object);

            var dataSets = await indexer.Object.GetAllDataAsync("Jobs");

            Assert.AreEqual(1, dataSets.Count());
            Assert.AreEqual(job.AdvertHtml.ToHtmlString(), dataSets.First().RowData["fullHtml"]);
        }


        [Test]
        public async Task AdditionalInfomationIsIndexed()
        {
            var job = new Job() { AdditionalInformationHtml = new HtmlString("<p>Example</p>") };

            var jobsProvider = new Mock<IJobsDataProvider>();
            jobsProvider.Setup(x => x.ReadJobs(It.IsAny<JobSearchQuery>())).Returns(Task.FromResult(new JobSearchResult() { Jobs = new List<Job>() { job } }));
            jobsProvider.Setup(x => x.ReadJob(It.IsAny<string>())).Returns(Task.FromResult(job));

            var indexer = new Mock<BaseJobsIndexer>();
            indexer.CallBase = true;
            indexer.SetupGet(x => x.JobsProvider).Returns(jobsProvider.Object);

            var dataSets = await indexer.Object.GetAllDataAsync("Jobs");

            Assert.AreEqual(1, dataSets.Count());
            Assert.AreEqual(job.AdditionalInformationHtml.ToHtmlString(), dataSets.First().RowData["additionalInfo"]);
        }

        [Test]
        public async Task EqualOpportunitiesIsIndexed()
        {
            var job = new Job() { EqualOpportunitiesHtml = new HtmlString("<p>Example</p>") };

            var jobsProvider = new Mock<IJobsDataProvider>();
            jobsProvider.Setup(x => x.ReadJobs(It.IsAny<JobSearchQuery>())).Returns(Task.FromResult(new JobSearchResult() { Jobs = new List<Job>() { job } }));
            jobsProvider.Setup(x => x.ReadJob(It.IsAny<string>())).Returns(Task.FromResult(job));

            var indexer = new Mock<BaseJobsIndexer>();
            indexer.CallBase = true;
            indexer.SetupGet(x => x.JobsProvider).Returns(jobsProvider.Object);

            var dataSets = await indexer.Object.GetAllDataAsync("Jobs");

            Assert.AreEqual(1, dataSets.Count());
            Assert.AreEqual(job.EqualOpportunitiesHtml.ToHtmlString(), dataSets.First().RowData["equalOpportunities"]);
        }

        [Test]
        public async Task ApplyUrlIsIndexed()
        {
            var job = new Job { ApplyUrl = new Uri("https://www.example.org") };

            var jobsProvider = new Mock<IJobsDataProvider>();
            jobsProvider.Setup(x => x.ReadJobs(It.IsAny<JobSearchQuery>())).Returns(Task.FromResult(new JobSearchResult() { Jobs = new List<Job>() { job } }));
            jobsProvider.Setup(x => x.ReadJob(It.IsAny<string>())).Returns(Task.FromResult(job));

            var indexer = new Mock<BaseJobsIndexer>();
            indexer.CallBase = true;
            indexer.SetupGet(x => x.JobsProvider).Returns(jobsProvider.Object);

            var dataSets = await indexer.Object.GetAllDataAsync("Jobs");

            Assert.AreEqual(1, dataSets.Count());
            Assert.AreEqual(job.ApplyUrl.ToString(), dataSets.First().RowData["applyUrl"]);
        }
    }
}
