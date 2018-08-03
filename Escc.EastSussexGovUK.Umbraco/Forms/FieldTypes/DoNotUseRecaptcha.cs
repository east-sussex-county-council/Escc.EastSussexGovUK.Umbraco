using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Forms.Core;
using BuiltInFieldTypes = Umbraco.Forms.Core.Providers.FieldTypes;
using Umbraco.Forms.Core.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.Forms.FieldTypes
{
    /// <summary>
    /// We can't remove the obsolete Recaptcha field type but we can change the label to tell people not to use it
    /// </summary>
    /// <seealso cref="Umbraco.Forms.Core.FieldType" />
    public class DoNotUseRecaptcha : BuiltInFieldTypes.Recaptcha
    {
        public DoNotUseRecaptcha() : base()
        {
            Name = "Do not use: Recaptcha";
            Description = "This answer type comes with Umbraco Forms but uses Recaptcha v1, which is obsolete.";
            Icon = "icon-block";
            SortOrder = 10000;
        }
    }
}