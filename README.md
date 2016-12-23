# Escc.EastSussexGovUK.Umbraco

This project is the root of our Umbraco installation for [www.eastsussex.gov.uk](https://www.eastsussex.gov.uk). 

The project contains the common [Umbraco](http://umbraco.com/) templates used by new sections of [www.eastsussex.gov.uk](https://www.eastsussex.gov.uk). The template design builds on the work of the [UK Government Digital Service](https://gds.blog.gov.uk/).

## Location data API

A location API makes available data from the Location template defined in this project. The API returns a list of locations filtered by the location type:

	https://hostname/umbraco/api/location/list?type=Library&type=MobileLibraryStop

This is designed for the 'Find a library' and 'Find a recycling site' features. You may call this API for other purposes (for example, you could map all the mobile library stops), but it is regarded as an internal API which may be changed without notice. If you require this data with more guarantees, please contact us to discuss setting up a more formal API. 

## Adding term dates to a topic page

1. Upload the term dates data to the Umbraco media library.
2. On a topic page, select 'Term dates' as a section layout
3. In the same section, select the term dates data in the 'Section [number]: image 1' field. All other fields in the section are ignored.

If no term dates data is selected, the term dates section is left blank. If the wrong kind of file is selected an `XmlException` is thrown with the message "Invalid character in the given encoding. Line 1, position 1."

## Development setup steps

1. From an Administrator command prompt, run `app-setup-dev.cmd` to set up a site in IIS.
2. Build the solution
3. Grant modify permissions to the application pool account on the web root folder and all children
4. Copy `packages\Umbraco*\Content\config\*.config` into `~\config`
6. In `~\web.config` set the `UmbracoConfigurationStatus` and `umbracoDbDSN`, or run the Umbraco installer.
8. In `~\web.config` add the contents of `web.config.xdt`
7. In `~\web.config` uncomment and complete the `Proxy` and `RemoteMasterPage` sections
8. At a command line, run the following two commands to add the document types to Umbraco. Substitute the hostname and port where you set up this project, and ensure the token matches the `Escc.Umbraco.Inception.AuthToken` value in the `appSettings` section of `web.config`.

		curl --insecure -X POST -d "" https://hostname:port/umbraco/api/UmbracoSetupApi/CreateUmbracoSupportingTypes?token=dev
		curl --insecure -X POST -d "" https://hostname:port/umbraco/api/UmbracoSetupApi/CreateUmbracoDocumentTypes?token=dev