<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StandardTopicPage.ascx.cs" Inherits="Escc.EastSussexGovUK.Umbraco.Web.Views.StandardTopicPage" EnableViewState="false"  %>
<asp:literal runat="server" id="openContent" /><%-- Hook for child controls to inject HTML into template --%>

<CmsPlaceholders:RichHtmlPlaceholderControl id="phIntro" runat="server" placeholdertobind="phDefIntro" editcontrolheight="300" tooltip="Enter the first block of text content for your topic. If you are not using images or subheadings, enter all of your content here."></CmsPlaceholders:RichHtmlPlaceholderControl>

<asp:PlaceHolder ID="sections" runat="server"></asp:PlaceHolder>

<CmsPlaceholders:RichHtmlPlaceholderControl id="subtitle7" runat="server" PlaceholderToBind="phDefSubtitle07" editcontrolheight="30" tooltip="Add a subtitle (optional). Link to #subtitle7 from topic navigation." Paragraphs="false" ElementName="h2" RenderContainerElement="true" EditControlClass="h2"></CmsPlaceholders:RichHtmlPlaceholderControl>
<CmsPlaceholders:RichHtmlPlaceholderControl id="phContent07" runat="server" PlaceholderToBind="phDefContent07" editcontrolheight="300"></CmsPlaceholders:RichHtmlPlaceholderControl>

<CmsPlaceholders:RichHtmlPlaceholderControl id="subtitle8" runat="server" PlaceholderToBind="phDefSubtitle08" editcontrolheight="30" tooltip="Add a subtitle (optional). Link to #subtitle8 from topic navigation." Paragraphs="false" ElementName="h2" RenderContainerElement="true" EditControlClass="h2"></CmsPlaceholders:RichHtmlPlaceholderControl>
<CmsPlaceholders:RichHtmlPlaceholderControl id="phContent08" runat="server" PlaceholderToBind="phDefContent08" editcontrolheight="300"></CmsPlaceholders:RichHtmlPlaceholderControl>

<CmsPlaceholders:RichHtmlPlaceholderControl id="subtitle9" runat="server" PlaceholderToBind="phDefSubtitle09" editcontrolheight="30" tooltip="Add a subtitle (optional). Link to #subtitle9 from topic navigation." Paragraphs="false" ElementName="h2" RenderContainerElement="true" EditControlClass="h2"></CmsPlaceholders:RichHtmlPlaceholderControl>
<CmsPlaceholders:RichHtmlPlaceholderControl id="phContent09" runat="server" PlaceholderToBind="phDefContent09" editcontrolheight="300"></CmsPlaceholders:RichHtmlPlaceholderControl>

<CmsPlaceholders:RichHtmlPlaceholderControl id="subtitle10" runat="server" PlaceholderToBind="phDefSubtitle10" editcontrolheight="30" tooltip="Add a subtitle (optional). Link to #subtitle10 from topic navigation." Paragraphs="false" ElementName="h2" RenderContainerElement="true" EditControlClass="h2"></CmsPlaceholders:RichHtmlPlaceholderControl>
<CmsPlaceholders:RichHtmlPlaceholderControl id="phContent10" runat="server" PlaceholderToBind="phDefContent10" editcontrolheight="300"></CmsPlaceholders:RichHtmlPlaceholderControl>

<CmsPlaceholders:RichHtmlPlaceholderControl id="subtitle11" runat="server" placeholdertobind="phDefSubtitle11" editcontrolheight="30" tooltip="Add a subtitle (optional). Link to #subtitle11 from topic navigation." Paragraphs="false" ElementName="h2" RenderContainerElement="true" EditControlClass="h2"></CmsPlaceholders:RichHtmlPlaceholderControl>
<CmsPlaceholders:RichHtmlPlaceholderControl id="phContent11" runat="server" placeholdertobind="phDefContent11" editcontrolheight="300"></CmsPlaceholders:RichHtmlPlaceholderControl>

<CmsPlaceholders:RichHtmlPlaceholderControl id="subtitle12" runat="server" placeholdertobind="phDefSubtitle12" editcontrolheight="30" tooltip="Add a subtitle (optional). Link to #subtitle12 from topic navigation." Paragraphs="false" ElementName="h2" RenderContainerElement="true" EditControlClass="h2"></CmsPlaceholders:RichHtmlPlaceholderControl>
<CmsPlaceholders:RichHtmlPlaceholderControl id="phContent12" runat="server" placeholdertobind="phDefContent12" editcontrolheight="300"></CmsPlaceholders:RichHtmlPlaceholderControl>

<CmsPlaceholders:RichHtmlPlaceholderControl id="subtitle13" runat="server" placeholdertobind="phDefSubtitle13" editcontrolheight="30" tooltip="Add a subtitle (optional). Link to #subtitle13 from topic navigation." Paragraphs="false" ElementName="h2" RenderContainerElement="true" EditControlClass="h2"></CmsPlaceholders:RichHtmlPlaceholderControl>
<CmsPlaceholders:RichHtmlPlaceholderControl id="phContent13" runat="server" placeholdertobind="phDefContent13" editcontrolheight="300"></CmsPlaceholders:RichHtmlPlaceholderControl>

<CmsPlaceholders:RichHtmlPlaceholderControl id="subtitle14" runat="server" placeholdertobind="phDefSubtitle14" editcontrolheight="30" tooltip="Add a subtitle (optional). Link to #subtitle14 from topic navigation." Paragraphs="false" ElementName="h2" RenderContainerElement="true" EditControlClass="h2"></CmsPlaceholders:RichHtmlPlaceholderControl>
<CmsPlaceholders:RichHtmlPlaceholderControl id="phContent14" runat="server" placeholdertobind="phDefContent14" editcontrolheight="300"></CmsPlaceholders:RichHtmlPlaceholderControl>

