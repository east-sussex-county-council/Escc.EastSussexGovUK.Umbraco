# Service alerts

Alerts about disruptions to council services which appear across sections of www.eastsussex.gov.uk or the entire site.

Alerts are created as pages in Umbraco using the `Alert` document type. They are aggregated under a page using the `Alerts` document type, which is a list view document type. This allows permissions to be restricted for that part of the content tree.

The `Alerts` document type uses `AlertsController` to load the data, and `~\Views\Alerts.cshtml` to return JSON data for all alerts. This includes an alert for school closures, which is read from an XML file in Azure storage using code from [Escc.ServiceClosures](https://github.com/east-sussex-county-council/Escc.ServiceClosures).

The JSON data is consumed by `alerts.js` which is loaded on all sitewide master pages and MVC layouts from the `Scripts*.ascx` files in the [Escc.EastSussexGovUK](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK) project. `alerts.js` also lives in `Escc.EastSussexGovUK`.

## Caching of alerts

There are 5 minutes caches and intervals in place at various points in the school closure alerts process:

* school closures data gets sent to www.eastsussex.gov.uk every 5 minutes
* the temporary API for school closures on www.eastsussex.gov.uk requested by this app is output cached for 5 minutes
* the result of that request is held in the application cache for 5 minutes
* the `alerts.cshtml` view specifies a 5 minute HTTP cache

In the worst case it could take 20 minutes to display a school closure, but more typically these 5 minute intervals will overlap.

Alerts created in Umbraco are read from the Umbraco cache and are subject to the 5 minute HTTP cache on the `alerts.cshtml` view.