using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Escc.EastSussexGovUK.Umbraco.Web.MicrosoftCmsMigration.Placeholders
{
    /// <summary>
    /// A collection of Umbraco properties which looks like a collection of Microsoft CMS placeholders
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    [Serializable]
    public class PlaceholderCollection : Dictionary<string,Placeholder>
    {
        public PlaceholderCollection()
        {
        }

        protected PlaceholderCollection(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}