using System;
using Escc.EastSussexGovUK.Umbraco.Jobs.TalentLink;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestFixture]
    public class RemoveDuplicateTextFormatterTests
    {
        [Test]
        public void RemoveClosingDateWithDay()
        {
            var parsedClosingDate = "Closing date: Sunday 23 April 2017";

            var result = new RemoveDuplicateTextFormatter(parsedClosingDate).FormatHtml(Properties.Resources.ClosingDateInBody1);

            Assert.AreEqual(Properties.Resources.ClosingDateInBody1.Replace("<p><span><span>Closing date: Sunday 23 April 2017</span></span></p>", String.Empty), result);
        }
    }
}
