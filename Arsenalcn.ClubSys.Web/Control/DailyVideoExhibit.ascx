<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DailyVideoExhibit.ascx.cs"
    Inherits="Arsenalcn.ClubSys.Web.Control.DailyVideoExhibit" %>
<%@ OutputCache Duration="3600" VaryByParam="none" %>
<asp:Panel ID="pnlVideoExhibit" runat="server" CssClass="InfoPanel">
    <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
        <a>每日集锦赏析</a>
    </h3>
    <div class="Block ClubSys_CollectionInfo" style="margin: 0px;">
        <asp:Literal ID="ltrlViewBtnLeft" runat="server"></asp:Literal>
        <div class="ClubSys_ItemPH" style="float: none; margin: 0px; padding: 0px;">

            <script type="text/javascript">
                GenSwfObject('PlayerVideoActive', 'swf/PlayerVideoActive.swf?XMLURL=ServerXml.aspx%3FVideoGuid=<%=VideoGuid.ToString() %>&ShowEffect=true', '160', '200');
            </script>

        </div>
        <div class="Clear">
        </div>
    </div>
</asp:Panel>
