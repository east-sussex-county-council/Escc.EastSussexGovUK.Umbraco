using Escc.Umbraco.PropertyEditors;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.RightsOfWayDeposits
{
    /// <summary>
    /// Content tab for <see cref="RightsOfWayDepositDocumentType"/>
    /// </summary>
    /// <seealso cref="Umbraco.Inception.BL.TabBase" />
    public class RightsOfWayDepositContentTab : TabBase
    {
        [UmbracoProperty("Deposit document", "DepositDocument", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 1, Description = "Select the deposit document containing the map, statement and declaration")]
        public string DepositDocument { get; set; }

        [UmbracoProperty("Owner's title", "HonorificTitle", BuiltInUmbracoDataTypes.Textbox, sortOrder: 3, Description = "For example, Dr or Cllr")]
        public string HonorificTitle { get; set; }

        [UmbracoProperty("Owner's first name", "GivenName", BuiltInUmbracoDataTypes.Textbox, sortOrder: 4, mandatory: true)]
        public string GivenName { get; set; }

        [UmbracoProperty("Owner's last name", "FamilyName", BuiltInUmbracoDataTypes.Textbox, sortOrder: 5, mandatory: true)]
        public string FamilyName { get; set; }

        [UmbracoProperty("Owner's suffix", "HonorificSuffix", BuiltInUmbracoDataTypes.Textbox, sortOrder: 6, Description = "For example, PhD or MBE")]
        public string HonorificSuffix { get; set; }

        [UmbracoProperty("Location", "Location", PropertyEditorAliases.UkLocationPropertyEditor, sortOrder: 7, Description = "If you include latitude and longitude a map of the location will be displayed.")]
        public string Location { get; set; }

        [UmbracoProperty("Ordnance Survey grid reference", "GridReference", BuiltInUmbracoDataTypes.Textbox, sortOrder: 8)]
        public string OrdnanceSurveyGridReference { get; set; }

        [UmbracoProperty("Parish", "Parish", ParishDataType.PropertyEditor, ParishDataType.DataTypeName, sortOrder: 9, mandatory:true)]
        public string Parish { get; set; }

        [UmbracoProperty("Description", "pageDescription", BuiltInUmbracoDataTypes.TextboxMultiple, sortOrder: 10, mandatory: true)]
        public string Description { get; set; }

        [UmbracoProperty("Date deposited", "DateDeposited", BuiltInUmbracoDataTypes.Date, sortOrder: 11)]
        public string DateDeposited { get; set; }
    }
}