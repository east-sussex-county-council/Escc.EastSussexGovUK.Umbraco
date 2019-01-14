using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// A lookup value and id used by our jobs service, with a count of how many jobs currently match
    /// </summary>
    public class JobsLookupValue
    {
        /// <summary>
        /// Gets or sets the identifier of the field the lookup value can be found in
        /// </summary>
        public string FieldId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the lookup value.
        /// </summary>
        public string LookupValueId { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets how many jobs currently match this lookup value
        /// </summary>
        public int Count { get; set; }
    }
}