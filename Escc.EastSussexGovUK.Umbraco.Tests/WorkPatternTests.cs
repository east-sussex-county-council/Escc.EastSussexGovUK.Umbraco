using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestFixture]
    public class WorkPatternTests
    {
        [Test]
        public void FullAndPartTimeCombined()
        {
            var workPattern = new WorkPattern();
            workPattern.WorkPatterns.Add(WorkPattern.FULL_TIME);
            workPattern.WorkPatterns.Add(WorkPattern.PART_TIME);

            var result = workPattern.ToString();

            Assert.AreEqual("Full or part time", result);
        }

        [Test]
        public void FullTimeAndOtherCombined()
        {
            var workPattern = new WorkPattern();
            workPattern.WorkPatterns.Add(WorkPattern.FULL_TIME);
            workPattern.WorkPatterns.Add("Other");

            var result = workPattern.ToString();

            Assert.AreEqual("Full time, Other", result);
        }


        [Test]
        public void PartTimeAndOthersCombined()
        {
            var workPattern = new WorkPattern();
            workPattern.WorkPatterns.Add(WorkPattern.PART_TIME);
            workPattern.WorkPatterns.Add("Other");
            workPattern.WorkPatterns.Add("Another");

            var result = workPattern.ToString();

            Assert.AreEqual("Part time, Other, Another", result);
        }
    }
}
