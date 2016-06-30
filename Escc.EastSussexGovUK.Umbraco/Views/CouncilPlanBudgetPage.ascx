<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CouncilPlanBudgetPage.ascx.cs" Inherits="Escc.EastSussexGovUK.Umbraco.Views.CouncilPlanBudgetPage" EnableViewState="false"  %>
<%@ Register Src="~/views/CouncilPlanMenu.ascx" TagPrefix="CouncilPlan" TagName="Menu" %>

<CmsPlaceholders:RichHtmlPlaceholderControl runat="server" PlaceholderToBind="phDefTitle" Paragraphs="false" EditControlClass="h1" ElementName="h1" ID="phTitle" />

<div class="text">
    <CouncilPlan:Menu runat="server" />
    <div class="plan-content">
        <CmsPlaceholders:RichHtmlPlaceholderControl runat="server" EditControlHeight="150" PlaceholderToBind="phDefContent" Paragraphs="true" ID="phContent" />
        <CmsPlaceholders:RawXhtmlPlaceholderControl runat="server" PlaceholderToBind="phDefFallbackHtml" id="phFallbackHtml" />
                        
        <asp:Literal ID="objectTag" runat="server" />
    </div>
</div>

<CmsTemplates:Related runat="server" />
