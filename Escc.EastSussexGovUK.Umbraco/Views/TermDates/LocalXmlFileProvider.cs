using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Xml.XPath;

namespace Escc.EastSussexGovUK.Umbraco.Views.TermDates
{
    /// <summary>
    /// Reads term dates data from a local XML file, the path of which is specified in web.config
    /// </summary>
    public class LocalXmlFileProvider : ITermDatesDataProvider
    {
        /// <summary>
        /// Gets the term dates data as a <see cref="XPathDocument" />.
        /// </summary>
        /// <returns></returns>
        public IXPathNavigable GetXPathData()
        {
            if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["TermDatesXml"])) throw new ConfigurationErrorsException("Path to term dates XML data not found in appSettings/TermDatesXml");

            return new XPathDocument(HostingEnvironment.MapPath(ConfigurationManager.AppSettings["TermDatesXml"]));
        }
    }
}