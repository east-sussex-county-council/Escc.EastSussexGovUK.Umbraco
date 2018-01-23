using Exceptionless;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Services
{
    /// <summary>
    /// Log changes to content and media so that we can track down what happened when something goes wrong
    /// </summary>
    /// <seealso cref="Umbraco.Core.ApplicationEventHandler" />
    public class AuditLoggingEventHandler : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            try
            {
                ContentService.Published += Log_ContentPublished;
                ContentService.UnPublished += Log_ContentUnpublished;
                ContentService.Copied += Log_ContentCopied;
                ContentService.Moved += Log_ContentMoved;
                ContentService.Trashed += Log_ContentTrashed;
                ContentService.Deleted += Log_ContentDeleted;

                MediaService.Created += Log_MediaCreated;
                MediaService.Moved += Log_MediaMoved;
                MediaService.Trashed += Log_MediaTrashed;
                MediaService.Deleted += Log_MediaDeleted;
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
            }
        }

        private void Log_MediaCreated(IMediaService sender, global::Umbraco.Core.Events.NewEventArgs<global::Umbraco.Core.Models.IMedia> e)
        {
            LogAction(e.Entity, "created");
        }

        private void Log_MediaMoved(IMediaService sender, global::Umbraco.Core.Events.MoveEventArgs<global::Umbraco.Core.Models.IMedia> e)
        {
            foreach (var moveInfo in e.MoveInfoCollection)
            {
                LogAction(moveInfo.Entity, "moved");
            }
        }

        private void Log_MediaTrashed(IMediaService sender, global::Umbraco.Core.Events.MoveEventArgs<global::Umbraco.Core.Models.IMedia> e)
        {
            foreach (var moveInfo in e.MoveInfoCollection)
            {
                LogAction(moveInfo.Entity, "moved to the Recycle Bin");
            }
        }

        private void Log_MediaDeleted(IMediaService sender, global::Umbraco.Core.Events.DeleteEventArgs<global::Umbraco.Core.Models.IMedia> e)
        {
            foreach (var entity in e.DeletedEntities)
            {
                LogAction(entity, "deleted");
            }
        }

        private void Log_ContentUnpublished(global::Umbraco.Core.Publishing.IPublishingStrategy sender, global::Umbraco.Core.Events.PublishEventArgs<global::Umbraco.Core.Models.IContent> e)
        {
            foreach (var entity in e.PublishedEntities)
            {
                LogAction(entity, "unpublished");
            }
        }

        private void Log_ContentPublished(global::Umbraco.Core.Publishing.IPublishingStrategy sender, global::Umbraco.Core.Events.PublishEventArgs<global::Umbraco.Core.Models.IContent> e)
        {
            foreach (var entity in e.PublishedEntities)
            {
                LogAction(entity, "published");
            }
        }

        private void Log_ContentCopied(IContentService sender, global::Umbraco.Core.Events.CopyEventArgs<global::Umbraco.Core.Models.IContent> e)
        {
            LogAction(e.Original, "copied");
        }

        private void Log_ContentMoved(IContentService sender, global::Umbraco.Core.Events.MoveEventArgs<global::Umbraco.Core.Models.IContent> e)
        {
            foreach (var moveInfo in e.MoveInfoCollection)
            {
                LogAction(moveInfo.Entity, "moved");
            }
        }

        private void Log_ContentTrashed(IContentService sender, global::Umbraco.Core.Events.MoveEventArgs<global::Umbraco.Core.Models.IContent> e)
        {
            foreach (var moveInfo in e.MoveInfoCollection)
            {
                LogAction(moveInfo.Entity, "moved to the Recycle Bin");
            }
        }

        private void Log_ContentDeleted(IContentService sender, global::Umbraco.Core.Events.DeleteEventArgs<global::Umbraco.Core.Models.IContent> e)
        {
            foreach (var entity in e.DeletedEntities)
            {
                LogAction(entity, "deleted");
            }
        }

        private static void LogAction(global::Umbraco.Core.Models.IContent entity, string actionTaken)
        {
            var user = UmbracoContext.Current.Security.CurrentUser;
            LogHelper.Info<AuditLoggingEventHandler>($"Content '{entity.Name}' with Id '{entity.Id}' has been {actionTaken} by user '{user.Name}' with Id '{user.Id}'");
        }

        private static void LogAction(global::Umbraco.Core.Models.IMedia entity, string actionTaken)
        {
            var user = UmbracoContext.Current.Security.CurrentUser;
            LogHelper.Info<AuditLoggingEventHandler>($"Media '{entity.Name}' with Id '{entity.Id}' has been {actionTaken} by user '{user.Name}' with Id '{user.Id}'");
        }
    }
}