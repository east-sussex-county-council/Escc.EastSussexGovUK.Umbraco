using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.Net;
using Escc.Umbraco.Caching;
using Escc.Umbraco.PropertyTypes;
using HtmlAgilityPack;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Controller for pages based on the 'Jobs RSS feed' Umbraco document type
    /// </summary>
    /// <seealso cref="RenderMvcController"/>
    public class JobsRssController : RenderMvcController
    {
        /// <summary>
        /// The default action to render the front-end view
        /// </summary>
        /// <param name="model"/>
        /// <returns/>
        public async new Task<ActionResult> Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var viewModel = new RssViewModel();
            viewModel.Metadata.Title = model.Content.Name;
            viewModel.Metadata.Description = model.Content.GetPropertyValue<string>("pageDescription_Content");

            // TODO: Need to follow link to page 2

            var detailPage = model.Content.GetPropertyValue<IPublishedContent>("JobDetailPage_Content");
            var sourceUrl = new TalentLinkUrl(model.Content.GetPropertyValue<string>("ResultsScriptUrl_Content")).LinkUrl + "&resultsperpage=200";
            var jobsData = new TalentLinkHtmlFromHttpRequest(new Uri(sourceUrl), new ConfigurationProxyProvider());
            var htmlStream = await jobsData.ReadHtml();

            var parsedHtml = new HtmlDocument();
            parsedHtml.Load(htmlStream);

            var links = parsedHtml.DocumentNode.SelectNodes("//td[@headers='th1']/a");
            foreach (var link in links)
            {
                var jobUrl = HttpUtility.HtmlDecode(link.Attributes["href"].Value);
                var absoluteJobUrl = new Uri(Request.Url, jobUrl);
                var query = HttpUtility.ParseQueryString(absoluteJobUrl.Query);
                if (detailPage != null)
                {
                    absoluteJobUrl = new Uri(detailPage.UrlWithDomain() + "?nPostingTargetID=" + query["nPostingTargetId"]);
                }
                var organisation = link.ParentNode.ParentNode.SelectSingleNode("./td[@headers='th2']").InnerText.Trim();
                var location = link.ParentNode.ParentNode.SelectSingleNode("./td[@headers='th3']").InnerText.Trim();
                var salary = link.ParentNode.ParentNode.SelectSingleNode("./td[@headers='th4']").InnerText.Trim();
                var closingDate = link.ParentNode.ParentNode.SelectSingleNode("./td[@headers='th5']").InnerText.Trim();
                viewModel.Items.Add(new HomePageItemViewModel()
                {
                    Link = new HtmlLink()
                    {
                        Url = absoluteJobUrl,
                        Text = link.InnerText
                    },
                    Id = query["nPostingTargetId"],
                    Description = organisation + " / " + location + " / " + salary + " / " + closingDate
                });
            }


            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache);

            return CurrentTemplate(viewModel);
        }
    }
}