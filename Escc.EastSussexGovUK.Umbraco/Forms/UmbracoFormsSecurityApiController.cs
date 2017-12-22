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
                // For every Umbraco User including disabled accounts, remove their Umbraco Forms permissions (both deny and allow).
                // This actually grants everyone Manage Forms permission because the default is to allow everyone.
                var page = 0;
                var total = 0;
                var users = Services.UserService.GetAll(page, 10, out total);
                while (users.Any())
                {
                    foreach (var user in users)
                    {
                        using (UserSecurityStorage userSecurityStorage = new UserSecurityStorage())
                        {
                            var userFormSecurityList = userSecurityStorage.GetUserSecurity(user.Id.ToString());
                            foreach (var userSecurity in userFormSecurityList) userSecurityStorage.DeleteUserSecurity(userSecurity);
                        }
                    }

                    page++;
                    users = Services.UserService.GetAll(page, 10, out total);
                }

                // For every form in Umbraco Forms, remove all the user permissions (both deny and allow).
                // This actually grants everyone access to every form because the default is to allow everyone.
                using (FormStorage formStorage = new FormStorage())
                {
                    using (UserFormSecurityStorage formSecurityStorage = new UserFormSecurityStorage())
                    {
                        IEnumerable<Form> allForms = formStorage.GetAllForms();
                        foreach (Form form in allForms)
                        {
                            formSecurityStorage.DeleteAllUserFormSecurityForForm(form.Id);
                        }
                    }
                }
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
                var page = 0;
                var total = 0;
                var users = Services.UserService.GetAll(page, 10, out total);
                while (users.Any())
                {
                    foreach (var user in users)
                    {
                        RemoveManageFormsPermissions(user, true);
                        RemoveDefaultAccessToForms(user, true);
                    }

                    page++;
                    users = Services.UserService.GetAll(page, 10, out total);
                }

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
                var page = 0;
                var total = 0;
                var users = Services.UserService.GetAll(page, 10, out total);
                while (users.Any())
                {
                    foreach (var user in users)
                    {
                        RemoveManageFormsPermissions(user, false);
                        RemoveDefaultAccessToForms(user, false);
                    }

                    page++;
                    users = Services.UserService.GetAll(page, 10, out total);
                }
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
            }

        }

        /// <summary>
        /// Each Umbraco User should have an Umbraco Forms permissions record which holds their overall permissions for Umbraco Forms.
        /// This preserves existing permissions and adds a 'deny all' permission if there is no record.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="forEveryone">if set to <c>true</c> overwrite all existing permissions with 'deny all'.</param>
        private void RemoveManageFormsPermissions(IUser user, bool forEveryone)
        {
            using (UserSecurityStorage userSecurityStorage = new UserSecurityStorage())
            {
                var userSecurity = userSecurityStorage.GetUserSecurity(user.Id.ToString()).FirstOrDefault();
                var hasSecurityAlready = (userSecurity != null);
                if (!hasSecurityAlready)
                {
                    userSecurity = UserSecurity.Create();
                    userSecurity.User = user.Id.ToString();
                }
                userSecurity.ManageForms = false;
                userSecurity.ManageDataSources = false;
                userSecurity.ManagePreValueSources = false;
                userSecurity.ManageWorkflows = false;

                if (!hasSecurityAlready)
                {
                    userSecurityStorage.InsertUserSecurity(userSecurity);
                }
                else if (forEveryone)
                {
                    userSecurityStorage.UpdateUserSecurity(userSecurity);
                }
            }
        }

        /// <summary>
        /// Each Umbraco User should have an Umbraco Forms permissions record for each form.
        /// This preserves existing permissions and adds a 'deny' permission if there is no record.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="forEveryone">if set to <c>true</c> overwrite all existing permissions with 'deny'.</param>
        private void RemoveDefaultAccessToForms(IUser user, bool forEveryone)
        {
            using (FormStorage formStorage = new FormStorage())
            {
                using (UserFormSecurityStorage formSecurityStorage = new UserFormSecurityStorage())
                {
                    IEnumerable<Form> allForms = formStorage.GetAllForms();
                    foreach (Form form in allForms)
                    {
                        var formSecurityForUser = formSecurityStorage.GetUserFormSecurity(user.Id, form.Id).FirstOrDefault();
                        var hasSecurityAlready = (formSecurityForUser != null);
                        if (!hasSecurityAlready)
                        {
                            formSecurityForUser = UserFormSecurity.Create();
                            formSecurityForUser.User = user.Id.ToString();
                            formSecurityForUser.Form = form.Id;
                        }
                        formSecurityForUser.HasAccess = false;

                        if (!hasSecurityAlready)
                        {
                            formSecurityStorage.InsertUserFormSecurity(formSecurityForUser);
                        }
                        else if (forEveryone)
                        {
                            formSecurityStorage.UpdateUserFormSecurity(formSecurityForUser);
                        }
                    }
                }
            }
        }
    }

}
