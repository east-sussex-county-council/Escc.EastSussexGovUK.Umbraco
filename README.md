Escc.EastSussexGovUK.Umbraco
============================

Core template files for configuring Umbraco v7 to work as the Content Management System for www.eastsussex.gov.uk

Microsoft.ApplicationBlocks.Data
--------------------------------
The reference from this project to Microsoft.ApplicationBlocks.Data.dll exists to resolve a conflict between the version used by Umbraco and the version used by ESCC. We cannot use assembly binding redirects to resolve this conflict because the Umbraco version does not have a strong name but the ESCC one does, therefore they have different identities even though they share the same name. 

We reference the Umbraco version so that it ends up in the bin folder, knowing that the ESCC code which uses the newer version of Microsoft.ApplicationBlocks.Data will not be called within this application scope.

Remote template for www.eastsussex.gov.uk
-----------------------------------------

To configure the remote template you need to copy settings from `web.config.example` to `web.config`, changing the
values to ones appropriate for your environment. 

The remote template also requires the following DLLs in the bin folder:

* EsccWebTeam.Cms.dll
* EsccWebTeam.Data.ActiveDirectory.dll
* EsccWebTeam.Data.Web.dll
* EsccWebTeam.Data.Xml.dll
* EsccWebTeam.EastSussexGovUK.dll

You can then add the remote template to an MVC layout using the following snippet and altering `ControlName` as required:

```
@Html.Partial("~/Usercontrols/EastSussexGovUK/MasterPageControl.ascx", new MasterPageControlData { Control = "ControlName" })
```

Mobile and desktop views are set up by default in this project.

### Switching between desktop and mobile views

Create a `masterpages` virtual directory under the Umbraco root, pointing to the `masterpages` folder in the `EsccwebTeam.EastSussexGovUK` project. This allows the link to `/masterpages/choose.ashx` to work. It also allows WebForms pages in child projects to find the master pages.

Hosted template for www.eastsussex.gov.uk
-----------------------------------------

It's possible to move from the remote template to the hosted template in several steps. A minimum requirement for using the hosted template is that up-to-date school closure data is available locally. 

### Load usercontrols from a local folder

1. Ensure that a `masterpages` virtual directory exists under the Umbraco root, pointing to the `masterpages` folder in the `EsccwebTeam.EastSussexGovUK` project. 

2. Ensure that the `BaseUrl` is set in the `EastSussexGovUK\GeneralSettings` section:
```
<add key="BaseUrl" value="https://www.eastsussex.gov.uk" />
```
 
3. Set CSS and JavaScript to be loaded remotely by configuring the `HandlerPath` settings as shown in `web.config.example`.

4. School closures will load data locally. Configure the file path in the `applicationSettings` section as shown in `web.config.example` and ensure the XML data is present.

5. Finally, remove the `EastSussexGovUK.RemoteMasterPage` section from `web.config` to load usercontrols locally from the `masterpages` folder.


### Use local links and resources

Remove the `BaseUrl` setting from the `EastSussexGovUK\GeneralSettings` section. All the links in the header and footer, including the search form, now point to local URLs. Ensure either that those URLs are present and working, or redirected to www.eastsussex.gov.uk.
 
1. Create an `img` virtual directory under the Umbraco root, pointing to the `img` folder in the `EsccwebTeam.EastSussexGovUK` project.

2. To enable the text size feature, set up the `Escc.Help.Website` project as an IIS application under the Umbraco root. Its `web.config` will need to remove the Umbraco HTTP modules and role provider.

### Use local CSS and JavaScript

Change the `HandlerPath` settings for both CSS and JavaScript in `web.config` to point to local URLs. Ensure that they correctly load the relevant content.