﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using Escc.EastSussexGovUK.Umbraco.Api.Jobs;
using Escc.EastSussexGovUK.Umbraco.Api.Jobs.TalentLink;
using Moq;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Api.Tests
{
    [TestFixture]
    public class TalentLinkJobAdvertHtmlParserTests
    {
        [Test]
        public async Task JobTitleParsedFromHtml()
        {
            var salaryParser = new Mock<ISalaryParser>();
            salaryParser.Setup(x => x.ParseSalary(It.IsAny<string>())).Returns(Task.FromResult(new Salary()));
            var workPatternParser = new Mock<IWorkPatternParser>();
            var parser = new TalentLinkJobAdvertHtmlParser(salaryParser.Object, workPatternParser.Object);

            var job = await parser.ParseJob(Properties.Resources.JobAdvert1Html, "example");

            Assert.AreEqual("Ceremonies Hosts", job.JobTitle);
        }

        [Test]
        public async Task JobReferenceParsedFromHtml()
        {
            var salaryParser = new Mock<ISalaryParser>();
            salaryParser.Setup(x => x.ParseSalary(It.IsAny<string>())).Returns(Task.FromResult(new Salary()));
            var workPatternParser = new Mock<IWorkPatternParser>();
            var parser = new TalentLinkJobAdvertHtmlParser(salaryParser.Object, workPatternParser.Object);

            var job = await parser.ParseJob(Properties.Resources.JobAdvert1Html, "example");

            Assert.AreEqual("CET01739", job.Reference);
        }


        [Test]
        public async Task LocationParsedFromHtml()
        {
            var salaryParser = new Mock<ISalaryParser>();
            salaryParser.Setup(x => x.ParseSalary(It.IsAny<string>())).Returns(Task.FromResult(new Salary()));
            var workPatternParser = new Mock<IWorkPatternParser>();
            var parser = new TalentLinkJobAdvertHtmlParser(salaryParser.Object, workPatternParser.Object);

            var job = await parser.ParseJob(Properties.Resources.JobAdvert1Html, "example");

            Assert.AreEqual("Lewes", job.Locations[0]);
        }


        [Test]
        public async Task OrganisationParsedFromHtml()
        {
            var salaryParser = new Mock<ISalaryParser>();
            salaryParser.Setup(x => x.ParseSalary(It.IsAny<string>())).Returns(Task.FromResult(new Salary()));
            var workPatternParser = new Mock<IWorkPatternParser>();
            var parser = new TalentLinkJobAdvertHtmlParser(salaryParser.Object, workPatternParser.Object);

            var job = await parser.ParseJob(Properties.Resources.JobAdvert1Html, "example");

            Assert.AreEqual("East Sussex County Council", job.Organisation);
        }


        [Test]
        public async Task DepartmentParsedFromHtml()
        {
            var salaryParser = new Mock<ISalaryParser>();
            salaryParser.Setup(x => x.ParseSalary(It.IsAny<string>())).Returns(Task.FromResult(new Salary()));
            var workPatternParser = new Mock<IWorkPatternParser>();
            var parser = new TalentLinkJobAdvertHtmlParser(salaryParser.Object, workPatternParser.Object);

            var job = await parser.ParseJob(Properties.Resources.JobAdvert1Html, "example");

            Assert.AreEqual("Communities, Economy and Transport", job.Department);
        }


        [Test]
        public async Task ContractTypeParsedFromHtml()
        {
            var salaryParser = new Mock<ISalaryParser>();
            salaryParser.Setup(x => x.ParseSalary(It.IsAny<string>())).Returns(Task.FromResult(new Salary()));
            var workPatternParser = new Mock<IWorkPatternParser>();
            var parser = new TalentLinkJobAdvertHtmlParser(salaryParser.Object, workPatternParser.Object);

            var job = await parser.ParseJob(Properties.Resources.JobAdvert1Html, "example");

            Assert.AreEqual("Casual", job.ContractType);
        }


        [Test]
        public async Task JobTypeParsedFromHtml()
        {
            var salaryParser = new Mock<ISalaryParser>();
            salaryParser.Setup(x => x.ParseSalary(It.IsAny<string>())).Returns(Task.FromResult(new Salary()));
            var workPatternParser = new Mock<IWorkPatternParser>();
            var parser = new TalentLinkJobAdvertHtmlParser(salaryParser.Object, workPatternParser.Object);

            var job = await parser.ParseJob(Properties.Resources.JobAdvert1Html, "example");

            Assert.AreEqual("Customer Services", job.JobType);
        }

        [Test]
        public async Task ClosingDateParsedFromHtml()
        {
            var salaryParser = new Mock<ISalaryParser>();
            salaryParser.Setup(x => x.ParseSalary(It.IsAny<string>())).Returns(Task.FromResult(new Salary()));
            var workPatternParser = new Mock<IWorkPatternParser>();
            var parser = new TalentLinkJobAdvertHtmlParser(salaryParser.Object, workPatternParser.Object);

            var job = await parser.ParseJob(Properties.Resources.JobAdvert1Html, "example");

            Assert.AreEqual(new DateTime(2017,01,29), job.ClosingDate);
        }

        [Test]
        public async Task AdvertTextParsedFromHtml()
        {
            var salaryParser = new Mock<ISalaryParser>();
            salaryParser.Setup(x => x.ParseSalary(It.IsAny<string>())).Returns(Task.FromResult(new Salary()));
            var workPatternParser = new Mock<IWorkPatternParser>();
            var parser = new TalentLinkJobAdvertHtmlParser(salaryParser.Object, workPatternParser.Object);

            var job = await parser.ParseJob(Properties.Resources.JobAdvert1Html, "example");

            Assert.IsTrue(job.AdvertHtml.ToHtmlString().Contains("Casual hours"));
            Assert.IsTrue(job.AdvertHtml.ToHtmlString().Contains("East Sussex Registration Service"));
            Assert.IsTrue(job.AdvertHtml.ToHtmlString().Contains("ceremonies take place at weekends"));
            Assert.IsTrue(job.AdvertHtml.ToHtmlString().Contains("Job Description and Person Specification"));
        }


        [Test]
        public async Task ApplyUrlParsedFromHtml()
        {
            var salaryParser = new Mock<ISalaryParser>();
            salaryParser.Setup(x => x.ParseSalary(It.IsAny<string>())).Returns(Task.FromResult(new Salary()));
            var workPatternParser = new Mock<IWorkPatternParser>();
            var parser = new TalentLinkJobAdvertHtmlParser(salaryParser.Object, workPatternParser.Object);

            var job = await parser.ParseJob(Properties.Resources.JobAdvert1Html, "example");

            // The final .Replace() is included because this test seems to switch between encoding and not encoding the : in the querystring parameter
            Assert.AreEqual("https://emea3.recruitmentplatform.com/syndicated/private/syd_apply.cfm?id=PFOFK026203F3VBQB7968LOH0&nPostingTargetID=37054&mask=esccext&jdescurl=https%3A%2F%2Femea3.recruitmentplatform.com%2Fsyndicated%2Flay%2Fjsoutputinitrapido.cfm%3Fcomponent%3Dlay9999_jdesc100a%26ID%3DPFOFK026203F3VBQB7968LOH0%26LG%3DUK%26mask%3Desccext%26browserchk%3Dno%26nPostingTargetID%3D37054", job.ApplyUrl.ToString().Replace(":%2F", "%3A%2F"));
        }
    }
}