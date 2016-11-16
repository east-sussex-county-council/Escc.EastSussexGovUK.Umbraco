using System;
using System.Collections.Generic;
using AST.AzureBlobStorage.Helper;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Services
{
    public class JobsHomeViewModelFromUmbraco : BaseJobsViewModelFromUmbracoBuilder, IViewModelBuilder<JobsHomeViewModel>
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="JobsHomeViewModelFromUmbraco" /> class.
        /// </summary>
        /// <param name="umbracoContent">Content from Umbraco using the 'Jobs' document type.</param>
        /// <param name="relatedLinksService">The related links service.</param>
        /// <param name="mediaUrlTransformer">The media URL transformer.</param>
        /// <exception cref="System.ArgumentNullException">umbracoContent
        /// or
        /// mediaUrlTransformer</exception>
        public JobsHomeViewModelFromUmbraco(IPublishedContent umbracoContent, IRelatedLinksService relatedLinksService, IMediaUrlTransformer mediaUrlTransformer) :
            base(umbracoContent, relatedLinksService, mediaUrlTransformer)
        {
        }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public JobsHomeViewModel BuildModel()
        {
            var model = new JobsHomeViewModel
            {
                JobsLogo = BuildImage("JobsLogo_Content"),
                HeaderBackgroundImage = BuildImage("HeaderBackgroundImage_Content"),
                LoginPage = BuildUri("LoginPage_Content"),
                SearchResultsPage = BuildUri("SearchResultsPage_Content"),
                ButtonNavigation = RelatedLinksService.BuildRelatedLinksViewModelFromUmbracoContent(UmbracoContent, "ButtonNavigation_Content"),
                ButtonImages = BuildImages("ButtonImages_Content")
        };

            return model;
        }

       
        private HtmlLink BuildUri(string alias)
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
       
    }
}