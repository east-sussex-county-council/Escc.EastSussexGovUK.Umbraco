using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Mvc;
using Escc.AddressAndPersonalDetails;
using Escc.Dates;
using Escc.EastSussexGovUK.Features;
using Escc.EastSussexGovUK.Umbraco.Web.Models;
using Escc.EastSussexGovUK.Umbraco.Web.Services;
using Escc.Umbraco.Caching;
using Escc.Umbraco.ContentExperiments;
using Escc.Umbraco.PropertyTypes;
using Exceptionless;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.UrlTransformers;
using Escc.EastSussexGovUK.Umbraco.Web.Ratings;
using Escc.EastSussexGovUK.Umbraco.Web.Skins;
using Examine;
using Escc.Umbraco.Expiry;
using latest = Escc.EastSussexGovUK.Umbraco.Web.Latest;
using Escc.EastSussexGovUK.Umbraco.Web.Latest;

namespace Escc.EastSussexGovUK.Umbraco.Web.Location
{
    public class LocationController : RenderMvcController
    {
        /// <summary>
        /// The default action to render the front-end view
        /// </summary>
        /// <param name="model"/>
        /// <returns/>
        public override ActionResult Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException("model");

            var expiryDate = new ExpiryDateFromExamine(model.Content.Id, ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"]);
            var mediaUrlTransformer = new RemoveMediaDomainUrlTransformer();
            var viewModel = MapUmbracoContentToViewModel(model.Content, expiryDate.ExpiryDate,
                    new UmbracoLatestService(model.Content),
                    new UmbracoSocialMediaService(model.Content),
                    new UmbracoEastSussex1SpaceService(model.Content),
                    new UmbracoWebChatSettingsService(model.Content, new UrlListReader()),
                    new RelatedLinksService(mediaUrlTransformer, new ElibraryUrlTransformer(), new RemoveAzureDomainUrlTransformer()),
                    new ContentExperimentSettingsService(),
                    new UmbracoEscisService(model.Content),
                    new RatingSettingsFromUmbraco(model.Content), 
                    mediaUrlTransformer, 
                    new SkinFromUmbraco());

            SetupHttpCaching(model, viewModel, expiryDate);

            return CurrentTemplate(viewModel);
        }

