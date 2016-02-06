<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopMatchList.ascx.cs"
Inherits="Arsenalcn.CasinoSys.Web.Control.TopMatchList" %>
<%@ OutputCache Duration="3600" VaryByParam="none" %>
<asp:Panel ID="pnlMatchTop" runat="server" CssClass="InfoPanel">
    <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
        <a>庄家排行榜(月排行)</a>
    </h3>
    <div class="Block">
        <ul>
            <li class="LiTitle">
                <a onclick="$(this).parent().parent().siblings('ul').hide();$('ul#MatchEarning').show();">
                    <asp:Literal ID="ltrlEarning" runat="server"></asp:Literal>
                </a> |
                <a onclick="$(this).parent().parent().siblings('ul').hide();$('ul#MatchLoss').show();">
                    <asp:Literal ID="ltrlLoss" runat="server"></asp:Literal>
                </a>
            </li>
        </ul>
        <asp:Repeater ID="rptMatchEarning" runat="server" OnItemDataBound="rptMatchEarning_ItemDataBound">
            <HeaderTemplate>
                <ul id="MatchEarning">
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Literal ID="ltrlMatchInfo" runat="server"></asp:Literal>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
        <asp:Repeater ID="rptMatchLoss" runat="server" OnItemDataBound="rptMatchLoss_ItemDataBound">
            <HeaderTemplate>
                <ul id="MatchLoss" style="display: none">
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Literal ID="ltrlMatchInfo" runat="server"></asp:Literal>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
    </div>
</asp:Panel>