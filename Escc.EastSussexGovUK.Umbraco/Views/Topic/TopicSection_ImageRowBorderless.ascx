<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopicSection_ImageRowBorderless.ascx.cs" Inherits="Escc.EastSussexGovUK.Umbraco.Views.Topic.TopicSection_ImageRowBorderless" %>

<div id="imageRow" class="imageRow" runat="server">
<div id="image1" runat="server" class="topicImageEven">
    <CmsPlaceholders:XhtmlImagePlaceholderControl id="phImage1" runat="server" />
    <CmsPlaceholders:TextPlaceholderControl ID="caption1" runat="server" CssClass="caption" />
</div>
<div id="image2" runat="server" class="topicImageEven">
    <CmsPlaceholders:XhtmlImagePlaceholderControl id="phImage2" runat="server" />
    <CmsPlaceholders:TextPlaceholderControl ID="caption2" runat="server" CssClass="caption" />
</div>
<div id="image3" runat="server" class="topicImageEven">
    <CmsPlaceholders:XhtmlImagePlaceholderControl id="phImage3" runat="server" />
    <CmsPlaceholders:TextPlaceholderControl ID="caption3" runat="server" CssClass="caption" />
</div>
</div>

<CmsPlaceholders:RichHtmlPlaceholderControl id="subtitle" runat="server" editcontrolheight="50" editcontrolwidth="440" Paragraphs="false" ElementName="h2" RenderContainerElement="true" />
<CmsPlaceholders:RichHtmlPlaceholderControl id="content" runat="server" editcontrolheight="300" editcontrolwidth="440" />
