using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using Exceptionless;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.TribePad
{
    public class LookupValuesFromTribePadCustomFieldParser : IJobLookupValuesParser
    {
        /// <summary>
        /// Parses the lookup values, eg working patterns.
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
                var lookupValuesXml = xml.Root.Elements("custom_fields").SingleOrDefault(x => HttpUtility.HtmlDecode(x.Element("label").Value) == fieldName);

                if (lookupValuesXml != null)
                {
                    foreach (var optionsXml in lookupValuesXml.Elements("options"))
                    {
                        lookupValues.Add(new JobsLookupValue()
                        {
                            FieldId = lookupValuesXml.Element("question_id")?.Value,
                            LookupValueId = optionsXml.Element("id").Value,
                            Text = HttpUtility.HtmlDecode(optionsXml.Element("label").Value)
                        });
                    }
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