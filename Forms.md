# Forms

We use [Umbraco Forms](https://umbraco.com/products/umbraco-forms/), but with some customisations to the out-of-the-box functionality.

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

## Permissions

Unfortunately Umbraco Forms grants access to all forms for new users, and to all users for new forms. The default permission set should be 'deny' in both cases, and `UmbracoFormsSecurityApiController` exists to create a 'deny' record wherever one is missing. This needs to be called frequently by a scheduled task to pick up and fix new users and new forms.

This issue is logged with Umbraco as [CON-1022](http://issues.umbraco.org/issue/CON-1022).

When you create a new form you will need to grant access to the form to anyone who needs to view the data for processing (assuming the default workflow is used which stores the data in Umbraco). 

1. Visiting the Users section in Umbraco. 
2. Expand the 'Users' tree, find the user who needs access, and ensure that they have access to the Forms section. If the user is not a web author, this should be the only section selected. 
3. Next expand 'Forms Security', find the user who needs access again, and tick 'Has Access' next to the form they need to view. 

### Finding out who has access to an existing form

Umbraco Forms shows you what forms a user has access to, but not which users have access to a form. `FormPermissionsEventHandler` adds a 'Permissions' menu item to each form which displays a list of the users who have access, and links each user's name to the edit view for their Umbraco Forms permissions.

This dialog is loaded using a route configured in `FormPermissionsEventHandler` to go to `FormPermissionsController`, which loads the `~\Views\Partials\Forms\Permissions.cshtml` view. 

## Securing uploads

Umbraco forms uploads files to the same `IFileSystem` as items in the media gallery, which means they are publicly available and insecure even though their contents may be sensitive.

Since we already use a [fork of UmbracoFileSystemProviders.Azure](https://github.com/east-sussex-county-council/UmbracoFileSystemProviders.Azure) as our media file system, we have customised this to recognise a forms upload and treat it differently. When uploading the file, if configured to do so, it will place a forms upload into a separate container in the same blob storage account. This container can be set to private access, preventing downloads via a request similar to `https://account.blob.core.windows.net/container/example-file-path.jpg`. When downloading a file it will simply not return anything, as it does not have access to the currently logged-in back office user account which would be needed to validate whether the user should have access to the form and any uploaded files.

This means we need another, secure, way to download the files which is implemented in `UmbracoFormsUploadsApiController`. A request for an uploaded file coming via this API will check which form the file was uploaded from, and check whether the current user has access to that form in their Umbraco Forms permissions. `~\App_Plugins\UmbracoForms\Backoffice\Common\RenderTypes\file.html` is customised to use the file path appended to the secure API, which causes files to be rendered correctly in the back office entries viewer.

This issue is logged with Umbraco as [CON-1454](http://issues.umbraco.org/issue/CON-1454).

## Separating the form design and form processing roles

By default in Umbraco Forms, to grant a user access to view and download their form data, you must also grant them 'Manage Forms' permission, which lets them modify and even delete their form. This opens up potential problems due to the lack of versioning, and due to privacy requirements that must be met. Form design is a role for specialists that are trained to understand such issues, and needs to be separate from processing the data that is submitted to an individual form.

This is achieved by the `~\App_Plugins\UmbracoFormsEntries` folder, which adds an extra branch in the tree for the Forms section for viewing entries without offering the ability to design a form. A client-side controller (`controller.js`) and view (`edit.html`) copied from the out-of-the-box files ensures that the built-in functionality for viewing forms is all available. These files may need to be manually replaced each time Umbraco Forms is updated, and the changes are documented in `controller.js` and `edit.html`.

This issue is logged with Umbraco as [CON-1455](http://issues.umbraco.org/issue/CON-1455).

## Ensuring you can always view form data

If the first field on an Umbraco form is optional and not completed, there is nothing to click on to view the data. A CSS file at `~\App_Plugins\UmbracoFormsEntries\umbraco-forms-entries.css` fixes this by adding some generated content that is always displayed and clickable. 

This issue is logged with Umbraco as [CON-1465](http://issues.umbraco.org/issue/CON-1465).

## Implementing a retention schedule

Every form that collects personal data must have a retention schedule, and ideally this should be automated to ensure that it is not forgotten. For retention schedules that are simply a set time after the form is submitted, this is implemented by `RetentionAfterSetDateWorkflow`. 

* Add a field called 'Delete after' to a form using the 'Hidden' answer type, ensuring it has the alias `deleteAfter`.
* Add a workflow using the 'Retention schedule: after a set date' workflow, and set the time period to keep records for.
* When a form is submitted and the workflow runs, a date is added to the `deleteAfter` field.
* Run a scheduled task regularly to call `https://hostname/umbraco/api/UmbracoFormsRetentionApi/ApplySetDateRetentionSchedule` using the HTTP `DELETE` method. It will look for the `deleteAfter` field on all form records, and delete any where it finds a date that has passed. 

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

`TextField` customises the built-in 'Short answer' field type to add an extra property for a just-in-time privacy notice. This extra property is used when the field is displayed using the `EastSussex` theme.

`Email` uses the `email` HTML field type to trigger a keypad with an @ symbol on touch screens, and validates an email address better than the built-in `Validate as email` option on the 'Short answer' field type.

`PhoneNumber` uses the `tel` HTML field type to trigger a phone keypad on mobiles, and validates a phone number better than the built-in `Validate as a number` option on the 'Short answer' field type. 

`FormattedTextField` provides a way for form designers to add some static HTML in the form. It is implemented as a question which does not need an answer. 

`EthnicGroup` inserts a standardised dropdown list of ethnic groups, and an 'other' box which appears if any of the 'other' options are selected. This dependency behaviour is achieved by adding code to `~\Views\Partials\Forms\Themes\EastSussex\Script.cshtml` which injects JSON into the page to hook into Umbraco Forms' own conditional logic.

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

**Multiple file uploads** should just use the 'File upload' field type multiple times. A native multiple file upload field type is [expected soon](https://github.com/PerplexInternetmarketing/Perplex-Umbraco-Forms/issues/2), and the next release of Umbraco Forms is due in Q1 2018.

We also **allow paragraphs in question help text**. By default pressing Enter in the help text field will close the 'Edit question' overlay, but `~\App_Plugins\UmbracoFormsEntries\BackOffice\allow-new-lines.js` fixes this by removing the attribute that triggers it. You can now press Enter to get a new line. A change to the way help text is rendered in `~\Views\Partials\Forms\Themes\EastSussex\Form.cshtml` completes the fix by replacing new lines with paragraph tags. 

## Implementing a more secure email workflow

The email workflows that come with Umbraco Forms are designed to send the form data as part of the email. However, the data may be sensitive and email is not a secure medium, so it is better to send a link to the form in the back office where the data is protected by TLS and authentication.

The 'Send email with template (Razor)' workflow allows you to customise the email that is sent, but the body of the email only has access to the fields submitted with the form, not to metadata about the form itself. To work around this:

* Add a field called 'Form entry id' to a form using the 'Hidden' answer type, ensuring it has the alias `formEntryId`.
* Add a workflow using the 'Save form entry id' workflow (implemented by `SaveIdAsFieldWorkflow`).
* Add a workflow using the 'Send email with template (Razor)' workflow **after** the 'Save form entry id' workflow and pick `Send-A-Link.cshtml` as the template. 
* When a form is submitted and the workflows run, the form id and form entry id are both added to the `formEntryId` field as separate values, and used by `Send-A-Link.cshtml` to build a link to view the entry. In case the Umbraco back office is on a separate URL from the instance sending the email, you can optionally specify the base URL in `web.config`:

		<configuration>
			<appSettings>
				<add key="BackOfficeBaseUrl" value="https://hostname/" />
			</appSettings>
		</configuration>

Unfortunately Umbraco Forms doesn't support a native URL to view a single form entry. To add that support, `~\App_Plugins\UmbracoFormsEntries\Backoffice\FormEntries\edit.html` adds a `data-entry-id` attribute with the entry id to the link to each form, and `controller.js` in the same folder has added code that looks for an `entry` parameter on the querystring and matches it up to the link. This will only work with low form volumes as the entry to view needs to be on the first page of results when they load. 