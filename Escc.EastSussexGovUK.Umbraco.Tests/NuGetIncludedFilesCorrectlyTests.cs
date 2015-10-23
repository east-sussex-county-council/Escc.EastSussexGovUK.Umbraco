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
        // Escc.CoreLegacyTemplates.Website project
        [TestCase(@"Views\StandardTopicPage.cshtml")] 
        
        // Escc.CustomerFocusTemplates.Website project
        [TestCase(@"App_Plugins\OpeningSoon\openingsoon.html")] 
        [TestCase(@"App_Plugins\OpeningSoon\scripts\jquery.timepicker.js")] 
        [TestCase(@"css\task-small.css")] 
        [TestCase(@"js\CustomerFocusTemplates\guide.js")]
        [TestCase(@"Views\Partials\_LocationIsOpen.cshtml")]
        [TestCase(@"Views\Location.cshtml")]

        // Escc.CampaignTemplates.Website project
        [TestCase(@"js\CampaignTemplates\social-work-campaign-landing.js")]

        // Escc.Home.Website project
        [TestCase(@"Views\HomePage.cshtml")]

        // Escc.Alerts.Website project
        [TestCase(@"Views\Alerts.cshtml")]

        // Escc.Umbraco.PropertyEditors project
        [TestCase(@"App_Plugins\Escc.Umbraco.PropertyEditors.RichTextPropertyEditor\controller.js")] 
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
