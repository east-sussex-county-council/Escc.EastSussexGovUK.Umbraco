﻿using System;
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
using Escc.Umbraco.PropertyEditors.PersonNamePropertyEditor;

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
                        var combinedFields = new StringBuilder()
                            .AppendLine(e.Fields["nodeName"]);

                            if (e.Fields.ContainsKey("Location_Content"))
                        {
                            var locationJson = e.Fields["Location_Content"];
                            if (!String.IsNullOrEmpty(locationJson))
                            {
                                var address = JsonConvert.DeserializeObject<BS7666Address>(locationJson);
                                combinedFields.AppendLine(address.ToString());
                                combinedFields.AppendLine(address.Postcode?.Replace(" ", String.Empty));
                            }
                        }

                        if (e.Fields.ContainsKey("Parish_Content"))
                        {
                            combinedFields.AppendLine(e.Fields["Parish_Content"]);
                        }

                        if (e.Fields.ContainsKey("pageDescription_Content"))
                        {
                            combinedFields.AppendLine(e.Fields["pageDescription_Content"]);
                        }

                        var ownerConverter = new PersonNamePropertyValueConverter();
                        for (var i = 1; i <= 5; i++)
                        {
                            if (e.Fields.ContainsKey($"Owner{i}_Content"))
                            {
                                var owner = ownerConverter.ConvertDataToSource(null, e.Fields[$"Owner{i}_Content"], false);
                                if (owner != null) combinedFields.AppendLine(owner.ToString());
                            }

                            if (e.Fields.ContainsKey($"OrganisationalOwner{i}_Content"))
                            {
                                combinedFields.AppendLine(e.Fields[$"OrganisationalOwner{i}_Content"]);
                            }
                        }

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