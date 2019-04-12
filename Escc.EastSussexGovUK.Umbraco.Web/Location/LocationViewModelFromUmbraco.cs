using Escc.AddressAndPersonalDetails;
using Escc.Dates;
using Escc.EastSussexGovUK.Umbraco.UrlTransformers;
using Escc.EastSussexGovUK.Umbraco.Web.Services;
using Escc.Umbraco.PropertyTypes;
using Exceptionless;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Web.Location
{
    /// <summary>
    /// Builds a <see cref="LocationViewModel"/> from an instance of the 'Location' document type or one of its derived types
    /// </summary>
    public class LocationViewModelFromUmbraco : IViewModelBuilder<LocationViewModel>
    {
        private readonly IPublishedContent _content;
        private readonly IMediaUrlTransformer _mediaUrlTransformer;
        private readonly IRelatedLinksService _relatedLinksService;

        /// <summary>
        /// Creates a new instance of <see cref="LocationViewModelFromUmbraco"/>
        /// </summary>
        /// <param name="content"></param>
        /// <param name="mediaUrlTransformer"></param>
        /// <param name="relatedLinksService"></param>
        public LocationViewModelFromUmbraco(IPublishedContent content, IMediaUrlTransformer mediaUrlTransformer, IRelatedLinksService relatedLinksService)
        {
            _content = content ?? throw new ArgumentNullException(nameof(content));
            _mediaUrlTransformer = mediaUrlTransformer ?? throw new ArgumentNullException(nameof(mediaUrlTransformer));
            _relatedLinksService = relatedLinksService ?? throw new ArgumentNullException(nameof(relatedLinksService));
        }

        /// <summary>
        /// Maps the Umbraco content to a <see cref="LocationViewModel"/>
        /// </summary>
        /// <returns></returns>
        public LocationViewModel BuildModel()
        {
            var model = new LocationViewModel
            {
                Content = new HtmlString(_mediaUrlTransformer.ParseAndTransformMediaUrlsInHtml(_content.GetPropertyValue<string>("content_Content"))),
                OpeningHoursDetails = new HtmlString(_mediaUrlTransformer.ParseAndTransformMediaUrlsInHtml(_content.GetPropertyValue<string>("openingHoursDetails_Content"))),
                Tab1Title = _content.GetPropertyValue<string>("tab1title_Content"),
                Tab1Content = new HtmlString(_mediaUrlTransformer.ParseAndTransformMediaUrlsInHtml(_content.GetPropertyValue<string>("tab1content_Content"))),
                Tab2Title = _content.GetPropertyValue<string>("tab2title_Content"),
                Tab2Content = new HtmlString(_mediaUrlTransformer.ParseAndTransformMediaUrlsInHtml(_content.GetPropertyValue<string>("tab2content_Content"))),
                Tab3Title = _content.GetPropertyValue<string>("tab3title_Content"),
                Tab3Content = new HtmlString(_mediaUrlTransformer.ParseAndTransformMediaUrlsInHtml(_content.GetPropertyValue<string>("tab3content_Content"))),
                Location = _content.GetPropertyValue<AddressInfo>("location_Content"),
                Email1Label = _content.GetPropertyValue<string>("email1label_Content"),
                Email2Label = _content.GetPropertyValue<string>("email2label_Content"),
                Email1 = _content.GetPropertyValue<string>("email1_Content"),
                Email2 = _content.GetPropertyValue<string>("email2_Content"),
                ContactFormUrl = string.IsNullOrEmpty(_content.GetPropertyValue<string>("contactFormURL")) ? null : new Uri(_content.GetPropertyValue<string>("contactFormURL"), UriKind.RelativeOrAbsolute),
                ContactFormLabel = _content.GetPropertyValue<string>("contactFormLabel"),
                Phone1Label = _content.GetPropertyValue<string>("phone1label_Content"),
                Phone2Label = _content.GetPropertyValue<string>("phone2label_Content"),
                Phone1 = _content.GetPropertyValue<string>("phone1_Content"),
                Phone2 = _content.GetPropertyValue<string>("phone2_Content"),
                Fax1Label = _content.GetPropertyValue<string>("fax1label_Content"),
                Fax2Label = _content.GetPropertyValue<string>("fax2label_Content"),
                Fax1 = _content.GetPropertyValue<string>("fax1_Content"),
                Fax2 = _content.GetPropertyValue<string>("fax2_Content")
            };

            var relatedLinksGroups = new RelatedLinksModelBuilder().OrganiseAsHeadingsAndSections(_relatedLinksService.BuildRelatedLinksViewModelFromUmbracoContent(_content, "relatedLinks_Content"));
            foreach (var linkGroup in relatedLinksGroups)
            {
                model.RelatedLinksGroups.Add(linkGroup);
            }

            // Opening times
            model.OpeningHours = DeserialiseOpeningHours(_content);
            WorkOutRelativeOpeningTimes(model);
            WorkOutNextOpenRelativeTime(model);

            // Photo
            var imageData = _content.GetPropertyValue<IPublishedContent>("photo_Content");
            if (imageData != null)
            {
                model.Photo = new Image()
                {
                    AlternativeText = imageData.Name,
                    ImageUrl = new Uri(imageData.Url, UriKind.Relative),
                    Width = imageData.GetPropertyValue<int>("umbracoWidth"),
                    Height = imageData.GetPropertyValue<int>("umbracoHeight")
                };
                model.Metadata.PageImageUrl = new Uri(new Uri(_content.UrlWithDomain()), model.Photo.ImageUrl).ToString();
            }

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

            if (model.NextOpen == DateTime.MaxValue)
            {
                model.NextOpen = null;
            }
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
            if (!model.NextOpen.HasValue)
            {
                return;
            }

            var ukNow = DateTime.Now.ToUkDateTime();

            // Default to a generic "on Monday"
            model.NextOpenRelativeTime = "on " + model.NextOpen.Value.DayOfWeek;

            // Customise for today and tomorrow
            if (model.NextOpen.Value.Date == ukNow.Date)
            {
                model.NextOpenRelativeTime = "today";
            }

            if (model.NextOpen.Value.Date == ukNow.Date.AddDays(1))
            {
                model.NextOpenRelativeTime = "tomorrow";
            }

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