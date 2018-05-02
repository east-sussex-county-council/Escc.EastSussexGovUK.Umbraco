using Escc.EastSussexGovUK.Umbraco.Jobs;
using Escc.EastSussexGovUK.Umbraco.Jobs.HtmlFormatters;
using Escc.EastSussexGovUK.Umbraco.RichTextHtmlFormatters;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestFixture]
    public class HouseStyleDateFormatterTests
    {
        [Test]
        public void AmericanDateIsPutInASensibleOrder()
        {
            var text = "A date like April 1 2017 is always wrong";

            var result = new HouseStyleDateFormatter().FormatHtml(text);

            Assert.AreEqual("A date like 1 April 2017 is always wrong", result);
        }

        [Test]
        public void CommaInDatesAreRemoved()
        {
            var text = "This is it, text with commas, and Sunday, 1 July, 2018";

            var result = new HouseStyleDateFormatter().FormatHtml(text);

            Assert.AreEqual("This is it, text with commas, and Sunday 1 July 2018", result);
        }

        [Test]
        public void RemoveOrdinalsFromDates()
        {
            var text = "This is the 1st Test, about 1st June 2019";

            var result = new HouseStyleDateFormatter().FormatHtml(text);

            Assert.AreEqual("This is the 1st Test, about 1 June 2019", result);
        }

        [Test]
        public void RemoveLeadingZeroesFromDates()
        {
            var text = "Phone 0345 01 02 03 from 09 June 2019";

            var result = new HouseStyleDateFormatter().FormatHtml(text);

            Assert.AreEqual("Phone 0345 01 02 03 from 9 June 2019", result);
        }
    }
}
