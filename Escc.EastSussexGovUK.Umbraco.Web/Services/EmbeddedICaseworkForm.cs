using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Web.Services
{
    /// <summary>
    /// Turn links to iCaseWork forms into embedded forms
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.IClientDependencySet" />
    public class EmbeddedICaseworkForm : IClientDependencySet
    {
        /// <summary>
        /// HTML from fields which might contain an embedded Google Maps link
        /// </summary>
        public IEnumerable<string> Html { get; set; }

        /// <summary>
        /// Determines whether the dependency is required
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the dependency is required; <c>false</c> otherwise
        /// </returns>
        public bool IsRequired()
        {
            if (Html == null) return false;
            return Html.Any(htmlString => Regex.IsMatch(htmlString, @"\.icasework\.com/(form|cases)", RegexOptions.IgnoreCase));
        }

        /// <summary>
        /// The content security policy aliases required for the dependent feature. These are registered in web.config using <see cref="!:ContentSecurityPolicy" />.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContentSecurityPolicyDependency> RequiresContentSecurityPolicy()
        {
            return new ContentSecurityPolicyDependency[1]
            {
                new ContentSecurityPolicyDependency(){ Alias = "iCaseWork" }
            };
        }

        /// <summary>
        /// The CSS which is required for the dependent feature. These are registered in web.config using <see cref="!:Escc.ClientDependencyFramework" />.
        /// </summary>
        /// <returns>
        /// A set of CSS file aliases, optionally qualified by media query aliases
        /// </returns>
        public IEnumerable<CssFileDependency> RequiresCss()
        {
            return new CssFileDependency[1]
            {
                new CssFileDependency() { CssFileAlias = "iCaseWork" }
            };
        }

        /// <summary>
        /// The JavaScript which is required for the dependent feature. These are registered in web.config using <see cref="!:Escc.ClientDependencyFramework" />.
        /// </summary>
        /// <returns>
        /// A set of JS file aliases, qualified by a priority value which defaults to 100
        /// </returns>
        public IEnumerable<JsFileDependency> RequiresJavaScript()
        {
            return new JsFileDependency[2]
            {
                new JsFileDependency() { JsFileAlias = "iFrameResizer", Priority = 10 },
                new JsFileDependency() { JsFileAlias = "iCaseWork" }
            };
        }
    }
}