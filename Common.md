# Common features across all Umbraco document types

## Transforming Elibrary links

Our website may contain many hundreds of links to the elibrary, and when the provider for the elibrary changes all of those links need to be replaced at once. To make that manageable we instead recommend linking to our own consistent URLs in the back-office, which are then transformed to the format required by the current elibrary provider before they are displayed to users.

Any properties that might include an external link should be passed through an `IElibraryProxyLinkConverter` before being displayed. This is usually `ElibraryProxyLinkConverter`. Both types are from the `Escc.Elibrary` NuGet package, which is available on our private feed.

*  `ElibraryUrlTransformer` wraps this for use with `Escc.Umbraco.PropertyTypes.RelatedLinksService`.
*  `ElibraryLinkFormatter` wraps this for use with `Escc.Umbraco.PropertyEditors.RichTextPropertyEditor` (no further configuration is required here - it is discovered automatically).

The supported URL formats are documented in the [Escc.Elibrary](https://github.com/east-sussex-county-council/Escc.Elibrary) project.