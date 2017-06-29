using Escc.EastSussexGovUK.Umbraco.DocumentTypes;
using Escc.Umbraco.PropertyEditors;
using Escc.Umbraco.PropertyEditors.PersonNamePropertyEditor;
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
        [UmbracoProperty("Deposit document", "DepositDocument", BuiltInUmbracoDataTypes.MultipleMediaPicker, sortOrder: 1, Description = "Select the deposit documents containing the map, statement and declaration")]
        public string DepositDocument { get; set; }

        [UmbracoProperty("Owner 1", "Owner1", PersonNameDataType.PropertyEditorAlias, sortOrder: 2, Description = "An individual who partly or wholly owns the land")]
        public string Owner1 { get; set; }

        [UmbracoProperty("Owner 2", "Owner2", PersonNameDataType.PropertyEditorAlias, sortOrder: 3, Description = "An individual who partly or wholly owns the land")]
        public string Owner2 { get; set; }

        [UmbracoProperty("Owner 3", "Owner3", PersonNameDataType.PropertyEditorAlias, sortOrder: 4, Description = "An individual who partly or wholly owns the land")]
        public string Owner3 { get; set; }

        [UmbracoProperty("Owner 4", "Owner4", PersonNameDataType.PropertyEditorAlias, sortOrder: 5, Description = "An individual who partly or wholly owns the land")]
        public string Owner4 { get; set; }

        [UmbracoProperty("Owner 5", "Owner5", PersonNameDataType.PropertyEditorAlias, sortOrder: 6, Description = "An individual who partly or wholly owns the land")]
        public string Owner5 { get; set; }

        [UmbracoProperty("Organisational owner 1", "OrganisationalOwner1", BuiltInUmbracoDataTypes.Textbox, sortOrder: 7, Description = "A business or trust, for example, that partly or wholly owns the land")]
        public string OrganisationalOwner1 { get; set; }

        [UmbracoProperty("Organisational owner 2", "OrganisationalOwner2", BuiltInUmbracoDataTypes.Textbox, sortOrder: 8, Description = "A business or trust, for example, that partly or wholly owns the land")]
        public string OrganisationalOwner2 { get; set; }

        [UmbracoProperty("Organisational owner 3", "OrganisationalOwner3", BuiltInUmbracoDataTypes.Textbox, sortOrder: 9, Description = "A business or trust, for example, that partly or wholly owns the land")]
        public string OrganisationalOwner3 { get; set; }

        [UmbracoProperty("Organisational owner 4", "OrganisationalOwner4", BuiltInUmbracoDataTypes.Textbox, sortOrder: 10, Description = "A business or trust, for example, that partly or wholly owns the land")]
        public string OrganisationalOwner4 { get; set; }

        [UmbracoProperty("Organisational owner 5", "OrganisationalOwner5", BuiltInUmbracoDataTypes.Textbox, sortOrder: 11, Description = "A business or trust, for example, that partly or wholly owns the land")]
        public string OrganisationalOwner5 { get; set; }

        [UmbracoProperty("Location", "Location", PropertyEditorAliases.UkLocationPropertyEditor, sortOrder: 12, Description = "If you include latitude and longitude a map of the location will be displayed.")]
        public string Location { get; set; }

        [UmbracoProperty("Ordnance Survey grid reference", "GridReference", BuiltInUmbracoDataTypes.Textbox, sortOrder: 13)]
        public string OrdnanceSurveyGridReference { get; set; }

        [UmbracoProperty("Parish", "Parish", ParishDataType.PropertyEditor, ParishDataType.DataTypeName, sortOrder: 14, mandatory:true, 
            Description = "To select multiple parishes, hold down Ctrl when you select a parish")]
        public string Parish { get; set; }

        [UmbracoProperty("Description", "pageDescription", BuiltInUmbracoDataTypes.TextboxMultiple, sortOrder: 15, mandatory: true)]
        public string Description { get; set; }

        [UmbracoProperty("Date deposited", "DateDeposited", BuiltInUmbracoDataTypes.Date, sortOrder: 16, mandatory:true)]
        public string DateDeposited { get; set; }

        [UmbracoProperty("Date expires", "DateExpires", ReadOnlyDateDataType.PropertyEditor, sortOrder: 17, description:"This is worked out for you based on the date deposited")]
        public string DateExpires { get; set; }
    }
}