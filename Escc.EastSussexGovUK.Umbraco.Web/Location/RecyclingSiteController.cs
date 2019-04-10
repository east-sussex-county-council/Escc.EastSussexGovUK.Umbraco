using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Web.Location
{
    /// <summary>
    /// Controller for the 'Recycling Site' document type, which adds extra properties to the <see cref="LocationViewModel"/> returned by <see cref="LocationController"/>
    /// </summary>
    public class RecyclingSiteController : LocationController
    {
        protected override LocationViewModel UpdateLocationViewModel(IPublishedContent content, LocationViewModel model)
        {
            // Get the types of waste which have been selected for this recycling site
            var recycledTypes = content.GetPropertyValue<IEnumerable<string>>("wasteTypes_Content");
            if (recycledTypes != null)
            {
                ((List<string>) model.WasteTypesRecycled).AddRange(recycledTypes);
            }

            var acceptedTypes = content.GetPropertyValue<IEnumerable<string>>("acceptedWasteTypes_Content");
            if (acceptedTypes != null)
            {
                ((List<string>) model.WasteTypesAccepted).AddRange(acceptedTypes);
            }

            // Get the authority responsible for this site
            var preValueId = content.GetPropertyValue<int>("responsibleAuthority_Content");
            if (preValueId > 0)
            {
                model.ResponsibleAuthority = umbraco.library.GetPreValueAsString(preValueId);
            }

            return model;
        }
    }
}