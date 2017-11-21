using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using Escc.EastSussexGovUK.Umbraco.Jobs.TalentLink;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestFixture]
    public class JobAdvertHtmlParserTests
    {
        [Test]
        public void JobTitleParsedFromHtml()
        {
            var parser = new TalentLinkJobAdvertHtmlParser(new TalentLinkSalaryParser(), new TalentLinkWorkPatternParser());

            var job = parser.ParseJob(Properties.Resources.JobAdvert1Html, "example");

            Assert.AreEqual("Ceremonies Hosts", job.JobTitle);
        }

        [Test]
        public void JobReferenceParsedFromHtml()
        {
            var parser = new TalentLinkJobAdvertHtmlParser(new TalentLinkSalaryParser(), new TalentLinkWorkPatternParser());

            var job = parser.ParseJob(Properties.Resources.JobAdvert1Html, "example");

            Assert.AreEqual("CET01739", job.Reference);
        }


        [Test]
        public void LocationParsedFromHtml()
        {
            var parser = new TalentLinkJobAdvertHtmlParser(new TalentLinkSalaryParser(), new TalentLinkWorkPatternParser());

            var job = parser.ParseJob(Properties.Resources.JobAdvert1Html, "example");

            Assert.AreEqual("Lewes", job.Location);
        }


        [Test]
        public void OrganisationParsedFromHtml()
        {
            var parser = new TalentLinkJobAdvertHtmlParser(new TalentLinkSalaryParser(), new TalentLinkWorkPatternParser());

            var job = parser.ParseJob(Properties.Resources.JobAdvert1Html, "example");

            Assert.AreEqual("East Sussex County Council", job.Organisation);
        }


        [Test]
        public void DepartmentParsedFromHtml()
        {
            var parser = new TalentLinkJobAdvertHtmlParser(new TalentLinkSalaryParser(), new TalentLinkWorkPatternParser());

            var job = parser.ParseJob(Properties.Resources.JobAdvert1Html, "example");

            Assert.AreEqual("Communities, Economy and Transport", job.Department);
        }


        [Test]
        public void ContractTypeParsedFromHtml()
        {
            var parser = new TalentLinkJobAdvertHtmlParser(new TalentLinkSalaryParser(), new TalentLinkWorkPatternParser());

            var job = parser.ParseJob(Properties.Resources.JobAdvert1Html, "example");

            Assert.AreEqual("Casual", job.ContractType);
        }


        [Test]
        public void JobTypeParsedFromHtml()
        {
            var parser = new TalentLinkJobAdvertHtmlParser(new TalentLinkSalaryParser(), new TalentLinkWorkPatternParser());

            var job = parser.ParseJob(Properties.Resources.JobAdvert1Html, "example");

            Assert.AreEqual("Customer Services", job.JobType);
        }

        [Test]
        public void ClosingDateParsedFromHtml()
        {
            var parser = new TalentLinkJobAdvertHtmlParser(new TalentLinkSalaryParser(), new TalentLinkWorkPatternParser());

            var job = parser.ParseJob(Properties.Resources.JobAdvert1Html, "example");

            Assert.AreEqual(new DateTime(2017,01,29), job.ClosingDate);
        }

        [Test]
        public void AdvertTextParsedFromHtml()
        {
            var parser = new TalentLinkJobAdvertHtmlParser(new TalentLinkSalaryParser(), new TalentLinkWorkPatternParser());

            var job = parser.ParseJob(Properties.Resources.JobAdvert1Html, "example");

            Assert.IsTrue(job.AdvertHtml.ToHtmlString().Contains("Casual hours"));
            Assert.IsTrue(job.AdvertHtml.ToHtmlString().Contains("East Sussex Registration Service"));
            Assert.IsTrue(job.AdvertHtml.ToHtmlString().Contains("ceremonies take place at weekends"));
            Assert.IsTrue(job.AdvertHtml.ToHtmlString().Contains("Job Description and Person Specification"));
        }


        [Test]
        public void ApplyUrlParsedFromHtml()
        {
            var parser = new TalentLinkJobAdvertHtmlParser(new TalentLinkSalaryParser(), new TalentLinkWorkPatternParser());

            var job = parser.ParseJob(Properties.Resources.JobAdvert1Html, "example");

            Assert.AreEqual("https://emea3.recruitmentplatform.com/syndicated/private/syd_apply.cfm?id=PFOFK026203F3VBQB7968LOH0&nPostingTargetID=37054&mask=esccext&jdescurl=https:%2F%2Femea3.recruitmentplatform.com%2Fsyndicated%2Flay%2Fjsoutputinitrapido.cfm%3Fcomponent%3Dlay9999_jdesc100a%26ID%3DPFOFK026203F3VBQB7968LOH0%26LG%3DUK%26mask%3Desccext%26browserchk%3Dno%26nPostingTargetID%3D37054", job.ApplyUrl.ToString());
        }
    }
}
