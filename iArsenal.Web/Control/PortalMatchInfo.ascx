<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PortalMatchInfo.ascx.cs" Inherits="iArsenal.Web.Control.PortalMatchInfo" %>
<div class="InfoPanel">
    <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
        <a>比赛信息</a></h3>
    <div class="Block">
        <div class="MatchTicket_CategoryName">
            <asp:Label ID="lblLeagueSeason" runat="server"></asp:Label>
        </div>
        <div class="MatchTicket_TeamInfo">
            <asp:Literal ID="ltrlTeamInfo" runat="server"></asp:Literal>
        </div>
        <div class="MatchTicket_TicketInfo">
            <asp:Label ID="lblTicketInfo" runat="server"></asp:Label>
        </div>
        <div class="MatchTicket_GameTime">
            <asp:Label ID="lblGameTime" runat="server"></asp:Label>
        </div>
    </div>
</div>
