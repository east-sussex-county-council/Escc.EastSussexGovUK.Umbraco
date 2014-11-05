<%@ Control Language="c#" AutoEventWireup="True" CodeBehind="Related.ascx.cs" Inherits="Escc.CoreLegacyTemplates.Website.MicrosoftCmsMigration.UserControls.Related" %>
<div class="related" role="navigation" id="related" runat="server">
<aside>
    <div class="section" id="pages" runat="server">
    <section>
        <h2><%# BoxTitle1 %></h2>
        <CmsPlaceholders:RichHtmlPlaceholderControl ID="phRelatedPages" runat="server" PlaceholderToBind="phDefRelatedPages" EmbedVideo="false" EditControlClass="no-formatselect no-numlist no-blockquote" />
        <asp:Literal runat="server" ID="docs" />
    </section>
    </div>

    <asp:PlaceHolder runat="server" ID="pagesOnly">
        <h2><%# BoxTitle1 %></h2>
        <CmsPlaceholders:RichHtmlPlaceholderControl ID="phRelatedPagesOnly" runat="server" PlaceholderToBind="phDefRelatedPages" EmbedVideo="false" />
        <asp:Literal runat="server" ID="docsOnly" />
    </asp:PlaceHolder>

    <div class="section" id="websites" runat="server">
        <section>
            <h2><%# BoxTitle2 %></h2>
            <CmsPlaceholders:RichHtmlPlaceholderControl ID="phRelatedSites" runat="server" PlaceholderToBind="phDefRelatedSites" EmbedVideo="false" EditControlClass="no-formatselect no-numlist no-blockquote" />
        </section>
    </div>

    <asp:PlaceHolder runat="server" ID="websitesOnly">
        <h2><%# BoxTitle2 %></h2>
        <CmsPlaceholders:RichHtmlPlaceholderControl ID="phRelatedSitesOnly" runat="server" PlaceholderToBind="phDefRelatedSites" EmbedVideo="false" />
    </asp:PlaceHolder>
</aside>
</div>