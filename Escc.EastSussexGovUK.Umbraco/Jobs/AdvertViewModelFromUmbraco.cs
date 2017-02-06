using System;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.EastSussexGovUK.Umbraco.Services;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    public class AdvertViewModelFromUmbraco : BaseViewModelFromUmbracoBuilder, IViewModelBuilder<JobAdvertViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobAdvertViewModel" /> class.
        /// </summary>
        /// <param name="umbracoContent">Content from Umbraco using the 'Job advert' document type.</param>
        /// <exception cref="System.ArgumentNullException">umbracoContent
        /// or
        /// mediaUrlTransformer</exception>
        public AdvertViewModelFromUmbraco(IPublishedContent umbracoContent) :
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
                LoginPage = BuildLinkToPage("LoginPage_Content"),
                ExamineSearcher = BuildJobsSearcherName("PublicOrRedeployment_Content")
            };

            return model;
        }
    }
}