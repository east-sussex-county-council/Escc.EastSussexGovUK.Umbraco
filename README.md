# Escc.EastSussexGovUK.Umbraco

This project is the root of our [Umbraco](http://umbraco.com/) installation for [www.eastsussex.gov.uk](https://www.eastsussex.gov.uk). It is tightly coupled with the [Escc.EastSussexGovUK](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK/) project, which contains the template elements shared by this and every other branded part of our website. If a feature is not documented here, it may be because it is implemented in the shared template.

The solution is broken up into several subprojects:

*  `Escc.EastSussexGovUK.Umbraco.Web` is the main Umbraco website. 
*  `Escc.EastSussexGovUK.Umbraco.Api` is a separate copy of Umbraco which hosts the Examine index for jobs data. See [Jobs](Jobs.md). 
*  `Escc.Jobs.UpdateIndexes` updates the Examine indexes for jobs when run as a scheduled task. 
*  `Escc.Jobs.SendAlerts` sends job alerts when run as a scheduled task.
*  `Escc.EastSussexGovUK.Umbraco` contains any code which needs to be shared between two or more of the above projects.

## Specific templates
*  [Home page](HomePage.md)
*  [Jobs](Jobs.md)
*  [Locations, including libraries and recycling sites](Location.md)
*  [Old templates migrated from Microsoft CMS](MicrosoftCms.md)

## Features available on most templates
*  [A/B testing](ABTesting.md)
*  [Skins](Skins.md)
*  [Service alerts](ServiceAlerts.md)
*  [Latest section](Latest.md)
*  [Banners](Banners.md)
*  [Embedding iCasework Forms](ICaseworkForms.md)
*  [Embedding Google Maps](GoogleMaps.md)
*  [Embedding YouTube videos](YouTuve.md)
*  [Making changes to HTML before it's displayed](ChangingHTML.md)
*  [East Sussex 1Space and ESCIS support](1SpaceESCIS.md)
*  [Social media support](SocialMedia.md)
*  [Web chat](WebChat.md)

## Forms
*  [Umbraco Forms](UmbracoForms.md)

## Development setup steps

1. From an Administrator command prompt, run `app-setup-dev.cmd` to set up a site in IIS.
2. Build the solution
3. Grant modify permissions to the application pool account on the web root folder and all children
4. Copy `packages\Umbraco*\Content\config\*.config` into `~\config`
6. In `~\web.config` set the `UmbracoConfigurationStatus` and `umbracoDbDSN`, or run the Umbraco installer.
8. In `~\web.config` add the contents of `web.config.xdt`
7. In `~\web.config` uncomment and complete the `Proxy` and `RemoteMasterPage` sections
8. In the Umbraco back office, go to the Developer > uSync BackOffice > Snapshots and click 'Apply all'
9. Create an IIS virtual directory called `~/img/` which points to the `img` folder of the [Escc.EastSussexGovUK.TemplateSource](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK) project.