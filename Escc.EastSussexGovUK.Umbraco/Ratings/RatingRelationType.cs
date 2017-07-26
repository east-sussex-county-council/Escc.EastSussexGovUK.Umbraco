using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;

namespace Escc.EastSussexGovUK.Umbraco.Ratings
{
    /// <summary>
    /// An umbraco relation between a rating page and its target page
    /// </summary>
    public class RatingRelationType : RelationType
    {
        public const string RelationTypeAlias = "Escc.EastSussexGovUK.Umbraco.Ratings";
        private const string ContentNodeGuid = "c66ba18e-eaf3-4cff-8a22-41b16d66a972";

        /// <summary>
        /// Initializes a new instance of the <see cref="RatingRelationType"/> class.
        /// </summary>
        public RatingRelationType() : base(new Guid(ContentNodeGuid), new Guid(ContentNodeGuid), RelationTypeAlias)
        {
        }
    }
}