using System;
using System.Collections.Generic;
using AST.AzureBlobStorage.Helper;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Services
{
    public class JobsComponentViewModelFromUmbraco : BaseJobsViewModelFromUmbracoBuilder, IViewModelBuilder<JobsComponentViewModel>
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="JobsHomeViewModelFromUmbraco" /> class.
        /// </summary>
        /// <param name="umbracoContent">Content from Umbraco using the 'Jobs' document type.</param>
        /// <param name="mediaUrlTransformer">The media URL transformer.</param>
        /// <exception cref="System.ArgumentNullException">umbracoContent
        /// or
        /// mediaUrlTransformer</exception>
        public JobsComponentViewModelFromUmbraco(IPublishedContent umbracoContent, IMediaUrlTransformer mediaUrlTransformer) :
            base(umbracoContent, mediaUrlTransformer)
        {
        }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public JobsComponentViewModel BuildModel()
        {
            var model = new JobsComponentViewModel
            {
                JobsLogo = BuildImage("JobsLogo_Content"),
                HeaderBackgroundImage = BuildImage("HeaderBackgroundImage_Content"),
                ScriptUrl = new TalentLinkUrl(UmbracoContent.GetPropertyValue<string>("ScriptUrl_Content"))
            };

            return model;
        }
    }
}