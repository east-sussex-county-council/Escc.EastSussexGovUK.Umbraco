using Escc.EastSussexGovUK.Umbraco.Jobs;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs
{
    public interface IJobResultsParser
    {
        /// <summary>
        /// Parses jobs from the HTML stream.
        /// </summary>
        /// <param name="htmlStream">The HTML stream.</param>
        /// <returns></returns>
        Task<IList<Job>> Parse(Stream htmlStream);
    }
}