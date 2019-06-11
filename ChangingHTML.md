# Making changes to HTML before it's displayed

## Rewriting links to be domain-relative

Less-experienced editors may paste hard-coded absolute links to our Umbraco back office site into their content and expect them to work. Security settings mean they won't work, so we need to prevent them being displayed by removing the back office domain, leaving the links expressed as relative to [https://www.eastsussex.gov.uk](https://www.eastsussex.gov.uk). This also deals with old media links which had to go direct to our Azure blob storage account URL before we switched to using [a fork of UmbracoFileSystemProviders.Azure](https://github.com/east-sussex-county-council/UmbracoFileSystemProviders.Azure).

`RemoveAzureDomainHtmlFormatter` takes care of this for links pasted into properties using a data type based on the `Rich text editor (ESCC)` property editor. This doesn't require any further configuration as it implements `Escc.Umbraco.PropertyEditors.RichTextPropertyEditor.IRichTextHtmlFormatter`, which is defined and automatically discovered by a property value converter in [Escc.Umbraco.PropertyEditors](https://github.com/east-sussex-county-council/Escc.Umbraco.PropertyEditors/).

For properties that gather a single URL, `RemoveMediaDomainUrlTransformer` and `RemoveAzureDomainUrlTransformer` make similar changes. These need to be referenced in any controller that requires the transformation to happen. For example:

	
	var mediaUrlTransformer = new RemoveMediaDomainUrlTransformer();
	var azureUrlTransformer = new RemoveAzureDomainUrlTransformer();
    var viewModel = new MyCustomViewModel
    {
        ExampleUrl = 
			mediaUrlTransformer.TransformUrl(
			azureUrlTransformer.TransformUrl(
				new Uri(content.GetPropertyValue<string>("exampleUrl"), UriKind.RelativeOrAbsolute)
			))
	};