using System;
using Escc.EastSussexGovUK.Umbraco.Web.Services;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Web.Jobs
{
    public class JobAdvertViewModelFromUmbraco : BaseViewModelFromUmbracoBuilder, IViewModelBuilder<JobAdvertViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobAdvertViewModel" /> class.
        /// </summary>
        /// <param name="umbracoContent">Content from Umbraco using the 'Job advert' document type.</param>
        /// <exception cref="System.ArgumentNullException">umbracoContent
        /// or
        /// mediaUrlTransformer</exception>
        public JobAdvertViewModelFromUmbraco(IPublishedContent umbracoContent) :
            base(umbracoContent, null)
        {
        }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <returns></returns>
        public JobAdvertViewModel BuildModel()
        {
            var model = new JobAdvertViewModel()
            {
                JobsLogo = BuildImage("JobsLogo_Content"),
                HeaderBackgroundImageSmall = BuildImage("HeaderBackgroundImage_Content"),
                HeaderBackgroundImageMedium = BuildImage("HeaderBackgroundImageMedium_Content"),
                HeaderBackgroundImageLarge = BuildImage("HeaderBackgroundImageLarge_Content"),
                JobsHomePage = BuildLinkToPage("JobsHomePage_Content"),
                LoginUrl = !string.IsNullOrEmpty(UmbracoContent.GetPropertyValue<string>("loginURL")) ? new Uri(UmbracoContent.GetPropertyValue<string>("loginURL"), UriKind.RelativeOrAbsolute) : null,
                SearchResultsPageForClosedJobs = BuildLinkToPage("SearchResultsPage_Content"),
                JobsSet = ParseJobsSet("PublicOrRedeployment_Content")
            };

            return model;
        }
    }
}