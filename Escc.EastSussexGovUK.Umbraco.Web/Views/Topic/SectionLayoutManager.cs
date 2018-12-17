using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using Escc.EastSussexGovUK.Umbraco.Web.MicrosoftCmsMigration.Placeholders;

namespace Escc.EastSussexGovUK.Umbraco.Web.Views.Topic
{
    /// <summary>
    /// Helper class for working with configurable sections in CMS templates
    /// </summary>
    public static class SectionLayoutManager
    {
        /// <summary>
        /// Parses the placeholder number from the placeholder name, where there is a series of similarly-named placeholders.
        /// </summary>
        /// <param name="placeholderName">Name of the placeholder.</param>
        /// <returns></returns>
        public static string ParsePlaceholderNumber(string placeholderName)
        {
            Match m = Regex.Match(placeholderName, "[0-9]+$");
            if (m.Captures.Count > 0)
            {
                return m.Captures[0].Value.TrimStart('0');
            }
            return String.Empty;
        }

        /// <summary>
        /// Gets the selected section layout.
        /// </summary>
        /// <param name="placeholders">The placeholders.</param>
        /// <param name="sectionPlaceholder">The section placeholder.</param>
        /// <param name="defaultLayout">The default layout.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown if section placeholder does not contain a value</exception>
        public static string GetSelectedSectionLayout(PlaceholderCollection placeholders, string sectionPlaceholder, string defaultLayout)
        {
            if (placeholders == null) throw new ArgumentNullException("placeholders");
            
            // Make sure we know which usercontrol to load for the section
            string selectedLayout = String.Empty;

            // Hopefully the layout name was saved with the page in a placeholder
            if (placeholders.ContainsKey(sectionPlaceholder) && placeholders[sectionPlaceholder].Value != null)
            {
                selectedLayout = placeholders[sectionPlaceholder].Value.ToString();
            }

            // If not, the page probably hasn't been saved since this code was introduced, so default to 
            // whichever layout was previously hard-coded into the template.
            // This is for published/unpublished mode. The default placeholder for edit mode is set earlier in SetDefaultSectionLayout, but it's done
            // in a way that only works when editing.
            if (selectedLayout.Length == 0)
            {
                selectedLayout = defaultLayout;
            }

            return selectedLayout;
        }

        /// <summary>
        /// Gets the path to the usercontrol to load for the selected layout
        /// </summary>
        /// <param name="layoutsConfigSectionName">Name of the layouts config section.</param>
        /// <param name="selectedLayout">The selected layout.</param>
        /// <returns></returns>
        public static string UserControlPath(string layoutsConfigSectionName, string selectedLayout)
        {
            // Check we have the data in web.config we need
            var sectionLayouts = ConfigurationManager.GetSection(layoutsConfigSectionName) as SectionLayoutConfigurationSection;
            if (sectionLayouts == null) throw new ConfigurationErrorsException("Could not find <" + layoutsConfigSectionName + "> configuration section.");
            
            // Try match on key, if saved in Microsoft CMS
            if (sectionLayouts.SectionLayouts[selectedLayout] != null)
            {
                return sectionLayouts.SectionLayouts[selectedLayout].DisplayControl;
            }

            // Try match on display name, if saved in Umbraco
            var array = new ConfigurationElement[sectionLayouts.SectionLayouts.Count]; 
            sectionLayouts.SectionLayouts.CopyTo(array, 0);
            var displayNameMatch = new List<ConfigurationElement>(array).FirstOrDefault(element => ((SectionLayoutConfigurationElement)element).DisplayName == selectedLayout);
            if (displayNameMatch != null) return ((SectionLayoutConfigurationElement)displayNameMatch).DisplayControl;

            throw new ArgumentException("Could not find " + selectedLayout + " in <" + layoutsConfigSectionName + " /> configuration section.");
        }

        /// <summary>
        /// Adds a suffix to the id of a placeholder control based on the suffix of its bound placeholder
        /// </summary>
        /// <param name="placeholder"></param>
        /// <param name="placeholderToBind"></param>
        public static void AddSuffixToPlaceholderId(Control placeholder, string placeholderToBind)
        {
            if (placeholder == null) throw new ArgumentNullException("placeholder");

            placeholder.ID += ParsePlaceholderNumber(placeholderToBind);
        }

    }
}
