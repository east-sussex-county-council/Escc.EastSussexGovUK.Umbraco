using System;
using System.Collections.Generic;
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
            var url = $"https://emea3.recruitmentplatform.com/syndicated/lay/jsoutputinitrapido.cfm?ID={id}&mask={mask}&component=lay9999_src350a";

            var handler = new HttpClientHandler()
            {
                Proxy = new ConfigurationProxyProvider().CreateProxy()
            };
            var client = new HttpClient(handler);
            var html = await client.GetStringAsync(url);

            var parsedHtml = new HtmlDocument();
            parsedHtml.LoadHtml(html);
            var body = (HtmlNodeNavigator)parsedHtml.CreateNavigator().SelectSingleNode("/html/body/div");
            if (body == null) return null;

            var keywords = (HtmlNodeNavigator)body.SelectSingleNode("//div[@id='div-srcparam1']");
            if (keywords != null) keywords.CurrentNode.ParentNode.RemoveChild(keywords.CurrentNode);


            var actions = (HtmlNodeNavigator)body.SelectSingleNode("//div[@id='actions']");
            if (actions != null) actions.CurrentNode.ParentNode.RemoveChild(actions.CurrentNode);

            var inlineStyles = parsedHtml.DocumentNode.SelectNodes("//*[@style]");
            foreach (var node in inlineStyles)
            {
                node.Attributes.Remove("style");
            }


            // HTML Agility Pack can isolate the form element but can't correctly select its children, so fall back on regex to remove the outer element
            var form = body.CurrentNode.InnerHtml;
            form = Regex.Replace(form, "</?form[^>]*>", String.Empty);

            var response = new HttpResponseMessage();
            response.Content = new StringContent(form);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }
    }
}
