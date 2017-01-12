using System;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.EastSussexGovUK.Umbraco.Services;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    public class JobAdvertViewModelFromUmbraco : BaseJobsViewModelFromUmbracoBuilder, IViewModelBuilder<JobAdvertViewModel>
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
                HeaderBackgroundImage = BuildImage("HeaderBackgroundImage_Content"),
                JobsHomePage = BuildLinkToPage("JobsHomePage_Content"),
                LoginPage = BuildLinkToPage("LoginPage_Content"),
                JobAdvertUrl = BuildUri("AdvertScriptUrl_Content")
            };

            return model;
        }
    }
}