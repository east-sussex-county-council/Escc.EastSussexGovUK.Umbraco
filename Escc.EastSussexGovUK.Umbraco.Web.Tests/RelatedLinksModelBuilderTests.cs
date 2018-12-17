using System;
using System.Collections.Generic;
using Escc.Umbraco.PropertyTypes;
using Escc.EastSussexGovUK.Umbraco.Web.Services;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Web.Tests
{
    [TestFixture]
    public class RelatedLinksModelBuilderTests
    {
        [Test]
        public void FirstLinkHasNoUrlAndIsHeading()
        {
            var links = new List<HtmlLink>()
            {
                new HtmlLink {Text = "Heading 1"},
                new HtmlLink() {Text = "Link 1", Url = new Uri("https://example.org/link1")}
            };

            var result = new RelatedLinksModelBuilder().OrganiseAsHeadingsAndSections(links);

            Assert.AreEqual("Heading 1", result[0].Heading.Text);
        }

        [Test]
        public void LinkWithNoUrlBetweenOtherLinksIsHeading()
        {
            var links = new List<HtmlLink>()
            {
                new HtmlLink {Text = "Heading 1"},
                new HtmlLink() {Text = "Link 1", Url = new Uri("https://example.org/link1")},
                new HtmlLink {Text = "Heading 2"},
                new HtmlLink() {Text = "Link 2", Url = new Uri("https://example.org/link2")}
            };

            var result = new RelatedLinksModelBuilder().OrganiseAsHeadingsAndSections(links);

            Assert.AreEqual("Heading 2", result[1].Heading.Text);
        }

        [Test]
        public void OrphanedHeadingIsIgnored()
        {
            var links = new List<HtmlLink>()
            {
                new HtmlLink {Text = "Heading 1"},
                new HtmlLink() {Text = "Link 1", Url = new Uri("https://example.org/link1")},
                new HtmlLink {Text = "Heading 2"}
            };

            var result = new RelatedLinksModelBuilder().OrganiseAsHeadingsAndSections(links);

            Assert.AreEqual(1, result.Count);
        }
    }
}
