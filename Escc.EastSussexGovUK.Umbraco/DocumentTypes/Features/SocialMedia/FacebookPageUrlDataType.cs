using Escc.Umbraco.PropertyEditors;
using Escc.Umbraco.PropertyEditors.DataTypes;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features.SocialMedia
{
    /// <summary>
    /// URL data type which only accepts Facebook URLs
    /// </summary>
    public static class FacebookUrlDataType
    {
        public const string DataTypeName = "Facebook URL";
        public const string PropertyEditorAlias = PropertyEditorAliases.UrlPropertyEditor;

        /// <summary>
        /// Creates the data type in Umbraco.
        /// </summary>
        public static void CreateDataType()
        {
            UrlDataType.CreateDataType(DataTypeName, false, "^https://www.facebook.com/");
        }
    }
}
