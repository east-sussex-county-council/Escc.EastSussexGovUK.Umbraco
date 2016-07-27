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
        // Escc.Alerts.Website project
        [TestCase(@"Views\Alerts.cshtml")]

        // EScc.Schools.TermDates project
        [TestCase(@"Views\TermDates\QuickAnswer.ascx")]

        // Escc.Umbraco.MediaSync project
        [TestCase(@"Config\uMediaSync.config")]
        [TestCase(@"App_Plugins\Escc.Umbraco.DataTypes.MediaUsage\mediausage.controller.js")]

        // Escc.Umbraco.PropertyEditors project
        [TestCase(@"App_Plugins\Escc.Umbraco.PropertyEditors.RichTextPropertyEditor\controller.js")]

        // Escc.Registration.MarriageSkin project
        [TestCase(@"MarriageSkin\js\min\marriage-skin.js")]
        [TestCase(@"MarriageSkin\css\min\marriage-skin-small.css")]
        [TestCase(@"MarriageSkin\img\marriage-banner.jpg")]

        // Escc.RubbishAndRecycling.SiteFinder package
        [TestCase(@"Views\Topic\RecyclingSiteFinder.ascx")]

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
