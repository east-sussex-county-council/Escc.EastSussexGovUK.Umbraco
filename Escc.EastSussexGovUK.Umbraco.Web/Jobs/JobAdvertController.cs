using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.Web.Services;
using Escc.Umbraco.Caching;
using Escc.Umbraco.ContentExperiments;
using Escc.Umbraco.PropertyTypes;
using Examine;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Task = System.Threading.Tasks.Task;
using Escc.EastSussexGovUK.Umbraco.Errors;
using Escc.Dates;
using System.Configuration;
using Escc.Umbraco.Expiry;
using System.Runtime.Caching;
using Escc.EastSussexGovUK.Umbraco.Web.Latest;
using Escc.EastSussexGovUK.Umbraco.Jobs.Api;
using Escc.EastSussexGovUK.Umbraco.Jobs;

namespace Escc.EastSussexGovUK.Umbraco.Web.Jobs
{
    /// <summary>
    /// Controller for pages based on the 'Job advert' Umbraco document type
    /// </summary>
    /// <seealso cref="Umbraco.Web.Mvc.RenderMvcController" />
    public class JobAdvertController : RenderMvcController
    {
        /// <summary>
        /// The default action to render the front-end view
        /// </summary>
        /// <param name="model"/>
        /// <returns/>
        public override ActionResult Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var viewModel = new JobAdvertViewModelFromUmbraco(model.Content).BuildModel();

            // Add common properties to the model
            var expiryDate = new ExpiryDateFromExamine(model.Content.Id, ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"]);
            var modelBuilder = new BaseViewModelBuilder();
            modelBuilder.PopulateBaseViewModel(viewModel, model.Content, new ContentExperimentSettingsService(),
                expiryDate.ExpiryDate,
                UmbracoContext.Current.InPreviewMode);
            modelBuilder.PopulateBaseViewModelWithInheritedContent(viewModel, 
                new UmbracoLatestService(model.Content), 
                new UmbracoSocialMediaService(model.Content),
                null,
                new UmbracoWebChatSettingsService(model.Content, new UrlListReader()),
                null);

            // Redirect non-preferred URL - these are sent out in job alerts until May 2018, and linked from the TalentLink back office
            if (!String.IsNullOrEmpty(Request.QueryString["nPostingTargetID"]))
            {
                var jobsProvider = new JobsDataFromApi(new Uri(ConfigurationManager.AppSettings["JobsApiBaseUrl"]), viewModel.JobsSet, new Uri(model.Content.UrlAbsolute()), new MemoryJobCacheStrategy(MemoryCache.Default, Request.QueryString["ForceCacheRefresh"] == "1"));
                viewModel.Job = Task.Run(async () => await jobsProvider.ReadJob(Request.QueryString["nPostingTargetID"])).Result;
                if (viewModel.Job.Id > 0)
                {
                    return new RedirectResult(viewModel.Job.Url.ToString(), true);
                }
            }
            else
            {
                // Show page for preferred URL
                var jobUrlSegment = Regex.Match(Request.Url.AbsolutePath, "/([0-9]+)/");
                if (jobUrlSegment.Success)
                {
                    var jobsProvider = new JobsDataFromApi(new Uri(ConfigurationManager.AppSettings["JobsApiBaseUrl"]), viewModel.JobsSet, new Uri(model.Content.UrlAbsolute()), new MemoryJobCacheStrategy(MemoryCache.Default, Request.QueryString["ForceCacheRefresh"] == "1"));
                    viewModel.Job = Task.Run(async () => await jobsProvider.ReadJob(jobUrlSegment.Groups[1].Value)).Result;
                    if (viewModel.Job.Id == 0 || viewModel.Job.ClosingDate < DateTime.Today)
                    {
                        // The job URL looks valid but the job isn't in the index, so it's probably closed.
                        // Find some similar jobs to suggest the user may want to apply for instead.
                        FindSimilarJobs(jobsProvider, viewModel);
                    }
                }
                else
                {
                    // The page was accessed by its default Umbraco URL with no parameters. Returning HttpNotFoundResult() ends up with a generic browser 404, 
                    // so to get our custom one we need to look it up and transfer control to it.
                    
                    // However, we need to check for "alttemplate" because a link from a TalentLink job alert or the TalentLink back office to an expired job
                    // is a link to the default Umbraco URL with just a querystring that doesn't match any data. That in turn will want to load the jobs CSS for
                    // a job that matches nothing, and that request ends up here. We want it to continue and return the JobsCss view, not our 404 page.
                    if (String.IsNullOrEmpty(Request.QueryString["alttemplate"]))
                    {
                        var notFoundUrl = new HttpStatusFromConfiguration().GetCustomUrlForStatusCode(404);
                        if (notFoundUrl != null && Server != null)
                        {
                            Server.TransferRequest(notFoundUrl + "?404;" + HttpUtility.UrlEncode(Request.Url.ToString()));
                        }
                    }
                }
            }

            // The page should expire when the job closes
            var expirySeconds = 86400; // one day
            if (viewModel.Job != null && viewModel.Job.ClosingDate != null)
            {
                var secondsToClosingDate = (int)(viewModel.Job.ClosingDate.Value.AddDays(1).Date.ToUkDateTime() - DateTime.Now.ToUkDateTime()).TotalSeconds;
                if (secondsToClosingDate >= 0) expirySeconds = secondsToClosingDate;
            }
            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, new IExpiryDateSource[] { expiryDate, new ExpiryDateFromPropertyValue(model.Content, "latestUnpublishDate_Latest") }, expirySeconds);

            return CurrentTemplate(viewModel);
        }

        private void FindSimilarJobs(IJobsDataProvider jobsProvider, JobAdvertViewModel model)
        {
            // Get the job title from the URL and use it as keywords to search for jobs that might be similar
            var urlPath = Request.Url.AbsolutePath.TrimEnd('/');
            var lastSlash = urlPath.LastIndexOf('/');
            if (lastSlash > -1) urlPath = urlPath.Substring(lastSlash + 1);

            var jobs = Task.Run(async () => await jobsProvider.ReadJobs(new JobSearchQuery() { KeywordsInTitle = urlPath.Replace("-", " ") })).Result;
            foreach (var job in jobs.Jobs)
            {
                if (model.SimilarJobs.Count >= 10) break;
                model.SimilarJobs.Add(job);
            }
        }
    }
}