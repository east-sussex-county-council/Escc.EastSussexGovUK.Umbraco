﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Features;
using Escc.EastSussexGovUK.Umbraco.Web.Models;
using Escc.EastSussexGovUK.Umbraco.Web.Services;
using Escc.Umbraco.Caching;
using Escc.Umbraco.ContentExperiments;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.UrlTransformers;
using Escc.EastSussexGovUK.Umbraco.Web.Ratings;
using Examine;
using Escc.Umbraco.Expiry;
using latest = Escc.EastSussexGovUK.Umbraco.Web.Latest;
using Escc.EastSussexGovUK.Umbraco.Web.Latest;
using Escc.EastSussexGovUK.Mvc;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Web.Controllers
{
    public class PersonController : RenderMvcController
    {
        /// <summary>
        /// The default action to render the front-end view
        /// </summary>
        /// <param name="model"/>
        /// <returns/>
        public new async Task<ActionResult> Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException("model");

            var mediaUrlTransformer = new RemoveMediaDomainUrlTransformer();
            var expiryDate = new ExpiryDateFromExamine(model.Content.Id, ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"], new ExpiryDateMemoryCache(TimeSpan.FromHours(1)));
            var viewModel = await MapUmbracoContentToViewModel(model.Content, expiryDate.ExpiryDate,
                    new UmbracoLatestService(model.Content),
                    new UmbracoSocialMediaService(model.Content),
                    new UmbracoEastSussex1SpaceService(model.Content),
                    new UmbracoWebChatSettingsService(model.Content, new UrlListReader()),
                    new RelatedLinksService(mediaUrlTransformer, new RemoveAzureDomainUrlTransformer()),
                    new ContentExperimentSettingsService(),
                    new UmbracoEscisService(model.Content),
                    new RatingSettingsFromUmbraco(model.Content),
                    mediaUrlTransformer);

            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, new IExpiryDateSource[] { expiryDate, new ExpiryDateFromPropertyValue(model.Content, "latestUnpublishDate_Latest") });

            return CurrentTemplate(viewModel);
        }

        private async Task<PersonViewModel> MapUmbracoContentToViewModel(IPublishedContent content, DateTime? expiryDate, latest.ILatestService latestService, ISocialMediaService socialMediaService, IEastSussex1SpaceService eastSussex1SpaceService, IWebChatSettingsService webChatSettingsService, IRelatedLinksService relatedLinksService, IContentExperimentSettingsService contentExperimentSettingsService, IEscisService escisService, IRatingSettingsProvider ratingSettings, IMediaUrlTransformer mediaUrlTransformer)
        {
            var model = new PersonViewModel
            {
                JobTitle = content.GetPropertyValue<string>("jobTitle_Content"),
                LeadingText = new HtmlString(mediaUrlTransformer.ParseAndTransformMediaUrlsInHtml(content.GetPropertyValue<string>("leadingText_Content"))),
                Subheading1 = content.GetPropertyValue<string>("subheading1_Content"),
                Content1 = new HtmlString(mediaUrlTransformer.ParseAndTransformMediaUrlsInHtml(content.GetPropertyValue<string>("content1_Content"))),
                Subheading2 = content.GetPropertyValue<string>("subheading2_Content"),
                Content2 = new HtmlString(mediaUrlTransformer.ParseAndTransformMediaUrlsInHtml(content.GetPropertyValue<string>("content2_Content"))),
                Contact = new HtmlString(mediaUrlTransformer.ParseAndTransformMediaUrlsInHtml(content.GetPropertyValue<string>("contact_Content")))
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
                    ImageUrl = new Uri(imageData.Url, UriKind.Relative),
                    Width = imageData.GetPropertyValue<int>("umbracoWidth"),
                    Height = imageData.GetPropertyValue<int>("umbracoHeight")
                };
                model.Metadata.PageImage.ImageUrl = new Uri(Request.Url, model.Photo.ImageUrl);
                model.Metadata.PageImage.AlternativeText = model.Photo.AlternativeText;
            }

            // Add common properties to the model
            var modelBuilder = new BaseViewModelBuilder(new EastSussexGovUKTemplateRequest(Request, webChatSettingsService: webChatSettingsService));
            await modelBuilder.PopulateBaseViewModel(model, content, contentExperimentSettingsService,
                expiryDate,
                UmbracoContext.Current.InPreviewMode);
            modelBuilder.PopulateBaseViewModelWithInheritedContent(model, latestService, socialMediaService, eastSussex1SpaceService, escisService, ratingSettings);

            return model;
        }
    }
}