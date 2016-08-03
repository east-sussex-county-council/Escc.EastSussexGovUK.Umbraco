using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Umbraco.Models;

namespace Escc.EastSussexGovUK.Umbraco.Services
{
    /// <summary>
    /// Build a <see cref="TaskViewModel"/>
    /// </summary>
    interface ITaskViewModelBuilder
    {
        /// <summary>
        /// Builds the model.
        /// </summary>
        /// <returns></returns>
        TaskViewModel BuildModel();
    }
}
