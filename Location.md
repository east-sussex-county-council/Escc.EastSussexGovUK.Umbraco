# Locations, including libraries and recycling sites

## Opening times on location pages

Opening times on a `Location` document type use an older version of the [Jumoo.OpeningSoon](https://www.nuget.org/packages/Jumoo.OpeningSoon/) property editor which is included in this repository in the `~\App_Plugins\OpeningSoon` folder. (It's not customised; it just pre-dates availability on NuGet.) This saves its data as JSON and this is deserialised by `LocationViewModelFromUmbraco` to the `OpeningTimes` class built for this project specifically to match the JSON format saved by this property editor. 

`LocationViewModelFromUmbraco` also calculates the relative opening times (like 'opening at 10am tomorrow') from the opening hours data. This is taken into account when setting up caching for a page. Because this information can change quickly, the cache timeout  set by `LocationController` matches the time this data remains valid. 

## Tabs on location pages

The `Location` template includes tabs at the bottom of the page. If you resize your browser you will see this change to an accordion at small screen sizes. This works using `accordion-and-tabs.js` in the [Escc.EastSussexGovUK](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK) project. 

`Location.cshtml` uses [Escc.ClientDependencyFramework](https://github.com/east-sussex-county-council/Escc.ClientDependencyFramework) to locate this file in `web.config`. The entry in `web.config` comes from the `Escc.EastSussexGovUK.ClientDependency` NuGet package in the [Escc.EastSussexGovUK](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK) project.

## Location data as KML

We publish KML data of the position of locations by their type. This works by registering an HTTP handler in `web.config`, and the handler then reads the data from the location data internal API (see below).

	<system.webServer>
		<handlers>
	      <add name="LocationsAsKml" verb="GET" path="/data/*.kml" type="Escc.EastSussexGovUK.Umbraco.Location.LocationsAsKmlHandler, Escc.EastSussexGovUK.Umbraco" />
		</handlers>
	</system.webServer>

Data is published at the following URLs:

* [https://www.eastsussex.gov.uk/data/childcare.kml](https://www.eastsussex.gov.uk/data/childcare.kml)
* [https://www.eastsussex.gov.uk/data/counciloffices.kml](https://www.eastsussex.gov.uk/data/counciloffices.kml)
* [https://www.eastsussex.gov.uk/data/daycentres.kml](https://www.eastsussex.gov.uk/data/daycentres.kml)
* [https://www.eastsussex.gov.uk/data/libraries.kml](https://www.eastsussex.gov.uk/data/libraries.kml)
* [https://www.eastsussex.gov.uk/data/parks.kml](https://www.eastsussex.gov.uk/data/parks.kml)
* [https://www.eastsussex.gov.uk/data/recyclingsites.kml](https://www.eastsussex.gov.uk/data/recyclingsites.kml)
* [https://www.eastsussex.gov.uk/data/registrationoffices.kml](https://www.eastsussex.gov.uk/data/registrationoffices.kml)
* [https://www.eastsussex.gov.uk/data/sportlocations.kml](https://www.eastsussex.gov.uk/data/sportlocations.kml)

## Location data as .vcf contact cards

Each location page includes a 'Save to contacts' link, which points to the same URL with a `.vcf` extension. This URL is rewritten by IIS to the original page URL with the querystring `?alttemplate=LocationVCard`. This triggers an Umbraco feature called 'Alternate templates' which uses the same controller as the original page, but renders it using the `LocationVCard.cshtml` view instead of `Location.cshtml`. This view creates a [vCard](https://en.wikipedia.org/wiki/VCard) that is understood by most programs that support contacts and addresses. 

 Rewriting the URL is configured in `web.config`:

    <system.webServer>
      <rewrite>
        <rules>
          <rule name="Location template *.vcf download" stopProcessing="true">
            <match url="(.*).vcf" />
            <action type="Rewrite" url="/{R:1}?alttemplate=LocationVCard" />
          </rule>
        </rules>
      </rewrite>
    </system.webServer>

## Location data API

A location API makes available data from the Location template defined in this project. The API returns a list of locations filtered by the location type:

	https://hostname/umbraco/api/location/list?type=Library&type=MobileLibraryStop

This is designed for the 'Find a library' and 'Find a recycling site' features. It can also be used to embed a Google map in a page by linking to a URL like the one above, selecting the text and applying Format > Embed. The resulting HTML is recognised and turned into a Google map by JavaScript in the `Escc.EastSussexGovUK` repository. 

You may call this API for other purposes (for example, you could map all the recycling sites), but it is regarded as an internal API which may be changed without notice.

## Adding a new waste type

To add a new waste type to those available at recycling sites you need to do two things:

* Add the new pre-value to `WasteTypesDataType`, which will update the waste types used by application code and configure any new installations correctly. This will fix the public user interface.
* Add the new pre-value manually to the `Waste types` data type in any existing Umbraco installations, because the scripted update only creates data types; it doesn't update existing ones. This will add the option to the Umbraco back-office user interface.