namespace Escc.EastSussexGovUK.Umbraco.Services
{
    /// <summary>
    /// A way to get an instance of a view model
    /// </summary>
    public interface IViewModelBuilder<out T>
    {
        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <returns></returns>
        T BuildModel();
    }
}