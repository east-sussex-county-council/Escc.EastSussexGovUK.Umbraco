﻿using System;
using Umbraco.Inception.Attributes;
using Escc.Umbraco.PropertyEditors;
using Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.RightsOfWayDeposits
{
    /// <summary>
    /// Definition for the Umbraco 'Rights of way deposits CSV download' document type
    /// </summary>
    /// <seealso cref="UmbracoGeneratedBase" />
    [UmbracoContentType("Rights of way Section 31 deposits CSV download", "RightsOfWayDepositsCsv", null, true, allowAtRoot: false, icon: BuiltInUmbracoContentTypeIcons.IconGrid, 
        Description = "Section 31 (6) of the Highways Act 1980 enables landowners to protect their land from gaining public rights of way through use by the public by depositing a map with the council.")]
    public class RightsOfWayDepositsCsvDocumentType : UmbracoGeneratedBase
    {
        [UmbracoProperty("Page URL", "umbracoUrlName", BuiltInUmbracoDataTypes.Textbox, sortOrder: 0)]
        public Uri UmbracoUrlName { get; set; }

        [UmbracoProperty("Author notes", "authorNotes", PropertyEditorAliases.RichTextPropertyEditor, RichTextAuthorNotesDataType.DataTypeName, sortOrder: 2)]
        public string AuthorNotes { get; set; }

        [UmbracoProperty("Copy of unpublished date (do not edit)", "unpublishAt", BuiltInUmbracoDataTypes.DateTime, sortOrder: 103)]
        public string UnpublishAt { get; set; }
    }
}