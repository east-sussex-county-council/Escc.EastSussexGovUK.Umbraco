using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using Escc.Net;
using HtmlAgilityPack;
using Umbraco.Web.WebApi;

namespace Escc.EastSussexGovUK.Umbraco.ApiControllers
{
    public class TalentLinkApiController : UmbracoApiController
    {
        [AcceptVerbs("GET")]
        public async Task<HttpResponseMessage> SearchFieldsHtml(string id, string mask)
        {
            var url = String.Format(ConfigurationManager.AppSettings["TalentLinkHtmlUrl"], id, mask);

            var htmlSource = new TalentLinkHtmlFromHttpRequest(new Uri(url), new ConfigurationProxyProvider());
            var htmlStream = await htmlSource.ReadHtml();

            var parsedHtml = new HtmlDocument();
            parsedHtml.Load(htmlStream);

            var filters = new IHtmlAgilityPackFilter[] { new RemoveOuterHtmlFromSearchFieldsFilter(), 
                                                         new RemoveElementByIdFilter("div-srcparam1"),
                                                         new RemoveElementByIdFilter("actions"),
                                                         new RemoveAttributeFilter("style")
                                                       };
            foreach (var filter in filters)
            {
                filter.Filter(parsedHtml);
            }

            var response = new HttpResponseMessage
            {
                Content = new StringContent(parsedHtml.DocumentNode.InnerHtml)
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }
    }
}
