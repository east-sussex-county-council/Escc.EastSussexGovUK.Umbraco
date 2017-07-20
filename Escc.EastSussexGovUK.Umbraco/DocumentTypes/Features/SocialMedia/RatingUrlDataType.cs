using Escc.Umbraco.PropertyEditors;
using Escc.Umbraco.PropertyEditors.DataTypes;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features.SocialMedia
{
    /// <summary>
    /// URL data type which only accepts URLs from the Customer Thermometer rating product
    /// </summary>
    public static class RatingUrlDataType
    {
        public const string DataTypeName = "Rating URL";
        public const string PropertyEditorAlias = PropertyEditorAliases.UrlPropertyEditor;

        /// <summary>
        /// Creates the data type in Umbraco.
        /// </summary>
        public static void CreateDataType()
        {
            UrlDataType.CreateDataType(DataTypeName, false, @"^https://app.customerthermometer.com/\?template=log_feedback&hash=");
        }
    }
}
