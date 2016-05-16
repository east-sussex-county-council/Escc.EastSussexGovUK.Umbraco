using System;
using System.Configuration;

namespace Escc.EastSussexGovUK.Umbraco.Views.Topic
{
    /// <summary>
    /// A list of section layouts registered in a web.config section
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface")]
    public class SectionLayoutConfigurationCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Gets the type of the <see cref="T:System.Configuration.ConfigurationElementCollection"/>.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The <see cref="T:System.Configuration.ConfigurationElementCollectionType"/> of this collection.
        /// </returns>
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
        }

        /// <summary>
        /// When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new SectionLayoutConfigurationElement();
        }

        /// <summary>
        /// Gets the element key for a specified configuration element when overridden in a derived class.
        /// </summary>
        /// <param name="element">The <see cref="T:System.Configuration.ConfigurationElement"/> to return the key for.</param>
        /// <returns>
        /// An <see cref="T:System.Object"/> that acts as the key for the specified <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((SectionLayoutConfigurationElement)element).Name;
        }


        /// <summary>
        /// Gets or sets the <see cref="SectionLayoutConfigurationElement"/> at the specified index.
        /// </summary>
        /// <value></value>
        public SectionLayoutConfigurationElement this[int index]
        {
            get
            {
                return (SectionLayoutConfigurationElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        /// <summary>
        /// Gets the <see cref="SectionLayoutConfigurationElement"/> with the specified name.
        /// </summary>
        /// <value></value>
        public new SectionLayoutConfigurationElement this[string Name]
        {
            get
            {
                return (SectionLayoutConfigurationElement)BaseGet(Name);
            }
        }

        /// <summary>
        /// Gets the index of the specified layout.
        /// </summary>
        /// <param name="layout">The layout.</param>
        /// <returns></returns>
        public int IndexOf(SectionLayoutConfigurationElement layout)
        {
            if (layout == null) throw new ArgumentNullException("layout");
            return BaseIndexOf(layout);
        }

        /// <summary>
        /// Adds the specified layout.
        /// </summary>
        /// <param name="layout">The layout.</param>
        public void Add(SectionLayoutConfigurationElement layout)
        {
            if (layout == null) throw new ArgumentNullException("layout");
            BaseAdd(layout);
        }

        /// <summary>
        /// Removes the specified layout.
        /// </summary>
        /// <param name="layout">The layout.</param>
        public void Remove(SectionLayoutConfigurationElement layout)
        {
            if (layout == null) throw new ArgumentNullException("layout");
            if (BaseIndexOf(layout) >= 0)
                BaseRemove(layout.Name);
        }

        /// <summary>
        /// Removes the layout at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        /// <summary>
        /// Removes the layout with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        public void Remove(string name)
        {
            BaseRemove(name);
        }

        /// <summary>
        /// Clears all layouts in the section
        /// </summary>
        public void Clear()
        {
            BaseClear();
        }

    }
}
