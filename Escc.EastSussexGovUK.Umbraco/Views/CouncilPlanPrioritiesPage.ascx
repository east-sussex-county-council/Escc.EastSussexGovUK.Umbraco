<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CouncilPlanPrioritiesPage.ascx.cs" Inherits="Escc.EastSussexGovUK.Umbraco.Views.CouncilPlanPrioritiesPage" EnableViewState="false"  %>
<%@ Register Src="~/views/CouncilPlanMenu.ascx" TagPrefix="CouncilPlan" TagName="Menu" %>

<CmsPlaceholders:RichHtmlPlaceholderControl runat="server" PlaceholderToBind="phDefTitle" Paragraphs="false" EditControlClass="h1" ElementName="h1" ID="phTitle" />

<div class="text">
    <CouncilPlan:Menu runat="server" />
    <div class="plan-content priorities">
        <CmsPlaceholders:RichHtmlPlaceholderControl runat="server" PlaceholderToBind="phDefContent" EditControlHeight="100" ID="phDefContent" Paragraphs="True" />
                        
        <div class="group">

            <div class="building-resilience">
                <CmsPlaceholders:TextPlaceholderControl runat="server" PlaceholderToBind="phDefPriority1" ID="phDefPriority1" ElementName="h2" />
                <CmsPlaceholders:RichHtmlPlaceholderControl runat="server" PlaceholderToBind="phDefObjectives1" EditControlHeight="100" ID="objectives1" />
            </div>

            <div class="economic-growth">
                <CmsPlaceholders:TextPlaceholderControl runat="server" PlaceholderToBind="phDefPriority2" ID="phDefPriority2" ElementName="h2" />
                <CmsPlaceholders:RichHtmlPlaceholderControl runat="server" PlaceholderToBind="phDefObjectives2" EditControlHeight="100" ID="phDefObjectives2" />
            </div>
        </div>
                        
        <asp:literal id="objectTag" runat="server" />

        <div class="group">
            <div class="best-use-of-resources">
                <CmsPlaceholders:TextPlaceholderControl runat="server" PlaceholderToBind="phDefPriority3" ID="phDefPriority3" ElementName="h2" />
                <CmsPlaceholders:RichHtmlPlaceholderControl runat="server" PlaceholderToBind="phDefObjectives3" EditControlHeight="100" ID="phDefObjectives3" />
            </div>

            <div class="vulnerable-people">
                <CmsPlaceholders:TextPlaceholderControl runat="server" PlaceholderToBind="phDefPriority4" ID="phDefPriority4" ElementName="h2" />
                <CmsPlaceholders:RichHtmlPlaceholderControl runat="server" PlaceholderToBind="phDefObjectives4" EditControlHeight="100" ID="phDefObjectives4" />
            </div>
        </div>
    </div>
</div>

<CmsTemplates:Related runat="server" />

