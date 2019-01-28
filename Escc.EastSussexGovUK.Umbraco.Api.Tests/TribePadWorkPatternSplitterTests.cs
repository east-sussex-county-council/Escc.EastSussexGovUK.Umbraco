using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Umbraco.Api.Jobs.TribePad;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Api.Tests
{
    [TestFixture]
    public class TribePadWorkPatternSplitterTests
    {
        [Test]
        public void FullTimePartTimeSplitOnOr()
        {
            var workPatterns = new List<JobsLookupValue>()
            {
                new JobsLookupValue() { Text = "Full Time or Part Time"}
            };
            var splitter = new TribePadWorkPatternSplitter();

            var results = splitter.SplitWorkPatterns(workPatterns);

            Assert.AreEqual(2, results.Count);
            Assert.AreEqual(1, results.Count(x => x.Text == "Full Time"));
            Assert.AreEqual(1, results.Count(x => x.Text == "Part Time"));
        }

        [Test]
        public void FullTimeTermTimeSplitOnHyphen()
        {
            var workPatterns = new List<JobsLookupValue>()
            {
                new JobsLookupValue() { Text = "Term Time only - Full Time"}
            };
            var splitter = new TribePadWorkPatternSplitter();

            var results = splitter.SplitWorkPatterns(workPatterns);

            Assert.AreEqual(2, results.Count);
            Assert.AreEqual(1, results.Count(x => x.Text == "Full Time"));
            Assert.AreEqual(1, results.Count(x => x.Text == "Term Time only"));
        }

        [Test]
        public void SingleWorkPatternNotSplit()
        {
            var workPatterns = new List<JobsLookupValue>()
            {
                new JobsLookupValue() { Text = "Full Time"}
            };
            var splitter = new TribePadWorkPatternSplitter();

            var results = splitter.SplitWorkPatterns(workPatterns);

            Assert.AreEqual(1, results.Count);
        }

        [Test]
        public void MixedCollectionReturnsSingleAndSplitValues()
        {
            var workPatterns = new List<JobsLookupValue>()
            {
                new JobsLookupValue() { Text = "Full Time or Part Time"},
                new JobsLookupValue() { Text = "Other" }
            };
            var splitter = new TribePadWorkPatternSplitter();

            var results = splitter.SplitWorkPatterns(workPatterns);

            Assert.AreEqual(3, results.Count);
            Assert.AreEqual(1, results.Count(x => x.Text == "Full Time"));
            Assert.AreEqual(1, results.Count(x => x.Text == "Part Time"));
            Assert.AreEqual(1, results.Count(x => x.Text == "Other"));
        }

        [Test]
        public void DuplicatesAreRemoved()
        {
            var workPatterns = new List<JobsLookupValue>()
            {
                new JobsLookupValue() { Text = "Full Time or Part Time"},
                new JobsLookupValue() { Text = "Full Time" }
            };
            var splitter = new TribePadWorkPatternSplitter();

            var results = splitter.SplitWorkPatterns(workPatterns);

            Assert.AreEqual(2, results.Count);
            Assert.AreEqual(1, results.Count(x => x.Text == "Full Time"));
            Assert.AreEqual(1, results.Count(x => x.Text == "Part Time"));
        }
    }
}
