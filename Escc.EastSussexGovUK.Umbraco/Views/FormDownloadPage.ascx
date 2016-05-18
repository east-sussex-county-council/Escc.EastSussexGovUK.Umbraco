<%@ Control language="c#" Codebehind="FormDownloadPage.ascx.cs" AutoEventWireup="True" Inherits="Escc.EastSussexGovUK.Umbraco.Views.FormDownloadPage" EnableViewState="false" %>
<CmsPlaceholders:RichHtmlPlaceholderControl id="phIntro" runat="server" placeholdertobind="phDefIntro" editcontrolheight="300" editcontrolwidth="455"></CmsPlaceholders:RichHtmlPlaceholderControl>
	
<asp:placeholder id="guidance" runat="server">

<h2>Guidance notes</h2><CmsPlaceholders:RichHtmlPlaceholderControl id="phGuidance" runat="server" placeholdertobind="phDefGuidance" editcontrolwidth="455" editcontrolheight="100"></CmsPlaceholders:RichHtmlPlaceholderControl>
<ul id="guidanceNotes" runat="server" class="guidanceNotes"><CmsPlaceholders:XhtmlAttachmentPlaceholderControl id="guidance1" runat="server" placeholdertobind="phDefGuidance1" ElementName="li" RendercontainerElement="true" AllowedExtensions="pdf"></CmsPlaceholders:XhtmlAttachmentPlaceholderControl><CmsPlaceholders:XhtmlAttachmentPlaceholderControl id="guidance2" runat="server" placeholdertobind="phDefGuidance2" ElementName="li" RendercontainerElement="true" allowedextensions="pdf"></CmsPlaceholders:XhtmlAttachmentPlaceholderControl><CmsPlaceholders:XhtmlAttachmentPlaceholderControl id="guidance3" runat="server" placeholdertobind="phDefGuidance3" ElementName="li" RendercontainerElement="true" allowedextensions="pdf"></CmsPlaceholders:XhtmlAttachmentPlaceholderControl><CmsPlaceholders:XhtmlAttachmentPlaceholderControl id="guidance4" runat="server" placeholdertobind="phDefGuidance4" ElementName="li" RendercontainerElement="true" allowedextensions="pdf"></CmsPlaceholders:XhtmlAttachmentPlaceholderControl><CmsPlaceholders:XhtmlAttachmentPlaceholderControl id="guidance5" runat="server" placeholdertobind="phDefGuidance5" ElementName="li" RendercontainerElement="true" allowedextensions="pdf"></CmsPlaceholders:XhtmlAttachmentPlaceholderControl></ul>
</asp:placeholder>
<div class="downloadWays">
<h2 id="formatHeading" runat="server"><asp:literal id="fourWays" runat="server">Three ways</asp:literal> to complete this form</h2>

<CmsPlaceholders:FormUrlPlaceholderControl id="phXhtml" runat="server" placeholdertobind="phDefXhtmlUrl" CssClass="formAttachment phXhtml" RenderContainerElement="true"></CmsPlaceholders:FormUrlPlaceholderControl>

<CmsPlaceholders:FormUrlPlaceholderControl id="phUpload" runat="server" placeholdertobind="phDefUploadUrl" cssclass="formAttachment phUpload" rendercontainerelement="true" LinkText="FormAttachmentUpload"></CmsPlaceholders:FormUrlPlaceholderControl>

<CmsPlaceholders:FormAttachmentPlaceholderControl id="phPdf" runat="server" placeholdertobind="phDefPdf" AttachmentType="Pdf" CssClass="formAttachment phPdf"></CmsPlaceholders:FormAttachmentPlaceholderControl>
<CmsPlaceholders:FormAttachmentPlaceholderControl id="phXlsPrint" runat="server" placeholdertobind="phDefXlsPrint" AttachmentType="XlsPrint" CssClass="formAttachment phXlsPrint"></CmsPlaceholders:FormAttachmentPlaceholderControl>
<CmsPlaceholders:FormAttachmentPlaceholderControl id="phRtfSign" runat="server" placeholdertobind="phDefRtfSign" AttachmentType="RtfAndSign" CssClass="formAttachment phRtfSign"></CmsPlaceholders:FormAttachmentPlaceholderControl>
<CmsPlaceholders:FormAttachmentPlaceholderControl id="phRtf" runat="server" placeholdertobind="phDefRtf" AttachmentType="Rtf" CssClass="formAttachment phRtf"></CmsPlaceholders:FormAttachmentPlaceholderControl>
<CmsPlaceholders:FormAttachmentPlaceholderControl id="phXls" runat="server" placeholdertobind="phDefXls" AttachmentType="Xls" CssClass="formAttachment phXls"></CmsPlaceholders:FormAttachmentPlaceholderControl>
      
</div>
 
<CmsPlaceholders:RichHtmlPlaceholderControl id="phFootnote" runat="server" editcontrolwidth="440" editcontrolheight="150" placeholdertobind="phDefFootnote" />

<p id="privacy" runat="server">Your personal details are protected when using our online forms. Read our <a href="/about-this-site/privacy-and-cookies-on-this-site/">privacy statement</a>.</p>
