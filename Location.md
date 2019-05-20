# Locations, including libraries and recycling sites

To add a new waste type to those available at recycling sites you need to do two things:

* Add the new pre-value to `WasteTypesDataType`, which will update the waste types used by application code and configure any new installations correctly. This will fix the public user interface.
* Add the new pre-value manually to the `Waste types` data type in any existing Umbraco installations, because the scripted update only creates data types; it doesn't update existing ones. This will add the option to the Umbraco back-office user interface.

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


## Location data API

A location API makes available data from the Location template defined in this project. The API returns a list of locations filtered by the location type:

	https://hostname/umbraco/api/location/list?type=Library&type=MobileLibraryStop

This is designed for the 'Find a library' and 'Find a recycling site' features. It can also be used to embed a Google map in a page by linking to a URL like the one above, selecting the text and applying Format > Embed. The resulting HTML is recognised and turned into a Google map by JavaScript in the `Escc.EastSussexGovUK` repository. 

You may call this API for other purposes (for example, you could map all the recycling sites), but it is regarded as an internal API which may be changed without notice.