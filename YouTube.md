# Embedding YouTube videos

Any property using an instance of the rich text property editor that's configured to load `TinyMCE-StyleSelector-Embed.css` will offer the ability to select a link and apply an `Embed link (eg video, map)` option. This applies a `.embed` class. 

`embed-youtube.js` in the `Escc.EastSussexGovUK.TemplateSource` project looks for the `.embed` class surrounding a link format that it knows how to convert to a YouTube video, and converts it.

For any rich text property that might contain an embedded YouTube video, add code similar to the following to the view:

	@using Escc.EastSussexGovUK.Features

    Html.Partial("~/Views/EastSussexGovUK/_FeatureDependencies.cshtml", new List<IClientDependencySet>()
	{
		new EmbeddedYouTubeVideos() { Html = new [] 
			{ 
				Model.MyRichTextField.ToString(), 
				Model.MyOtherRichTextField.ToString() 
			}}
	});

`EmbeddedYouTubeVideos` is an `IClientDependencySet` which causes `embed-youtube.js` and its dependencies to be loaded and changes to the Content Security Policy to be applied, but only if an embedded YouTube video is found on the page.

For more information see the documentation for [Escc.EastSussexGovUK](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK).