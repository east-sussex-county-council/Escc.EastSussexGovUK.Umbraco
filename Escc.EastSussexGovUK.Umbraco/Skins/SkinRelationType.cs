using System;
using Umbraco.Core.Models;

namespace Escc.EastSussexGovUK.Umbraco.Skins
{
    /// <summary>
    /// An Umbraco relation between a skin settings page and the content nodes the skin is applied to
    /// </summary>
    /// <seealso cref="Umbraco.Core.Models.RelationType" />
    internal class SkinRelationType : RelationType
    {
        internal const string RelationTypeAlias = "Escc.EastSussexGovUK.Umbraco.Skin";
        private const string ContentNodeGuid = "c66ba18e-eaf3-4cff-8a22-41b16d66a972";

        /// <summary>
        /// Initializes a new instance of the <see cref="SkinRelationType"/> class.
        /// </summary>
        internal SkinRelationType() : base(new Guid(ContentNodeGuid), new Guid(ContentNodeGuid), RelationTypeAlias)
        {
        }
    }
}