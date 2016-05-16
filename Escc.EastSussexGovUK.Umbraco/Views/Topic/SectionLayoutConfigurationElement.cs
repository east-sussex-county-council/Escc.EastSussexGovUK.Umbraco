using System;
using System.Configuration;

namespace Escc.EastSussexGovUK.Umbraco.Views.Topic
{
    /// <summary>
    /// web.config element to register a custom section layout
    /// </summary>
    public class SectionLayoutConfigurationElement : ConfigurationElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SectionLayoutConfigurationElement"/> class.
        /// </summary>
        public SectionLayoutConfigurationElement()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionLayoutConfigurationElement"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="displayControl">The display control.</param>
        public SectionLayoutConfigurationElement(string name, string displayName, string displayControl)
        {
            Name = name;
            DisplayName = displayName;
            DisplayControl = displayControl;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionLayoutConfigurationElement"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="displayControl">The display control.</param>
        /// <param name="editControl">The edit control.</param>
        public SectionLayoutConfigurationElement(string name, string displayName, string displayControl, string editControl)
        {
            Name = name;
            DisplayName = displayName;
            DisplayControl = displayControl;
            EditControl = editControl;
        }

        /// <summary>
        /// Gets or sets the name, used as a key to identify this section layout
        /// </summary>
        /// <value>The name.</value>
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the display name shown to editors when choosing a layout
        /// </summary>
        /// <value>The display name.</value>
        [ConfigurationProperty("displayName", IsRequired = true)]
        public string DisplayName
        {
            get
            {
                return (string)this["displayName"];
            }
            set
            {
                this["displayName"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the path to the usercontrol for the layout in published mode
        /// </summary>
        /// <value>The control path.</value>
        [ConfigurationProperty("displayControl", IsRequired = true)]
        public string DisplayControl
        {
            get
            {
                return (string)this["displayControl"];
            }
            set
            {
                this["displayControl"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the path to the usercontrol for the layout in edit mode. If not set, value is assumed to be the same as <seealso cref="DisplayControl"/>
        /// </summary>
        /// <value>The control path.</value>
        [ConfigurationProperty("editControl", IsRequired = false)]
        public string EditControl
        {
            get
            {
                string editControl = (string)this["editControl"];

                return String.IsNullOrEmpty(editControl) ? DisplayControl : editControl;
            }
            set
            {
                this["editControl"] = value;
            }
        }
    }

}
