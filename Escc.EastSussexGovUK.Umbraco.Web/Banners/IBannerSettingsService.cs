using System.Collections.Generic;

namespace Escc.EastSussexGovUK.Umbraco.Web.Banners
{
    /// <summary>
    /// A service which gets banner display settings from a data source
    /// </summary>
    public interface IBannerSettingsService
    {
        /// <summary>
        /// Reads the banner settings from the data source.
        /// </summary>
        /// <returns></returns>
        IList<Banner> ReadBannerSettings();
    }
}