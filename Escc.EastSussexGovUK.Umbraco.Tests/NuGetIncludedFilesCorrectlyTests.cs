using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestFixture]
    public class NuGetIncludedFilesCorrectlyTests
    {
        [TestCase(@"Views\StandardTopicPage.cshtml")] // Escc.CoreLegacyTemplates.Website project
        [TestCase(@"Views\Location.cshtml")] // Escc.CustomerFocusTemplates.Website project
        [TestCase(@"js\CustomerFocusTemplates\guide.js")] // Escc.CustomerFocusTemplates.Website project
        [TestCase(@"js\CampaignTemplates\social-work-campaign-landing.js")] // Escc.CampaignTemplates.Website project
        [TestCase(@"Views\HomePage.cshtml")] // Escc.Home.Website project
        [TestCase(@"Views\Alerts.cshtml")] // Escc.Alerts.Website project
        [TestCase(@"App_Plugins\Escc.Umbraco.PropertyEditors.RichTextPropertyEditor\controller.js")] // Escc.Umbraco.PropertyEditors project
        public void NuGetPackagesCorrectlyIncludedInProject(string filePathWhichShouldBeIncluded)
        {
            var commonParent = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent;
            var reader = new XPathDocument(Path.Combine(commonParent.FullName, @"escc.eastsussexgovuk.umbraco\escc.eastsussexgovuk.umbraco.csproj"));
            var nav = reader.CreateNavigator();
            nav.MoveToRoot();

            var namespaces = new XmlNamespaceManager(nav.NameTable);
            namespaces.AddNamespace("ms", "http://schemas.microsoft.com/developer/msbuild/2003");

            var node = nav.SelectSingleNode(@"/ms:Project/ms:ItemGroup/ms:Content[@Include='" + filePathWhichShouldBeIncluded + "']", namespaces);

            Assert.IsNotNull(node);
        }
    }
}
