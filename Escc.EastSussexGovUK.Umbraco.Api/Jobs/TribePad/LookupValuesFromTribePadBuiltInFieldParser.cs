using System;
using System.Collections.Generic;
using System.Web;
using System.Xml.Linq;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using Exceptionless;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.TribePad
{
    public class LookupValuesFromTribePadBuiltInFieldParser : IJobLookupValuesParser
    {
        /// <summary>
        /// Parses the lookup values, eg job types.
        /// </summary>
        /// <param name="sourceData">The XML.</param>
        /// <param name="fieldName">Name of the field containing the lookup values.</param>
        /// <returns></returns>
        public IList<JobsLookupValue> ParseLookupValues(string sourceData, string fieldName)
        {
            try
            {
                var xml = XDocument.Parse(sourceData);
                var lookupValues = new List<JobsLookupValue>();
                var lookupValuesXml = xml.Root.Element(fieldName).Elements();

                foreach (var valueXml in lookupValuesXml)
                {
                    lookupValues.Add(new JobsLookupValue()
                    {
                        LookupValueId = valueXml.Element("id").Value,
                        Text = HttpUtility.HtmlDecode(valueXml.Element("name").Value)
                    });
                }

                return lookupValues;
            }
            catch (Exception exception)
            {
                exception.Data.Add("fieldName", fieldName);
                exception.ToExceptionless().Submit();
                return null;
            }
        }
    }
}