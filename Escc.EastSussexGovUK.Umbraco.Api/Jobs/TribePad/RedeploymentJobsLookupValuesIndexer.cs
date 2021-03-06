﻿using System;
using System.Configuration;
using Escc.EastSussexGovUK.Umbraco.Api.Jobs.Examine;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using Escc.Net;
using Escc.Net.Configuration;
using Examine;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.TribePad
{
    /// <summary>
    /// Creates an Examine indexes of lookup values jobs posted to TribePad, based on the TribePad URLs in the 'TribePad...' configuration settings
    /// </summary>
    /// <seealso cref="BaseLookupValuesIndexer" />
    public class RedeploymentJobsLookupValuesIndexer : BaseLookupValuesIndexer
    {
        private static readonly Uri LookupValuesApiUrl = new Uri(ConfigurationManager.AppSettings["TribePadLookupValuesUrl"]);

        /// <summary>
        /// Initializes a new instance of the <see cref="RedeploymentJobsLookupValuesIndexer"/> class.
        /// </summary>
        public RedeploymentJobsLookupValuesIndexer() : base(new JobsLookupValuesFromTribePad(LookupValuesApiUrl, new LookupValuesFromTribePadBuiltInFieldParser(), new LookupValuesFromTribePadCustomFieldParser(), new TribePadWorkPatternSplitter(), new ConfigurationProxyProvider()))
        {
        }

        /// <summary>
        /// Gets the jobs data provider used to check how many jobs match each lookup value
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// This cannot be injected into the constructor as, when the provider uses Examine, it is too early in the lifecycle and crashes IIS!
        /// </remarks>
        protected override IJobsDataProvider GetJobsDataProvider()
        {
            return new JobsDataFromExamine(ExamineManager.Instance.SearchProviderCollection["RedeploymentJobsSearcher"], null, new SalaryRangeLuceneQueryBuilder(), null);
        }
    }
}