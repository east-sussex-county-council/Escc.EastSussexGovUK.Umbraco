using System.ComponentModel;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features;
using Escc.Umbraco.PropertyEditors;
using Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.CampaignTemplates
{
    /// <summary>
    /// Content tab for the campaign tiles document type
    /// </summary>
    public class CampaignTilesContentTab : TabBase
    {
        /// <summary>
        /// Gets or sets the images to be linked using <see cref="TileNavigation"/>
        /// </summary>
        [UmbracoProperty("Tile images", "TileImages", BuiltInUmbracoDataTypes.MultipleMediaPicker, sortOrder: 1,
            Description = "Select the images to link to pages selected for tile navigation, below.")]
        public string TileImages { get; set; }

        /// <summary>
        /// Gets or sets the tile navigation.
        /// </summary>
        [UmbracoProperty("Tile navigation", "TileNavigation", BuiltInUmbracoDataTypes.RelatedLinks, sortOrder: 2,
            Description = "The pages and page titles to link to using the tile images selected above.")]
        public string TileNavigation { get; set; }
   
        [UmbracoProperty("Tile 1 description", "Tile1Description", BuiltInUmbracoDataTypes.Textbox, sortOrder: 3,
            Description="Add an optional description to appear below the title of the first tile")]
        public string Tile1Description { get; set; }

        [UmbracoProperty("Tile 2 description", "Tile2Description", BuiltInUmbracoDataTypes.Textbox, sortOrder: 4,
            Description = "Add an optional description to appear below the title of the second tile")]
        public string Tile2Description { get; set; }

        [UmbracoProperty("Tile 3 description", "Tile3Description", BuiltInUmbracoDataTypes.Textbox, sortOrder: 5,
            Description = "Add an optional description to appear below the title of the third tile")]
        public string Tile3Description { get; set; }

        [UmbracoProperty("Tile 4 description", "Tile4Description", BuiltInUmbracoDataTypes.Textbox, sortOrder: 6,
            Description = "Add an optional description to appear below the title of the fourth tile")]
        public string Tile4Description { get; set; }

        [UmbracoProperty("Tile 5 description", "Tile5Description", BuiltInUmbracoDataTypes.Textbox, sortOrder: 7,
            Description = "Add an optional description to appear below the title of the fifth tile")]
        public string Tile5Description { get; set; }

        [UmbracoProperty("Tile 6 description", "Tile6Description", BuiltInUmbracoDataTypes.Textbox, sortOrder: 8,
            Description = "Add an optional description to appear below the title of the sixth tile")]
        public string Tile6Description { get; set; }

        [UmbracoProperty("Tile 7 description", "Tile7Description", BuiltInUmbracoDataTypes.Textbox, sortOrder: 9,
            Description = "Add an optional description to appear below the title of the seventh tile")]
        public string Tile7Description { get; set; }

        [UmbracoProperty("Tile 8 description", "Tile8Description", BuiltInUmbracoDataTypes.Textbox, sortOrder: 10,
            Description = "Add an optional description to appear below the title of the eighth tile")]
        public string Tile8Description { get; set; }

        [UmbracoProperty("Tile 9 description", "Tile9Description", BuiltInUmbracoDataTypes.Textbox, sortOrder: 11,
            Description = "Add an optional description to appear below the title of the ninth tile")]
        public string Tile9Description { get; set; }

        [UmbracoProperty("Tile 10 description", "Tile10Description", BuiltInUmbracoDataTypes.Textbox, sortOrder: 12,
            Description = "Add an optional description to appear below the title of the 10th tile")]
        public string Tile10Description { get; set; }

        [UmbracoProperty("Tile 11 description", "Tile11Description", BuiltInUmbracoDataTypes.Textbox, sortOrder: 13,
            Description = "Add an optional description to appear below the title of the 11th tile")]
        public string Tile11Description { get; set; }

        [UmbracoProperty("Tile 12 description", "Tile12Description", BuiltInUmbracoDataTypes.Textbox, sortOrder: 14,
            Description = "Add an optional description to appear below the title of the 12th tile")]
        public string Tile121Description { get; set; }
    }
}