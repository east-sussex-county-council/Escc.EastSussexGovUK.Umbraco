using System;
using System.Collections.Generic;
using AST.AzureBlobStorage.Helper;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Services
{
    public class JobSearchResultsViewModelFromUmbraco : BaseJobsViewModelFromUmbracoBuilder, IViewModelBuilder<JobSearchResultsViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobSearchResultsViewModelFromUmbraco" /> class.
        /// </summary>
        /// <param name="umbracoContent">Content from Umbraco using the 'Jobs' document type.</param>
        /// <param name="relatedLinksService">The related links service.</param>
        /// <param name="mediaUrlTransformer">The media URL transformer.</param>
        /// <exception cref="System.ArgumentNullException">umbracoContent
        /// or
        /// mediaUrlTransformer</exception>
        public JobSearchResultsViewModelFromUmbraco(IPublishedContent umbracoContent, IRelatedLinksService relatedLinksService, AzureMediaUrlTransformer mediaUrlTransformer) :
            base(umbracoContent, relatedLinksService, mediaUrlTransformer)
        {
        }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public JobSearchResultsViewModel BuildModel()
        {
            var model = new JobSearchResultsViewModel()
            {
                JobsLogo = BuildImage("JobsLogo_Content"),
                HeaderBackgroundImage = BuildImage("HeaderBackgroundImage_Content"),
                LoginPage = BuildLinkToPage("LoginPage_Content"),
                ResultsScriptUrl = BuildUri("ResultsScriptUrl_Content"),
                ButtonNavigation = RelatedLinksService.BuildRelatedLinksViewModelFromUmbracoContent(UmbracoContent, "ButtonNavigation_Content"),
                ButtonImages = BuildImages("ButtonImages_Content")
            };
            model.ResultsLinkUrl = new Uri(model.ResultsScriptUrl.ToString().Replace("laydisplayrapido.cfm", "jsoutputinitrapido.cfm").Replace("&browserchk=no", String.Empty));

            return model;
        }

        private Uri BuildUri(string alias)
        {
            var url = UmbracoContent.GetPropertyValue<string>(alias);
            if (!String.IsNullOrEmpty(url))
            {
                return new Uri(url);
            }
            return null;
        }

        private HtmlLink BuildLinkToPage(string alias)
        {
            var linkedPage = UmbracoContent.GetPropertyValue<IPublishedContent>(alias);
            if (linkedPage != null)
            {
                return new HtmlLink()
                {
                    Text = linkedPage.Name,
                    Url = new Uri(linkedPage.UrlAbsolute())
                };
            }
            return null;
        }

        private Image BuildImage(string alias)
        {
            var imageData = UmbracoContent.GetPropertyValue<IPublishedContent>(alias);
            if (imageData == null) return null;

            return new Image()
            {
                AlternativeText = imageData.Name,
                ImageUrl = MediaUrlTransformer.TransformMediaUrl(new Uri(imageData.Url, UriKind.Relative)),
                Width = imageData.GetPropertyValue<int>("umbracoWidth"),
                Height = imageData.GetPropertyValue<int>("umbracoHeight")
            };
        }
    }
}