<CmsPlaceholders:RichHtmlPlaceholderControl id="subtitle15" runat="server" placeholdertobind="phDefSubtitle15" editcontrolheight="30" tooltip="Add a subtitle (optional). Link to #subtitle15 from topic navigation." Paragraphs="false" ElementName="h2" RenderContainerElement="true" EditControlClass="h2"></CmsPlaceholders:RichHtmlPlaceholderControl>
<CmsPlaceholders:RichHtmlPlaceholderControl id="phContent15" runat="server" placeholdertobind="phDefContent15" editcontrolheight="300"></CmsPlaceholders:RichHtmlPlaceholderControl>

<CmsPlaceholders:RichHtmlPlaceholderControl id="subtitle16" runat="server" placeholdertobind="phDefSubtitle16" editcontrolheight="30" tooltip="Add a subtitle (optional). Link to #subtitle16 from topic navigation." Paragraphs="false" ElementName="h2" RenderContainerElement="true" EditControlClass="h2"></CmsPlaceholders:RichHtmlPlaceholderControl>
<CmsPlaceholders:RichHtmlPlaceholderControl id="phContent16" runat="server" placeholdertobind="phDefContent16" editcontrolheight="300"></CmsPlaceholders:RichHtmlPlaceholderControl>

<CmsPlaceholders:RichHtmlPlaceholderControl id="subtitle17" runat="server" placeholdertobind="phDefSubtitle17" editcontrolheight="30" tooltip="Add a subtitle (optional). Link to #subtitle17 from topic navigation." Paragraphs="false" ElementName="h2" RenderContainerElement="true" EditControlClass="h2"></CmsPlaceholders:RichHtmlPlaceholderControl>
<CmsPlaceholders:RichHtmlPlaceholderControl id="phContent17" runat="server" placeholdertobind="phDefContent17" editcontrolheight="300"></CmsPlaceholders:RichHtmlPlaceholderControl>

<CmsPlaceholders:RichHtmlPlaceholderControl id="subtitle18" runat="server" placeholdertobind="phDefSubtitle18" editcontrolheight="30" tooltip="Add a subtitle (optional). Link to #subtitle18 from topic navigation." Paragraphs="false" ElementName="h2" RenderContainerElement="true" EditControlClass="h2"></CmsPlaceholders:RichHtmlPlaceholderControl>
<CmsPlaceholders:RichHtmlPlaceholderControl id="phContent18" runat="server" placeholdertobind="phDefContent18" editcontrolheight="300"></CmsPlaceholders:RichHtmlPlaceholderControl>

<CmsPlaceholders:RichHtmlPlaceholderControl id="subtitle19" runat="server" placeholdertobind="phDefSubtitle19" editcontrolheight="30" tooltip="Add a subtitle (optional). Link to #subtitle19 from topic navigation." Paragraphs="false" ElementName="h2" RenderContainerElement="true" EditControlClass="h2"></CmsPlaceholders:RichHtmlPlaceholderControl>
<CmsPlaceholders:RichHtmlPlaceholderControl id="phContent19" runat="server" placeholdertobind="phDefContent19" editcontrolheight="300"></CmsPlaceholders:RichHtmlPlaceholderControl>

<CmsPlaceholders:RichHtmlPlaceholderControl id="subtitle20" runat="server" placeholdertobind="phDefSubtitle20" editcontrolheight="30" tooltip="Add a subtitle (optional). Link to #subtitle20 from topic navigation." Paragraphs="false" ElementName="h2" RenderContainerElement="true" EditControlClass="h2"></CmsPlaceholders:RichHtmlPlaceholderControl>
<CmsPlaceholders:RichHtmlPlaceholderControl id="phContent20" runat="server" placeholdertobind="phDefContent20" editcontrolheight="300"></CmsPlaceholders:RichHtmlPlaceholderControl>

<div id="logos" runat="server">
<CmsPlaceholders:Xhtmlimageplaceholdercontrol id="phLogo01" runat="server" placeholdertobind="phDefLogo01"></CmsPlaceholders:Xhtmlimageplaceholdercontrol>
<CmsPlaceholders:Xhtmlimageplaceholdercontrol id="phLogo02" runat="server" placeholdertobind="phDefLogo02"></CmsPlaceholders:Xhtmlimageplaceholdercontrol>
<CmsPlaceholders:Xhtmlimageplaceholdercontrol id="phLogo03" runat="server" placeholdertobind="phDefLogo03"></CmsPlaceholders:Xhtmlimageplaceholdercontrol>
<CmsPlaceholders:Xhtmlimageplaceholdercontrol id="phLogo04" runat="server" placeholdertobind="phDefLogo04"></CmsPlaceholders:Xhtmlimageplaceholdercontrol>
<CmsPlaceholders:Xhtmlimageplaceholdercontrol id="phLogo05" runat="server" placeholdertobind="phDefLogo05"></CmsPlaceholders:Xhtmlimageplaceholdercontrol>
<CmsPlaceholders:Xhtmlimageplaceholdercontrol id="phLogo06" runat="server" placeholdertobind="phDefLogo06"></CmsPlaceholders:Xhtmlimageplaceholdercontrol>
<CmsPlaceholders:Xhtmlimageplaceholdercontrol id="phLogo07" runat="server" placeholdertobind="phDefLogo07"></CmsPlaceholders:Xhtmlimageplaceholdercontrol>
<CmsPlaceholders:Xhtmlimageplaceholdercontrol id="phLogo08" runat="server" placeholdertobind="phDefLogo08"></CmsPlaceholders:Xhtmlimageplaceholdercontrol>
</div>

<asp:literal runat="server" id="endContent" /><%-- Hook for child controls to inject HTML into template --%>