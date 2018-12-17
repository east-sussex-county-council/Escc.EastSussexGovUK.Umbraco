using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Umbraco.Jobs;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.JobTransformers
{
    /// <summary>
    /// A transformer makes changes to a <see cref="Job"/>
    /// </summary>
    public interface IJobTransformer
    {
        /// <summary>
        /// Transforms the job.
        /// </summary>
        /// <param name="job">The job.</param>
        void TransformJob(Job job);
    }
}
