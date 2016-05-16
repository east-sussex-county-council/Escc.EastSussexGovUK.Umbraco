using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Web.Mvc;
using System.Xml.XPath;
using System.Xml.Xsl;
using Escc.Dates;
using Escc.Net;
using Escc.Umbraco.MicrosoftCmsMigration;
using Exceptionless;

namespace Escc.EastSussexGovUK.Umbraco.Views.Topic
{
    public partial class FloodAlerts : ViewUserControl<MicrosoftCmsViewModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Do nothing unless we're on a page where this control is visible
            if (!this.Visible) return;

            // Do nothing if the URL or XSLT isn't in web.config
            if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["FloodAlertsRssUrl"])) return;
            if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["FloodAlertsXslPath"])) return;

            // Caching ensures we only get data once every 10 minutes, and only when it's working
            const string htmlCache = "EsccWebTeam.EastSussexCC.FloodAlerts.Html";
            const string errorCache = "EsccWebTeam.EastSussexCC.FloodAlerts.Error";

            // If there's been an error recently, don't retry for the next 10 minutes (per application).
            if (Cache[errorCache] != null) return;

            try
            {
                // Get the flood data from the Environment Agency, or local cache. 
                if (Cache[htmlCache] != null)
                {
                    this.flood.Text = Cache[htmlCache].ToString();
                }
                else
                {
                    this.flood.Text = ReadHtmlFromEnvironmentAgency();

                    // Save in cache to prevent re-requesting
                    Cache.Insert(htmlCache, this.flood.Text, null, DateTime.UtcNow.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration);
                }
            }
            catch (WebException ex)
            {
                // If something goes wrong with flood alerts we want to know, but we don't want our site to be affected 
                // so report the error and continue.
                this.Visible = false;

                // Publish the error once every 10 minutes
                ex.Data.Add("Next time you'll be notified if this error continues", DateTime.UtcNow.AddMinutes(10).ToShortTimeString());
                ex.ToExceptionless().Submit();

                // Remember that an error has occurred
                Cache.Insert(errorCache, ex.Message, null, DateTime.UtcNow.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration);
            }
        }

        private static string ReadHtmlFromEnvironmentAgency()
        {
            // Request data from the Environment Agency, timing out if they're not responding
            XPathDocument sourceXml;
            var client = new HttpRequestClient(new ConfigurationProxyProvider());
            var webRequest = client.CreateRequest(new Uri(ConfigurationManager.AppSettings["FloodAlertsRssUrl"]));
            webRequest.Timeout = 5000;
            using (var response = webRequest.GetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    sourceXml = new XPathDocument(responseStream);
                }
            }

            // Extract the date, format it and pass it back to the XSLT. Date is UTC but may need to be displayed as BST, so let
            // DateTime.Parse do the work rather than attempting it in XSLT.
            var nav = sourceXml.CreateNavigator();
            nav.MoveToRoot();
            nav = nav.SelectSingleNode("/rss/channel/lastBuildDate");
            var pubDate = DateTime.Parse(nav.InnerXml, CultureInfo.CurrentCulture);
            var xsltArguments = new XsltArgumentList();
            xsltArguments.AddParam("displayDate", String.Empty, pubDate.ToUkDateTime().ToBritishTime());

            // Transform the XML data to HTML
            var xslt = new XslCompiledTransform();
            xslt.Load(ConfigurationManager.AppSettings["FloodAlertsXslPath"]);
            using (var transformed = new StringWriter(CultureInfo.CurrentCulture))
            {
                xslt.Transform(sourceXml, xsltArguments, transformed);
                return transformed.ToString();
            }
        }
    }
}