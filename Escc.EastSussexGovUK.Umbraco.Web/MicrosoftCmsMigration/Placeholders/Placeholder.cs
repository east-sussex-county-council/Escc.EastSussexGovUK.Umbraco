namespace Escc.EastSussexGovUK.Umbraco.Web.MicrosoftCmsMigration.Placeholders
{
    /// <summary>
    ///  An Umbraco property which looks like a Microsoft CMS placeholder
    /// </summary>
    public class Placeholder
    {
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the value as string, used with placeholders containing XML.
        /// </summary>
        /// <value>
        /// The XML as string.
        /// </value>
        public string XmlAsString { 
            get { return Value != null ? Value.ToString() : null; } 
            set { Value = value; } 
        }
    }
}