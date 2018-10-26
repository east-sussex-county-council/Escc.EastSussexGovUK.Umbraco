﻿using Escc.EastSussexGovUK.Skins;
using Escc.EastSussexGovUK.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Skins.RegistrationAcademy
{
    /// <summary>
    /// A website skin for the Registration Academy pages
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Skins.CustomerFocusSkin" />
    /// <seealso cref="Escc.EastSussexGovUK.IClientDependencySet" />
    public class RegistrationAcademySkin : CustomerFocusSkin, IClientDependencySet
    {
        private bool _isRequired;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationAcademySkin"/> class.
        /// </summary>
        /// <param name="applySkin">if set to <c>true</c> [apply skin].</param>
        public RegistrationAcademySkin(bool applySkin = true) : base()
        {
            _isRequired = applySkin;
        }

        /// <summary>
        /// Determines whether the skin should be applied
        /// </summary>
        /// <returns>
        /// <c>true</c> if the skin should be applied; <c>false</c> otherwise
        /// </returns>
        public override bool IsRequired()
        {
            return _isRequired;
        }

        /// <summary>
        /// The content security policy aliases required for the skin. These are registered in web.config using <see cref="!:ContentSecurityPolicy" />.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<ContentSecurityPolicyDependency> RequiresContentSecurityPolicy()
        {
            return base.RequiresContentSecurityPolicy();
        }

        /// <summary>
        /// The CSS which is required for the skin. These are registered in web.config using <see cref="!:Escc.ClientDependencyFramework" />.
        /// </summary>
        /// <returns>
        /// A set of CSS file aliases, optionally qualified by media query aliases
        /// </returns>
        public override IEnumerable<CssFileDependency> RequiresCss()
        {
            return new List<CssFileDependency>(base.RequiresCss())
            {
                new CssFileDependency() {CssFileAlias = "RegistrationAcademySkinSmall"},
                new CssFileDependency() { CssFileAlias = "RegistrationAcademySkinMedium", MediaQueryAlias = "Medium"},
                new CssFileDependency() { CssFileAlias = "RegistrationAcademySkinLarge", MediaQueryAlias = "Large"}
            };
        }

        /// <summary>
        /// The JavaScript which is required for the skin. These are registered in web.config using <see cref="!:Escc.ClientDependencyFramework" />.
        /// </summary>
        /// <returns>
        /// A set of JS file aliases, qualified by a priority value which defaults to 100
        /// </returns>
        public override IEnumerable<JsFileDependency> RequiresJavaScript()
        {
            return new List<JsFileDependency>(base.RequiresJavaScript())
            {
                new JsFileDependency() {JsFileAlias = "RegistrationAcademySkin"}
            };
        }
    }
}