# Embedding Google Maps

Any property using an instance of the rich text property editor that's configured to load `TinyMCE-StyleSelector-Embed.css` will offer the ability to select a link an apply an `Embed link (eg video, map)` option. This applies a `.embed` class. 

`embed-googlemaps.js` in the `Escc.EastSussexGovUK.TemplateSource` project looks for the `.embed` class surrounding a link format that it knows how to convert to a Google Map, and converts it.

For any rich text property that might contain an embedded Google Map, add code similar to the following to the view:

	@using Escc.EastSussexGovUK.Features

    Html.Partial("~/Views/EastSussexGovUK/_FeatureDependencies.cshtml", new List<IClientDependencySet>()
	{
		new EmbeddedGoogleMaps() { Html = new [] 
			{ 
				Model.MyRichTextField.ToString(), 
				Model.MyOtherRichTextField.ToString() 
			}}
	});

`EmbeddedGoogleMaps` is an `IClientDependencySet` which causes `embed-googlemaps.js` and its dependencies to be loaded and changes to the Content Security Policy to be applied, but only if an embedded Google Map is found on the page.

For more information see the documentation for [Escc.EastSussexGovUK](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK).