# Privacy notice

The 'Privacy notice' document type and template standardises the way we publish privacy notices for forms and applications, ensuring that all the information required for [GDPR](https://gdpr-info.eu/) compliance is included.

## Bootstrap tabs

The 'Privacy notice' template has similar requirements to the [Guide](Guide.md) template, in that it should be possible to print several separate pages together as one document. 

'Privacy notice' takes a different approach to implementing this. The content edited and rendered as a single page, then divided into tabs using [tabs support in Bootstrap](https://getbootstrap.com/docs/4.0/components/navs/#tabs). It requires JavaScript to be enabled.

## Editor notes

The privacy notice document type uses [uEditorNotes](https://www.nuget.org/packages/tooorangey.uEditorNotes/) to display hard-coded text from the template to editors while they're editing. This means that the hard-coded text is specified twice: once in the data type configured in the Umbraco back-office, and once in the `~\Views\PrivacyNotice.cshtml` template file.

The display of properties using the `uEditorNotes` property editor is updated by `~\App_Plugins\Escc.UmbracoBranding\branding-v2.css`, which is enabled by `package.manifest` in the same folder. 