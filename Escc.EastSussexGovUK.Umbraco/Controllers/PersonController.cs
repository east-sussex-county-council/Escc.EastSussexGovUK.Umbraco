using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using AST.AzureBlobStorage.Helper;
using Escc.EastSussexGovUK.MasterPages.Features;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.EastSussexGovUK.Umbraco.Services;
using Escc.EastSussexGovUK.UmbracoViews.Services;
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
    public class PersonController : RenderMvcController
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

        private static PersonViewModel MapUmbracoContentToViewModel(IPublishedContent content, ILatestService latestService, ISocialMediaService socialMediaService, IEastSussex1SpaceService eastSussex1SpaceService, IWebChatSettingsService webChatSettingsService, IRelatedLinksService relatedLinksService, IContentExperimentSettingsService contentExperimentSettingsService, IEscisService escisService)
        {
            var model = new PersonViewModel
            {
                JobTitle = content.GetPropertyValue<string>("jobTitle_Content"),
                LeadingText = new HtmlString(ContentHelper.ParseContent(content.GetPropertyValue<string>("leadingText_Content"))),
                Subheading1 = content.GetPropertyValue<string>("subheading1_Content"),
                Content1 = new HtmlString(ContentHelper.ParseContent(content.GetPropertyValue<string>("content1_Content"))),
                Subheading2 = content.GetPropertyValue<string>("subheading2_Content"),
                Content2 = new HtmlString(ContentHelper.ParseContent(content.GetPropertyValue<string>("content2_Content"))),
                Contact = new HtmlString(ContentHelper.ParseContent(content.GetPropertyValue<string>("contact_Content")))
            };

            model.Person.Name.Titles.Add(content.GetPropertyValue<string>("HonorificTitle_Content"));
            model.Person.Name.GivenNames.Add(content.GetPropertyValue<string>("GivenName_Content"));
            model.Person.Name.FamilyName = content.GetPropertyValue<string>("FamilyName_Content");
            model.Person.Name.Suffixes.Add(content.GetPropertyValue<string>("HonorificSuffix_Content"));
            model.Person.EmailAddresses.Add(content.GetPropertyValue<string>("email_Content"));
            model.Person.TelephoneNumbers.Add(content.GetPropertyValue<string>("phone_Content"));

            var relatedLinksGroups = new RelatedLinksModelBuilder().OrganiseAsHeadingsAndSections(relatedLinksService.BuildRelatedLinksViewModelFromUmbracoContent(content, "relatedLinks_Content"));
            foreach (var linkGroup in relatedLinksGroups)
            {
                model.RelatedLinksGroups.Add(linkGroup);
            }

            var imageData = content.GetPropertyValue<IPublishedContent>("photo_Content");
            if (imageData != null) {
                model.Photo = new Image()
                {
                    AlternativeText = imageData.Name,
                    ImageUrl = ContentHelper.TransformUrl(new Uri(imageData.Url, UriKind.Relative)),
                    Width = imageData.GetPropertyValue<int>("umbracoWidth"),
                    Height = imageData.GetPropertyValue<int>("umbracoHeight")
                };
            }

            // Add common properties to the model
            var modelBuilder = new BaseViewModelBuilder();
            modelBuilder.PopulateBaseViewModel(model, content, contentExperimentSettingsService, UmbracoContext.Current.InPreviewMode);
            modelBuilder.PopulateBaseViewModelWithInheritedContent(model, latestService, socialMediaService, eastSussex1SpaceService, webChatSettingsService, escisService);

            return model;
        }
    }
}