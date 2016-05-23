<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopicSection_FeaturedImage.ascx.cs" Inherits="Escc.EastSussexGovUK.Umbraco.Views.Topic.TopicSection_FeaturedImage" %>

<figure id="image1" runat="server" class="topicImageNoWrap">
    <CmsPlaceholders:XhtmlImagePlaceholderControl ID="phImage1" runat="server" />
    <figcaption runat="server"><CmsPlaceholders:TextPlaceholderControl ID="caption1" runat="server" /></figcaption>
</figure>                                                                                           
<figure id="image2" runat="server" class="topicImageNoWrap">                                        
    <CmsPlaceholders:XhtmlImagePlaceholderControl ID="phImage2" runat="server" />                   
    <figcaption runat="server"><CmsPlaceholders:TextPlaceholderControl ID="caption2" runat="server"  /></figcaption>
</figure>                                                                                           
<figure id="image3" runat="server" class="topicImageNoWrap">                                        
    <CmsPlaceholders:XhtmlImagePlaceholderControl ID="phImage3" runat="server" />                   
    <figcaption runat="server"><CmsPlaceholders:TextPlaceholderControl ID="caption3" runat="server" /></figcaption>
</figure>

<CmsPlaceholders:RichHtmlPlaceholderControl id="subtitle" runat="server" editcontrolheight="30" Paragraphs="false" ElementName="h2" RenderContainerElement="true" />
<CmsPlaceholders:RichHtmlPlaceholderControl id="content" runat="server" editcontrolheight="300" />