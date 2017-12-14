using Escc.EastSussexGovUK.Umbraco.DocumentTypes;
using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.Forms
{
    /// <summary>
    /// Umbraco data type for selecting an Umbraco Form
    /// </summary>
    [UmbracoDataType(DataTypeName, PropertyEditor, typeof(FormDataType), DataTypeDatabaseType.Nvarchar)]
    internal class FormDataType : IPreValueProvider
    {
        internal const string DataTypeName = "Form";
        internal const string PropertyEditor = "UmbracoForms.FormPicker";
        public IDictionary<string, PreValue> PreValues { get; } = new Dictionary<string, PreValue>();
    }
}