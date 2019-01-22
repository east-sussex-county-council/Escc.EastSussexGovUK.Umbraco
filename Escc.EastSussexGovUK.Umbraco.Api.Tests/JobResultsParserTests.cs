using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Umbraco.Api.Jobs.TalentLink;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Api.Tests
{
    [TestFixture]
    public class JobResultsParserTests
    {
        [Test]
        public async Task MultipleJobsAreParsed()
        {
            using (var htmlStream = new MemoryStream(Encoding.UTF8.GetBytes(String.Format(Properties.Resources.JobResultsOuterHtml, Properties.Resources.MultipleJobResults))))
            {
                var parser = new TalentLinkJobResultsHtmlParser(new TalentLinkSalaryFromDescriptionParser());

                var jobs = await parser.Parse(htmlStream);

                Assert.AreEqual(10, jobs.Jobs.Count);
            }
        }

        [Test]
        public async Task PresenceOfMorePagesIsDetected()
        {
            using (var htmlStream = new MemoryStream(Encoding.UTF8.GetBytes(String.Format(Properties.Resources.JobResultsOuterHtml, Properties.Resources.MultipleJobResults))))
            {
                var parser = new TalentLinkJobResultsHtmlParser(new TalentLinkSalaryFromDescriptionParser());

                var jobs = await parser.Parse(htmlStream);

                Assert.AreEqual(false, jobs.IsLastPage);
            }
        }

        [Test]
        public async Task LastPageIsDetected()
        {
            using (var htmlStream = new MemoryStream(Encoding.UTF8.GetBytes(String.Format(Properties.Resources.JobResultsOuterHtml, Properties.Resources.LastPageOfJobs))))
            {
                var parser = new TalentLinkJobResultsHtmlParser(new TalentLinkSalaryFromDescriptionParser());

                var jobs = await parser.Parse(htmlStream);

                Assert.AreEqual(true, jobs.IsLastPage);
            }
        }

        [Test]
        public async Task NoJobsFoundIsParsedCorrectly()
        {
            using (var htmlStream = new MemoryStream(Encoding.UTF8.GetBytes(String.Format(Properties.Resources.JobResultsOuterHtml, Properties.Resources.NoJobsResult))))
            {
                var parser = new TalentLinkJobResultsHtmlParser(new TalentLinkSalaryFromDescriptionParser());

                var jobs = await parser.Parse(htmlStream);

                Assert.AreEqual(0, jobs.Jobs.Count);
            }
        }

        [Test]
        public async Task HtmlEntityInJobResultIsDecoded()
        {
            using (var htmlStream = new MemoryStream(Encoding.UTF8.GetBytes(String.Format(Properties.Resources.JobResultsOuterHtml, Properties.Resources.MultipleJobResults))))
            {
                var parser = new TalentLinkJobResultsHtmlParser(new TalentLinkSalaryFromDescriptionParser());

                var jobs = await parser.Parse(htmlStream);

                Assert.AreEqual("Learner Support Assistant – Bank Staff (Sussex Downs College)", jobs.Jobs[8].JobTitle);
            }
        }

        [Test]
        public async Task JobIdIsParsed()
        {
            using (var htmlStream = new MemoryStream(Encoding.UTF8.GetBytes(String.Format(Properties.Resources.JobResultsOuterHtml, Properties.Resources.MultipleJobResults))))
            {
                var parser = new TalentLinkJobResultsHtmlParser(new TalentLinkSalaryFromDescriptionParser());

                var jobs = await parser.Parse(htmlStream);

                Assert.AreEqual(33720, jobs.Jobs[0].Id);
            }
        }

        [Test]
        public async Task ClosingDateIsParsedAsUkDate()
        {
            using (var htmlStream = new MemoryStream(Encoding.UTF8.GetBytes(String.Format(Properties.Resources.JobResultsOuterHtml, Properties.Resources.MultipleJobResults))))
            {
                var parser = new TalentLinkJobResultsHtmlParser(new TalentLinkSalaryFromDescriptionParser());

                var jobs = await parser.Parse(htmlStream);

                // this date is invalid if parsed as a US date
                Assert.AreEqual(new DateTime(2017, 1, 30), jobs.Jobs[3].ClosingDate);
            }
        }

        [Test]
        public async Task SalaryIsParsed()
        {
            using (var htmlStream = new MemoryStream(Encoding.UTF8.GetBytes(String.Format(Properties.Resources.JobResultsOuterHtml, Properties.Resources.MultipleJobResults))))
            {
                var parser = new TalentLinkJobResultsHtmlParser(new TalentLinkSalaryFromDescriptionParser());

                var jobs = await parser.Parse(htmlStream);

                Assert.AreEqual("£20,000 to £24,999 per annum", jobs.Jobs[2].Salary.SalaryRange);
            }
        }

        [Test]
        public async Task LocationIsParsed()
        {
            using (var htmlStream = new MemoryStream(Encoding.UTF8.GetBytes(String.Format(Properties.Resources.JobResultsOuterHtml, Properties.Resources.MultipleJobResults))))
            {
                var parser = new TalentLinkJobResultsHtmlParser(new TalentLinkSalaryFromDescriptionParser());

                var jobs = await parser.Parse(htmlStream);

                Assert.AreEqual("Hastings", jobs.Jobs[6].Locations[0]);
            }
        }

        [Test]
        public async Task OrganisationIsParsed()
        {
            using (var htmlStream = new MemoryStream(Encoding.UTF8.GetBytes(String.Format(Properties.Resources.JobResultsOuterHtml, Properties.Resources.MultipleJobResults))))
            {
                var parser = new TalentLinkJobResultsHtmlParser(new TalentLinkSalaryFromDescriptionParser());

                var jobs = await parser.Parse(htmlStream);

                Assert.AreEqual("Sussex Downs College", jobs.Jobs[9].Organisation);
            }
        }
    }
}
