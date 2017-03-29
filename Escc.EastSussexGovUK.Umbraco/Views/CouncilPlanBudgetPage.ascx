<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CouncilPlanBudgetPage.ascx.cs" Inherits="Escc.EastSussexGovUK.Umbraco.Views.CouncilPlanBudgetPage" EnableViewState="false"  %>
<%@ Register Src="~/views/CouncilPlanMenu.ascx" TagPrefix="CouncilPlan" TagName="Menu" %>
<%@ Register tagPrefix="CouncilPlan" tagName="Related" src="~/Views/CouncilPlanRelated.ascx" %>

<div class="content text-content">
    <CmsPlaceholders:RichHtmlPlaceholderControl runat="server" PlaceholderToBind="phDefTitle" Paragraphs="false" EditControlClass="h1" ElementName="h1" ID="phTitle" />

    <CouncilPlan:Menu runat="server" />
    <div class="plan-content">
        <CmsPlaceholders:RichHtmlPlaceholderControl runat="server" EditControlHeight="150" PlaceholderToBind="phDefContent" Paragraphs="true" ID="phContent" />
                        
        <asp:Literal ID="objectTag" runat="server" />
        <CouncilPlan:Related runat="server" />
    </div>
</div>