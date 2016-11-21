using System.Collections.Generic;

namespace Escc.EastSussexGovUK.Umbraco.Models
{
    /// <summary>
    /// Model for an RSS feed
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Models.BaseViewModel" />
    public class RssViewModel<T> : BaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RssViewModel{T}"/> class.
        /// </summary>
        public RssViewModel()
        {
            Items = new List<T>();
        }

        /// <summary>
        /// Gets the items to include in the feed
        /// </summary>
        public IList<T> Items { get; private set; } 
    }
}