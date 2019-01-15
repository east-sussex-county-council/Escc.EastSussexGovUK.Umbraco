using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using HtmlAgilityPack;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.TalentLink
{
    /// <summary>
    /// Parse an HTML list of job results into a list of jobs
    /// </summary>
    public class TalentLinkJobResultsHtmlParser : IJobResultsParser
    {
        private readonly ISalaryParser _salaryParser;

        /// <summary>
        /// Initializes a new instance of the <see cref="TalentLinkJobResultsHtmlParser"/> class.
        /// </summary>
        /// <param name="salaryParser">The salary parser.</param>
        public TalentLinkJobResultsHtmlParser(ISalaryParser salaryParser)
        {
            _salaryParser = salaryParser;
        }

        /// <summary>
        /// Parses jobs from the HTML stream.
        /// </summary>
        /// <param name="htmlStream">The HTML stream.</param>
        /// <returns></returns>
        public Task<JobsParseResult> Parse(Stream htmlStream)
        {
            var parsedHtml = new HtmlDocument();
            parsedHtml.Load(htmlStream);

            var parseResult = new JobsParseResult();
            ParseIsLastPage(parsedHtml, parseResult);
            ParseJobs(parsedHtml, parseResult);

            return Task.FromResult(parseResult);
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
                    job.Id = Int32.Parse(query["nPostingTargetId"], CultureInfo.InvariantCulture);
                    job.JobTitle = HttpUtility.HtmlDecode(link.InnerText);
                    job.Organisation = HttpUtility.HtmlDecode(link.ParentNode.ParentNode.SelectSingleNode("./td[@headers='th2']")?.InnerText?.Trim());
                    job.Locations.Add(HttpUtility.HtmlDecode(link.ParentNode.ParentNode.SelectSingleNode("./td[@headers='th3']")?.InnerText?.Trim()));
                    job.Salary = _salaryParser.ParseSalaryFromDescription(HttpUtility.HtmlDecode(link.ParentNode.ParentNode.SelectSingleNode("./td[@headers='th4']")?.InnerText?.Trim()));
                    job.Salary.SearchRange = job.Salary.SalaryRange;
                    job.ClosingDate = DateTime.Parse(link.ParentNode.ParentNode.SelectSingleNode("./td[@headers='th5']")?.InnerText?.Trim(), new CultureInfo("en-GB"));
                    jobs.Jobs.Add(job);
                }
            }
        }
    }
}