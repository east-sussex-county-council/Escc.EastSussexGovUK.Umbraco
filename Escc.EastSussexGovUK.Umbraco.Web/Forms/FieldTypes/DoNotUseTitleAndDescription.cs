using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Forms.Core;
using BuiltInFieldTypes = Umbraco.Forms.Core.Providers.FieldTypes;
using Umbraco.Forms.Core.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.Web.Forms.FieldTypes
{
    /// <summary>
    /// We can't remove the obsolete 'Title and description' field type but we can change the label to tell people not to use it
    /// </summary>
    /// <seealso cref="Umbraco.Forms.Core.FieldType" />
    public class DoNotUseTitleAndDescription : BuiltInFieldTypes.Text
    {
        public DoNotUseTitleAndDescription() : base()
        {
            Name = "Do not use: Title and description";
            Description = "This answer type comes with Umbraco Forms but 'Formatted text' is more flexible, so use that instead.";
            Icon = "icon-block";
            SortOrder = 10000;
        }
    }
}