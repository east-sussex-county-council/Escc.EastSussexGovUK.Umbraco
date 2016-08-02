using Escc.EastSussexGovUK.Umbraco.Models;

namespace Escc.EastSussexGovUK.Umbraco.Services
{
    /// <summary>
    /// A way to get an instance of a <see cref="HomePageItemViewModel"/>
    /// </summary>
    public interface IHomePageItemProvider
    {
        /// <summary>
        /// Gets the home page item.
        /// </summary>
        /// <returns></returns>
        HomePageItemViewModel GetHomePageItem();
    }
}