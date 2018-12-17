using System.Web;
using Escc.EastSussexGovUK.Umbraco.Jobs.TalentLink;

namespace Escc.EastSussexGovUK.Umbraco.Web.Jobs
{
    /// <summary>
    /// A standard Lumesse TalentLink component, which may be configurable in the TalentLink back-office
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Models.BaseViewModel" />
    public class JobsComponentViewModel : BaseJobsViewModel
    {   
        /// <summary>
        /// Gets or sets the URL of the script to embed the component in the page.
        /// </summary>
        public TalentLinkUrl ScriptUrl { get; set; }

        /// <summary>
        /// Gets or sets whether this component is a form.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is a form; otherwise, <c>false</c>.
        /// </value>
        public bool IsForm { get; set; }

        /// <summary>
        /// Gets or sets the HTML content below the component.
        /// </summary>
        /// <value>
        /// The content below component.
        /// </value>
        public IHtmlString ContentBelowComponent { get; set; }
    }
}