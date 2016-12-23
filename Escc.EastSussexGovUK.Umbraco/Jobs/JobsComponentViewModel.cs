using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.Umbraco.PropertyTypes;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
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
    }
}