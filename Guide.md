# Guide

The `Guide` document type acts as a container for its child type, `Guide step`. It is not designed to be displayed by itself, and normally redirects to its first child page.

## Creating the document types

The `Guide` document type only has one required property:

*  Create a data type called `Section navigation` using the `Radio button list` property editor, with two pre-values: `Bulleted list` and `Numbered list with 'next' and 'previous' buttons`.
*  Create a property called `Section navigation` with the alias `SectionNavigation_Navigation`, based on the `Section navigation` data type.

A `Guide` was intended to be a sequential walkthrough of a particular topic area, so by default it uses a numbered list and 'Previous' and 'Next' navigation. However, it has become used as a container for any section, sequential or otherwise, therefore it also supports bulleted navigation without the 'Previous' and 'Next' navigation. `Guide step` is an ordinary Umbraco document type and template, except that it reads this setting from the `Section navigation` property on its parent `Guide` page.

To create the `Guide step` document type create a tab called `Content` with the following properties:

*  A property called `Content` with the alias `content_Content`, and using the `Rich text editor (ESCC)` property editor from [Escc.Umbraco.PropertyEditors](https://github.com/east-sussex-county-council/Escc.Umbraco.PropertyEditors) configured to allow the style selector, bold, bulleted and numbered lists, links and tables. Include the `TinyMCE-Content`, `TinyMCE-StyleSelector-Embed` and `TinyMCE-StyleSelector-Headings` stylesheets.
*  A property called `Related links` with the alias `relatedLinks_Content` using the `Related links` data type.
*  A property called `Partner images` with the alias `partnerImages_Content` using the `Multiple media picker` data type.

You should also add the standard fields for [latest](Latest.md), [social media](SocialMedia.md) and [East Sussex 1Space and ESCIS](1SpaceESCIS.md). 


## Printing a guide

We expect that people will sometimes want to print every child page of a `Guide` and so, when the URL for the `Guide` includes the suffix `/print`, it will gather the content from all of its child pages together, display it on its own template, and launch the print dialog using `guide.js`.   

`GuidePrintViewContentFinder` looks for `/print` after the URL of a `Guide` and recognises it as a request for the `Guide`. `GuidePrintViewEventHandler` registers `GuidePrintViewContentFinder` so that it can do this. `GuideController` then checks again for `/print` and decides whether to render the print view or redirect to the first child page.