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
using Escc.Umbraco.PropertyTypes;

namespace Escc.EastSussexGovUK.Umbraco.Ratings
{
    /// <summary>
    /// Maintain an Umbraco relation between a rating page, and the content node it should be applied to
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public class RatingRelationEventHandler : ApplicationEventHandler
    {
        private const string TargetPageIdPropertyAlias = "WhereToDisplayIt_Content";

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
                ContentService.Copying += ContentService_Copying;
                ContentService.Deleting += ContentService_Deleting;
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
            }
        }

        /// <summary>
        /// When copying a rating page, remove the target page, as only one rating should relate to a given target page
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Umbraco.Core.Events.CopyEventArgs{IContent}"/> instance containing the event data.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void ContentService_Copying(IContentService sender, global::Umbraco.Core.Events.CopyEventArgs<IContent> e)
        {
            try
            {
                if (e.Copy.ContentType.Alias != "Rating") return;

                e.Copy.Properties[TargetPageIdPropertyAlias].Value = null;
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
            }
        }

        /// <summary>
        /// When a rating page is saved, relate it to the pages the rating is being applied to
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
                    if (node.ContentType.Alias != "Rating") continue;

                    RemoveExistingRelations(node);

                    AddNewRelations(node);
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
                    // If this is a content page linked to rating page, delete the relation
                    var relationsToRatings = ApplicationContext.Current.Services.RelationService.GetByChildId(node.Id).Where(r => r.RelationType.Alias == RatingRelationType.RelationTypeAlias);
                    foreach (var relation in relationsToRatings)
                    {
                        ApplicationContext.Current.Services.RelationService.Delete(relation);
                    }

                    // If this is an rating page, delete the relation 
                    var relationsFromRatings = ApplicationContext.Current.Services.RelationService.GetByParentId(node.Id).Where(r => r.RelationType.Alias == RatingRelationType.RelationTypeAlias);
                    foreach (var relation in relationsFromRatings)
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
        /// Adds the new relation between a rating page and its content node.
        /// </summary>
        /// <param name="node">The node.</param>
        private static void AddNewRelations(IContent node)
        {
            var multiNodeTreePicker = node.Properties[TargetPageIdPropertyAlias];
            if (multiNodeTreePicker == null) return;

            var treePickerValue = multiNodeTreePicker.Value?.ToString();
            if (String.IsNullOrEmpty(treePickerValue)) return;

            var targetNodeIds = treePickerValue.Split(',');
            var relationType = ApplicationContext.Current.Services.RelationService.GetRelationTypeByAlias(RatingRelationType.RelationTypeAlias);
            foreach (var targetNodeId in targetNodeIds)
            {
                try
                {
                    var targetNode = Int32.Parse(targetNodeId, CultureInfo.InvariantCulture);
                    ApplicationContext.Current.Services.RelationService.Save(new Relation(node.Id, targetNode, relationType));
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
        /// Removes the existing relation between a rating page and its content nodes
        /// </summary>
        /// <param name="node">The node.</param>
        private static void RemoveExistingRelations(IContent node)
        {
            var relations = ApplicationContext.Current.Services.RelationService.GetByParentId(node.Id).Where(r => r.RelationType.Alias == RatingRelationType.RelationTypeAlias);
            foreach (var relation in relations)
            {
                ApplicationContext.Current.Services.RelationService.Delete(relation);
            }
        }

        /// <summary>
        /// Ensures the relation type exists between a rating node and its target content nodes
        /// </summary>
        private static void EnsureRelationTypeExists()
        {
            if (ApplicationContext.Current.Services.RelationService.GetRelationTypeByAlias(RatingRelationType.RelationTypeAlias) == null)
            {
                var relationType = new RatingRelationType();
                ApplicationContext.Current.Services.RelationService.Save(relationType);
            }
        }
    }
}