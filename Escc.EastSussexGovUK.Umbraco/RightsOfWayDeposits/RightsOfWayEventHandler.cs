using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Escc.Dates;
using Examine;
using UmbracoExamine;
using System.Text;
using Escc.AddressAndPersonalDetails;
using Newtonsoft.Json;
using Umbraco.Core.Logging;
using Exceptionless;

namespace Escc.EastSussexGovUK.Umbraco.RightsOfWayDeposits
{
    /// <summary>
    /// Register handlers used by the rights of way deposits pages
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
            ExamineManager.Instance.IndexProviderCollection["RightsOfWayDepositsIndexer"].GatheringNodeData += RightsOfWayEventHandler_GatheringNodeData;
        }

        private void RightsOfWayEventHandler_GatheringNodeData(object sender, IndexingNodeDataEventArgs e)
        {
            if (e.IndexType == IndexTypes.Content)
            {
                try
                {
                    if (e.Fields.ContainsKey("nodeTypeAlias") && e.Fields["nodeTypeAlias"] == "RightsOfWayDeposit")
                    {
                        var owner = new PersonName();
                        if (e.Fields.Keys.Contains("HonorificTitle_Content")) owner.Titles.Add(e.Fields["HonorificTitle_Content"]);
                        owner.GivenNames.Add(e.Fields["GivenName_Content"]);
                        owner.FamilyName = e.Fields["FamilyName_Content"];
                        if (e.Fields.Keys.Contains("HonorificSuffix_Content")) owner.Suffixes.Add(e.Fields["HonorificSuffix_Content"]);

                        var locationJson = e.Fields["Location_Content"];
                        BS7666Address address = null;
                        if (!String.IsNullOrEmpty(locationJson))
                        {
                            address = JsonConvert.DeserializeObject<BS7666Address>(locationJson);
                        }

                        var combinedFields = new StringBuilder()
                            .AppendLine(e.Fields["nodeName"])
                            .AppendLine(owner.ToString())
                            .AppendLine(address == null ? null : address.ToString())
                            .AppendLine(address == null ? null : address.Postcode?.Replace(" ", String.Empty))
                            .AppendLine(e.Fields["Parish_Content"])
                            .AppendLine(e.Fields["pageDescription_Content"]);

                        e.Fields.Add("RightsOfWayDepositSearch", combinedFields.ToString());
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error<Exception>($"Error combining fields into RightsOfWayDepositSearch Examine field for node {e.NodeId}", ex);
                    ex.ToExceptionless().Submit();
                }
            }
        }

        /// <summary>
        /// Registers a Save handler which works out the date of expiry for a rights of way Section 31 deposit
        /// </summary>
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