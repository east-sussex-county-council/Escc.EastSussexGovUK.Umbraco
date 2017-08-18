# Adding term dates to a topic page

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
