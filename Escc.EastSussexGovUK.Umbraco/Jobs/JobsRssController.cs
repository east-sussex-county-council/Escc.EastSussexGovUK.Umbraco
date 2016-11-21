using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Caching;
using Escc.Dates;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.Net;
using Escc.Umbraco.Caching;
using Escc.Umbraco.PropertyTypes;
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

            var viewModel = new RssViewModel<Job>();
            viewModel.Metadata.Title = model.Content.Name;
            viewModel.Metadata.Description = model.Content.GetPropertyValue<string>("pageDescription_Content");

            List<Job> jobs = null;
            if (HttpContext.Cache["Jobs"] == null || Request.QueryString["ForceCacheRefresh"] == "1")
            {
                var sourceUrl = new TalentLinkUrl(model.Content.GetPropertyValue<string>("ResultsScriptUrl_Content")).LinkUrl;
                var detailPage = model.Content.GetPropertyValue<IPublishedContent>("JobDetailPage_Content");

                var parser = new JobResultsHtmlParser(detailPage != null ? new Uri(detailPage.UrlWithDomain()) : Request.Url);
                var jobsProvider = new JobsDataFromTalentLink(sourceUrl, new ConfigurationProxyProvider(), parser);

                jobs = await jobsProvider.ReadJobs();

                HttpContext.Cache.Insert("Jobs", jobs, null, DateTime.Now.AddHours(1), Cache.NoSlidingExpiration);
            }
            else
            {
                jobs = (List<Job>) HttpContext.Cache["Jobs"];
            }

            foreach (var job in jobs)
            {
                viewModel.Items.Add(job);
            }

            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache);

            return CurrentTemplate(viewModel);
        }
    }
}