using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Models
{
    public class TalentLinkUrl
    {
        private readonly string _scriptUrl;
        private readonly NameValueCollection _queryString;

        public TalentLinkUrl(string scriptUrl)
        {
            _scriptUrl = scriptUrl;
            _queryString = HttpUtility.ParseQueryString(new Uri(scriptUrl).Query);
        }

        public Uri ScriptUrl { get { return new Uri(_scriptUrl);} }

        public Uri LinkUrl { get { return new Uri(_scriptUrl.Replace("laydisplayrapido.cfm", "jsoutputinitrapido.cfm").Replace("&browserchk=no", String.Empty)); }  }

        public string Id { get { return _queryString["id"]; } }

        public string Mask { get { return _queryString["mask"]; } }
    }
}