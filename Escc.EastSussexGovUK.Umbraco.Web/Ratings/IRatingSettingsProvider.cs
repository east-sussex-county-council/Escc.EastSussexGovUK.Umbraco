using Escc.EastSussexGovUK.Umbraco.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Web.Ratings
{
    /// <summary>
    /// Gets the settings for a page rating service
    /// </summary>
    public interface IRatingSettingsProvider
    {
        /// <summary>
        /// Reads the rating settings.
        /// </summary>
        /// <returns></returns>
        RatingSettings ReadRatingSettings();
    }
}
