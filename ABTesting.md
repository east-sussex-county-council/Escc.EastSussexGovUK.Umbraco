# A/B testing

A/B testing of Umbraco pages is supported using [Escc.Umbraco.ContentExperiments](https://github.com/east-sussex-county-council/Escc.Umbraco.ContentExperiments) and [Google Analytics Experiments](https://developers.google.com/analytics/solutions/experiments).

Each controller that needs to support Google Analytics experiments should create an instance of `ContentExperimentSettingsService`, and usually pass it to `BaseViewModelBuilder.PopulateBaseViewModel()`. This checks for an experiment configured in Umbraco, and then sets the `ContentExperimentPageSettings` property of the model.

	public new async Task<ActionResult> Index(RenderModel model)
    {
		var viewModel = new MyCustomModel();

		var templateRequest = new EastSussexGovUKTemplateRequest(Request);
		var modelBuilder = new BaseViewModelBuilder(templateRequest);
		await modelBuilder.PopulateBaseViewModel(viewModel, model.Content, 
				new ContentExperimentSettingsService(),  
				null,
                UmbracoContext.Current.InPreviewMode);

        return CurrentTemplate(viewModel);
	}  

This project has layout views (`UmbracoDesktop.cshtml` and `UmbracoFullScreen.cshtml`) and which inherit from the sitewide layout views provided by the [Escc.EastSussexGovUK](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK/) project, and add support for rendering the Google Analytics Experiments code using the `_ContentExperiment.cshtml` partial view. So long as the inherited layout views are configured in `web.config` the code is added to the page in the right place.

	<configuration>
	  <configSections>
	    <sectionGroup name="Escc.EastSussexGovUK">
	      <section name="GeneralSettings" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
	    </sectionGroup>
	  </configSections>
	
	  <Escc.EastSussexGovUK>
	    <GeneralSettings>
	      <add key="DesktopMvcLayout" value="~/views/layouts/UmbracoDesktop.cshtml" />
	      <add key="PlainMvcLayout" value="~/views/layouts/UmbracoPlain.cshtml" />
	      <add key="FullScreenMvcLayout" value="~/views/layouts/UmbracoFullScreen.cshtml" />
	    </GeneralSettings>
	  </Escc.EastSussexGovUK>
	</configuration> 