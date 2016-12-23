using System.Collections.Generic;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.Umbraco.PropertyTypes;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// The model for the start page of the jobs service
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Models.BaseViewModel" />
    public class JobsHomeViewModel : BaseJobsViewModel
    {
        /// <summary>
        /// Gets or sets the URLs the buttons should link to
        /// </summary>
        public IList<HtmlLink> ButtonNavigation { get; set; }

        /// <summary>
        /// Gets or sets the images to link using <see cref="ButtonNavigation"/>
        /// </summary>
        public IList<Image> ButtonImages { get; set; } 
    }
}