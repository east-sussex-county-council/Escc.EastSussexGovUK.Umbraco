using Exceptionless;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Forms.Core;
using Umbraco.Forms.Data.Storage;
using Umbraco.Web.WebApi;
using Umbraco.Core.Models.Membership;
using System.Web.Http;

namespace Escc.EastSussexGovUK.Umbraco.Forms
{
    [Authorize]
    public class UmbracoFormsSecurityApiController : UmbracoApiController
    {
        /// <summary>
        /// Removes all Umbraco Forms security settings, which leaves all users with default access to every form and Manage Forms permission.
        /// </summary>
        /// <remarks>Typically used only for testing.</remarks>
        [HttpPost]
        public void ResetFormsSecurity()
        {
            try
            {
                var formsSecurity = new UmbracoFormsSecurity();
                formsSecurity.ResetFormsSecurity(Services.UserService);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
            }
        }


        /// <summary>
        /// Locks down Umbraco Forms security by creating a 'deny' record for every user and every form.
        /// </summary>
        /// <remarks>Typically used only for testing, or as a clean start when installing Umbraco Forms.</remarks>
        [HttpPost]
        public void LockdownFormsSecurity()
        {
            try
            {
                var formsSecurity = new UmbracoFormsSecurity();
                formsSecurity.LockdownFormsSecurity(Services.UserService);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
            }
        }

        /// <summary>
        /// Where a user or a form has no security settings in Umbraco Forms, add a 'deny all' record.
        /// </summary>
        /// <remarks>
        /// Where a user or a form has no security settings in Umbraco Forms, they are allowed access by default.
        /// This happens for new users and new forms. Since there is no event to watch for new users or new forms,
        /// run this method frequently to check for missing permissions and create a 'deny all' record. Where 
        /// permissions have already been set they are left unchanged.
        /// </remarks>
        [HttpPost]
        public void FixFormsSecurity()
        {
            try
            {
                var formsSecurity = new UmbracoFormsSecurity();
                formsSecurity.FixFormsSecurity(Services.UserService);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
            }

        }
    }

}
