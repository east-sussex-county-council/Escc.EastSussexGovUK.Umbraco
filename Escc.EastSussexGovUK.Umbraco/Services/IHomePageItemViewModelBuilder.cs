using Escc.EastSussexGovUK.Umbraco.Models;

namespace Escc.EastSussexGovUK.Umbraco.Services
{
    /// <summary>
    /// A way to get an instance of a <see cref="HomePageItemViewModel"/>
    /// </summary>
    public interface IHomePageItemViewModelBuilder
    {
        /// <summary>
        /// Gets the home page item.
        /// </summary>
        /// <returns></returns>
        HomePageItemViewModel BuildModel();
    }
}