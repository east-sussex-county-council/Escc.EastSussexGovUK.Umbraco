using Escc.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    class MockLinkFormatter : IHtmlLinkFormatter
    {
        public string AbbreviateUrl(Uri urlToAbbreviate)
        {
            return "replaced link";
        }

        public string AbbreviateUrl(Uri urlToAbbreviate, Uri baseUrl)
        {
            throw new NotImplementedException();
        }

        public string AbbreviateUrl(Uri urlToAbbreviate, Uri baseUrl, int maximumLength)
        {
            throw new NotImplementedException();
        }

        public string TextOutsideLinks(string text)
        {
            throw new NotImplementedException();
        }
    }
}
