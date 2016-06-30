<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CouncilPlanMonitoringPage.ascx.cs" Inherits="Escc.EastSussexGovUK.Umbraco.Views.CouncilPlanMonitoringPage" EnableViewState="false"  %>
<%@ Register Src="~/views/CouncilPlanMenu.ascx" TagPrefix="CouncilPlan" TagName="Menu" %>

<CmsPlaceholders:RichHtmlPlaceholderControl runat="server" PlaceholderToBind="phDefTitle" Paragraphs="false" EditControlClass="h1" ElementName="h1" ID="phTitle" />

<div class="text">
    <CouncilPlan:Menu runat="server" />
    <div class="plan-content">
        <CmsPlaceholders:RichHtmlPlaceholderControl runat="server" PlaceholderToBind="phDefContent" EditControlHeight="500" ID="phContent" />
        <p class="button-nav" runat="server" id="aims"><a id="aimsLink" runat="server">Go back to priority overview</a></p>
    </div>
</div>

<CmsTemplates:Related runat="server" />
