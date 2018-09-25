using System;
using System.Globalization;
using Escc.EastSussexGovUK.Umbraco.MicrosoftCmsMigration;
using Escc.Html;
using Exceptionless;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Escc.EastSussexGovUK.Umbraco.Landing
{
    /// <summary>
    /// Event handler to register actions for the Standard Landing Page template
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public class StandardLandingPageEventHandler : ApplicationEventHandler
    {
        /// <summary>
        /// Overridable method to execute when Bootup is completed, this allows you to perform any other bootup logic required for the application.
        ///             Resolution is frozen so now they can be used to resolve instances.
        /// </summary>
        /// <param name="umbracoApplication"/><param name="applicationContext"/>
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ContentService.Saving += ContentService_Saving;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void ContentService_Saving(IContentService sender, SaveEventArgs<IContent> args)
        {
            try
            {
                foreach (var entity in args.SavedEntities)
                {
                    if (entity.ContentType.Alias != "standardLandingPage") continue;

                    var descriptionsProperty = entity.Properties["descriptions"];
                    if (descriptionsProperty == null) continue;

                    string descriptionsValue = null;
                    if (descriptionsProperty.Value != null && !String.IsNullOrWhiteSpace(descriptionsProperty.Value.ToString()))
                    {
                        descriptionsValue = umbraco.library.GetPreValueAsString(Int32.Parse(descriptionsProperty.Value.ToString(), CultureInfo.InvariantCulture));
                    }

                    var useLinks = Views.StandardLandingPage.DisplayAsListsOfLinks(descriptionsValue, entity.Level);
                    for (var i = 1; i <= 15; i++)
                    {
                        var propertyAlias = "defDesc" + i.ToString("00", CultureInfo.InvariantCulture) + "_Content";
                        var html = entity.GetValue<string>(propertyAlias);

                        if (html != null)
                        {
                            if (useLinks)
                            {
                                // Strip text outside links, and put links into a list
                                html = CmsUtilities.ShouldBeUnorderedList(html);
                            }
                            else
                            {
                                // Otherwise just strip any HTML from the placeholder, which will be there if pasted from Word - it should be a text description
                                html = new HtmlTagSantiser().StripTags(html);
                            }

                            entity.SetValue(propertyAlias, html);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
            }
        }
    }
}