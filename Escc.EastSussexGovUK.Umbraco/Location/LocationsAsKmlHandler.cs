using Escc.EastSussexGovUK.Umbraco.ApiControllers;
using Escc.EastSussexGovUK.Umbraco.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Location
{
    public class LocationsAsKmlHandler : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: https://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            using (var client = new WebClient())
            {
                var filename = Path.GetFileNameWithoutExtension(context.Request.Url.AbsolutePath);
                var locationDocumentTypeAlias = ConvertFilenameToUmbracoAlias(filename);
                if (String.IsNullOrEmpty(locationDocumentTypeAlias))
                {
                    context.Response.StatusCode = 404;
                    return;
                }

                var json = client.DownloadString(new Uri(context.Request.Url, "/umbraco/api/location/list?type=" + HttpUtility.UrlEncode(locationDocumentTypeAlias)));
                var results = JsonConvert.DeserializeObject<List<LocationApiResult>>(json);

                context.Response.ContentType = "application/vnd.google-earth.kml+xml";
                context.Response.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                context.Response.Write("<kml xmlns=\"http://www.opengis.net/kml/2.2\">");
                foreach (var result in results)
                {
                    context.Response.Write("<Placemark>" +
                      "<name>" + HttpUtility.HtmlEncode(result.Name) + "</name>" +
                      "<description>" + HttpUtility.HtmlEncode(result.Description) + "</description>" +
                      "<Point>" +
                        "<coordinates>" + result.Latitude + "," + result.Longitude + ",0</coordinates>" +
                      "</Point>" +
                    "</Placemark>");
                }
                context.Response.Write("</kml>");

                // Allow the data to be cached for 24 hours
                context.Response.Cache.SetCacheability(HttpCacheability.Public);
                context.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(1));
                context.Response.Cache.SetMaxAge(TimeSpan.FromDays(1));
            }
        }

        private string ConvertFilenameToUmbracoAlias(string filename)
        {
            filename = filename.ToUpperInvariant();
            var valid = new Dictionary<string, string>()
            {
                { "CHILDCARE", "Childcare" },
                { "COUNCILOFFICES", "CouncilOffice" },
                { "DAYCENTRES", "DayCentre" },
                { "LIBRARIES", "Library" },
                { "MOBILELIBRARYSTOPS", "MobileLibraryStop" },
                { "PARKS", "Park" },
                { "RECYCLINGSITES", "RecyclingSite" },
                { "REGISTRATIONOFFICES", "RegistrationOffice" },
                { "SPORTLOCATIONS", "SportLocation" }
            };

            if (valid.ContainsKey(filename))
            {
                return valid[filename];
            }
            else return null;
        }

        #endregion
    }
}
