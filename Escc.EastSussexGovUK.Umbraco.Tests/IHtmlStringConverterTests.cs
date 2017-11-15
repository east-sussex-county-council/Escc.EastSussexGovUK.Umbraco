using Escc.EastSussexGovUK.Umbraco.WebApi;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestFixture]
    public class IHtmlStringConverterTests
    {
        [Test]
        public void JobDeserialisesSuccessfully()
        {
            var job = JsonConvert.DeserializeObject<Job>(Properties.Resources.JobAdvertJson, new[] { new IHtmlStringConverter() });

            Assert.IsTrue(job.AdditionalInformationHtml.ToHtmlString().Contains("Enhanced DBS"));
        }

    }
}
