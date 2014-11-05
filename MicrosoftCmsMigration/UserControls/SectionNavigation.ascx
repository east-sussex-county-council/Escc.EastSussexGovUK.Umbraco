<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SectionNavigation.ascx.cs" Inherits="Escc.CoreLegacyTemplates.Website.MicrosoftCmsMigration.UserControls.SectionNavigation" %>
<div class="section-nav" runat="server" id="highlightLinks" role="navigation">
<CmsPlaceholders:XhtmlImagePlaceholderControl runat="server" ID="highlightImage" />
<CmsPlaceholders:RichHtmlPlaceholderControl id="highlightContent" runat="server" editcontrolheight="100" Paragraphs="false" EditControlClass="no-formatselect no-numlist no-blockquote" />
</div>