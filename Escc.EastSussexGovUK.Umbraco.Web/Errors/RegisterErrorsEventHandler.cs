using System;
using Exceptionless;
using Umbraco.Core;
using Umbraco.Web.Routing;

namespace Escc.EastSussexGovUK.Umbraco.Web.Errors
{
    /// <summary>
    /// Register a last-chance content finder to handle 404s with the Umbraco application scope
    /// </summary>
    public class RegisterErrorsEventHandler : ApplicationEventHandler
    {
        /// <summary>
        /// Overidable method to execute when all resolvers have been initialized but resolution is not frozen so they can be modified in this method
        /// </summary>
        /// <param name="umbracoApplication"></param>
        /// <param name="applicationContext"></param>
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            try
            {
                ContentLastChanceFinderResolver.Current.SetFinder(new Umbraco.Web.Errors.NotFoundContentFinder());
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
            }
        }
    }
}