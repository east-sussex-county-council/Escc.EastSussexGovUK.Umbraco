using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using UmbracoForms = Umbraco.Forms;
using Umbraco.Core;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;
using Umbraco.Forms.Data.Storage;
using System.Globalization;

namespace Escc.EastSussexGovUK.Umbraco.App_Plugins.Escc.UmbracoFormsAdmin
{
    [Tree("forms", "formEntries", "Form entries")]
    [PluginController("UmbracoFormsEntries")]
    public class UmbracoFormsEntriesTreeController : TreeController
    {
        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            // check if we're rendering the root node's children
            if (id == Constants.System.Root.ToInvariantString())
            {
                var tree = new TreeNodeCollection();
                
                 using (FormStorage formStorage = new FormStorage())
                {
                    IEnumerable<UmbracoForms.Core.Form> forms = formStorage.GetForms();
                    foreach (UmbracoForms.Core.Form form in forms)
                    {
                        if (UmbracoForms.Web.FormsSecurity.HasAccessToForm(form.Id))
                        {
                            tree.Add(CreateTreeNode(form.Id.ToString(), id, queryStrings, form.Name, "icon-autofill"));
                        }
                    }
                }

                tree.Sort(new TreeNodeComparer());

                return tree;
            }
            // this tree doesn't support rendering more than 1 level
            throw new NotSupportedException();
        }
        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
            return null;
        }

        private class TreeNodeComparer : IComparer<TreeNode>
        {
            public int Compare(TreeNode x, TreeNode y)
            {
                return StringComparer.Create(CultureInfo.CurrentUICulture, true).Compare(x.Name, y.Name);
            }
        }
    }
}