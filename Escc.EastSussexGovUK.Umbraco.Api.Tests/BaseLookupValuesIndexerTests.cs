using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Umbraco.Api.Jobs.Examine;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using Moq;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Api.Tests
{
    [TestFixture]
    public class BaseLookupValuesIndexerTests
    {
        [Test]
        public async Task LocationDataSetsAreCreated()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            lookupValuesProvider.Setup(x => x.ReadLocations()).Returns(Task.FromResult(new List<JobsLookupValue>() {
                new JobsLookupValue() { LookupValueId = "1", Text = "Example" }
            } as IList<JobsLookupValue>));
            var indexer = new Mock<BaseLookupValuesIndexer>(lookupValuesProvider.Object);
            indexer.CallBase = true;

            var dataSets = await indexer.Object.GetAllDataAsync("Job");

            Assert.NotZero(dataSets.Count(x => x.RowData["group"] == "Location"));
        }

        [Test]
        public async Task JobTypeDataSetsAreCreated()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            lookupValuesProvider.Setup(x => x.ReadJobTypes()).Returns(Task.FromResult(new List<JobsLookupValue>() {
                new JobsLookupValue() { LookupValueId = "1", Text = "Example" }
            } as IList<JobsLookupValue>));
            var indexer = new Mock<BaseLookupValuesIndexer>(lookupValuesProvider.Object);
            indexer.CallBase = true;

            var dataSets = await indexer.Object.GetAllDataAsync("Job");

            Assert.NotZero(dataSets.Count(x => x.RowData["group"] == "JobType"));
        }

        [Test]
        public async Task SalaryRangeDataSetsAreCreated()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            lookupValuesProvider.Setup(x => x.ReadSalaryRanges()).Returns(Task.FromResult(new List<JobsLookupValue>() {
                new JobsLookupValue() { LookupValueId = "1", Text = "Example" }
            } as IList<JobsLookupValue>));
            var indexer = new Mock<BaseLookupValuesIndexer>(lookupValuesProvider.Object);
            indexer.CallBase = true;

            var dataSets = await indexer.Object.GetAllDataAsync("Job");

            Assert.NotZero(dataSets.Count(x => x.RowData["group"] == "SalaryRange"));
        }

        [Test]
        public async Task SalaryFrequencyDataSetsAreCreated()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            lookupValuesProvider.Setup(x => x.ReadSalaryFrequencies()).Returns(Task.FromResult(new List<JobsLookupValue>() {
                new JobsLookupValue() { LookupValueId = "1", Text = "Example" }
            } as IList<JobsLookupValue>));
            var indexer = new Mock<BaseLookupValuesIndexer>(lookupValuesProvider.Object);
            indexer.CallBase = true;

            var dataSets = await indexer.Object.GetAllDataAsync("Job");

            Assert.NotZero(dataSets.Count(x => x.RowData["group"] == "SalaryFrequency"));
        }

        [Test]
        public async Task PayGradeDataSetsAreCreated()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            lookupValuesProvider.Setup(x => x.ReadPayGrades()).Returns(Task.FromResult(new List<JobsLookupValue>() {
                new JobsLookupValue() { LookupValueId = "1", Text = "Example" }
            } as IList<JobsLookupValue>));
            var indexer = new Mock<BaseLookupValuesIndexer>(lookupValuesProvider.Object);
            indexer.CallBase = true;

            var dataSets = await indexer.Object.GetAllDataAsync("Job");

            Assert.NotZero(dataSets.Count(x => x.RowData["group"] == "PayGrade"));
        }

        [Test]
        public async Task WorkPatternDataSetsAreCreated()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            lookupValuesProvider.Setup(x => x.ReadWorkPatterns()).Returns(Task.FromResult(new List<JobsLookupValue>() {
                new JobsLookupValue() { LookupValueId = "1", Text = "Example" }
            } as IList<JobsLookupValue>));
            var indexer = new Mock<BaseLookupValuesIndexer>(lookupValuesProvider.Object);
            indexer.CallBase = true;

            var dataSets = await indexer.Object.GetAllDataAsync("Job");

            Assert.NotZero(dataSets.Count(x => x.RowData["group"] == "WorkPattern"));
        }

        [Test]
        public async Task ContractTypeDataSetsAreCreated()
        {
            var lookupValuesProvider = new Mock<IJobsLookupValuesProvider>();
            lookupValuesProvider.Setup(x => x.ReadContractTypes()).Returns(Task.FromResult(new List<JobsLookupValue>() {
                new JobsLookupValue() { LookupValueId = "1", Text = "Example" }
            } as IList<JobsLookupValue>));
            var indexer = new Mock<BaseLookupValuesIndexer>(lookupValuesProvider.Object);
            indexer.CallBase = true;

            var dataSets = await indexer.Object.GetAllDataAsync("Job");

            Assert.NotZero(dataSets.Count(x => x.RowData["group"] == "ContractType"));
        }
    }
}
