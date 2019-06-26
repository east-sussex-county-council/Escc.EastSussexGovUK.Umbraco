# Escc.EastSussexGovUK.Umbraco

This project is the root of our [Umbraco](http://umbraco.com/) installation for [www.eastsussex.gov.uk](https://www.eastsussex.gov.uk). It is tightly coupled with the [Escc.EastSussexGovUK](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK/) project, which contains the template elements shared by this and every other branded part of our website. If a feature is not documented here, it may be because it is implemented in the shared template.

The solution is broken up into several subprojects:

*  `Escc.EastSussexGovUK.Umbraco.Web` is the main Umbraco website. 
*  `Escc.EastSussexGovUK.Umbraco.Api` is a separate copy of Umbraco which hosts the Examine index for jobs data. See [Jobs](Jobs.md). 
*  `Escc.Jobs.UpdateIndexes` updates the Examine indexes for jobs when run as a scheduled task. 
*  `Escc.Jobs.SendAlerts` sends job alerts when run as a scheduled task.
*  `Escc.EastSussexGovUK.Umbraco` contains any code which needs to be shared between two or more of the above projects.

## Configuring the Umbraco application

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

1. Ensure that [Escc.WebApplicationSetupScripts](https://github.com/east-sussex-county-council/Escc.WebApplicationSetupScripts) and [Escc.EastSussexGovUK](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK) are already set up in folders alongside this repository. 
2. From an Administrator command prompt, run `app-setup-dev.cmd` to set up a site in IIS and prepare the Umbraco configuration files.
3. Build the solution
4. Obtain an Umbraco Forms licence file called `umbracoForms.lic` and copy it into both `~\Escc.EastSussexGovUK.Umbraco.Web\bin` and `~\Escc.EastSussexGovUK.Umbraco.Api\bin`
5. Go to `https://localhost:port/umbraco` in a browser to run the Umbraco installer (where `port` is the one you chose for the web application when you ran `app-setup-dev.cmd`). Alternatively, if you already have an database, in both `~\Escc.EastSussexGovUK.Umbraco.Web\web.config` and `~\Escc.EastSussexGovUK.Umbraco.Api\web.config` set the `umbracoConfigurationStatus` and `umbracoDbDSN` values.
6. In the Umbraco back office, go to the Developer > uSync BackOffice > Snapshots and click 'Apply all'
7. In `~\Escc.EastSussexGovUK.Umbraco.Web\web.config` add the `Proxy` and `RemoteMasterPage` sections
