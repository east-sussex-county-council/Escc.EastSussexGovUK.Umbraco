using Escc.EastSussexGovUK.Umbraco.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Escc.EastSussexGovUK.Umbraco.UrlTransformers;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Core.Models;

namespace Escc.EastSussexGovUK.Umbraco.RightsOfWayDeposits
{
    public class RightsOfWayDepositsViewModelFromUmbraco : BaseViewModelFromUmbracoBuilder, IViewModelBuilder<RightsOfWayDepositsViewModel>
    {
        public RightsOfWayDepositsViewModelFromUmbraco(IPublishedContent umbracoContent) : base(umbracoContent, null, null)
        {
        }

        public RightsOfWayDepositsViewModel BuildModel()
        {
            return new RightsOfWayDepositsViewModel();
        }
    }
}