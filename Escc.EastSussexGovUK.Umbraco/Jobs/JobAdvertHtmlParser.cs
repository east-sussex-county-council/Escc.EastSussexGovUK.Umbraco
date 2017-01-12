using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Exceptionless;
using HtmlAgilityPack;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Parses a job from the HTML returned by TalentLink
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.IJobAdvertParser" />
    public class JobAdvertHtmlParser : IJobAdvertParser
    {
        /// <summary>
        /// Parses a job.
        /// </summary>
        /// <param name="sourceData">The source data for the job.</param>
        /// <returns></returns>
        public Job ParseJob(string sourceData)
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

                    job.Salary = ParseValueFromElementById(htmlDocument, "span", "JDText-salary");
                    // Tidy up whitespace in the salary, then translate it to a more usable format
                    job.Salary = Regex.Replace(job.Salary, @"\s+", " ").Replace(" - ", "-");
                    job.Salary = Regex.Replace(job.Salary, "^([0-9]+)-([0-9]+) GBP Year", "£$1–£$2 per annum");
                    job.Salary = Regex.Replace(job.Salary, "^([0-9]+) GBP Year", "£$1 per annum");

                    DateTime closingDate;
                    DateTime.TryParse(ParseValueFromElementById(htmlDocument, "span", "JDText-Param9"), new CultureInfo("en-GB"), DateTimeStyles.AssumeLocal, out closingDate);
                    job.ClosingDate = closingDate;

                    var agilityPackFormatters = new IHtmlAgilityPackHtmlFormatter[]
                    {
                        new RemoveUnwantedAttributesFormatter(new string[] { "style" }),
                        new ReplaceElementNameFormatter("h5", "h2"), 
                    };
                    foreach (var formatter in agilityPackFormatters)
                    {
                        formatter.FormatHtml(htmlDocument);
                    }

                    var parsedHtml = ParseValueFromElementById(htmlDocument, "div", "JD-Field1") + Environment.NewLine +
                                     ParseValueFromElementById(htmlDocument, "div", "JD-Field2") + Environment.NewLine +
                                     ParseValueFromElementById(htmlDocument, "div", "JD-Field4") + Environment.NewLine +
                                     ParseValueFromElementById(htmlDocument, "div", "JD-Field5") + Environment.NewLine +
                                     ParseValueFromElementById(htmlDocument, "div", "JD-Field6") + Environment.NewLine +
                                     ParseValueFromElementById(htmlDocument, "div", "JD-Documents");

                    var htmlFormatters = new IHtmlStringFormatter[] { new CloseEmptyElementsFormatter() };
                    foreach (var formatter in htmlFormatters)
                    {
                        parsedHtml = formatter.FormatHtml(parsedHtml);
                    }

                    job.AdvertHtml = new HtmlString(parsedHtml);

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