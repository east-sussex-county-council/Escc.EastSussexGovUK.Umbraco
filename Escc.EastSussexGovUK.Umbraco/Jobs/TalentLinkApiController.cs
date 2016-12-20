﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Escc.EastSussexGovUK.Umbraco.ApiControllers;
using Escc.Net;
using HtmlAgilityPack;
using Umbraco.Core;
using Umbraco.Web.WebApi;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Web API which acts as a facade for data from the TalentLink system
    /// </summary>
    /// <seealso cref="UmbracoApiController"/>
    public class TalentLinkApiController : UmbracoApiController
    {
        /// <summary>
        /// Gets the HTML for the search fields from inside the search form
        /// </summary>
        /// <param name="id">The TalentLink account id.</param>
        /// <param name="mask">The TalentLink site id.</param>
        /// <returns></returns>
        [AcceptVerbs("GET")]
        public async Task<HttpResponseMessage> SearchFieldsHtml(string id, string mask)
        {
            var url = String.Format(ConfigurationManager.AppSettings["TalentLinkHtmlUrl"], id, mask);

            var htmlSource = new JobsDataFromTalentLink(new Uri(url), new Uri(url), new ConfigurationProxyProvider(), null, null);
            var htmlStream = await htmlSource.ReadSearchFieldsHtml();

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