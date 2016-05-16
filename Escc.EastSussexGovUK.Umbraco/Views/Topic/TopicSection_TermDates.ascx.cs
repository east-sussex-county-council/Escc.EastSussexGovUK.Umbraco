using System;
using System.Web;
using AST.AzureBlobStorage.Helper;
using Escc.Schools.TermDates.Website;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Views.Topic
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
                var termDatesDataUrl = ContentHelper.TransformUrl(new Uri(termDates.Url, UriKind.Relative));
                var provider = new UrlProvider(termDatesDataUrl, HttpContext.Current.Cache);

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
