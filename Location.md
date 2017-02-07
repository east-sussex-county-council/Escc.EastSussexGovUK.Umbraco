# Location data API

A location API makes available data from the Location template defined in this project. The API returns a list of locations filtered by the location type:

	https://hostname/umbraco/api/location/list?type=Library&type=MobileLibraryStop

This is designed for the 'Find a library' and 'Find a recycling site' features. You may call this API for other purposes (for example, you could map all the mobile library stops), but it is regarded as an internal API which may be changed without notice. If you require this data with more guarantees, please contact us to discuss setting up a more formal API. 