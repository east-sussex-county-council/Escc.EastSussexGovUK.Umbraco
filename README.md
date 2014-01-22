Escc.EastSussexGovUK.Umbraco
============================

Core template files for configuring Umbraco v7 to work as the Content Management System for www.eastsussex.gov.uk

Remote template for www.eastsussex.gov.uk
-----------------------------------------

To configure the remote template you need to copy settings from `web.config.example` to `web.config`, changing the
values to ones appropriate for your environment. 

You need to create a folder to store the cached template files. Grant the IIS process (eg `IIS_IUSRS`) modify 
access on the folder, and add the path in `web.config` as shown in `web.config.example`.

The remote template also requires the following DLLs in the bin folder:

EsccWebTeam.Cms.dll
EsccWebTeam.Data.ActiveDirectory.dll
EsccWebTeam.Data.Web.dll
EsccWebTeam.Data.Xml.dll
EsccWebTeam.EastSussexGovUK.dll

You can then add the remote template to an MVC Umbraco template by creating an Umbraco macro for each `.ascx` file in the 
`UserControls/EastSussexGovUK` folder, then adding the macros to your Umbraco template.