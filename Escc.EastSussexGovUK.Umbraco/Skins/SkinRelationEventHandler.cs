using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Exceptionless;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Skins
{
    /// <summary>
    /// Maintain an Umbraco relation between a skin page and the content nodes it should be applied to
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]

    public class SkinRelationEventHandler : ApplicationEventHandler
    {
        private const string TargetPageIdPropertyAlias = "WhereToApplyIt_Apply_skin";

        /// <summary>
        /// Overridable method to execute when Bootup is completed, this allows you to perform any other bootup logic required for the application.
        /// Resolution is frozen so now they can be used to resolve instances.
        /// </summary>
        /// <param name="umbracoApplication"></param>
        /// <param name="applicationContext"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            try
            {
                EnsureRelationTypeExists();

                ContentService.Saved += ContentService_Saved;
                ContentService.Deleting += ContentService_Deleting;
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
            }
        }

        /// <summary>
        /// When a skin page is saved, relate it to the pages the skin is being applied to
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Umbraco.Core.Events.SaveEventArgs{IContent}"/> instance containing the event data.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void ContentService_Saved(IContentService sender, global::Umbraco.Core.Events.SaveEventArgs<IContent> e)
        {
            try
            {
                foreach (var node in e.SavedEntities)
                {
                    if (node.ContentType.Alias != "Skin") continue;

                    RemoveExistingRelation(node);

                    AddNewRelation(node);
                }

            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
            }
        }

        /// <summary>
        /// Deletes the relation when a page is deleted from the recycle bin
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DeleteEventArgs{IContent}"/> instance containing the event data.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void ContentService_Deleting(IContentService sender, DeleteEventArgs<IContent> e)
        {
            try
            {
                foreach (var node in e.DeletedEntities)
                {
                    // If this is a content page linked to skin, delete the relation
                    var relationsToSkins = ApplicationContext.Current.Services.RelationService.GetByChildId(node.Id).Where(r => r.RelationType.Alias == SkinRelationType.RelationTypeAlias);
                    foreach (var relation in relationsToSkins)
                    {
                        ApplicationContext.Current.Services.RelationService.Delete(relation);
                    }

                    // If this is an skin page, delete the relations to all affected pages
                    var relationsFromSkins = ApplicationContext.Current.Services.RelationService.GetByParentId(node.Id).Where(r => r.RelationType.Alias == SkinRelationType.RelationTypeAlias);
                    foreach (var relation in relationsFromSkins)
                    {
                        ApplicationContext.Current.Services.RelationService.Delete(relation);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
            }
        }

        /// <summary>
        /// Adds the new relation between a skin and its content nodes.
        /// </summary>
        /// <param name="node">The node.</param>
        private void AddNewRelation(IContent node)
        {
            var applyToPages = node.Properties[TargetPageIdPropertyAlias];
            if (applyToPages != null && applyToPages.Value != null && !String.IsNullOrEmpty(applyToPages.Value.ToString()))
            {
                try
                {
                    var pageIds = applyToPages.Value.ToString().Split(',');
                    foreach (var pageId in pageIds)
                    {
                        var targetPageId = Int32.Parse(pageId, CultureInfo.InvariantCulture);
                        ApplicationContext.Current.Services.RelationService.Save(new Relation(node.Id, targetPageId, ApplicationContext.Current.Services.RelationService.GetRelationTypeByAlias(SkinRelationType.RelationTypeAlias)));
                    }
                }
                catch (FormatException)
                {
                }
                catch (OverflowException)
                {
                }
            }
        }

        /// <summary>
        /// Removes the existing relation between a skin page and its content nodes
        /// </summary>
        /// <param name="node">The node.</param>
        private static void RemoveExistingRelation(IContent node)
        {
            var relations = ApplicationContext.Current.Services.RelationService.GetByParentId(node.Id).Where(r => r.RelationType.Alias == SkinRelationType.RelationTypeAlias);
            foreach (var relation in relations)
            {
                ApplicationContext.Current.Services.RelationService.Delete(relation);
            }
        }

        /// <summary>
        /// Ensures the relation type exists between a skin node and its target content nodes
        /// </summary>
        private static void EnsureRelationTypeExists()
        {
            if (ApplicationContext.Current.Services.RelationService.GetRelationTypeByAlias(SkinRelationType.RelationTypeAlias) == null)
            {
                var relationType = new SkinRelationType();
                ApplicationContext.Current.Services.RelationService.Save(relationType);
            }
        }
    }
}