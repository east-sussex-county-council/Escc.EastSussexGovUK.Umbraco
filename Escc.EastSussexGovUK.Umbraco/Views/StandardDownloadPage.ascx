<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StandardDownloadPage.ascx.cs" Inherits="Escc.EastSussexGovUK.Umbraco.Views.StandardDownloadPage" %>

<CmsTemplates:Latest id="latest" runat="server" />

<div class="text">
    <div id="image1" runat="server" class="topicImageOdd">
	    <CmsPlaceholders:XhtmlImagePlaceholderControl id="phImage01" runat="server" placeholderToBind="phDefImage01" />
        <CmsPlaceholders:TextPlaceholderControl ID="caption01" runat="server" CssClass="caption" PlaceholderToBind="phDefCaption01" ElementName="P" RenderContainerElement="true" />
        <p id="alt01" runat="server"></p>
    </div>

    <CmsPlaceholders:RichHtmlplaceholdercontrol runat="server" placeholdertobind="phDefIntro" editcontrolheight="300" editcontrolwidth="350" id="intro" />
	
	<asp:placeholder runat="server" id="downloadList">
	    <table class="downloadList">
	    <thead><tr>
	    <th id="col1head" runat="server"></th>
	    <th id="col0" runat="server"></th>
	    </tr></thead>
	    <tbody>
	    <tr id="row01" runat="server" class="firstRow">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile01" runat="server" placeholdertobind="phDefFile01"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col1" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile01a" runat="server" placeholdertobind="phDefFile01a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td>
	    </tr>
	    <tr id="row02" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile02" runat="server" placeholdertobind="phDefFile02"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col2" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile02a" runat="server" placeholdertobind="phDefFile02a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td>
	    </tr>
	    <tr id="row03" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile03" runat="server" placeholdertobind="phDefFile03"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col3" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile03a" runat="server" placeholdertobind="phDefFile03a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td>
	    </tr>
	    <tr id="row04" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile04" runat="server" placeholdertobind="phDefFile04"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col4" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile04a" runat="server" placeholdertobind="phDefFile04a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row05" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile05" runat="server" placeholdertobind="phDefFile05"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col5" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile05a" runat="server" placeholdertobind="phDefFile05a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row06" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile06" runat="server" placeholdertobind="phDefFile06"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col6" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile06a" runat="server" placeholdertobind="phDefFile06a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row07" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile07" runat="server" placeholdertobind="phDefFile07"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col7" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile07a" runat="server" placeholdertobind="phDefFile07a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row08" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile08" runat="server" placeholdertobind="phDefFile08"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col8" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile08a" runat="server" placeholdertobind="phDefFile08a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row09" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile09" runat="server" placeholdertobind="phDefFile09"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col9" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile09a" runat="server" placeholdertobind="phDefFile09a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row10" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile10" runat="server" placeholdertobind="phDefFile10"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col10" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile10a" runat="server" placeholdertobind="phDefFile10a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row11" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile11" runat="server" placeholdertobind="phDefFile11"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col11" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile11a" runat="server" placeholdertobind="phDefFile11a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row12" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile12" runat="server" placeholdertobind="phDefFile12"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col12" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile12a" runat="server" placeholdertobind="phDefFile12a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row13" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile13" runat="server" placeholdertobind="phDefFile13"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col13" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile13a" runat="server" placeholdertobind="phDefFile13a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row14" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile14" runat="server" placeholdertobind="phDefFile14"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col14" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile14a" runat="server" placeholdertobind="phDefFile14a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row15" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile15" runat="server" placeholdertobind="phDefFile15"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col15" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile15a" runat="server" placeholdertobind="phDefFile15a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row16" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile16" runat="server" placeholdertobind="phDefFile16"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col16" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile16a" runat="server" placeholdertobind="phDefFile16a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row17" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile17" runat="server" placeholdertobind="phDefFile17"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col17" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile17a" runat="server" placeholdertobind="phDefFile17a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row18" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile18" runat="server" placeholdertobind="phDefFile18"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col18" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile18a" runat="server" placeholdertobind="phDefFile18a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row19" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile19" runat="server" placeholdertobind="phDefFile19"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col19" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile19a" runat="server" placeholdertobind="phDefFile19a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row20" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile20" runat="server" placeholdertobind="phDefFile20"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col20" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile20a" runat="server" placeholdertobind="phDefFile20a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row21" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile21" runat="server" placeholdertobind="phDefFile21"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col21" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile21a" runat="server" placeholdertobind="phDefFile21a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row22" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile22" runat="server" placeholdertobind="phDefFile22"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col22" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile22a" runat="server" placeholdertobind="phDefFile22a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row23" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile23" runat="server" placeholdertobind="phDefFile23"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col23" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile23a" runat="server" placeholdertobind="phDefFile23a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row24" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile24" runat="server" placeholdertobind="phDefFile24"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col24" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile24a" runat="server" placeholdertobind="phDefFile24a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row25" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile25" runat="server" placeholdertobind="phDefFile25"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col25" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile25a" runat="server" placeholdertobind="phDefFile25a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row26" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile26" runat="server" placeholdertobind="phDefFile26"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col26" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile26a" runat="server" placeholdertobind="phDefFile26a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row27" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile27" runat="server" placeholdertobind="phDefFile27"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col27" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile27a" runat="server" placeholdertobind="phDefFile27a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row28" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile28" runat="server" placeholdertobind="phDefFile28"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col28" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile28a" runat="server" placeholdertobind="phDefFile28a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row29" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile29" runat="server" placeholdertobind="phDefFile29"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col29" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile29a" runat="server" placeholdertobind="phDefFile29a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row30" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile30" runat="server" placeholdertobind="phDefFile30"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col30" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile30a" runat="server" placeholdertobind="phDefFile30a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row31" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile31" runat="server" placeholdertobind="phDefFile31"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col31" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile31a" runat="server" placeholdertobind="phDefFile31a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row32" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile32" runat="server" placeholdertobind="phDefFile32"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col32" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile32a" runat="server" placeholdertobind="phDefFile32a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row33" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile33" runat="server" placeholdertobind="phDefFile33"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col33" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile33a" runat="server" placeholdertobind="phDefFile33a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row34" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile34" runat="server" placeholdertobind="phDefFile34"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col34" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile34a" runat="server" placeholdertobind="phDefFile34a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row35" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile35" runat="server" placeholdertobind="phDefFile35"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col35" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile35a" runat="server" placeholdertobind="phDefFile35a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row36" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile36" runat="server" placeholdertobind="phDefFile36"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col36" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile36a" runat="server" placeholdertobind="phDefFile36a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row37" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile37" runat="server" placeholdertobind="phDefFile37"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col37" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile37a" runat="server" placeholdertobind="phDefFile37a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row38" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile38" runat="server" placeholdertobind="phDefFile38"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col38" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile38a" runat="server" placeholdertobind="phDefFile38a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row39" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile39" runat="server" placeholdertobind="phDefFile39"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col39" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile39a" runat="server" placeholdertobind="phDefFile39a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    <tr id="row40" runat="server">
	    <td><CmsPlaceholders:MainAttachmentPlaceholderControl id="phFile40" runat="server" placeholdertobind="phDefFile40"></CmsPlaceholders:MainAttachmentPlaceholderControl></td>
	    <td id="col40" runat="server" class="alt"><CmsPlaceholders:AlternativeAttachmentPlaceholderControl id="phFile40a" runat="server" placeholdertobind="phDefFile40a"></CmsPlaceholders:AlternativeAttachmentPlaceholderControl></td></tr>
	    </tbody>
	    </table>
	</asp:placeholder>

	<CmsPlaceholders:RichHtmlplaceholdercontrol runat="server" editcontrolheight="150" placeholdertobind="phDefFootnote" ID="footnote" />
</div>

<CmsTemplates:Related runat="server" />