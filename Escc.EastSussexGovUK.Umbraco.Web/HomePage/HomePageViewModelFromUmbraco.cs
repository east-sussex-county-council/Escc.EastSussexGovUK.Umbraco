using System;
using System.Collections.Generic;
using System.Linq;
using Escc.EastSussexGovUK.Umbraco.Web.Services;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Web.HomePage
{
    public class HomePageViewModelFromUmbraco : BaseViewModelFromUmbracoBuilder, IViewModelBuilder<HomePageViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomePageViewModel" /> class.
        /// </summary>
        /// <param name="umbracoContent">Content from Umbraco using the 'Home page' document type.</param>
        /// <param name="relatedLinksService">The related links service.</param>
        /// <exception cref="System.ArgumentNullException">umbracoContent
        /// or
        /// mediaUrlTransformer</exception>
        public HomePageViewModelFromUmbraco(IPublishedContent umbracoContent, IRelatedLinksService relatedLinksService) :
            base(umbracoContent, relatedLinksService)
        {
        }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <returns></returns>
        public HomePageViewModel BuildModel()
        {
            var model = new HomePageViewModel()
            {
                TopTasksTitle = UmbracoContent.GetPropertyValue<string>("TopTasksTitle_Content"),
                NewsTitle = UmbracoContent.GetPropertyValue<string>("NewsTitle_Content"),
                LibrariesTitle = UmbracoContent.GetPropertyValue<string>("LibrariesTitle_Content"),
                RecyclingTitle = UmbracoContent.GetPropertyValue<string>("RecyclingTitle_Content"),
                InvolvedTitle = UmbracoContent.GetPropertyValue<string>("InvolvedTitle_Content"),
                JobsLogo = BuildImage("JobsLogo_Content"),
                JobSearchResultsPage = BuildLinkToPage("JobSearchResultsPage_Content"),
                JobsHomePage = BuildLinkToPage("JobsHomePage_Content")
            };

            var involvedRss = UmbracoContent.Children<IPublishedContent>()
                .FirstOrDefault(child => child.ContentType.Alias == "HomePageItems" && child.Name.ToUpperInvariant().Contains("INVOLVED"));
            if (involvedRss != null)
            {
                model.InvolvedRssUrl = new Uri(involvedRss.UrlWithDomain());

                ((List<HomePageItemViewModel>)model.InvolvedItems).AddRange(
                        involvedRss.Children<IPublishedContent>()
                        .Where(child => child.ContentType.Alias == "HomePageItem")
                        .Take(5)
                        .Select(child => new HomePageItemViewModelFromUmbraco(child).BuildModel())
                        );
            }

            var newsRss = UmbracoContent.Children<IPublishedContent>()
                .FirstOrDefault(child => child.ContentType.Alias == "HomePageItems" && child.Name.ToUpperInvariant().Contains("NEWS"));
            if (newsRss != null)
            {
                model.NewsRssUrl = new Uri(newsRss.UrlWithDomain());

                ((List<HomePageItemViewModel>)model.NewsItems).AddRange(
                    newsRss.Children<IPublishedContent>()
                    .Where(child => child.ContentType.Alias == "HomePageItem" && !String.IsNullOrEmpty(child.GetPropertyValue<string>("Image_Content")))
                    .Take(2)
                    .Select(child => new HomePageItemViewModelFromUmbraco(child).BuildModel())
                    );
            }

            var termDates = UmbracoContent.GetPropertyValue<IPublishedContent>("TermDatesData_Content");
            if (termDates != null && !String.IsNullOrEmpty(termDates.Url))
            {
                model.TermDatesDataUrl = new Uri(termDates.Url, UriKind.Relative);
            }

            ((List<HtmlLink>)model.TopTasksLinks).AddRange(RelatedLinksService.BuildRelatedLinksViewModelFromUmbracoContent(UmbracoContent, "TopTasksContent_Content"));
            ((List<HtmlLink>)model.ReportLinks).AddRange(RelatedLinksService.BuildRelatedLinksViewModelFromUmbracoContent(UmbracoContent, "ReportTab_Content"));
            ((List<HtmlLink>)model.ApplyLinks).AddRange(RelatedLinksService.BuildRelatedLinksViewModelFromUmbracoContent(UmbracoContent, "ApplyTab_Content"));
            ((List<HtmlLink>)model.PayLinks).AddRange(RelatedLinksService.BuildRelatedLinksViewModelFromUmbracoContent(UmbracoContent, "PayTab_Content"));
            ((List<HtmlLink>)model.SchoolLinks).AddRange(RelatedLinksService.BuildRelatedLinksViewModelFromUmbracoContent(UmbracoContent, "SchoolLinks_Content"));
            ((List<HtmlLink>)model.LibrariesContent).AddRange(RelatedLinksService.BuildRelatedLinksViewModelFromUmbracoContent(UmbracoContent, "LibrariesContent_Content"));
            ((List<HtmlLink>)model.InvolvedLinks).AddRange(RelatedLinksService.BuildRelatedLinksViewModelFromUmbracoContent(UmbracoContent, "InvolvedLinks_Content"));

            return model;
        }
    }
}