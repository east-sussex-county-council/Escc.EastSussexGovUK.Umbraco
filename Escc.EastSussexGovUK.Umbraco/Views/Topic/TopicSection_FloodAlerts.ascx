<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopicSection_FloodAlerts.ascx.cs" Inherits="Escc.EastSussexGovUK.Umbraco.Views.Topic.TopicSection_FloodAlerts" %>
<%@ Register TagPrefix="template" TagName="FloodAlerts" Src="floodalerts.ascx" %>
<CmsPlaceholders:RichHtmlPlaceholderControl id="subtitle" runat="server" editcontrolheight="50" editcontrolwidth="440" Paragraphs="false" ElementName="h2" RenderContainerElement="true" />
<CmsPlaceholders:RichHtmlPlaceholderControl id="content" runat="server" editcontrolheight="300" editcontrolwidth="440" />
<div class="small medium">
<article>
    <template:FloodAlerts runat="server" />
</article>
</div>
