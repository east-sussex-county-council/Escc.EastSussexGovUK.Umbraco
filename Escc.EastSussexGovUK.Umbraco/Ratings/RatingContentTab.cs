using Escc.Umbraco.PropertyEditors.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.Ratings
{
    /// <summary>
    /// Content tab for the 'Rating' document type in Umbraco
    /// </summary>
    public class RatingContentTab : TabBase
    {
        [UmbracoProperty("Where to display it?", "WhereToDisplayIt", MultiNodeTreePickerDataType.PropertyEditorAlias, MultiNodeTreePickerDataType.DataTypeName, description: "Select the target pages on which to display this rating", sortOrder: 3)]
        public string WhereToDisplayIt { get; set; }

        [UmbracoProperty("URL to rate as poor", "RatingUrlPoor", RatingUrlDataType.PropertyEditorAlias, RatingUrlDataType.DataTypeName, sortOrder: 9,
            description: "Copy and paste the link to click to rate a page as poor.")]
        public Uri RatingUrlPoor { get; set; }

        [UmbracoProperty("URL to rate as adequate", "RatingUrlAdequate", RatingUrlDataType.PropertyEditorAlias, RatingUrlDataType.DataTypeName, sortOrder: 10,
            description: "Copy and paste the link to click to rate a page as adequate.")]
        public Uri RatingUrlAdequate { get; set; }

        [UmbracoProperty("URL to rate as good", "RatingUrlGood", RatingUrlDataType.PropertyEditorAlias, RatingUrlDataType.DataTypeName, sortOrder: 11,
            description: "Copy and paste the link to click to rate a page as good.")]
        public Uri RatingUrlGood { get; set; }

        [UmbracoProperty("URL to rate as excellent", "RatingUrlExcellent", RatingUrlDataType.PropertyEditorAlias, RatingUrlDataType.DataTypeName, sortOrder: 12,
            description: "Copy and paste the link to click to rate a page as excellent.")]
        public Uri RatingUrlExcellent { get; set; }

    }
}
