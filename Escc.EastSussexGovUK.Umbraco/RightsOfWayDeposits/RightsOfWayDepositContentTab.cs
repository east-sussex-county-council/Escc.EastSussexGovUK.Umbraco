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

        [UmbracoProperty("Location 1", "Location", PropertyEditorAliases.UkLocationPropertyEditor, sortOrder: 12, Description = "If you include latitude and longitude a map of the location will be displayed.")]
        public string Location1 { get; set; }

        [UmbracoProperty("Location 2", "Location2", PropertyEditorAliases.UkLocationPropertyEditor, sortOrder: 13, Description = "If you include latitude and longitude a map of the location will be displayed.")]
        public string Location2 { get; set; }

        [UmbracoProperty("Location 3", "Location3", PropertyEditorAliases.UkLocationPropertyEditor, sortOrder: 14, Description = "If you include latitude and longitude a map of the location will be displayed.")]
        public string Location3 { get; set; }

        [UmbracoProperty("Location 4", "Location4", PropertyEditorAliases.UkLocationPropertyEditor, sortOrder: 15, Description = "If you include latitude and longitude a map of the location will be displayed.")]
        public string Location4 { get; set; }

        [UmbracoProperty("Location 5", "Location5", PropertyEditorAliases.UkLocationPropertyEditor, sortOrder: 16, Description = "If you include latitude and longitude a map of the location will be displayed.")]
        public string Location5 { get; set; }

        [UmbracoProperty("Ordnance Survey grid references", "GridReference", BuiltInUmbracoDataTypes.Textbox, sortOrder: 17, 
            Description = "If there are multiple locations, type a list of grid references separated by commas")]
        public string OrdnanceSurveyGridReference { get; set; }

        [UmbracoProperty("Parish", "Parish", ParishDataType.PropertyEditor, ParishDataType.DataTypeName, sortOrder: 18, mandatory:true, 
            Description = "To select multiple parishes, hold down Ctrl when you select a parish")]
        public string Parish { get; set; }

        [UmbracoProperty("Description", "pageDescription", BuiltInUmbracoDataTypes.TextboxMultiple, sortOrder: 19)]
        public string Description { get; set; }

        [UmbracoProperty("Date deposited", "DateDeposited", BuiltInUmbracoDataTypes.Date, sortOrder: 20, mandatory:true)]
        public string DateDeposited { get; set; }

        [UmbracoProperty("Date expires", "DateExpires", ReadOnlyDateDataType.PropertyEditor, sortOrder: 21, description:"This is worked out for you based on the date deposited")]
        public string DateExpires { get; set; }
    }
}