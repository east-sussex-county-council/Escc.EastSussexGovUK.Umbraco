# Escc.EastSussexGovUK.Umbraco

This project is the root of our Umbraco installation for www.eastsussex.gov.uk. It contains little code or configuration itself, instead acting as a shell which pulls in all of its templates and features as NuGet packages.

## Development setup steps

1. From an Administrator command prompt, run `app-setup-dev.cmd` to set up a site in IIS.
2. Build the solution
3. Grant modify permissions to the application pool account on the web root folder and all children
4. Copy `packages\Umbraco*\Content\config\*.config` into `~\config`
5. Drop and re-add the following NuGet packages, which makes them add their settings to `web.config` and `clientdependency.config`.
	* Escc.Alerts.Website
	* Escc.CustomerFocusTemplates.Website
	* Escc.CoreLegacyTemplates.Website
	* Escc.EastSussexGovUK.UmbracoViews
	* Escc.ClientDependencyFramework.Umbraco
6. In `~\web.config` set the `UmbracoConfigurationStatus` and `umbracoDbDSN`, or run the Umbraco installer.
7. In `~\web.config` uncomment and complete the `Proxy` and `RemoteMasterPage` sections
8. In `~\web.config` update the `bindingRedirect` for Exceptionless to:

		<bindingRedirect oldVersion="0.0.0.0-1.5.2092.0" newVersion="1.5.2121.0" />

	You may need to copy the Exceptionless dll files from the `~\packages\Exceptionless*.1.5.2121.0\*` to `~\bin` too.

Microsoft.ApplicationBlocks.Data
--------------------------------
The reference from this project to Microsoft.ApplicationBlocks.Data.dll exists to resolve a conflict between the version used by Umbraco and the version used by ESCC. We cannot use assembly binding redirects to resolve this conflict because the Umbraco version does not have a strong name but the ESCC one does, therefore they have different identities even though they share the same name. 

We reference the Umbraco version so that it ends up in the bin folder, knowing that the ESCC code which uses the newer version of Microsoft.ApplicationBlocks.Data will not be called within this application scope.