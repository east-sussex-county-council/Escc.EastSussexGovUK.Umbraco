<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopicSection_Normal.ascx.cs" Inherits="Escc.EastSussexGovUK.Umbraco.Web.Views.Topic.TopicSection_Normal" %>

<figure id="image1" runat="server" class="topicImageOdd">
    <CmsPlaceholders:XhtmlImagePlaceholderControl ID="phImage1" runat="server" />
    <figcaption runat="server"><CmsPlaceholders:TextPlaceholderControl ID="caption1" runat="server" /></figcaption>
</figure>
<figure id="image2" runat="server" class="topicImageOdd">
    <CmsPlaceholders:XhtmlImagePlaceholderControl ID="phImage2" runat="server" />
   <figcaption runat="server"><CmsPlaceholders:TextPlaceholderControl ID="caption2" runat="server" /></figcaption>
</figure>
<figure id="image3" runat="server" class="topicImageOdd">
    <CmsPlaceholders:XhtmlImagePlaceholderControl ID="phImage3" runat="server" />
    <figcaption runat="server"><CmsPlaceholders:TextPlaceholderControl ID="caption3" runat="server" /></figcaption>
</figure>

<CmsPlaceholders:RichHtmlPlaceholderControl ID="subtitle" runat="server" EditControlHeight="30" Paragraphs="false" ElementName="h2" RenderContainerElement="true" EditControlClass="h2" />
<CmsPlaceholders:RichHtmlPlaceholderControl ID="content" runat="server" EditControlHeight="300" />
