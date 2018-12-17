using System;
using Escc.EastSussexGovUK.Umbraco.Web.Services;
using Umbraco.Core.Models;

namespace Escc.EastSussexGovUK.Umbraco.Web.Jobs
{
    public class JobsRssViewModelFromUmbraco : BaseViewModelFromUmbracoBuilder, IViewModelBuilder<JobsRssViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobsRssViewModel" /> class.
        /// </summary>
        /// <param name="umbracoContent">Content from Umbraco using the 'Jobs RSS' document type.</param>
        /// <exception cref="System.ArgumentNullException">umbracoContent
        /// or
        /// mediaUrlTransformer</exception>
        public JobsRssViewModelFromUmbraco(IPublishedContent umbracoContent) :
            base(umbracoContent, null)
        {
        }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <returns></returns>
        public JobsRssViewModel BuildModel()
        {
            var model = new JobsRssViewModel()
            {
                JobAdvertPage = BuildLinkToPage("JobDetailPage_Content"),
                JobsSet = ParseJobsSet("PublicOrRedeployment_Content")
            };

            return model;
        }
    }
}