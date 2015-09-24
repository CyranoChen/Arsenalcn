<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopContestList.ascx.cs"
    Inherits="Arsenalcn.CasinoSys.Web.Control.TopContestList" %>
<%@ OutputCache Duration="3600" VaryByParam="none" %>
<asp:Panel ID="pnlContestTop" runat="server" CssClass="InfoPanel">
    <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
        <asp:Literal ID="ltrlContestTitle" runat="server"></asp:Literal>
    </h3>
    <div class="Block">
        <ul>
            <li class="LiTitle">
                <a onclick="$(this).parent().parent().siblings('ul').hide();$('ul#ContestUpper').show();">上半赛区</a> |
                <a onclick="$(this).parent().parent().siblings('ul').hide();$('ul#ContestLower').show();">下半赛区</a> |
                <a href="http://bbs.arsenalcn.com/showtopic-108147.aspx" target="_blank"><em style="font-size: 12px; text-decoration: none">【规则】</em></a>
            </li>
        </ul>
        <asp:Repeater ID="rptContestUpper" runat="server" OnItemDataBound="rptContestUpper_ItemDataBound">
            <HeaderTemplate>
                <ul id="ContestUpper">
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Literal ID="ltrlUpperGamblerRank" runat="server"></asp:Literal>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
        <asp:Repeater ID="rptContestLower" runat="server" OnItemDataBound="rptContestLower_ItemDataBound">
            <HeaderTemplate>
                <ul id="ContestLower" style="display: none">
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Literal ID="ltrlLowerGamblerRank" runat="server"></asp:Literal>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
    </div>
</asp:Panel>
