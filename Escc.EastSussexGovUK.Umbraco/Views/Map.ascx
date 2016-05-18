<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Map.ascx.cs" Inherits="Escc.EastSussexGovUK.Umbraco.Views.Map" %>
<CmsPlaceholders:RichHtmlPlaceholderControl id="phIntro" runat="server" editcontrolwidth="455" placeholdertobind="phDefIntro"
	editcontrolheight="150"></CmsPlaceholders:RichHtmlPlaceholderControl>
	
<CmsPlaceholders:XhtmlImagePlaceholderControl id="phMap" runat="server" tooltip="Add a map" placeholdertobind="phDefMap" cssclass="mapNavigation"></CmsPlaceholders:XhtmlImagePlaceholderControl>
	
<CmsPlaceholders:RawXhtmlPlaceholderControl id="phImageMap" runat="server" placeholdertoBind="phDefImageMapXhtml" Text="Add the XHTML to make the map an image map (optional)." />

<CmsPlaceholders:RichHtmlPlaceholderControl id="phContent01" runat="server" placeholdertobind="phDefContent01" editcontrolheight="300" editcontrolwidth="440" RenderContainerElement="true"></CmsPlaceholders:RichHtmlPlaceholderControl>