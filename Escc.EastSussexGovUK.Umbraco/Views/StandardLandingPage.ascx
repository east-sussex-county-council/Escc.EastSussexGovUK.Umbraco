<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StandardLandingPage.ascx.cs" Inherits="Escc.EastSussexGovUK.Umbraco.Views.StandardLandingPage" %>
<%@ Register TagPrefix="Cms" Namespace="Escc.EastSussexGovUK.Umbraco.MicrosoftCmsMigration" Assembly="Escc.EastSussexGovUK.Umbraco" %>

<CmsPlaceholders:XhtmlImagePlaceholderControl runat="server" PlaceholderToBind="defImage" CssClass="intro" id="image" />
<CmsPlaceholders:RichHtmlPlaceholderControl id="intro" runat="server" editcontrolwidth="455" placeholdertobind="phDefIntro"
editcontrolheight="150" />

<div runat="server" id="section1">
    <CmsPlaceholders:RichHtmlPlaceholderControl ID="listTitle01" runat="server" PlaceholderToBind="phDefListTitle01" EditControlHeight="50" RenderContainerElement="True" ElementName="h2"
        Paragraphs="false" EmbedVideo="false" UppercaseFirstLetter="true" />
    <CmsPlaceholders:RichHtmlPlaceholderControl ID="desc01" runat="server" PlaceholderToBind="defDesc01" EditControlHeight="100" RenderContainerElement="True" ElementName="p" Paragraphs="false" EmbedVideo="false" CssClass="medium large" />
    <asp:PlaceHolder runat="server" id="section1Content" />
    <Cms:PresentationModeContainer runat="server" Mode="Unpublished">
        <p class="editHelp" runat="server" id="from1">From this page</p>
    </Cms:PresentationModeContainer>
</div><div runat="server" id="section2">
    <CmsPlaceholders:RichHtmlPlaceholderControl ID="listTitle02" runat="server" PlaceholderToBind="phDefListTitle02" EditControlHeight="50" RenderContainerElement="True" ElementName="h2"
        Paragraphs="false" EmbedVideo="false" UppercaseFirstLetter="true" />
    <CmsPlaceholders:RichHtmlPlaceholderControl ID="desc02" runat="server" PlaceholderToBind="defDesc02" EditControlHeight="100" RenderContainerElement="True" ElementName="p" Paragraphs="false" EmbedVideo="false" CssClass="medium large"  />
    <asp:PlaceHolder runat="server" id="section2Content" />
    <Cms:PresentationModeContainer runat="server" Mode="Unpublished">
        <p class="editHelp" runat="server" id="from2">From this page</p>
    </Cms:PresentationModeContainer>
</div><div runat="server" id="section3">
    <CmsPlaceholders:RichHtmlPlaceholderControl ID="listTitle03" runat="server" PlaceholderToBind="phDefListTitle03" EditControlHeight="50" RenderContainerElement="True" ElementName="h2"
        Paragraphs="false" EmbedVideo="false" UppercaseFirstLetter="true" />
    <CmsPlaceholders:RichHtmlPlaceholderControl ID="desc03" runat="server" PlaceholderToBind="defDesc03" EditControlHeight="100" RenderContainerElement="True" ElementName="p" Paragraphs="false" EmbedVideo="false" CssClass="medium large"  />
    <asp:PlaceHolder runat="server" id="section3Content" />
    <Cms:PresentationModeContainer runat="server" Mode="Unpublished">
        <p class="editHelp" runat="server" id="from3">From this page</p>
    </Cms:PresentationModeContainer>
</div><div runat="server" id="section4">
    <CmsPlaceholders:RichHtmlPlaceholderControl ID="listTitle04" runat="server" PlaceholderToBind="phDefListTitle04" EditControlHeight="50" RenderContainerElement="True" ElementName="h2"
        Paragraphs="false" EmbedVideo="false" UppercaseFirstLetter="true" />
    <CmsPlaceholders:RichHtmlPlaceholderControl ID="desc04" runat="server" PlaceholderToBind="defDesc04" EditControlHeight="100" RenderContainerElement="True" ElementName="p" Paragraphs="false" EmbedVideo="false" CssClass="medium large"  />
    <asp:PlaceHolder runat="server" id="section4Content" />
        <Cms:PresentationModeContainer runat="server" Mode="Unpublished">
        <p class="editHelp" runat="server" id="from4">From this page</p>
    </Cms:PresentationModeContainer>
