<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopicSection_ChildrenLibrary.ascx.cs" Inherits="Escc.EastSussexGovUK.Umbraco.Views.Topic.TopicSection_ChildrenLibrary" %>
<CmsPlaceholders:RichHtmlPlaceholderControl id="subtitle" runat="server" editcontrolheight="30" tooltip="Add a subtitle (optional). Link to #subtitle" Paragraphs="false" ElementName="h2" RenderContainerElement="true" EditControlClass="h2" />
<CmsPlaceholders:RichHtmlPlaceholderControl id="content" runat="server" editcontrolheight="300" />
<asp:Literal runat="server" ID="html"/>
