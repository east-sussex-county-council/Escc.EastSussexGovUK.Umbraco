# Forms

We use [Umbraco Forms](https://umbraco.com/products/umbraco-forms/), but with some customisations to the out-of-the-box functionality. The following changes are applied in addition to those in [Escc.Umbraco.Forms](https://github.com/east-sussex-county-council/Escc.Umbraco.Forms).

## Creating a new form

When you create a new form you should visit the Settings section for the form (top right in the Umbraco back office) and tick 'Load assets with client dependency'. You will also need to change the workflow for the form from the default, which is to email information to the Web Staff inbox. 

## Versioning

### The problem

Umbraco Forms does not have any versioning of forms. If you modify the fields on a form, you will do so for all previous form submissions as well as all future submissions. If you add a field, it will appear as if the question was asked and not answered. If you modify a field, the old answers will be displayed alongside the new question, or the data may be lost if the underlying field type is changed. If you delete a field, all of the historical data for that field will be deleted.

This is logged with Umbraco as [CON-1467](http://issues.umbraco.org/issue/CON-1467). 

### The solution

**Any change to a form MUST be versioned.**

This means data submitted can be recombined with the questions asked. This includes even minor changes, as privacy law based on the GDPR requires us to be able to prove, for example, what exactly was consented to in a situation where personal data is collected with consent as the legal basis for processing.

Before making any change to a form, right-click the form in the Umbraco back office and select `Copy`. Make your changes to the copy of the existing form and, when you're ready, go to the Content section and find the page on which the form is displayed, remove the old version of the form and select the new one instead.

With many versions of many forms, a naming convention is essential. Use the following format:

	{name} ({version}) used {date issued} to {date expired}

For example:

	My form (v0001) used 2018-01-01 to 2018-01-31 
	My form (v0002) used 2018-02-01 to 2018-01-28
	My form (v0003) used 2018-03-01 to current

- The name of a form is not displayed to website visitors, so it can include metadata, and does not need to change even if the title shown to website visitors is changed. 
- The version number is there as an easy way to refer to the form. Always use 4 digits for the version number so that versions of a form will sort into chronological order. 
- The date provides essential information about when a change was made. Using the ISO8601 date format will ensure that dates for a form line up into columns making them easier to scan.

## Securing uploads

Umbraco forms uploads files to the same `IFileSystem` as items in the media gallery, which means they are publicly available and insecure even though their contents may be sensitive.

Since we already use a [fork of UmbracoFileSystemProviders.Azure](https://github.com/east-sussex-county-council/UmbracoFileSystemProviders.Azure) as our media file system, we have customised this to recognise a forms upload and treat it differently. When uploading the file, if configured to do so, it will place a forms upload into a separate container in the same blob storage account. This container can be set to private access, preventing downloads via a request similar to `https://account.blob.core.windows.net/container/example-file-path.jpg`. When downloading a file it will simply not return anything, as it does not have access to the currently logged-in back office user account which would be needed to validate whether the user should have access to the form and any uploaded files.

This means we need another, secure, way to download the files which is implemented in `AzureSecureFormUploadsController`. This inherits from `Escc.Umbraco.Forms.Security.SecureFormUploadsController` which checks, when an uploaded file is requested, whether the current user has access to that form in their Umbraco Forms permissions. The file is returned from Azure blob storage only if that test passes. 

`~\App_Plugins\UmbracoForms\Backoffice\Common\RenderTypes\file.html` is customised by `Escc.Umbraco.Forms.Security` with a route for its secure API which causes files to be rendered correctly in the back office entries viewer. That needs to be replaced with the route for the Azure secure API, configured by `AzureSecureFormUploadsEventHandler`. It's not enough to modify the file as it's replaced with the original every time the project is built, so the `Escc.EastSussexGovUK.Umbraco.csproj` file contains an extra MSBuild task that updates `file.html` with the path to the Azure secure API at the end of the build process. 

This issue is logged with Umbraco as [CON-1454](http://issues.umbraco.org/issue/CON-1454).

## Implementing an East Sussex theme

Modified copies of built-in views make up an `EastSussex` theme in `~\Views\Partials\Forms\Themes\EastSussex`. This was necessary for two reasons:

- to implement Umbraco Forms without compromising our content security policy (see below)
- to update the HTML to be as close as possible to the forms HTML expected by our sitewide forms styling, rather than the default styles based on Bootstrap.

Forms are then displayed using a `Form` document type which has a template that hard-codes the use of the `EastSussex` theme.

### Implementing a content security policy

Umbraco Forms by default uses some inline scripts and styles. Overriding default configuration in the `EastSussex` theme allows most of these to be removed, or permitted by adding a nonce to the content security policy.

The built-in file `~\Views\Partials\Forms\DatePicker.cshtml` needed modification to add a nonce to our content security policy allowing the inline script. This change may need to be repeated each time Umbraco Forms is updated.

This issue is logged with Umbraco as [CON-1181](http://issues.umbraco.org/issue/CON-1181).

## Custom field types

Several field types in the `Escc.EastSussexGovUK.Umbraco.Forms.FieldTypes` namespace add an extra property for a just-in-time privacy notice to fields which are part of either Umbraco Forms or `Escc.Umbraco.Forms.FieldTypes`. This extra property is used when the field is displayed using the `EastSussex` theme.

`AgreeToTerms` lets you create a checkbox with a link to an Umbraco page or document, to create a "I've read and agree to these terms and conditions" question.

`Address` lets you collect a UK address using a postcode lookup. It is an Umbraco Forms wrapper around a standard MVC forms partial view from the [Escc.FindAddress.Mvc](https://github.com/east-sussex-county-council/Escc.FindAddress.Mvc) project. It stores its data as a JSON object including latitude, longitude, UPRN and USRN. A custom render view at `~\App_Plugins\UmbracoForms\BackOffice\Common\RenderTypes\address.html` ensures that the JSON is not displayed in the entries viewer. 

`School` lets you pick a school from the autocomplete suggestions or type a school name that's not in the list. When choosing from the autocomplete suggestions, it also saves and submits the school code. The autocomplete connects to a URL which needs to be set in `web.config`. The URL should support a `name` querystring parameter to filter the results by school name:

	<appSettings>
    	<add key="SchoolApiUrl" value="https://hostname/api/schools" />
	</appSettings>

and which returns JSON data in the following format:

	[
		{"SchoolName":"Example School #1","SchoolCode":"1234567"},
		{"SchoolName":"Example School #2","SchoolCode":"2345678"},
		...
	]

A suitable API is published by the `Escc.Schools.Website` project.

**Multiple file uploads** should just use the 'File upload' field type multiple times. A native multiple file upload field type is [expected](https://github.com/PerplexInternetmarketing/Perplex-Umbraco-Forms/issues/2).

## Printing form entries

It can be useful to print form entries directly from the entries viewer. This doesn't work by default as the back-office is not designed for printing. `~\App_Plugins\Escc.EastSussexGovUK.Umbraco.Forms\back-office.css` hides elements that overlap the form data when printing.