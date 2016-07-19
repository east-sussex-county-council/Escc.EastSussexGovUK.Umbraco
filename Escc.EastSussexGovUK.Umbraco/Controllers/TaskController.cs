using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using AST.AzureBlobStorage.Helper;
using Escc.EastSussexGovUK.MasterPages.Features;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.EastSussexGovUK.Umbraco.Services;
using Escc.Umbraco.Caching;
using Escc.Umbraco.ContentExperiments;
using Escc.Umbraco.PropertyTypes;
using EsccWebTeam.EastSussexGovUK;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Escc.EastSussexGovUK.Umbraco.Controllers
{
    public class TaskController : RenderMvcController
    {
        /// <summary>
        /// The default action to render the front-end view
        /// </summary>
        /// <param name="model"/>
        /// <returns/>
        public override ActionResult Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException("model");

            var viewModel = MapUmbracoContentToViewModel(model.Content,
                    new UmbracoLatestService(model.Content),
                    new UmbracoSocialMediaService(model.Content, new EastSussexGovUKContext().DoNotTrack),
                    new UmbracoEastSussex1SpaceService(model.Content),
                    new UmbracoWebChatSettingsService(model.Content, new UrlListReader()),
                    new RelatedLinksService(),
                    new ContentExperimentSettingsService(),
                    new UmbracoEscisService(model.Content));

            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, new List<string>() { "latestUnpublishDate_Latest" });

            return CurrentTemplate(viewModel);
        }

        private static TaskViewModel MapUmbracoContentToViewModel(IPublishedContent content, ILatestService latestService, ISocialMediaService socialMediaService, IEastSussex1SpaceService eastSussex1SpaceService, IWebChatSettingsService webChatSettingsService, IRelatedLinksService relatedLinksService, IContentExperimentSettingsService contentExperimentSettingsService, IEscisService escisService)
        {
            var model = new TaskViewModel
            {
                LeadingText = new HtmlString(ContentHelper.ParseContent(content.GetPropertyValue<string>("leadingText_Content"))),
                StartPageUrl = ContentHelper.TransformUrl(new Uri(content.GetPropertyValue<string>("startPageUrl_Content"), UriKind.RelativeOrAbsolute)),
                StartButtonText = content.GetPropertyValue<string>("startButtonText_Content"),
                Subheading1 = content.GetPropertyValue<string>("subheading1_Content"),
                Content1 = new HtmlString(ContentHelper.ParseContent(content.GetPropertyValue<string>("content1_Content"))),
                Subheading2 = content.GetPropertyValue<string>("subheading2_Content"),
                Content2 = new HtmlString(ContentHelper.ParseContent(content.GetPropertyValue<string>("content2_Content"))),
                Subheading3 = content.GetPropertyValue<string>("subheading3_Content"),
                Content3 = new HtmlString(ContentHelper.ParseContent(content.GetPropertyValue<string>("content3_Content"))),
                Subheading4 = content.GetPropertyValue<string>("subheading4_Content"),
                Content4 = new HtmlString(ContentHelper.ParseContent(content.GetPropertyValue<string>("content4_Content")))
            };

            var relatedLinksGroups = new RelatedLinksModelBuilder().OrganiseAsHeadingsAndSections(relatedLinksService.BuildRelatedLinksViewModelFromUmbracoContent(content, "relatedLinks_Content"));
            foreach (var linkGroup in relatedLinksGroups)
            {
                model.RelatedLinksGroups.Add(linkGroup);
            }

            var partnerImages = content.GetPropertyValue<IEnumerable<IPublishedContent>>("partnerImages_Content");
            foreach (var imageData in partnerImages)
            {
               var image = new Image()
               {
                   AlternativeText = imageData.Name,
                   ImageUrl = ContentHelper.TransformUrl(new Uri(imageData.Url, UriKind.Relative)),
                   Width = imageData.GetPropertyValue<int>("umbracoWidth"),
                   Height = imageData.GetPropertyValue<int>("umbracoHeight")
               };
                model.PartnerImages.Add(image);
            }

            // Add common properties to the model
            var modelBuilder = new BaseViewModelBuilder();
            modelBuilder.PopulateBaseViewModel(model, content, contentExperimentSettingsService, UmbracoContext.Current.InPreviewMode);
            modelBuilder.PopulateBaseViewModelWithInheritedContent(model, latestService, socialMediaService, eastSussex1SpaceService, webChatSettingsService, escisService);

            return model;
        }
    }
}