        private void SetupHttpCaching(RenderModel model, LocationViewModel viewModel, IExpiryDateSource expiryDate)
        {
            var cacheService = new HttpCachingService();
            var cacheExpiryProperties = new IExpiryDateSource[] { expiryDate, new ExpiryDateFromPropertyValue(model.Content, "latestUnpublishDate_Latest") };
            var ukNow = DateTime.Now.ToUkDateTime();
            const int oneHour = 3600;

            // Ensure we don't cache the page longer than the opening times data passed to the view remains valid.
            // In the last hour before closing there's a countdown, so it's only correct for one minute.
            // If there's no opening times data, just fall back to the default cache expiry settings.
            if (viewModel.OpenUntil.HasValue)
            {
                var secondsRemaining = Convert.ToInt32((viewModel.OpenUntil - ukNow).Value.TotalSeconds);
                if (secondsRemaining <= oneHour) secondsRemaining = 60;
                cacheService.SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, cacheExpiryProperties, secondsRemaining);
            }
            else if (viewModel.NextOpen.HasValue)
            {
                var secondsRemaining = Convert.ToInt32((viewModel.NextOpen - ukNow).Value.TotalSeconds);
                if (secondsRemaining <= oneHour) secondsRemaining = 60;
                cacheService.SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, cacheExpiryProperties, secondsRemaining);
            }
            else
            {
                cacheService.SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, cacheExpiryProperties);
            }
        }

        /// <summary>
        /// Maps the Umbraco content to the view model.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="expiryDate">The expiry date.</param>
        /// <param name="latestService">The latest service.</param>
        /// <param name="socialMediaService">The social media service.</param>
        /// <param name="eastSussex1SpaceService">The East Sussex One Space service.</param>
        /// <param name="webChatSettingsService">The web chat settings service.</param>
        /// <param name="relatedLinksService">The related links service.</param>
        /// <param name="contentExperimentSettingsService">The content experiment settings service.</param>
        /// <param name="escisService">The escis service.</param>
        /// <param name="ratingSettings">The rating settings.</param>
        /// <param name="mediaUrlTransformer">The media URL transformer.</param>
        /// <param name="skinService">The skin service.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">content
        /// or
        /// latestService
        /// or
        /// socialMediaService
        /// or
        /// eastSussex1SpaceService
        /// or
        /// webChatSettingsService
        /// or
        /// relatedLinksService</exception>
        /// <exception cref="System.ArgumentNullException">content
        /// or
        /// latestService
        /// or
        /// socialMediaService
        /// or
        /// eastSussex1SpaceService
        /// or
        /// webChatSettingsService
        /// or
        /// relatedLinksService
        /// or
        /// contentExperimentSettingsService</exception>
        /// <remarks>
        /// Method is virtual so that document types which inherit from the 'Location' type can also inherit and extend the controller
        /// </remarks>
        protected virtual LocationViewModel MapUmbracoContentToViewModel(IPublishedContent content, DateTime? expiryDate, latest.ILatestService latestService, ISocialMediaService socialMediaService, IEastSussex1SpaceService eastSussex1SpaceService, IWebChatSettingsService webChatSettingsService, IRelatedLinksService relatedLinksService, IContentExperimentSettingsService contentExperimentSettingsService, IEscisService escisService, IRatingSettingsProvider ratingSettings, IMediaUrlTransformer mediaUrlTransformer, ISkinToApplyService skinService)
        {
            if (content == null) throw new ArgumentNullException("content");
            if (latestService == null) throw new ArgumentNullException("latestService");
            if (socialMediaService == null) throw new ArgumentNullException("socialMediaService");
            if (eastSussex1SpaceService == null) throw new ArgumentNullException("eastSussex1SpaceService");
            if (webChatSettingsService == null) throw new ArgumentNullException("webChatSettingsService");
            if (relatedLinksService == null) throw new ArgumentNullException("relatedLinksService");
            if (contentExperimentSettingsService == null) throw new ArgumentNullException("contentExperimentSettingsService");
            if (ratingSettings == null) throw new ArgumentNullException(nameof(ratingSettings));
            if (skinService == null) throw new ArgumentNullException(nameof(skinService));

            var model = new LocationViewModel
            {
                Content = new HtmlString(mediaUrlTransformer.ParseAndTransformMediaUrlsInHtml(content.GetPropertyValue<string>("content_Content"))),
                OpeningHoursDetails = new HtmlString(mediaUrlTransformer.ParseAndTransformMediaUrlsInHtml(content.GetPropertyValue<string>("openingHoursDetails_Content"))),
                Tab1Title = content.GetPropertyValue<string>("tab1title_Content"),
                Tab1Content = new HtmlString(mediaUrlTransformer.ParseAndTransformMediaUrlsInHtml(content.GetPropertyValue<string>("tab1content_Content"))),
                Tab2Title = content.GetPropertyValue<string>("tab2title_Content"),
                Tab2Content = new HtmlString(mediaUrlTransformer.ParseAndTransformMediaUrlsInHtml(content.GetPropertyValue<string>("tab2content_Content"))),
                Tab3Title = content.GetPropertyValue<string>("tab3title_Content"),
                Tab3Content = new HtmlString(mediaUrlTransformer.ParseAndTransformMediaUrlsInHtml(content.GetPropertyValue<string>("tab3content_Content"))),
                Location = content.GetPropertyValue<AddressInfo>("location_Content"),
                Email1Label = content.GetPropertyValue<string>("email1label_Content"),
                Email2Label = content.GetPropertyValue<string>("email2label_Content"),
                Email1 = content.GetPropertyValue<string>("email1_Content"),
                Email2 = content.GetPropertyValue<string>("email2_Content"),
                Phone1Label = content.GetPropertyValue<string>("phone1label_Content"),
                Phone2Label = content.GetPropertyValue<string>("phone2label_Content"),
                Phone1 = content.GetPropertyValue<string>("phone1_Content"),
                Phone2 = content.GetPropertyValue<string>("phone2_Content"),
                Fax1Label = content.GetPropertyValue<string>("fax1label_Content"),
                Fax2Label = content.GetPropertyValue<string>("fax2label_Content"),
                Fax1 = content.GetPropertyValue<string>("fax1_Content"),
                Fax2 = content.GetPropertyValue<string>("fax2_Content")
            };

            var relatedLinksGroups = new RelatedLinksModelBuilder().OrganiseAsHeadingsAndSections(relatedLinksService.BuildRelatedLinksViewModelFromUmbracoContent(content, "relatedLinks_Content"));
            foreach (var linkGroup in relatedLinksGroups)
            {
                model.RelatedLinksGroups.Add(linkGroup);
            }

            // Opening times
            model.OpeningHours = DeserialiseOpeningHours(content);
            WorkOutRelativeOpeningTimes(model);
            WorkOutNextOpenRelativeTime(model);

            // Photo
            var imageData = content.GetPropertyValue<IPublishedContent>("photo_Content");
            if (imageData != null)
            {
                model.Photo = new Image()
                {
                    AlternativeText = imageData.Name,
                    ImageUrl = new Uri(imageData.Url, UriKind.Relative),
                    Width = imageData.GetPropertyValue<int>("umbracoWidth"),
                    Height = imageData.GetPropertyValue<int>("umbracoHeight")
                };
                model.Metadata.PageImageUrl = new Uri(Request.Url, model.Photo.ImageUrl).ToString();
            }

            // Add common properties to the model
            var modelBuilder = new BaseViewModelBuilder();
            modelBuilder.PopulateBaseViewModel(model, content, contentExperimentSettingsService,
                expiryDate,
                UmbracoContext.Current.InPreviewMode, skinService);
            modelBuilder.PopulateBaseViewModelWithInheritedContent(model, latestService, socialMediaService, eastSussex1SpaceService, webChatSettingsService, escisService, ratingSettings);

            return model;
        }

        /// <summary>
        /// Deserialise opening times saved by the OpeningSoon property editor
        /// http://our.umbraco.org/projects/backoffice-extensions/openingsoon
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private static IList<OpeningTimes> DeserialiseOpeningHours(IPublishedContent content)
        {
            var openingHoursJson = content.GetPropertyValue<string>("openingHours_Content");

            try
            {
                using (var stream = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(openingHoursJson)))
                {
                    var js = new DataContractJsonSerializer(typeof(OpeningTimes[]));
                    return new List<OpeningTimes>((OpeningTimes[])js.ReadObject(stream));
                }
            }
            catch (SerializationException e)
            {
                // Report and continue. Better to show a page with no opening times than nothing at all.
                e.ToExceptionless().Submit();    
                return new List<OpeningTimes>();
            }
        }

        /// <summary>
        /// Work out when the location is open until or next open
        /// </summary>
        /// <param name="model"></param>
        private static void WorkOutRelativeOpeningTimes(LocationViewModel model)
        {
            var ukNow = DateTime.Now.ToUkDateTime();
            model.NextOpen = DateTime.MaxValue;

            foreach (var opening in model.OpeningHours)
            {
                WorkOutOpenUntil(model, ukNow, opening);
                WorkOutNextOpen(model, opening, ukNow);
            }

            if (model.NextOpen == DateTime.MaxValue) model.NextOpen = null;
        }

        private static void WorkOutOpenUntil(LocationViewModel model, DateTime ukNow, OpeningTimes opening)
        {
            var today = (ukNow.DayOfWeek.ToString().Substring(0, 2).ToUpperInvariant() == opening.Day.Substring(0, 2).ToUpperInvariant());

            var now = ukNow.TimeOfDay;
            var open1 = (opening.OpensAtTime.HasValue && opening.ClosesAtTime.HasValue && now >= opening.OpensAtTime.Value.TimeOfDay && now <= opening.ClosesAtTime.Value.TimeOfDay);
            var open2 = (opening.OpensAgainAtTime.HasValue && opening.ClosesAgainAtTime.HasValue && now >= opening.OpensAgainAtTime.Value.TimeOfDay && now <= opening.ClosesAgainAtTime.Value.TimeOfDay);
            var open = today && opening.Scheduled && (open1 || open2);


            if (open && open1)
            {
                model.OpenUntil = opening.ClosesAtTime;
            }
            if (open && open2)
            {
                model.OpenUntil = opening.ClosesAgainAtTime;
            }
        }

        private static void WorkOutNextOpen(LocationViewModel model, OpeningTimes opening, DateTime ukNow)
        {
            if (opening.Scheduled && opening.OpensAtTime.HasValue && opening.OpensAtTime > ukNow && opening.OpensAtTime < model.NextOpen)
            {
                model.NextOpen = opening.OpensAtTime.Value;
            }
            if (opening.Scheduled && opening.OpensAgainAtTime.HasValue && opening.OpensAgainAtTime > ukNow && opening.OpensAgainAtTime < model.NextOpen)
            {
                model.NextOpen = opening.OpensAgainAtTime.Value;
            }

            // If it's open this day, and we've no better offer, it'll be open again this time next week
            if (opening.Scheduled && opening.OpensAtTime.HasValue)
            {
                var sameTimeNextWeek = opening.OpensAtTime.Value.AddDays(7);
                if (model.NextOpen > sameTimeNextWeek)
                {
                    model.NextOpen = sameTimeNextWeek;
                }
            }
        }

        /// <summary>
        /// Convert the date and time the location is next open into English text relative to today. It can't be in the view as Razor can't handle this.
        /// </summary>
        /// <param name="model"></param>
        private static void WorkOutNextOpenRelativeTime(LocationViewModel model)
        {
            if (!model.NextOpen.HasValue) return;

            var ukNow = DateTime.Now.ToUkDateTime();

            // Default to a generic "on Monday"
            model.NextOpenRelativeTime = "on " + model.NextOpen.Value.DayOfWeek;

            // Customise for today and tomorrow
            if (model.NextOpen.Value.Date == ukNow.Date) model.NextOpenRelativeTime = "today";
            if (model.NextOpen.Value.Date == ukNow.Date.AddDays(1)) model.NextOpenRelativeTime = "tomorrow";

            // Next open earlier than now, on the same day next week
            if (model.NextOpen.Value.DayOfWeek == ukNow.DayOfWeek && model.NextOpen.Value.TimeOfDay < ukNow.TimeOfDay)
            {
                model.NextOpenRelativeTime = "next " + model.NextOpen.Value.DayOfWeek;
            }

            // Add a time conforming to our house style
            model.NextOpenRelativeTime = model.NextOpenRelativeTime + " at " + model.NextOpen.Value.ToBritishTime();
        }
    }
}