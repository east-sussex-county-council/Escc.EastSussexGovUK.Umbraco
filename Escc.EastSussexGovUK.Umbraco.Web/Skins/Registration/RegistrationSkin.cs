using Escc.EastSussexGovUK;
using Escc.EastSussexGovUK.Skins;
using Escc.EastSussexGovUK.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Web.Skins.Registration
{
    /// <summary>
    /// A skin for registration service pages
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Skins.CustomerFocusSkin" />
    public class RegistrationSkin : CustomerFocusSkin
    {
        private readonly Uri _requestUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationSkin"/> class.
        /// </summary>
        /// <param name="requestUrl">The request URL.</param>
        /// <exception cref="System.ArgumentException">requestUrl must be an absolute URL</exception>
        public RegistrationSkin(Uri requestUrl) : base()
        {
            if (!requestUrl.IsAbsoluteUri) throw new ArgumentException("requestUrl must be an absolute URL");
            _requestUrl = requestUrl;
        }

        /// <summary>
        /// Determines whether the skin should be applied
        /// </summary>
        /// <returns>
        /// <c>true</c> if the skin should be applied; <c>false</c> otherwise
        /// </returns>
        public override bool IsRequired()
        {
            if (_requestUrl != null)
            {
                return _requestUrl.AbsolutePath.StartsWith("/community/registration", StringComparison.OrdinalIgnoreCase);
            }
            else return true;
        }

        /// <summary>
        /// The CSS which is required for the skin. These are registered in web.config using <see cref="Escc.ClientDependencyFramework" />.
        /// </summary>
        /// <returns>
        /// A set of CSS file aliases, optionally qualified by media query aliases
        /// </returns>
        public override IEnumerable<CssFileDependency> RequiresCss()
        {
            return new List<CssFileDependency>(base.RequiresCss())
            {
                new CssFileDependency() {CssFileAlias = "RegistrationSkinSmall"},
                new CssFileDependency() { CssFileAlias = "RegistrationSkinMedium", MediaQueryAlias = "Medium"},
                new CssFileDependency() { CssFileAlias = "RegistrationSkinLarge", MediaQueryAlias = "Large"}
            };
        }

        /// <summary>
        /// The JavaScript which is required for the skin. These are registered in web.config using <see cref="Escc.ClientDependencyFramework" />.
        /// </summary>
        /// <returns>
        /// A set of JS file aliases, qualified by a priority value which defaults to 100
        /// </returns>
        public override IEnumerable<JsFileDependency> RequiresJavaScript()
        {
            return new List<JsFileDependency>(base.RequiresJavaScript())
            {
                new JsFileDependency() {JsFileAlias = "RegistrationSkin"}
            };
        }
    }
}