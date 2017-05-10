using System.Web;
using Escc.EastSussexGovUK.Umbraco.Jobs.TalentLink;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.EastSussexGovUK.Umbraco.Services;
using Umbraco.Core.Models;
using Umbraco.Web;
using Escc.EastSussexGovUK.Umbraco.UrlTransformers;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    public class ComponentViewModelFromUmbraco : BaseViewModelFromUmbracoBuilder, IViewModelBuilder<JobsComponentViewModel>
    {
        private readonly IMediaUrlTransformer _mediaUrlTransformer;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobsHomeViewModelFromUmbraco" /> class.
        /// </summary>
        /// <param name="umbracoContent">Content from Umbraco using the 'Jobs' document type.</param>
        /// <param name="mediaUrlTransformer">The media URL transformer.</param>
        /// <exception cref="System.ArgumentNullException">umbracoContent
        /// or
        /// mediaUrlTransformer</exception>
        public ComponentViewModelFromUmbraco(IPublishedContent umbracoContent, IMediaUrlTransformer mediaUrlTransformer) :
            base(umbracoContent)
        {
            _mediaUrlTransformer = mediaUrlTransformer;
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
                JobsHomePage = BuildLinkToPage("JobsHomePage_Content"),
                LoginPage = BuildLinkToPage("LoginPage_Content"),
                HeaderBackgroundImageSmall = BuildImage("HeaderBackgroundImage_Content"),
                HeaderBackgroundImageMedium = BuildImage("HeaderBackgroundImageMedium_Content"),
                HeaderBackgroundImageLarge = BuildImage("HeaderBackgroundImageLarge_Content"),
                ScriptUrl = new TalentLinkUrl(UmbracoContent.GetPropertyValue<string>("ScriptUrl_Content")),
                IsForm = UmbracoContent.GetPropertyValue<bool>("IsForm_Content"),
                ContentBelowComponent = new HtmlString(_mediaUrlTransformer.ParseAndTransformMediaUrlsInHtml(UmbracoContent.GetPropertyValue<string>("ContentBelow_Content")))
            };

            return model;
        }
    }
}