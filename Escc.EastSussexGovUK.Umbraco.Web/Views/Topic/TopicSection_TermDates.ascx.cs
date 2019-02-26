using Escc.EastSussexGovUK.Umbraco.Web.Views.TermDates;
using Escc.Net.Configuration;
using System;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Web.Views.Topic
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
    public partial class TopicSection_TermDates : TopicSection
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (UmbracoContext.Current == null || !UmbracoContext.Current.PageId.HasValue) return;

            var content = UmbracoContext.Current.ContentCache.GetById(UmbracoContext.Current.PageId.Value);
            if (content == null) return;
            
            var termDates = content.GetPropertyValue<IPublishedContent>(PlaceholderToBindImage01 + "_Content");
            if (termDates != null && !String.IsNullOrEmpty(termDates.Url))
            {
                var cache = UmbracoContext.Current.InPreviewMode ? null : HttpContext.Current.Cache;
                var termDatesDataUrl = new Uri(termDates.Url, UriKind.Relative);
                var provider = new UrlProvider(new Uri(Request.Url, termDatesDataUrl), cache, new ConfigurationProxyProvider());

                var quickAnswer = (QuickAnswer)LoadControl("~/Views/TermDates/QuickAnswer.ascx");
                quickAnswer.TermDatesDataProvider = provider;
                this.container.Controls.Add(quickAnswer);

                var table = (TermDatesTable)LoadControl("~/Views/TermDates/TermDatesTable.ascx");
                table.TermDatesDataProvider = provider;
                this.container.Controls.Add(table);
            }
        }
    }
}
