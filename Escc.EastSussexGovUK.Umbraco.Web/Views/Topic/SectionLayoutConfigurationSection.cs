using System.Configuration;

namespace Escc.EastSussexGovUK.Umbraco.Web.Views.Topic
{
    /// <summary>
    /// Configuration section to register details of available section layouts
    /// </summary>
    public class SectionLayoutConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// A web.config configuration section type to list section layouts
        /// </summary>
        /// <value>The section layouts.</value>
        [ConfigurationProperty("SectionLayouts", IsDefaultCollection = true)]
        public SectionLayoutConfigurationCollection SectionLayouts
        {
            get
            {
                return (SectionLayoutConfigurationCollection)base["SectionLayouts"];
            }
        }
    }

}
