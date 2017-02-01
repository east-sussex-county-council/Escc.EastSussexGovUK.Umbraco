using System;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.Umbraco.PropertyTypes;

namespace Escc.EastSussexGovUK.Umbraco.HomePage
{
    /// <summary>
    /// A small piece of content which is used to build up the home page
    /// </summary>
    public class HomePageItemViewModel : BaseViewModel
    {
        public HtmlLink Link { get; set; }
        public string Description { get; set; }
        public Image Image { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Id { get; set; }
    }
}