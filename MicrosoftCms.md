# Old templates migrated from Microsoft CMS

This site used to use WebForms templates in Microsoft CMS. For the conversion to Umbraco the existing templates were converted to ASCX usercontrols which can be loaded by the Razor view for the equivalent Umbraco template. 

The controls in the `Escc.EastSussexGovUK.Umbraco.MicrosoftCmsMigration` namespace replicate the original Microsoft CMS controls as closely as possible, with the code altered only to get their data from Umbraco content nodes instead. This ensured that the migration was as smooth as possible, with all the template logic preserved.

The Umbraco templates set up as equivalents of the old Microsoft CMS templates all use `MicrosoftCmsPageController` and `MicrosoftCmsViewModel`. These are as minimal as possible, leaving most of the page logic to the original controls.

`MicrosoftCmsUrlRedirectionHandler` exists to redirect old Microsoft CMS URLs, which all ended in `.htm`, to their nearest equivalent in Umbraco, making the assumption that the replacement page is most likely found at the same URL. If it's not there (because a lot of changes have happened since then) then the usual 404 page will be displayed when Umbraco cannot find the redirected URL.

The handler must be configured in `web.config` as follows:

	<system.webServer>
	  <handlers>
      	<add name="MicrosoftCmsUrlRedirection" verb="GET" path="*.htm" type="Escc.EastSussexGovUK.Umbraco.Web.MicrosoftCmsMigration.MicrosoftCmsUrlRedirectionHandler, Escc.EastSussexGovUK.Umbraco.Web" />
	  </handlers>
	</system.webServer>
  
## Standard topic page - displaying term dates

Term dates are stored as XML so that they can be [made available as open data](http://data.gov.uk/dataset/east-sussex-county-council-term-dates).

This is an example of the XML format, showing the three possible time periods: terms, holidays and INSET days.

	<TermDates>
	  <SchoolYear startYear="2012">
	    <Holiday name="Summer holiday" startDate="2012-07-23" endDate="2012-09-03" />
	    <InsetDay name="INSET day" startDate="2012-09-04" endDate="2012-09-04" />
	    <Term name="Term 1" startDate="2012-09-05" endDate="2012-10-26" />
	  </SchoolYear>
	</TermDates>

1. Upload the term dates data to the Umbraco media library.
2. On a topic page, select 'Term dates' as a section layout
3. In the same section, select the term dates data in the 'Section [number]: image 1' field. All other fields in the section are ignored.

If no term dates data is selected, the term dates section is left blank. If the wrong kind of file is selected an `XmlException` is thrown with the message "Invalid character in the given encoding. Line 1, position 1."