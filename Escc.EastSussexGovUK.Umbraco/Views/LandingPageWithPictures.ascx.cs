using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Escc.Umbraco.MicrosoftCmsMigration;

namespace Escc.EastSussexGovUK.Umbraco.Views
{
    public partial class LandingPageWithPictures : ViewUserControl<MicrosoftCmsViewModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.item01.Visible = (this.phListTitle01.HasContent && this.phListDesc01.HasContent);
            this.item02.Visible = (this.phListTitle02.HasContent && this.phListDesc02.HasContent);
            this.item03.Visible = (this.phListTitle03.HasContent && this.phListDesc03.HasContent);
            this.item04.Visible = (this.phListTitle04.HasContent && this.phListDesc04.HasContent);
            this.item05.Visible = (this.phListTitle05.HasContent && this.phListDesc05.HasContent);
            this.item06.Visible = (this.phListTitle06.HasContent && this.phListDesc06.HasContent);
            this.item07.Visible = (this.phListTitle07.HasContent && this.phListDesc07.HasContent);
            this.item08.Visible = (this.phListTitle08.HasContent && this.phListDesc08.HasContent);
            this.item09.Visible = (this.phListTitle09.HasContent && this.phListDesc09.HasContent);
            this.item10.Visible = (this.phListTitle10.HasContent && this.phListDesc10.HasContent);
            this.item11.Visible = (this.phListTitle11.HasContent && this.phListDesc11.HasContent);
            this.item12.Visible = (this.phListTitle12.HasContent && this.phListDesc12.HasContent);
            this.item13.Visible = (this.phListTitle13.HasContent && this.phListDesc13.HasContent);
            this.item14.Visible = (this.phListTitle14.HasContent && this.phListDesc14.HasContent);
            this.item15.Visible = (this.phListTitle15.HasContent && this.phListDesc15.HasContent);

            this.phImage01.LinkUrl = ParseLink(this.phListTitle01.Html);
            this.phImage02.LinkUrl = ParseLink(this.phListTitle02.Html);
            this.phImage03.LinkUrl = ParseLink(this.phListTitle03.Html);
            this.phImage04.LinkUrl = ParseLink(this.phListTitle04.Html);
            this.phImage05.LinkUrl = ParseLink(this.phListTitle05.Html);
            this.phImage06.LinkUrl = ParseLink(this.phListTitle06.Html);
            this.phImage07.LinkUrl = ParseLink(this.phListTitle07.Html);
            this.phImage08.LinkUrl = ParseLink(this.phListTitle08.Html);
            this.phImage09.LinkUrl = ParseLink(this.phListTitle09.Html);
            this.phImage10.LinkUrl = ParseLink(this.phListTitle10.Html);
            this.phImage11.LinkUrl = ParseLink(this.phListTitle11.Html);
            this.phImage12.LinkUrl = ParseLink(this.phListTitle12.Html);
            this.phImage13.LinkUrl = ParseLink(this.phListTitle13.Html);
            this.phImage14.LinkUrl = ParseLink(this.phListTitle14.Html);
            this.phImage15.LinkUrl = ParseLink(this.phListTitle15.Html);
        }

        /// <summary>
        /// Parse a URL from an HTML string
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private static Uri ParseLink(string html)
        {
            if (String.IsNullOrEmpty(html)) return null;

            var match = Regex.Match(html, "href=\"(?<url>[^\"]+)\"", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (!match.Success) return null;

            return new Uri(match.Groups["url"].Value, UriKind.RelativeOrAbsolute);
        }
    }
}