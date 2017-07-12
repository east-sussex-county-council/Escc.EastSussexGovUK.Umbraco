﻿using System;
using Escc.EastSussexGovUK.Umbraco.ContentFinders;
using Exceptionless;
using Umbraco.Core;
using Umbraco.Web.Routing;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Wire up <see cref="JobAdvertContentFinder"/> and <see cref="JobAlertContentFinder"/> so that job pages can have search-engine-friendly URLs
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public class JobsEventHandler : ApplicationEventHandler
    {
        /// <summary>
        /// OVerridable method to execute when All resolvers have been initialized but resolution is not frozen so they can be modified in this method
        /// </summary>
        /// <param name="umbracoApplication"></param>
        /// <param name="applicationContext"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            try
            {
                 // Insert my finders before the default Umbraco content finder
                ContentFinderResolver.Current.InsertTypeBefore<ContentFinderByNiceUrl, JobAdvertContentFinder>();
                ContentFinderResolver.Current.InsertTypeBefore<ContentFinderByNiceUrl, JobAlertContentFinder>();
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
            }
           
        }
    }
}