using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Escc.Dates;

namespace Escc.EastSussexGovUK.Umbraco.RightsOfWayDeposits
{
    /// <summary>
    /// Registers a Save handler which works out the date of expiry for a rights of way Section 31 deposit
    /// </summary>
    /// <seealso cref="Umbraco.Core.IApplicationEventHandler" />
    public class RightsOfWayEventHandler : IApplicationEventHandler
    {
        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
        }

        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ContentService.Saving += ContentService_Saving;
        }

        private void ContentService_Saving(IContentService sender, global::Umbraco.Core.Events.SaveEventArgs<global::Umbraco.Core.Models.IContent> e)
        {
            foreach (var entity in e.SavedEntities)
            {
                // Work out the date the deposit expires and put that into another property.
                // It needs to go into an Umbraco property so that Examine will index it and sort on it, because sorting dates 
                // when using UmbracoContentIndexer only works when configured in ExamineIndex.config and that only supports Umbraco properties
                if (entity.ContentType.Alias == "RightsOfWayDeposit")
                {
                    var dateDeposited = entity.GetValue<DateTime>("DateDeposited_Content");
                    if (dateDeposited == DateTime.MinValue)
                    {
                        entity.SetValue("DateExpires_Content", null);
                    }
                    else
                    {
                        var depositExpires = new RightsOfWayDepositExpiryCalculator().CalculateExpiry(dateDeposited);
                        entity.SetValue("DateExpires_Content", depositExpires.ToIso8601Date());
                    }
                }
            }
        }

        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
        }
    }
}