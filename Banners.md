# Banners

This project uses Umbraco to manage where to display promotional banners, and it publishes those settings as JSON.

Each banner is configured in Umbraco using a page based on the `Banner` document type. They are aggregated under a page using the `Banners` document type, which is a list view document type. This allows permissions to be restricted for that part of the content tree.

The `Banner` document type requires several properties:

* A media picker with the alias `bannerImage_Content`.
* A content picker with the alias `targetPage_Content` to select the page the banner should link to.
* A URL (or text string) property editor with the alias `targetUrl_Content` to enter the URL to link to if no Umbraco page is selected in `targetPage_Content`. Use this to link to another website, for example.
* A multi-node tree picker with the alias `whereToDisplayIt_Content` to select the target pages on which to display the banner.
* A multi-line text box with the alias `whereElseToDisplayIt` where editors can paste any target page URLs, one per line. This is useful for targeting external applications like the online library catalogue.
* A checkbox with the alias `inherit` which adds a banner to those from parent pages rather than replacing them.
* A checkbox with the alias `cascade` which causes the banner to appear on descendant pages.

A web API returns JSON data for all banners as `https://hostname/umbraco/api/Banners/GetBanners`. This response specifies a 24 hour HTTP cache to prevent clients requesting data from Umbraco on every page view.

The JSON data is consumed by `banners.js` in the `Escc.EastSussexGovUK` project.