# Escc.EastSussexGovUK.Umbraco

This project is the root of our Umbraco installation for [www.eastsussex.gov.uk](https://www.eastsussex.gov.uk). 

The project contains the common [Umbraco](http://umbraco.com/) templates for www.eastsussex.gov.uk used by generic content pages migrated from Microsoft Content Management Server 2002. 

It also acts as a shell which pulls in additional templates and features as NuGet packages.

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