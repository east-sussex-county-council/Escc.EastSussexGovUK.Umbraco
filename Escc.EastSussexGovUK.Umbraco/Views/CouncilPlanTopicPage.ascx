<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CouncilPlanTopicPage.ascx.cs" Inherits="Escc.EastSussexGovUK.Umbraco.Views.CouncilPlanTopicPage" EnableViewState="false"  %>
<%@ Register Src="~/views/CouncilPlanMenu.ascx" TagPrefix="CouncilPlan" TagName="Menu" %>
<%@ Register tagPrefix="CouncilPlan" tagName="Related" src="~/Views/CouncilPlanRelated.ascx" %>

<div class="content text-content">
    <CmsPlaceholders:RichHtmlPlaceholderControl runat="server" PlaceholderToBind="phDefTitle" Paragraphs="false" EditControlClass="h1" ElementName="h1" ID="phTitle" />

    <CouncilPlan:Menu runat="server" />
    <div class="plan-content">
        <div class="columns" id="columns" runat="server">
            <div class="column column1" id="column1" runat="server">
                <CmsPlaceholders:TextPlaceholderControl runat="server" PlaceholderToBind="phDefPriorityTitle" ID="phDefPriorityTitle" ElementName="p" CssClass="highlighted" />

                <CmsPlaceholders:RichHtmlPlaceholderControl runat="server" PlaceholderToBind="phDefContent01" EditControlHeight="700" ID="phContent01" />
                                
                <%-- Additional placeholders for Annual Report --%>
                <CmsPlaceholders:XhtmlImagePlaceholderControl runat="server" PlaceholderToBind="phDefImage09" ID="phImage09" />
                <CmsPlaceholders:RichHtmlPlaceholderControl runat="server" PlaceholderToBind="phDefContent04" EditControlHeight="200" ID="phContent04" />
                <CmsPlaceholders:XhtmlImagePlaceholderControl runat="server" PlaceholderToBind="phDefImage10" ID="phImage10" />
                <CmsPlaceholders:RichHtmlPlaceholderControl runat="server" PlaceholderToBind="phDefContent05" EditControlHeight="200" ID="phContent05" />
                <CmsPlaceholders:XhtmlImagePlaceholderControl runat="server" PlaceholderToBind="phDefImage11" ID="phImage11" />
                <CmsPlaceholders:RichHtmlPlaceholderControl runat="server" PlaceholderToBind="phDefContent06" EditControlHeight="200" ID="phContent06" />
                <CmsPlaceholders:XhtmlImagePlaceholderControl runat="server" PlaceholderToBind="phDefImage12" ID="phImage12" />
                <CmsPlaceholders:RichHtmlPlaceholderControl runat="server" PlaceholderToBind="phDefContent07" EditControlHeight="200" ID="phContent07" />
                <CmsPlaceholders:XhtmlImagePlaceholderControl runat="server" PlaceholderToBind="phDefImage13" ID="phImage13" />
                <CmsPlaceholders:RichHtmlPlaceholderControl runat="server" PlaceholderToBind="phDefContent08" EditControlHeight="200" ID="phContent08" />
                <CmsPlaceholders:XhtmlImagePlaceholderControl runat="server" PlaceholderToBind="phDefImage14" ID="phImage14" />
                <CmsPlaceholders:RichHtmlPlaceholderControl runat="server" PlaceholderToBind="phDefContent09" EditControlHeight="200" ID="phContent09" />
                <CmsPlaceholders:XhtmlImagePlaceholderControl runat="server" PlaceholderToBind="phDefImage15" ID="phImage15" />
                <CmsPlaceholders:RichHtmlPlaceholderControl runat="server" PlaceholderToBind="phDefContent10" EditControlHeight="200" ID="phContent10" />
                                
                <div class="person" runat="server" id="leader">
                    <CmsPlaceholders:XhtmlImagePlaceholderControl runat="server" PlaceholderToBind="phDefImage01" ID="leaderPhoto" />
                    <CmsPlaceholders:RichHtmlPlaceholderControl runat="server" PlaceholderToBind="phDefLeaderName" EditControlHeight="30" ElementName="p" ID="phLeaderName" />
                    <p class="position">Leader</p>
                </div>

                <div class="person" runat="server" id="chiefExec">
                    <CmsPlaceholders:XhtmlImagePlaceholderControl runat="server" PlaceholderToBind="phDefImage02" ID="chiefExecPhoto" />
                    <CmsPlaceholders:RichHtmlPlaceholderControl runat="server" PlaceholderToBind="phDefChiefExecName" EditControlHeight="30" ElementName="p" ID="phChiefExecName" />
                    <p class="position">Chief Executive</p>
                </div>
            </div>
            <div class="column column2" id="column2" runat="server">
                <CmsPlaceholders:RichHtmlPlaceholderControl runat="server" ID="aims" PlaceholderToBind="phDefContent02" EditControlHeight="300" ElementName="div" CssClass="aims-box" />

                <CmsPlaceholders:RichHtmlPlaceholderControl runat="server" ID="content3" PlaceholderToBind="phDefContent03" EditControlHeight="300" />
                <div class="logos" runat="server" id="logos">
                    <CmsPlaceholders:XhtmlImagePlaceholderControl runat="server" PlaceholderToBind="phDefImage03" ID="logo1" />
                    <CmsPlaceholders:XhtmlImagePlaceholderControl runat="server" PlaceholderToBind="phDefImage04" ID="logo2" />
                </div>
            </div>
        </div>
        <div class="images" runat="server" id="images">
            <CmsPlaceholders:XhtmlImagePlaceholderControl runat="server" PlaceholderToBind="phDefImage05" ID="image1" />
            <CmsPlaceholders:XhtmlImagePlaceholderControl runat="server" PlaceholderToBind="phDefImage06" ID="image2" />
            <CmsPlaceholders:XhtmlImagePlaceholderControl runat="server" PlaceholderToBind="phDefImage07" ID="image3" />
            <CmsPlaceholders:XhtmlImagePlaceholderControl runat="server" PlaceholderToBind="phDefImage08" ID="image4" />
        </div>
        <CouncilPlan:Related runat="server" />
    </div>
</div>