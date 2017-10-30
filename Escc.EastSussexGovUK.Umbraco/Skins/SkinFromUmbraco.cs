using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Skins
{
    /// <summary>
    /// Gets the skin to apply to a page based on a selection made using the 'Skin' document type
    /// </summary>
    public class SkinFromUmbraco : ISkinToApplyService
    {
        /// <summary>
        /// Looks up the name of the skin for a page.
        /// </summary>
        /// <param name="pageId">The node identifier.</param>
        /// <returns></returns>
        public string LookupSkinForPage(int pageId)
        {
            var relationToSkin = ApplicationContext.Current.Services.RelationService.GetByChildId(pageId).FirstOrDefault(r => r.RelationType.Alias == SkinRelationType.RelationTypeAlias);
            if (relationToSkin == null) return null;

            var skinPage = UmbracoContext.Current.ContentCache.GetById(relationToSkin.ParentId);
            if (skinPage == null) return null;

            return skinPage.GetPropertyValue<string>("Skin_Apply_skin");
        }
    }
}