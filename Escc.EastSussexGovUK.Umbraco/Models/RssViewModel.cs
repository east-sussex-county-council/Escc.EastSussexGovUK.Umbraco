using System.Collections.Generic;
using Escc.EastSussexGovUK.UmbracoViews.ViewModels;

namespace Escc.EastSussexGovUK.Umbraco.Models
{
    public class RssViewModel : BaseViewModel
    {
        public RssViewModel()
        {
            Items = new List<HomePageItemViewModel>();
        }

        public string Description { get; set; }
        public IList<HomePageItemViewModel> Items { get; private set; } 
    }
}