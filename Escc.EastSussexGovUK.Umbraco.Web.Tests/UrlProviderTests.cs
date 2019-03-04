using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Escc.EastSussexGovUK.Umbraco.Web.Views.TermDates;

namespace Escc.EastSussexGovUK.Umbraco.Web.Tests
{
    [TestFixture]
    public class UrlProviderTests
    {
        [Test]
        public void ResponseOtherThanOkReturnsEmptyXml()
        {
            var provider = new UrlProvider(new Uri("https://example.org/not-found"), null, null);

            var data = provider.GetXPathData();
            
            Assert.IsInstanceOf(typeof(IXPathNavigable), data);
        }
    }
}
