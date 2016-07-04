<%@ Control Language="c#" AutoEventWireup="True" CodeBehind="CouncilPlanRelated.ascx.cs" Inherits="Escc.EastSussexGovUK.Umbraco.Views.CouncilPlanRelated" %>
<div class="related-links columns" role="navigation" id="related" runat="server">
<aside>
    <div class="column1" id="pages" runat="server">
    <section>
        <h2><%# BoxTitle1 %></h2>
        <CmsPlaceholders:RichHtmlPlaceholderControl ID="phRelatedPages" runat="server" PlaceholderToBind="phDefRelatedPages" EmbedVideo="false" />
        <asp:Literal runat="server" ID="docs" />
    </section>
    </div>

    <asp:PlaceHolder runat="server" ID="pagesOnly">
        <h2><%# BoxTitle1 %></h2>
        <CmsPlaceholders:RichHtmlPlaceholderControl ID="phRelatedPagesOnly" runat="server" PlaceholderToBind="phDefRelatedPages" EmbedVideo="false" />
        <asp:Literal runat="server" ID="docsOnly" />
    </asp:PlaceHolder>

    <div class="column2" id="websites" runat="server">
        <section>
            <h2><%# BoxTitle2 %></h2>
            <CmsPlaceholders:RichHtmlPlaceholderControl ID="phRelatedSites" runat="server" PlaceholderToBind="phDefRelatedSites" EmbedVideo="false" />
        </section>
    </div>

    <asp:PlaceHolder runat="server" ID="websitesOnly">
        <h2><%# BoxTitle2 %></h2>
        <CmsPlaceholders:RichHtmlPlaceholderControl ID="phRelatedSitesOnly" runat="server" PlaceholderToBind="phDefRelatedSites" EmbedVideo="false" />
    </asp:PlaceHolder>
</aside>
</div>