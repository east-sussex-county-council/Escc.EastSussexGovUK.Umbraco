using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using HtmlAgilityPack;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Parse an HTML list of job results into a list of jobs
    /// </summary>
    public class JobResultsHtmlParser : IJobResultsParser
    {
        private readonly Uri _baseUrlForJobDetails;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobResultsHtmlParser"/> class.
        /// </summary>
        /// <param name="baseUrlForJobDetails">The base URL for job details.</param>
        /// <exception cref="System.ArgumentNullException">
        /// htmlStream
        /// or
        /// baseUrlForJobDetails
        /// </exception>
        public JobResultsHtmlParser(Uri baseUrlForJobDetails)
        {
            _baseUrlForJobDetails = baseUrlForJobDetails;
        }

        /// <summary>
        /// Parses jobs from the HTML stream.
        /// </summary>
        /// <param name="htmlStream">The HTML stream.</param>
        /// <returns></returns>
        public JobsParseResult Parse(Stream htmlStream)
        {
            var parsedHtml = new HtmlDocument();
            parsedHtml.Load(htmlStream);

            var parseResult = new JobsParseResult();
            ParseIsLastPage(parsedHtml, parseResult);
            ParseJobs(parsedHtml, parseResult);

            return parseResult;
        }

        private void ParseIsLastPage(HtmlDocument parsedHtml, JobsParseResult parseResult)
        {
            // default to true to prevent any infinite loops in consuming applications looking for the last page
            parseResult.IsLastPage = true;

            var paging = parsedHtml.DocumentNode.SelectNodes("//span[@class='Lst-NavPage']");
            if (paging != null)
            {
                var currentPageFound = false;
                foreach (var pagingNode in paging)
                {
                    if (pagingNode.SelectSingleNode("./a") == null)
                    {
                        // this is the current page, because it's not linked
                        currentPageFound = true;
                        continue;
                    }

                    // we're looking for another page after the current page
                    if (currentPageFound)
                    {
                        parseResult.IsLastPage = false;
                        break;
                    }
                }
            }
        }

        private void ParseJobs(HtmlDocument parsedHtml, JobsParseResult jobs)
        {
            var links = parsedHtml.DocumentNode.SelectNodes("//td[@headers='th1']/a");
            if (links != null)
            {
                foreach (var link in links)
                {
                    var job = new Job();

                    var jobUrl = HttpUtility.HtmlDecode(link.Attributes["href"].Value);
                    var absoluteUrl = new Uri(new Uri("http://example.org"), jobUrl);
                    var query = HttpUtility.ParseQueryString(absoluteUrl.Query);
                    job.Id = query["nPostingTargetId"];
                    if (_baseUrlForJobDetails != null)
                    {
                        job.Url = new Uri(_baseUrlForJobDetails + "?nPostingTargetID=" + job.Id);
                    }
                    job.JobTitle = HttpUtility.HtmlDecode(link.InnerText);
                    job.Organisation = HttpUtility.HtmlDecode(link.ParentNode.ParentNode.SelectSingleNode("./td[@headers='th2']")?.InnerText?.Trim());
                    job.Location = HttpUtility.HtmlDecode(link.ParentNode.ParentNode.SelectSingleNode("./td[@headers='th3']")?.InnerText?.Trim());
                    job.Salary = HttpUtility.HtmlDecode(link.ParentNode.ParentNode.SelectSingleNode("./td[@headers='th4']")?.InnerText?.Trim());
                    job.ClosingDate = DateTime.Parse(link.ParentNode.ParentNode.SelectSingleNode("./td[@headers='th5']")?.InnerText?.Trim(), new CultureInfo("en-GB"));
                    jobs.Jobs.Add(job);
                }
            }
        }
    }
}