using System;
using System.Web.Mvc;
using System.Web.UI;
using Escc.EastSussexGovUK.Umbraco.Web.MicrosoftCmsMigration;
using Escc.EastSussexGovUK.Umbraco.Web.MicrosoftCmsMigration.Placeholders;
using Escc.EastSussexGovUK.Umbraco.Web.Views.Topic;
using Exceptionless;

namespace Escc.EastSussexGovUK.Umbraco.Web.Views
{
    public partial class StandardTopicPage : ViewUserControl<MicrosoftCmsViewModel>
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            // Need to do this here, not in OnInit, because otherwise the code-behind for the usercontrols
            // doesn't run when you edit a posting and then click "Exit".
            var placeholders = CmsUtilities.Placeholders;
            SetupSections(placeholders, "phDefSection01", "phDefSubtitle01", "phDefContent01", "phDefImage01", "phDefImage04", "phDefImage05", "phDefCaption01", "phDefCaption04", "phDefCaption05", "phDefAltAsCaption01", "phDefAltAsCaption04", "phDefAltAsCaption05", "Normal");
            SetupSections(placeholders, "phDefSection02", "phDefSubtitle02", "phDefContent02", "phDefTopicImageNoWrap01", "phDefImage06", "phDefImage07", "phDefCaption14", "phDefCaption06", "phDefCaption07", "phDefAltAsCaption14", "phDefAltAsCaption06", "phDefAltAsCaption07", "FeaturedImage");
            SetupSections(placeholders, "phDefSection03", "phDefSubtitle03", "phDefContent03", "phDefImage02", "phDefImage08", "phDefImage09", "phDefCaption02", "phDefCaption08", "phDefCaption09", "phDefAltAsCaption02", "phDefAltAsCaption08", "phDefAltAsCaption09", "Alternative");
            SetupSections(placeholders, "phDefSection04", "phDefSubtitle04", "phDefContent04", "phDefImage03", "phDefImage10", "phDefImage11", "phDefCaption03", "phDefCaption10", "phDefCaption11", "phDefAltAsCaption03", "phDefAltAsCaption10", "phDefAltAsCaption11", "Normal");
            SetupSections(placeholders, "phDefSection05", "phDefSubtitle05", "phDefContent05", "phDefTopicImageNoWrap02", "phDefImage12", "phDefImage13", "phDefCaption15", "phDefCaption12", "phDefCaption13", "phDefAltAsCaption15", "phDefAltAsCaption12", "phDefAltAsCaption13", "FeaturedImage");
            SetupSections(placeholders, "phDefSection06", "phDefSubtitle06", "phDefContent06", "phDefImage16", "phDefImage17", "phDefImage18", "phDefCaption16", "phDefCaption17", "phDefCaption18", "phDefAltAsCaption16", "phDefAltAsCaption17", "phDefAltAsCaption18", "Normal");

            // logos at bottom of template - visible only if used
            logos.Visible = (phLogo01.HasContent || phLogo02.HasContent || phLogo03.HasContent || phLogo04.HasContent || phLogo05.HasContent || phLogo06.HasContent || phLogo07.HasContent || phLogo08.HasContent);
        }

        /// <summary>
        /// Sets the placeholder names and values and the layout used for a particular section.
        /// </summary>
        /// <param name="placeholders">The Umbraco fields</param>
        /// <param name="sectionPlaceholder">The section placeholder.</param>
        /// <param name="subtitlePlaceholder">The subtitle placeholder.</param>
        /// <param name="contentPlaceholder">The content placeholder.</param>
        /// <param name="imagePlaceholder1">The 1st image placeholder.</param>
        /// <param name="imagePlaceholder2">The 2nd image placeholder.</param>
        /// <param name="imagePlaceholder3">The 3rd image placeholder.</param>
        /// <param name="captionPlaceholder1">The 1st caption placeholder.</param>
        /// <param name="captionPlaceholder2">The 2nd caption placeholder.</param>
        /// <param name="captionPlaceholder3">The 3rd caption placeholder.</param>
        /// <param name="altAsCaptionPlaceholder1">The 1st alt as caption placeholder.</param>
        /// <param name="altAsCaptionPlaceholder2">The 2nd alt as caption placeholder.</param>
        /// <param name="altAsCaptionPlaceholder3">The 3rd alt as caption placeholder.</param>
        /// <param name="defaultLayout">The default layout.</param>
        private void SetupSections(PlaceholderCollection placeholders, string sectionPlaceholder, string subtitlePlaceholder, string contentPlaceholder, string imagePlaceholder1, string imagePlaceholder2, string imagePlaceholder3, string captionPlaceholder1, string captionPlaceholder2, string captionPlaceholder3, string altAsCaptionPlaceholder1, string altAsCaptionPlaceholder2, string altAsCaptionPlaceholder3, string defaultLayout)
        {
            // Load the usercontrol appropriate to the chosen layout, and pass in all the names of the placeholders it should use
            string selectedLayout = SectionLayoutManager.GetSelectedSectionLayout(placeholders, sectionPlaceholder, defaultLayout);

            try
            {
                Control selectedControl = this.LoadControl(SectionLayoutManager.UserControlPath("Escc.EastSussexGovUK.Umbraco/TopicSectionLayouts", selectedLayout));
                TopicSection sectionControl = selectedControl as TopicSection;

                if (sectionControl != null)
                {
                    sectionControl.PlaceholderToBindSection = sectionPlaceholder;
                    sectionControl.PlaceholderToBindSubtitle = subtitlePlaceholder;
                    sectionControl.PlaceholderToBindContent = contentPlaceholder;
                    sectionControl.PlaceholderToBindImage01 = imagePlaceholder1;
                    sectionControl.PlaceholderToBindImage02 = imagePlaceholder2;
                    sectionControl.PlaceholderToBindImage03 = imagePlaceholder3;
                    sectionControl.PlaceholderToBindCaption01 = captionPlaceholder1;
                    sectionControl.PlaceholderToBindCaption02 = captionPlaceholder2;
                    sectionControl.PlaceholderToBindCaption03 = captionPlaceholder3;
                    sectionControl.PlaceholderToBindAltAsCaption01 = altAsCaptionPlaceholder1;
                    sectionControl.PlaceholderToBindAltAsCaption02 = altAsCaptionPlaceholder2;
                    sectionControl.PlaceholderToBindAltAsCaption03 = altAsCaptionPlaceholder3;
                }
                // Add the section to the page
                this.sections.Controls.Add(selectedControl);
            }
            catch (ArgumentException exception)
            {
                // Expected if an obsolete section has been removed from config, but is still referenced by the page
                exception.ToExceptionless().Submit();
            }
        }

    }
}