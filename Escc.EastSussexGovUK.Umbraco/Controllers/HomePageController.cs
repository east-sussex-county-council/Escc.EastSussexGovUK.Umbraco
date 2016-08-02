using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AST.AzureBlobStorage.Helper;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.EastSussexGovUK.Umbraco.Services;
using Escc.Umbraco.Caching;
using Escc.Umbraco.ContentExperiments;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Escc.EastSussexGovUK.Umbraco.Controllers
{
    /// <summary>
    /// The controller for the Home Page document type
    /// </summary>
    public class HomePageController : RenderMvcController
    {
        /// <summary>
        /// The default action to render the front-end view
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">model</exception>
        public override ActionResult Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException("model");

            var viewModel = MapUmbracoContentToViewModel(model.Content, new UmbracoOnAzureRelatedLinksService(), new ContentExperimentSettingsService());

            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, new List<string>() { "latestUnpublishDate_Latest" });

            return CurrentTemplate(viewModel);
        }

        private static HomePageViewModel MapUmbracoContentToViewModel(IPublishedContent publishedContent, IRelatedLinksService relatedLinksService, IContentExperimentSettingsService contentExperimentSettingsService)
        {
            var model = new HomePageViewModel()
            {
                TopTasksTitle = publishedContent.GetPropertyValue<string>("TopTasksTitle_Content"),
                NewsTitle = publishedContent.GetPropertyValue<string>("NewsTitle_Content"),
                LibrariesTitle = publishedContent.GetPropertyValue<string>("LibrariesTitle_Content"),
                RecyclingTitle = publishedContent.GetPropertyValue<string>("RecyclingTitle_Content"),
                InvolvedTitle = publishedContent.GetPropertyValue<string>("InvolvedTitle_Content")
            };

            var modelBuilder = new BaseViewModelBuilder();
            modelBuilder.PopulateBaseViewModel(model, publishedContent, contentExperimentSettingsService, UmbracoContext.Current.InPreviewMode);

            var involvedRss = publishedContent.Children<IPublishedContent>()
                .FirstOrDefault(child => child.ContentType.Alias == "HomePageItems" && child.Name.ToUpperInvariant().Contains("INVOLVED"));
            if (involvedRss != null)
            {
                model.InvolvedRssUrl = new Uri(involvedRss.UrlWithDomain());

                ((List<HomePageItemViewModel>)model.InvolvedItems).AddRange(
                        involvedRss.Children<IPublishedContent>()
                        .Where(child => child.ContentType.Alias == "HomePageItem")
                        .Take(5)
                        .Select(child => new HomePageItemFromUmbraco(child).GetHomePageItem())
                        );
            }

            var newsRss = publishedContent.Children<IPublishedContent>()
                .FirstOrDefault(child => child.ContentType.Alias == "HomePageItems" && child.Name.ToUpperInvariant().Contains("NEWS"));
            if (newsRss != null)
            {
                model.NewsRssUrl = new Uri(newsRss.UrlWithDomain());

                ((List<HomePageItemViewModel>)model.NewsItems).AddRange(
                    newsRss.Children<IPublishedContent>()
                    .Where(child => child.ContentType.Alias == "HomePageItem" && !String.IsNullOrEmpty(child.GetPropertyValue<string>("Image_Content")))
                    .Take(2)
                    .Select(child => new HomePageItemFromUmbraco(child).GetHomePageItem())
                    );          
            }

            var termDates = publishedContent.GetPropertyValue<IPublishedContent>("TermDatesData_Content");
            if (termDates != null && !String.IsNullOrEmpty(termDates.Url))
            {
                model.TermDatesDataUrl = ContentHelper.TransformUrl(new Uri(termDates.Url, UriKind.Relative));
            }

            ((List<HtmlLink>)model.TopTasksLinks).AddRange(relatedLinksService.BuildRelatedLinksViewModelFromUmbracoContent(publishedContent, "TopTasksContent_Content"));
            ((List<HtmlLink>)model.ReportLinks).AddRange(relatedLinksService.BuildRelatedLinksViewModelFromUmbracoContent(publishedContent, "ReportTab_Content"));
            ((List<HtmlLink>)model.ApplyLinks).AddRange(relatedLinksService.BuildRelatedLinksViewModelFromUmbracoContent(publishedContent, "ApplyTab_Content"));
            ((List<HtmlLink>)model.PayLinks).AddRange(relatedLinksService.BuildRelatedLinksViewModelFromUmbracoContent(publishedContent, "PayTab_Content"));
            ((List<HtmlLink>)model.SchoolLinks).AddRange(relatedLinksService.BuildRelatedLinksViewModelFromUmbracoContent(publishedContent, "SchoolLinks_Content"));
            ((List<HtmlLink>)model.LibrariesContent).AddRange(relatedLinksService.BuildRelatedLinksViewModelFromUmbracoContent(publishedContent, "LibrariesContent_Content"));
            ((List<HtmlLink>)model.InvolvedLinks).AddRange(relatedLinksService.BuildRelatedLinksViewModelFromUmbracoContent(publishedContent, "InvolvedLinks_Content"));

            return model;
        }
    }
}