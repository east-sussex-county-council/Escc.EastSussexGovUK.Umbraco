using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Examine;
using UmbracoExamine;
using System.Text;
using Escc.AddressAndPersonalDetails;
using Newtonsoft.Json;
using Umbraco.Core.Logging;
using Exceptionless;
using Escc.Umbraco.PropertyEditors.PersonNamePropertyEditor;
using System.Globalization;

namespace Escc.EastSussexGovUK.Umbraco.Web.RightsOfWayModifications
{
    /// <summary>
    /// Register handlers used by the rights of way definitive map modification orders pages
    /// </summary>
    /// <seealso cref="Umbraco.Core.IApplicationEventHandler" />
    public class RightsOfWayModificationsEventHandler : IApplicationEventHandler
    {
        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
        }

        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            if (ExamineManager.Instance.IndexProviderCollection["RightsOfWayModificationsIndexer"] != null)
            {
                ExamineManager.Instance.IndexProviderCollection["RightsOfWayModificationsIndexer"].GatheringNodeData += RightsOfWayEventHandler_GatheringNodeData;
            }
        }

        private void RightsOfWayEventHandler_GatheringNodeData(object sender, IndexingNodeDataEventArgs e)
        {
            if (e.IndexType == IndexTypes.Content)
            {
                try
                {
                    if (e.Fields.ContainsKey("nodeTypeAlias") && e.Fields["nodeTypeAlias"] == "RightsOfWayModification")
                    {
                        var combinedFields = new StringBuilder()
                            .AppendLine(e.Fields["nodeName"]);

                        if (e.Fields.ContainsKey("Parish"))
                        {
                            combinedFields.AppendLine(e.Fields["Parish"]);
                        }

                        if (e.Fields.ContainsKey("pageDescription"))
                        {
                            combinedFields.AppendLine(e.Fields["pageDescription"]);
                        }

                        var ownerConverter = new PersonNamePropertyValueConverter();
                        for (var i = 1; i <= 5; i++)
                        {
                            if (e.Fields.ContainsKey($"Owner{i}"))
                            {
                                var owner = ownerConverter.ConvertDataToSource(null, e.Fields[$"Owner{i}"], false);
                                if (owner != null) combinedFields.AppendLine(owner.ToString());
                            }

                            if (e.Fields.ContainsKey($"OrganisationalOwner{i}"))
                            {
                                combinedFields.AppendLine(e.Fields[$"OrganisationalOwner{i}"]);
                            }

                            if (e.Fields.ContainsKey($"Location{i}"))
                            {
                                var locationJson = e.Fields[$"Location{i}"];
                                if (!String.IsNullOrEmpty(locationJson))
                                {
                                    var address = JsonConvert.DeserializeObject<BS7666Address>(locationJson);
                                    combinedFields.AppendLine(address.ToString());
                                    combinedFields.AppendLine(address.Postcode?.Replace(" ", String.Empty));
                                }
                            }
                        }

                        e.Fields.Add("RightsOfWayModificationSearch", combinedFields.ToString());
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error<Exception>($"Error combining fields into RightsOfWayModificationSearch Examine field for node {e.NodeId}", ex);
                    ex.ToExceptionless().Submit();
                }
            }
        }

        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
        }
    }
}