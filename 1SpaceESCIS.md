# East Sussex 1Space and ESCIS support

[East Sussex 1Space](https://1space.eastsussex.gov.uk/) is a website maintained by East Sussex County Council which lists care services. [ESCIS](https://www.escis.org.uk/) is a website maintained by East Sussex County Council which has local information for communities across the county. Most templates in Umbraco support displaying search forms for both of these sites.

Wherever you want to configure whether to display the East Sussex 1Space or ESCIS search forms you need to create properties on your document type, which are usually placed on a 'Social media and promotion' tab:

*  Create a data type called `Show widget` using the `Radio button list` property editor, with three pre-values for `Show`, `Hide` and `Inherit`.
*  Create a property on your document type called `EastSussex1Space: show widget?` with an alias of `eastsussex1space_Social_media_and_promotion` which uses the `Show widget` data type.
*  Create a property on your document type called `ESCIS: show widget?` with an alias of `escis_Social_media_and_promotion` which uses the `Show widget` data type.
  
In a new installation you would create the 'Social media and promotion' tab as a separate document type and add it as a [composition](https://our.umbraco.com/documentation/Getting-Started/Data/Defining-content/#creating-a-document-type) to other document types. However, this was set up before compositions were available in Umbraco, so the fields are defined separately on different document types, including some parent document types (which is a feature replaced by compositions).

Each controller that needs to support an East Sussex 1Space or ESCIS search form should create an instance of `UmbracoEastSussex1SpaceService` or `UmbracoEscisService`, and usually pass it to `BaseViewModelBuilder.PopulateBaseViewModelWithInheritedContent()`. This evaluates the 'inherit' option, recursing up the hierarchy of content nodes to determine whether to display each form, and then sets the `ShowEastSussex1SpaceWidget` or `ShowEscisWidget` property of the model.


	public new async Task<ActionResult> Index(RenderModel model)
    {
		var viewModel = new MyCustomModel();

		// Populate the view model with the 'East Sussex 1Space' setting
		var templateRequest = new EastSussexGovUKTemplateRequest(Request);
		var modelBuilder = new BaseViewModelBuilder(templateRequest);
		modelBuilder.PopulateBaseViewModelWithInheritedContent(viewModel, 
                null, null, 
				new UmbracoEastSussex1SpaceService(model.Content), 
				new UmbracoEscisService(model.Content), 
				null);

        return CurrentTemplate(viewModel);
	} 

Include the following partial view in any template which needs to display the forms:

	@Html.Partial("~/Views/EastSussexGovUK/Features/_SupportingContentDesktop.cshtml")

This partial view is part of the [Escc.EastSussexGovUK.Mvc](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK/blob/master/DotNetFrameworkMvc.md#add-common-features-to-pages-using-partial-views) project.

Note that some document types (for example, the home page) may allow entry of East Sussex 1Space or ESCIS settings without displaying them. This makes sense as the East Sussex 1Space or ESCIS forms might be configured to cascade to all pages below that page.