</div><div runat="server" id="section5">
    <CmsPlaceholders:RichHtmlPlaceholderControl ID="listTitle05" runat="server" PlaceholderToBind="phDefListTitle05" EditControlHeight="50" RenderContainerElement="True" ElementName="h2"
        Paragraphs="false" EmbedVideo="false" UppercaseFirstLetter="true" />
    <CmsPlaceholders:RichHtmlPlaceholderControl ID="desc05" runat="server" PlaceholderToBind="defDesc05" EditControlHeight="100" RenderContainerElement="True" ElementName="p" Paragraphs="false" EmbedVideo="false" CssClass="medium large"  />
    <asp:PlaceHolder runat="server" id="section5Content" />
    <Cms:PresentationModeContainer runat="server" Mode="Unpublished">
        <p class="editHelp" runat="server" id="from5">From this page</p>
    </Cms:PresentationModeContainer>
</div><div runat="server" id="section6">
    <CmsPlaceholders:RichHtmlPlaceholderControl ID="listTitle06" runat="server" PlaceholderToBind="phDefListTitle06" EditControlHeight="50" RenderContainerElement="True" ElementName="h2"
        Paragraphs="false" EmbedVideo="false" UppercaseFirstLetter="true" />
    <CmsPlaceholders:RichHtmlPlaceholderControl ID="desc06" runat="server" PlaceholderToBind="defDesc06" EditControlHeight="100" RenderContainerElement="True" ElementName="p" Paragraphs="false" EmbedVideo="false" CssClass="medium large"  />
    <asp:PlaceHolder runat="server" id="section6Content" />
    <Cms:PresentationModeContainer runat="server" Mode="Unpublished">
        <p class="editHelp" runat="server" id="from6">From this page</p>
    </Cms:PresentationModeContainer>
</div><div runat="server" id="section7">
    <CmsPlaceholders:RichHtmlPlaceholderControl ID="listTitle07" runat="server" PlaceholderToBind="phDefListTitle07" EditControlHeight="50" RenderContainerElement="True" ElementName="h2"
        Paragraphs="false" EmbedVideo="false" UppercaseFirstLetter="true" />
    <CmsPlaceholders:RichHtmlPlaceholderControl ID="desc07" runat="server" PlaceholderToBind="defDesc07" EditControlHeight="100" RenderContainerElement="True" ElementName="p" Paragraphs="false" EmbedVideo="false" CssClass="medium large"  />
    <asp:PlaceHolder runat="server" id="section7Content" />
    <Cms:PresentationModeContainer runat="server" Mode="Unpublished">
        <p class="editHelp" runat="server" id="from7">From this page</p>
    </Cms:PresentationModeContainer>
</div><div runat="server" id="section8">
    <CmsPlaceholders:RichHtmlPlaceholderControl ID="listTitle08" runat="server" PlaceholderToBind="phDefListTitle08" EditControlHeight="50" RenderContainerElement="True" ElementName="h2"
        Paragraphs="false" EmbedVideo="false" UppercaseFirstLetter="true" />
    <CmsPlaceholders:RichHtmlPlaceholderControl ID="desc08" runat="server" PlaceholderToBind="defDesc08" EditControlHeight="100" RenderContainerElement="True" ElementName="p" Paragraphs="false" EmbedVideo="false"  CssClass="medium large" />
    <asp:PlaceHolder runat="server" id="section8Content" />
        <Cms:PresentationModeContainer runat="server" Mode="Unpublished">
        <p class="editHelp" runat="server" id="from8">From this page</p>
    </Cms:PresentationModeContainer>
</div><div runat="server" id="section9">
    <CmsPlaceholders:RichHtmlPlaceholderControl ID="listTitle09" runat="server" PlaceholderToBind="phDefListTitle09" EditControlHeight="50" RenderContainerElement="True" ElementName="h2"
        Paragraphs="false" EmbedVideo="false" UppercaseFirstLetter="true" />
    <CmsPlaceholders:RichHtmlPlaceholderControl ID="desc09" runat="server" PlaceholderToBind="defDesc09" EditControlHeight="100" RenderContainerElement="True" ElementName="p" Paragraphs="false" EmbedVideo="false" CssClass="medium large"  />
    <asp:PlaceHolder runat="server" id="section9Content" />
    <Cms:PresentationModeContainer runat="server" Mode="Unpublished">
        <p class="editHelp" runat="server" id="from9">From this page</p>
    </Cms:PresentationModeContainer>
</div><div runat="server" id="section10">
    <CmsPlaceholders:RichHtmlPlaceholderControl ID="listTitle10" runat="server" PlaceholderToBind="phDefListTitle10" EditControlHeight="50" RenderContainerElement="True" ElementName="h2"
        Paragraphs="false" EmbedVideo="false" UppercaseFirstLetter="true" />
    <CmsPlaceholders:RichHtmlPlaceholderControl ID="desc10" runat="server" PlaceholderToBind="defDesc10" EditControlHeight="100" RenderContainerElement="True" ElementName="p" Paragraphs="false" EmbedVideo="false" CssClass="medium large"  />
    <asp:PlaceHolder runat="server" id="section10Content" />
        <Cms:PresentationModeContainer runat="server" Mode="Unpublished">
        <p class="editHelp" runat="server" id="from10">From this page</p>
    </Cms:PresentationModeContainer>
