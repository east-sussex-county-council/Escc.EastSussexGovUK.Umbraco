# East Sussex County Council home page

Our home page highlights pulls together some of the most popular tasks on our website, and gives the council a space to highlight subjects that are relevant at the current time.

## News stories

Create a single instance of the 'Home page items' document type with a name that includes "News", and this will be rendered as an RSS feed of news stories. An 'Analytics' tab allows Google campaign tracking parameters to be added to all links in the RSS feed so that usage of the feed can be tracked. 

Below the RSS feed create an instance of the 'Home page item' document type for each news story. 

All published news stories will appear in the RSS feed, but only two can be displayed on the home page, which are the two that are first when the nodes are sorted in the Umbraco back office. You can use standard publishing and unpublishing dates in Umbraco to schedule the addition and removal of news items.

Images should be 236px wide x 198px high, and this is enforced by using a querystring that loads the image through [ImageProcessor](https://imageprocessor.org/), which comes with Umbraco. The default configuration of ImageProcessor doesn't work because we're storing media items in Azure blob storage using [a fork of UmbracoFileSystemProviders.Azure](https://github.com/east-sussex-county-council/UmbracoFileSystemProviders.Azure). You need to disable the `LocalFileImageService` and `RemoteImageService` in `~\config\imageprocessor\security.config` and enable the `CloudImageService` as follows:

	<service prefix="media/" name="CloudImageService" type="ImageProcessor.Web.Services.CloudImageService, ImageProcessor.Web">
      <settings>
        <setting key="Container" value="media"/>
        <setting key="MaxBytes" value="8194304"/>
        <setting key="Timeout" value="30000"/>
        <setting key="Host" value="https://{storage-account-name}.blob.core.windows.net/media"/>
      </settings>
    </service>

## Report / Apply / Pay

The tabs code for this is an old version of JQuery UI. It would be difficult to get a new version of this code now, so if JQuery needs to be updated and causes this to break, switch to using the more modern tabs code that is used on the 'Location' and 'Job advert' templates.

## School term dates

School term dates are one of the most viewed things on our website. They need to be uploaded to the Umbraco media library as an XML file, maintained by the editorial team, with the data in the following format:


	<?xml version="1.0" encoding="utf-8" ?>
	<TermDates>
  	  <SchoolYear startYear="2019">
	    <Holiday name="Summer holiday" startDate="2019-07-24" endDate="2019-09-03" />
	    <InsetDay name="INSET day" startDate="2019-09-04" endDate="2019-09-04" />
	    <Term name="Term 1" startDate="2019-09-05" endDate="2019-10-25" />
	  </SchoolYear>
	</TermDates>

Any element apart from the root element `TermDates` can be repeated as often as necessary, and data for past years can be left in place. Only INSET days that apply to every school in the county can safely be included here. Every school will have other INSET days in addition to the standard ones. 

The media item must then be selected in the relevant property when editing the home page in Umbraco.

You need to remove `xml` from the `disallowedUploadFiles` section in `~\config\umbracoSettings.config` in order to upload this file.

## Libraries

The libraries section includes a form that sends a search term to the [Elibrary](https://e-library.eastsussex.gov.uk). Hidden fields in the form provide defaults for the other values expected by an elibrary search. This form may need to be updated when the elibrary is upgraded or replaced.

## East Sussex Jobs

Edit the home page in Umbraco to select where the jobs search box should submit its search terms to. This should be an instance of the 'Job search results' document type. A hidden field is included in the form so that the search results querystring shows where the user started their search - this is useful in Google Analytics.

    <input type="hidden" name="source" value="eastsussexgovuk-home" />

The lists of locations and categories in the job search form, together with the number of matching jobs, are requested from the jobs API. They are wrapped in a `try { } catch { }` block so that the home page will still load even if the API is unavailable. For more information on how jobs work see [Jobs](Jobs.md). 

## Where to recycle

The list of waste types is hard-coded, so if it is updated the application must be recompiled and deployed.

The recycling site search form is installed as part of the [Escc.RubbishAndRecycling.SiteFinder](https://github.com/east-sussex-county-council/Escc.RubbishAndRecycling.SiteFinder) NuGet package, which is available on our private feed. 

This same form appears as part of the `Escc.RubbishAndRecycling.SiteFinder` application, so the same hard-coded set of waste types is exposed as a Web API by `~\WebApi\WasteTypesController.cs` for that application to re-use.

It also appears as an option in the 'Standard Topic Page' template in this application, which is [migrated from Microsoft CMS](MicrosoftCms.md).

## Get involved

This section works the same way as the News stories section, except that images are not used. The instance of the 'Home page items' document type must include "Involved" in its name.

It was set up this way so that this section could be updated with the latest consultations and important council meetings, but it hasn't been used like that so the content and the RSS feed are usually static.
