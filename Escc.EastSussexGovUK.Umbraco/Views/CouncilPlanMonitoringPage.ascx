<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CouncilPlanMonitoringPage.ascx.cs" Inherits="Escc.EastSussexGovUK.Umbraco.Views.CouncilPlanMonitoringPage" EnableViewState="false"  %>
<%@ Register Src="~/views/CouncilPlanMenu.ascx" TagPrefix="CouncilPlan" TagName="Menu" %>
<%@ Register tagPrefix="CouncilPlan" tagName="Related" src="~/Views/CouncilPlanRelated.ascx" %>

<div class="content text-content monitoring">
    <CmsPlaceholders:RichHtmlPlaceholderControl runat="server" PlaceholderToBind="phDefTitle" Paragraphs="false" EditControlClass="h1" ElementName="h1" ID="phTitle" />

    <p class="button-nav"><a id="aimsLink1" runat="server">Go back to priority overview</a></p>
    <CmsPlaceholders:RichHtmlPlaceholderControl runat="server" PlaceholderToBind="phDefContent" EditControlHeight="500" ID="phContent" />
    <p class="button-nav"><a id="aimsLink2" runat="server">Go back to priority overview</a></p>
    <CouncilPlan:Related runat="server" />
</div>