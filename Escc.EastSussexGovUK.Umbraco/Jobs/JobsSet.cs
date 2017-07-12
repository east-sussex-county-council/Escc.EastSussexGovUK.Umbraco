using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// A set of jobs to be handled by a set of jobs pages
    /// </summary>
    public enum JobsSet
    {
        /// <summary>
        /// Jobs available to the general public
        /// </summary>
        PublicJobs,

        /// <summary>
        /// Jobs available only to employees eligible for redeployment
        /// </summary>
        RedeploymentJobs
    }
}