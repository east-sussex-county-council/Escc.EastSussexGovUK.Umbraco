using Escc.AddressAndPersonalDetails;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;
using Umbraco.Forms.Core;

namespace Escc.EastSussexGovUK.Umbraco.Forms.FieldTypes
{
    /// <summary>
    /// An Umbraco Forms field type to enter a UK address using a postcode lookup
    /// </summary>
    /// <seealso cref="Umbraco.Forms.Core.FieldType" />
    public class Address : FieldType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Address"/> class.
        /// </summary>
        public Address()
        {
            Id = new Guid("78ef3152-51b7-4952-87c7-e3b4edd686ab");
            Name = "Address";
            Description = "A UK address using a postcode lookup";
            DataType = FieldDataType.LongString;
            FieldTypeViewName = "FieldType.Address.cshtml";
            Icon = "icon-home";
            HideLabel = true; // The label is shown, but not using the standard Forms code
            SupportsPrevalues = false;
            SupportsRegex = false;
            SortOrder = 25;
            RenderView = "address"; // /App_Plugins/UmbracoForms/BackOffice/Common/RenderTypes/address.html
        }

        /// <summary>
        /// Require a JavaScript file that combines the address into a field that can pass or fail validation
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        public override IEnumerable<string> RequiredJavascriptFiles(Field field)
        {
            var list = new List<string>(base.RequiredJavascriptFiles(field));
            list.Add("~/js/forms-address.js");
            return list;
        }

        public override IEnumerable<object> ProcessSubmittedValue(Field field, IEnumerable<object> postedValues, HttpContextBase context)
        {
            // We don't want the posted value, which is only used to pass validation. 
            // We want the values from all the fields that make up the selected address.
            if (context.Request.HttpMethod.ToUpperInvariant() == "POST")
            {
                var address = new BS7666Address()
                {
                    Uprn = context.Request.Form[field.Id + ".Uprn"],
                    Usrn = context.Request.Form[field.Id + ".Usrn"],
                    Paon = context.Request.Form[field.Id + ".Paon"],
                    StreetName = context.Request.Form[field.Id + ".StreetName"],
                    Locality = context.Request.Form[field.Id + ".Locality"],
                    Town = context.Request.Form[field.Id + ".Town"],
                    AdministrativeArea = context.Request.Form[field.Id + ".AdministrativeArea"],
                    Postcode = context.Request.Form[field.Id + ".Postcode"]
                };

                double parsed;
                if (!String.IsNullOrEmpty(context.Request.Form[field.Id + ".GeoCoordinate.Latitude"]))
                {
                    if (double.TryParse(context.Request.Form[field.Id + ".GeoCoordinate.Latitude"], out parsed))
                    {
                        address.GeoCoordinate.Latitude = parsed;
                    }
                }
                if (!String.IsNullOrEmpty(context.Request.Form[field.Id + ".GeoCoordinate.Longitude"]))
                {
                    if (double.TryParse(context.Request.Form[field.Id + ".GeoCoordinate.Longitude"], out parsed))
                    {
                        address.GeoCoordinate.Longitude = parsed;
                    }
                }

                // Store the value as a JSON object representing the address
                return new object[] { JsonConvert.SerializeObject(address) };
            }

            // There is no value when the control is initially loaded on a GET request
            return new object[0];
        }
    }
}