using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Escc.Umbraco.MicrosoftCmsMigration;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Views
{
    public partial class CouncilPlanMenu : ViewUserControl<MicrosoftCmsViewModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            var currentPage = umbracoHelper.TypedContent(Model.PageId);

            // Create a list of visible pages, including the home page
            var pages = new List<IPublishedContent>();

            var homePage = currentPage.AncestorOrSelf("CouncilPlanHomePage");
            if (homePage != null)
            {
                pages.Add(homePage);

                var children = homePage.Children.Where(p => p.IsVisible()).OrderBy(o => o.SortOrder);
                pages.AddRange(children);
            }

            foreach (var menuItem in pages)
            {
                var itemHtml = new StringBuilder();
                var priorityClass = CouncilPlanUtility.PriorityClass(menuItem.Id);

                itemHtml.Append("<li>");

                var classString = string.Format(CultureInfo.InvariantCulture, "class=\"council-plan {0}\"", priorityClass);
                var encodedName = HttpUtility.HtmlEncode(menuItem.Name);

                if (menuItem.Id == currentPage.Id)
                {
                    itemHtml.AppendFormat("<em {0}>{1}</em>", classString, encodedName);
                }
                else
                {
                    itemHtml.AppendFormat("<a href=\"{0}\" {1}>{2}</a>", menuItem.Url, classString, encodedName);
                }
                itemHtml.Append("</li>");
                list.Controls.Add(new LiteralControl(itemHtml.ToString()));
            }
        }
    }
}