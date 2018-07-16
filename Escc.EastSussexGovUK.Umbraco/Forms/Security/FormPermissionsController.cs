using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace Escc.EastSussexGovUK.Umbraco.Forms.Security
{
    public class FormPermissionsController : UmbracoAuthorizedController
    {
        // GET: Permissions
        public ActionResult PermissionsForForm(Guid formId)
        {
            var formsSecurity = new UmbracoFormsSecurity();
            var form = formsSecurity.PermissionsToForm(Services.UserService, formId);

            return View("~/Views/Partials/Forms/Permissions.cshtml", form);
        }
    }
}