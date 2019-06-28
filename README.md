# Escc.EastSussexGovUK.Umbraco

This project is the root of our [Umbraco](http://umbraco.com/) installation for [www.eastsussex.gov.uk](https://www.eastsussex.gov.uk). It is tightly coupled with the [Escc.EastSussexGovUK](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK/) project, which contains the template elements shared by this and every other branded part of our website. If a feature is not documented here, it may be because it is implemented in the shared template.

The solution is broken up into several subprojects:

*  `Escc.EastSussexGovUK.Umbraco.Web` is the main Umbraco website. 
*  `Escc.EastSussexGovUK.Umbraco.Api` is a separate copy of Umbraco which hosts the Examine index for jobs data. See [Jobs](Jobs.md). 
*  `Escc.Jobs.UpdateIndexes` updates the Examine indexes for jobs when run as a scheduled task. 
*  `Escc.Jobs.SendAlerts` sends job alerts when run as a scheduled task.
*  `Escc.EastSussexGovUK.Umbraco` contains any code which needs to be shared between two or more of the above projects.

## Configuring the Umbraco application

*  Master and slave roles for Umbraco instances are configured by `RegisterServerRoleEventHandler`. Add the following setting to `web.config` on the back-office site only to ensure that the correct roles are assigned:

		<appSettings>
	      <add key="IsUmbracoBackOffice" value="true" />
		<appSettings>
 
*  [Auditing and debugging](Debugging.md)
*  Configuring redirects and custom error pages is documented in [Redirects and custom errors](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK/blob/master/RedirectsAndCustomErrors.md) in the Escc.EastSussexGovUK project
*  Document types, media types, data types and template definitions are all managed using [uSync snapshots](https://usync.readthedocs.io/)
*  [Branding the login screen](Branding.md)
*  [Customising Umbraco page previews](UmbracoPreview.md)
*  A sitemap of media items is published by [Escc.Umbraco.SiteMap](https://github.com/east-sussex-county-council/Escc.Umbraco.SiteMap) to enable search engines to better index documents uploaded to Umbraco. This means that items uploaded to the media library can be discovered by search engines even if they are not linked from a page.
*  The content and media recycle bins are cleared of old items on a regular basis by [Escc.Umbraco.RecycleBinManager](https://github.com/east-sussex-county-council/Escc.Umbraco.RecycleBinManager)

## Specific templates
*  [Home page](HomePage.md)
*  [Guide](Guide.md)
*  [Jobs](Jobs.md)
*  [Campaigns](Campaigns.md)
*  [Locations, including libraries and recycling sites](Location.md)
*  [Privacy notice](PrivacyNotice.md)
*  [Rights of way - Section 31 deposits and definitive map modifications](RightsOfWay.md)
*  [WebForms templates migrated from Microsoft CMS](MicrosoftCms.md) (includes details of the 'Standard topic page' and 'Map' templates and displaying school term dates)

## Features available on most templates
*  [A/B testing](ABTesting.md)
*  Web analytics support using heatmaps is provided by [Escc.Umbraco.HeatmapAnalytics](https://github.com/east-sussex-county-council/Escc.Umbraco.HeatmapAnalytics)
*  [Skins](Skins.md)
*  [Service alerts](ServiceAlerts.md)
*  [Latest section](Latest.md)
*  [Banners](Banners.md)
*  [Embedding iCasework Forms](ICaseworkForms.md)
*  [Embedding Google Maps](GoogleMaps.md)
*  [Embedding YouTube videos](YouTuve.md)
*  [Making changes to HTML before it's displayed](ChangingHTML.md)
*  [East Sussex 1Space and ESCIS support](1SpaceESCIS.md)
*  [Ratings](Ratings.md) (also known as Customer Thermometer)
*  [Social media support](SocialMedia.md)
*  [Web chat](WebChat.md)
*  Changing the [text size](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK/blob/master/TextSize.md) for the site is documented in the `Escc.EastSussexGovUK` project, but this project implements the API that sets the cookie it requires, using `RegisterTextSizeRoute` to wire up a controller and a view that do not require an Umbraco document type and content node. 
*  Alternative text is required for images by the inclusion of [Escc.Umbraco](https://github.com/east-sussex-county-council/Escc.Umbraco)
*  Validation and automatic formatting of rich text fields is provided by [Escc.Umbraco.PropertyEditors](https://github.com/east-sussex-county-council/Escc.Umbraco.PropertyEditors/)
*  A media folder is maintained for each page, and media usage is tracked, by [Escc.Umbraco.MediaSync](https://github.com/east-sussex-county-council/Escc.Umbraco.MediaSync/)
*  Media is stored in Azure blob storage by [our fork of UmbracoFileSystemProviders.Azure](https://github.com/east-sussex-county-council/UmbracoFileSystemProviders.Azure)
*  Page expiry is controlled and notifications are sent to editors by [Escc.Umbraco.Expiry](https://github.com/east-sussex-county-council/Escc.Umbraco.Expiry)

## Forms
*  [Umbraco Forms](UmbracoForms.md)

## Development setup steps

1.  Create an Azure storage account or get the [Azure storage emulator](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator).

2.  Ensure that [Escc.WebApplicationSetupScripts](https://github.com/east-sussex-county-council/Escc.WebApplicationSetupScripts) and [Escc.EastSussexGovUK](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK) are already set up in folders alongside this repository.

3.  Get the `Web.Debug.config` files for `Escc.EastSussexGovUK.Umbraco.Web` and `Escc.EastSussexGovUK.Umbraco.Api` from our secrets store and place them in the  `Escc.EastSussexGovUK.Umbraco.Web` and `Escc.EastSussexGovUK.Umbraco.Api` folders. If you don't have access to these files, follow the manual steps later. 

4.  From an Administrator command prompt, run `app-setup-dev.cmd` to set up a site in IIS and prepare the Umbraco configuration files.

5.  Build the solution.

6.  Obtain an Umbraco Forms licence file called `umbracoForms.lic` and copy it into `~\Escc.EastSussexGovUK.Umbraco.Web\bin`.

7.  Go to `https://localhost:port/umbraco` in a browser to run the Umbraco installer (where `port` is the one you chose for the web application when you ran `app-setup-dev.cmd`). Use a SQL Server database as you'll need to connect to the same one from the API project. Alternatively, if you already have an database, in `~\Escc.EastSussexGovUK.Umbraco.Web\web.config` set the `umbracoConfigurationStatus` and `umbracoDbDSN` values.

8.  Copy the `umbracoDbDSN` value from `~\Escc.EastSussexGovUK.Umbraco.Web\web.config` to `~\Escc.EastSussexGovUK.Umbraco.Api\web.config`.

9.  If you didn't have `Web.Debug.config` earlier, configure the `system.net/mailSettings` section in `~\Escc.EastSussexGovUK.Umbraco.Web\web.config` with a `from` address and details of your SMTP server.

10.  If you didn't have `Web.Debug.config` earlier, add proxy server and web API credentials to `~\Escc.EastSussexGovUK.Umbraco.Web\web.config` using [Escc.Net](https://github.com/east-sussex-county-council/Escc.Net) (if a page loads slowly and without template elements such as the header and footer, you  need to configure the proxy).

11.  Configure `~\Escc.EastSussexGovUK.Umbraco.Web\config\FileSystemProviders.config` and `~\Escc.EastSussexGovUK.Umbraco.Web\config\imageprocessor\security.config` as described in the documentation for [our fork of UmbracoFileSystemProviders.Azure](https://github.com/east-sussex-county-council/UmbracoFileSystemProviders.Azure/tree/escc). Be sure to read the documentation on the `escc` branch as there are extra fields to configure for Umbraco Forms.

12.  Configure a project in Exceptionless to report errors to, and enter the details in  `~\Escc.EastSussexGovUK.Umbraco.Web\web.config`, `~\Escc.EastSussexGovUK.Umbraco.Api\web.config`, `~\Escc.Jobs.SendAlerts\app.config` and `~\Escc.Jobs.UpdateIndexes\app.config`: 

	`<exceptionless apiKey="API_KEY_HERE" serverUrl="https://hostname" />` 

13.  In the Umbraco back office for the web application project, go to the Developer > uSync BackOffice > Snapshots and click 'Apply all'.

14.  You should now have a working back-office site with no content. Go to the content section and create a home page. 

### Additional steps to set up redirects

1.  Add a SQL Server connection string named `RedirectsReader` in the `connectionStrings` section of `~\Escc.EastSussexGovUK.Umbraco.Web\web.config`.

For more detail see [Escc.Redirects](https://github.com/east-sussex-county-council/Escc.Redirects).


### Additional steps to set up Umbraco Forms

1.  If you didn't have `Web.Debug.config` earlier, add the `SchoolApiUrl` to `~\Escc.EastSussexGovUK.Umbraco.Web\web.config`

For more detail see [Umbraco Forms](UmbracoForms.md).

### Additional steps to set up the jobs pages

1.  Set an Azure storage connection string in the `connectionStrings` section of `~\Escc.EastSussexGovUK.Umbraco.Web\web.config` and `~\Escc.Jobs.SendAlerts\app.config`.

2.  Copy the `system.net/mailSettings` section in `~\Escc.EastSussexGovUK.Umbraco.Web\web.config` to `~\Escc.Jobs.SendAlerts\app.config`.

3.  If you didn't have `Web.Debug.config` earlier, add proxy server credentials to `~\Escc.EastSussexGovUK.Umbraco.Api\web.config` using [Escc.Net](https://github.com/east-sussex-county-council/Escc.Net) to connect to our external jobs provider.

4.  If you didn't have `Web.Debug.config` earlier, add the URLs for our external jobs provider to `~\Escc.EastSussexGovUK.Umbraco.Api\web.config`.

5.  Create the jobs pages in Umbraco using the dedicated document types for jobs. 

For more detail see [Jobs](Jobs.md).

### Additional steps to set up service alerts

1.  Set an Azure storage connection string named `Escc.ServiceClosures.AzureStorage` in the `connectionStrings` section of `~\Escc.EastSussexGovUK.Umbraco.Web\web.config`. This enables support for school closure alerts.

For more detail see [Service Alerts](ServiceAlerts.md). 


### Additional steps to set up the recycling site finder

The recycling site finder can appear on the 'Home page' and 'Standard Topic Page' templates.

1.  If you didn't have `Web.Debug.config` earlier, add `LocateApi*` settings to `~\Escc.EastSussexGovUK.Umbraco.Web\web.config`. 

For more detail see [Escc.RubbishAndRecycling.SiteFinder](https://github.com/east-sussex-county-council/Escc.RubbishAndRecycling.SiteFinder). 
