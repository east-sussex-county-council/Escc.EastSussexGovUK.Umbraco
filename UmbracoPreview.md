# Customising Umbraco page previews

This project has layout views (`UmbracoDesktop.cshtml` and `UmbracoFullScreen.cshtml`) and which inherit from the sitewide layout views provided by the [Escc.EastSussexGovUK](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK/) project. These must be configured in `web.config`:

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

These layout views load the `~\Views\Layouts\_CmsPreview.cshtml` partial view, which in turn loads `~\css\umbraco-preview.css` and `~\js\umbraco-preview.js` if a page is being previewed. Any of these three files can be modified to add code that affects all pages being previewed in Umbraco.
