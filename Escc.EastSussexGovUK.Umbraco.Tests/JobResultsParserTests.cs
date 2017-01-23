using System;
using System.IO;
using System.Text;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using Escc.EastSussexGovUK.Umbraco.Jobs.TalentLink;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestFixture]
    public class JobResultsParserTests
    {
        [Test]
        public void MultipleJobsAreParsed()
        {
            using (var htmlStream = new MemoryStream(Encoding.UTF8.GetBytes(String.Format(Properties.Resources.JobResultsOuterHtml, Properties.Resources.MultipleJobResults))))
            {
                var parser = new JobResultsHtmlParser(new TalentLinkSalaryParser());

                var jobs = parser.Parse(htmlStream);

                Assert.AreEqual(10, jobs.Jobs.Count);
            }
        }

        [Test]
        public void PresenceOfMorePagesIsDetected()
        {
            using (var htmlStream = new MemoryStream(Encoding.UTF8.GetBytes(String.Format(Properties.Resources.JobResultsOuterHtml, Properties.Resources.MultipleJobResults))))
            {
                var parser = new JobResultsHtmlParser(new TalentLinkSalaryParser());

                var jobs = parser.Parse(htmlStream);

                Assert.AreEqual(false, jobs.IsLastPage);
            }
        }

        [Test]
        public void LastPageIsDetected()
        {
            using (var htmlStream = new MemoryStream(Encoding.UTF8.GetBytes(String.Format(Properties.Resources.JobResultsOuterHtml, Properties.Resources.LastPageOfJobs))))
            {
                var parser = new JobResultsHtmlParser(new TalentLinkSalaryParser());

                var jobs = parser.Parse(htmlStream);

                Assert.AreEqual(true, jobs.IsLastPage);
            }
        }

        [Test]
        public void NoJobsFoundIsParsedCorrectly()
        {
            using (var htmlStream = new MemoryStream(Encoding.UTF8.GetBytes(String.Format(Properties.Resources.JobResultsOuterHtml, Properties.Resources.NoJobsResult))))
            {
                var parser = new JobResultsHtmlParser(new TalentLinkSalaryParser());

                var jobs = parser.Parse(htmlStream);

                Assert.AreEqual(0, jobs.Jobs.Count);
            }
        }

        [Test]
        public void HtmlEntityInJobResultIsDecoded()
        {
            using (var htmlStream = new MemoryStream(Encoding.UTF8.GetBytes(String.Format(Properties.Resources.JobResultsOuterHtml, Properties.Resources.MultipleJobResults))))
            {
                var parser = new JobResultsHtmlParser(new TalentLinkSalaryParser());

                var jobs = parser.Parse(htmlStream);

                Assert.AreEqual("Learner Support Assistant – Bank Staff (Sussex Downs College)", jobs.Jobs[8].JobTitle);
            }
        }

        [Test]
        public void JobIdIsParsed()
        {
            using (var htmlStream = new MemoryStream(Encoding.UTF8.GetBytes(String.Format(Properties.Resources.JobResultsOuterHtml, Properties.Resources.MultipleJobResults))))
            {
                var parser = new JobResultsHtmlParser(new TalentLinkSalaryParser());

                var jobs = parser.Parse(htmlStream);

                Assert.AreEqual("33720", jobs.Jobs[0].Id);
            }
        }

        [Test]
        public void ClosingDateIsParsedAsUkDate()
        {
            using (var htmlStream = new MemoryStream(Encoding.UTF8.GetBytes(String.Format(Properties.Resources.JobResultsOuterHtml, Properties.Resources.MultipleJobResults))))
            {
                var parser = new JobResultsHtmlParser(new TalentLinkSalaryParser());

                var jobs = parser.Parse(htmlStream);

                // this date is invalid if parsed as a US date
                Assert.AreEqual(new DateTime(2017, 1, 30), jobs.Jobs[3].ClosingDate);
            }
        }

        [Test]
        public void SalaryIsParsed()
        {
            using (var htmlStream = new MemoryStream(Encoding.UTF8.GetBytes(String.Format(Properties.Resources.JobResultsOuterHtml, Properties.Resources.MultipleJobResults))))
            {
                var parser = new JobResultsHtmlParser(new TalentLinkSalaryParser());

                var jobs = parser.Parse(htmlStream);

                Assert.AreEqual("£20,000 to £24,999", jobs.Jobs[2].Salary);
            }
        }

        [Test]
        public void LocationIsParsed()
        {
            using (var htmlStream = new MemoryStream(Encoding.UTF8.GetBytes(String.Format(Properties.Resources.JobResultsOuterHtml, Properties.Resources.MultipleJobResults))))
            {
                var parser = new JobResultsHtmlParser(new TalentLinkSalaryParser());

                var jobs = parser.Parse(htmlStream);

                Assert.AreEqual("Hastings", jobs.Jobs[6].Location);
            }
        }

        [Test]
        public void OrganisationIsParsed()
        {
            using (var htmlStream = new MemoryStream(Encoding.UTF8.GetBytes(String.Format(Properties.Resources.JobResultsOuterHtml, Properties.Resources.MultipleJobResults))))
            {
                var parser = new JobResultsHtmlParser(new TalentLinkSalaryParser());

                var jobs = parser.Parse(htmlStream);

                Assert.AreEqual("Sussex Downs College", jobs.Jobs[9].Organisation);
            }
        }
    }
}
