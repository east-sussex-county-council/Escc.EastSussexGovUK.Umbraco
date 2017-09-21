﻿using System;
using System.Globalization;
using System.Linq;
using System.Web;
using Escc.Dates;
using Exceptionless;
using HtmlAgilityPack;
using Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;
using Escc.Html;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.TalentLink
{
    /// <summary>
    /// Parses a job from the HTML returned by TalentLink
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.IJobAdvertParser" />
    public class TalentLinkJobAdvertHtmlParser : IJobAdvertParser
    {
        private readonly ISalaryParser _salaryParser;
        private readonly IWorkPatternParser _workPatternParser;
        private readonly IRichTextHtmlFormatter[] _htmlFormatters;

        /// <summary>
        /// Initializes a new instance of the <see cref="TalentLinkJobAdvertHtmlParser"/> class.
        /// </summary>
        /// <param name="salaryParser">The salary parser.</param>
        /// <param name="workPatternParser">The work pattern parser.</param>
        /// <exception cref="System.ArgumentNullException">salaryParser</exception>
        public TalentLinkJobAdvertHtmlParser(ISalaryParser salaryParser, IWorkPatternParser workPatternParser, params IRichTextHtmlFormatter[] htmlFormatters)
        {
            if (salaryParser == null) throw new ArgumentNullException(nameof(salaryParser));
            if (workPatternParser == null) throw new ArgumentNullException(nameof(workPatternParser));
            _salaryParser = salaryParser;
            _workPatternParser = workPatternParser;
            _htmlFormatters = htmlFormatters;
        }

        /// <summary>
        /// Parses a job.
        /// </summary>
        /// <param name="sourceData">The source data for the job.</param>
        /// <returns></returns>
        public Job ParseJob(string sourceData, string jobId)
        {
            HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
            htmlDocument.OptionFixNestedTags = true;
            htmlDocument.LoadHtml(sourceData);

            // ParseErrors is an ArrayList containing any errors from the Load statement
            if (htmlDocument.ParseErrors != null && htmlDocument.ParseErrors.Count() > 0)
            {
                foreach (var error in htmlDocument.ParseErrors)
                {
                    var exception = new HttpParseException("Unable to parse job HTML from TalentLink");
                    exception.Data.Add("Job ID", jobId);
                    exception.Data.Add("Type of error", error.Code);
                    exception.Data.Add("Reason", error.Reason);
                    exception.Data.Add("Line", error.Line);
                    exception.Data.Add("Position on line", error.LinePosition);
                    exception.Data.Add("Source HTML", error.SourceText);
                    exception.ToExceptionless().Submit();
                }
                return null;
            }
            else
            {
                if (htmlDocument.DocumentNode != null)
                {
                    var job = new Job();
                    job.JobTitle = ParseValueFromElementById(htmlDocument, "h3", "JDText-Title");
                    job.Reference = ParseValueFromElementById(htmlDocument, "span", "JDText-Param2");
                    if (!String.IsNullOrEmpty(job.Reference))
                    {
                        job.JobTitle = job.JobTitle.Replace(" (" + job.Reference + ")", String.Empty);
                    }
                    job.Location = ParseValueFromElementById(htmlDocument, "span", "JDText-Param3");
                    job.Organisation = ParseValueFromElementById(htmlDocument, "span", "JDText-Param4");
                    job.Department = ParseValueFromElementById(htmlDocument, "span", "JDText-Param5");
                    job.ContractType = ParseValueFromElementById(htmlDocument, "span", "JDText-Param6");
                    job.JobType = ParseValueFromElementById(htmlDocument, "span", "JDText-Param7");
                    job.Salary = _salaryParser.ParseSalaryFromHtml(htmlDocument);

                    DateTime closingDate;
                    DateTime.TryParse(ParseValueFromElementById(htmlDocument, "span", "JDText-Param9"), new CultureInfo("en-GB"), DateTimeStyles.AssumeLocal, out closingDate);
                    job.ClosingDate = closingDate;

                    var agilityPackFormatters = new IHtmlAgilityPackHtmlFormatter[]
                    {
                        new RemoveUnwantedAttributesFormatter(new string[] { "style" }),
                        new ReplaceElementNameFormatter("h5", "h2"),
                        new RemoveElementByNameAndContentFormatter("h2", "Job Details"),
                        new RemoveElementsWithNoContentFormatter(),
                        new TruncateLongLinksFormatter(new HtmlLinkFormatter())
                    };
                    foreach (var formatter in agilityPackFormatters)
                    {
                        formatter.FormatHtml(htmlDocument);
                    }

                    // JDText-Field4 is additional info (ie small print) and JDText-Field6 is information for redeployees (ie more small print)
                    var additionalInfo = ParseValueFromElementById(htmlDocument, "span", "JDText-Field4") + Environment.NewLine +
                                         ParseValueFromElementById(htmlDocument, "span", "JDText-Field6");
                    if (!String.IsNullOrEmpty(additionalInfo))
                    {
                        additionalInfo = ApplyStringFormatters(additionalInfo);
                        job.AdditionalInformationHtml = new HtmlString(additionalInfo);
                    }

                    var equalOpportunities = ParseValueFromElementById(htmlDocument, "span", "JDText-Field5");
                    if (!String.IsNullOrEmpty(equalOpportunities))
                    {
                        equalOpportunities = ApplyStringFormatters(equalOpportunities);
                        job.EqualOpportunitiesHtml = new HtmlString(equalOpportunities);
                    }

                    var parsedHtml = ParseValueFromElementById(htmlDocument, "div", "JD-Field1") + Environment.NewLine +
                                     ParseValueFromElementById(htmlDocument, "div", "JD-Field2") + Environment.NewLine +
                                     ParseValueFromElementById(htmlDocument, "div", "JD-Documents");

                    parsedHtml = ApplyStringFormatters(parsedHtml);
                    parsedHtml = new RemoveDuplicateTextFormatter("Closing date: " + job.ClosingDate.Value.ToBritishDate()).FormatHtml(parsedHtml);
                    parsedHtml = new RemoveDuplicateTextFormatter("Closing date: " + job.ClosingDate.Value.ToBritishDateWithDay()).FormatHtml(parsedHtml);
                    parsedHtml = new RemoveDuplicateTextFormatter("Salary: " + job.Salary.SalaryRange).FormatHtml(parsedHtml);
                    parsedHtml = new RemoveDuplicateTextFormatter("Salary: " + job.Salary.SalaryRange.Replace(" to ", " - ")).FormatHtml(parsedHtml);
                    parsedHtml = new RemoveDuplicateTextFormatter("Contract type: " + job.ContractType).FormatHtml(parsedHtml);

                    job.AdvertHtml = new HtmlString(parsedHtml);
                    job.WorkPattern = _workPatternParser.ParseWorkPatternFromHtml(parsedHtml);

                    var applyLink = htmlDocument.DocumentNode.SelectSingleNode($"//div[@id='JD-ActApplyDirect']/a");
                    if (applyLink != null)
                    {
                        job.ApplyUrl = new Uri(HttpUtility.HtmlDecode(applyLink.Attributes["href"].Value), UriKind.RelativeOrAbsolute);
                    }

                    return job;
                }
                return null;
            }
        }

        private static string ApplyStringFormatters(string parsedHtml)
        {
            var htmlFormatters = new IHtmlStringFormatter[] { new CloseEmptyElementsFormatter(), new HouseStyleDateFormatter(), new RedeploymentHeaderFormatter() };
            foreach (var formatter in htmlFormatters)
            {
                parsedHtml = formatter.FormatHtml(parsedHtml);
            }

            return parsedHtml;
        }

        private static string ParseValueFromElementById(HtmlDocument htmlDocument, string elementName, string elementId)
        {
            var node = htmlDocument.DocumentNode.SelectSingleNode($"//{elementName}[@id='{elementId}']");
            if (node != null)
            {
                return HttpUtility.HtmlDecode(node.InnerHtml.Trim().Replace("&nbsp;", " "));
            }
            return String.Empty;
        }
    }
}