</div><div runat="server" id="section11">
    <CmsPlaceholders:RichHtmlPlaceholderControl ID="listTitle11" runat="server" PlaceholderToBind="phDefListTitle11" EditControlHeight="50" RenderContainerElement="True" ElementName="h2"
        Paragraphs="false" EmbedVideo="false" UppercaseFirstLetter="true" />
    <CmsPlaceholders:RichHtmlPlaceholderControl ID="desc11" runat="server" PlaceholderToBind="defDesc11" EditControlHeight="100" RenderContainerElement="True" ElementName="p" Paragraphs="false" EmbedVideo="false" CssClass="medium large"  />
    <asp:PlaceHolder runat="server" id="section11Content" />
        <Cms:PresentationModeContainer runat="server" Mode="Unpublished">
        <p class="editHelp" runat="server" id="from11">From this page</p>
    </Cms:PresentationModeContainer>
</div><div runat="server" id="section12">
    <CmsPlaceholders:RichHtmlPlaceholderControl ID="listTitle12" runat="server" PlaceholderToBind="phDefListTitle12" EditControlHeight="50" RenderContainerElement="True" ElementName="h2"
        Paragraphs="false" EmbedVideo="false" UppercaseFirstLetter="true" />
    <CmsPlaceholders:RichHtmlPlaceholderControl ID="desc12" runat="server" PlaceholderToBind="defDesc12" EditControlHeight="100" RenderContainerElement="True" ElementName="p" Paragraphs="false" EmbedVideo="false" CssClass="medium large"  />
    <asp:PlaceHolder runat="server" id="section12Content" />
        <Cms:PresentationModeContainer runat="server" Mode="Unpublished">
        <p class="editHelp" runat="server" id="from12">From this page</p>
    </Cms:PresentationModeContainer>
</div><div runat="server" id="section13">
    <CmsPlaceholders:RichHtmlPlaceholderControl ID="listTitle13" runat="server" PlaceholderToBind="phDefListTitle13" EditControlHeight="50" RenderContainerElement="True" ElementName="h2"
        Paragraphs="false" EmbedVideo="false" UppercaseFirstLetter="true" />
    <CmsPlaceholders:RichHtmlPlaceholderControl ID="desc13" runat="server" PlaceholderToBind="defDesc13" EditControlHeight="100" RenderContainerElement="True" ElementName="p" Paragraphs="false" EmbedVideo="false" CssClass="medium large"  />
    <asp:PlaceHolder runat="server" id="section13Content" />
        <Cms:PresentationModeContainer runat="server" Mode="Unpublished">
        <p class="editHelp" runat="server" id="from13">From this page</p>
    </Cms:PresentationModeContainer>
</div><div runat="server" id="section14">
    <CmsPlaceholders:RichHtmlPlaceholderControl ID="listTitle14" runat="server" PlaceholderToBind="phDefListTitle14" EditControlHeight="50" RenderContainerElement="True" ElementName="h2"
        Paragraphs="false" EmbedVideo="false" UppercaseFirstLetter="true" />
    <CmsPlaceholders:RichHtmlPlaceholderControl ID="desc14" runat="server" PlaceholderToBind="defDesc14" EditControlHeight="100" RenderContainerElement="True" ElementName="p" Paragraphs="false" EmbedVideo="false" CssClass="medium large"  />
    <asp:PlaceHolder runat="server" id="section14Content" />
    <Cms:PresentationModeContainer runat="server" Mode="Unpublished">
        <p class="editHelp" runat="server" id="from14">From this page</p>
    </Cms:PresentationModeContainer>
</div><div runat="server" id="section15">
    <CmsPlaceholders:RichHtmlPlaceholderControl ID="listTitle15" runat="server" PlaceholderToBind="phDefListTitle15" EditControlHeight="50" RenderContainerElement="True" ElementName="h2"
        Paragraphs="false" EmbedVideo="false" UppercaseFirstLetter="true" />
    <CmsPlaceholders:RichHtmlPlaceholderControl ID="desc15" runat="server" PlaceholderToBind="defDesc15" EditControlHeight="100" RenderContainerElement="True" ElementName="p" Paragraphs="false" EmbedVideo="false" CssClass="medium large"  />
    <asp:PlaceHolder runat="server" id="section15Content" />
    <Cms:PresentationModeContainer runat="server" Mode="Unpublished">
        <p class="editHelp" runat="server" id="from15">From this page</p>
    </Cms:PresentationModeContainer>
</div>
