# iCaseworkForms

[iCasework](https://www.icasework.com/) is case management software which includes its own form builder. Links to the forms can be recognised an embedded into Umbraco pages.

Any property using an instance of the rich text property editor that's configured to load `TinyMCE-StyleSelector-Embed.css` will offer the ability to select a link and apply an `Embed link (eg video, map)` option. This applies a `.embed` class. 

`icasework-forms.js` looks for the `.embed` class surrounding a link to an iCasework form, and converts it to an embedded form.

The link formats it recognises are:

*	`https://{icasework-account}.icasework.com/form?Type={form-id}&Login=false&Embed=True`
*	`https://{icasework-account}.icasework.com/cases?&public=true&byCaseType=false&byKeyword=true&max=20&title=Disclosure%20Log&Embed=True`

For any rich text property that might contain an embedded iCasework form, add code similar to the following to the view:

	@using Escc.EastSussexGovUK.Features

    Html.Partial("~/Views/EastSussexGovUK/_FeatureDependencies.cshtml", new List<IClientDependencySet>()
	{
		new EmbeddedICaseworkForm() { Html = new [] 
			{ 
				Model.MyRichTextField.ToString(), 
				Model.MyOtherRichTextField.ToString() 
			}}
	});

`EmbeddedICaseworkForm` is an `IClientDependencySet` which causes `icasework-forms.js` and its dependencies to be loaded and changes to the Content Security Policy to be applied, but only if an embedded iCasework form is found on the page.

The CSS and JavaScript files and the Content Security Policy required for embedding iCaseWork forms must all be registered in `web.config`. These settings are added when you transform `web.config` using `~\Escc.EastSussexGovUK.Umbraco.Web\Web.config.xdt`.