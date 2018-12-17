using System;
using Exceptionless;
using Umbraco.Core;
using Umbraco.Web.Routing;

namespace Escc.EastSussexGovUK.Umbraco.Web.Guide
{
    /// <summary>
    /// Wire up <see cref="GuidePrintViewContentFinder"/> so that the Guide print view works
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public class GuidePrintViewEventHandler : ApplicationEventHandler
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
                 // Insert my finder before the default Umbraco content finder
                ContentFinderResolver.Current.InsertTypeBefore<ContentFinderByNiceUrl, GuidePrintViewContentFinder>();
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
            }
           
        }
    }
}