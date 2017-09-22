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
        [TestCase(@"css\TinyMCE-Content.css")]
        [TestCase(@"css\TinyMCE-StyleSelector-Headings.css")]
        [TestCase(@"App_Plugins\Escc.Umbraco.PropertyEditors.RichTextPropertyEditor\controller.js")]

        // Escc.Registration.RegistrationSkin project
        [TestCase(@"RegistrationSkin\js\min\registration-skin.js")]
        [TestCase(@"RegistrationSkin\css\min\registration-skin-small.css")]
        [TestCase(@"RegistrationSkin\img\registration-skin.png")]

        // Escc.RubbishAndRecycling.SiteFinder package
        [TestCase(@"Views\Topic\RecyclingSiteFinder.ascx")]

        // Escc.EastSussexGovUK.Rss project
        [TestCase(@"EastSussexGovUK\Rss\rss-to-html.ashx")]

        // Escc.Umbraco.EditorTools project
        [TestCase(@"App_Plugins\EditorTools\Content\Site.css")]
        [TestCase(@"App_Plugins\EditorTools\fonts\glyphicons-halflings-regular.eot")]
        [TestCase(@"App_Plugins\EditorTools\lang\en.xml")]
        [TestCase(@"App_Plugins\EditorTools\Scripts\bootstrap.js")]
        [TestCase(@"App_Plugins\EditorTools\Views\Content\Index.cshtml")]

        // Escc.EastSussexGovUK.TemplateSource project
        [TestCase(@"favicon.ico")]
        [TestCase(@"apple-touch-icon.png")]
        [TestCase(@"robots.txt")]

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
