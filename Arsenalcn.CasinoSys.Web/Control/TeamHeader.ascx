<%@ Control Language="C#" CodeBehind="TeamHeader.ascx.cs" Inherits="Arsenalcn.CasinoSys.Web.Control.TeamHeader" %>
<div class="CasinoSys_Header">
    <div class="Clear">
    </div>
    <div class="GameItemList">
        <div class="CasinoSys_CategoryName">
            <asp:Literal ID="ltrlTeamDisplayName" runat="server"></asp:Literal>
        </div>
        <div class="CasinoSys_GameName">
            <asp:Literal ID="ltrlTeamLogo" runat="server"></asp:Literal>
        </div>
    </div>
    <div class="GameItemList CasinoSys_GameBet">
        <ul>
            <li>主教练:<asp:Literal ID="ltrlTeamCoach" runat="server"></asp:Literal></li>
            <li>主场:<asp:Literal ID="ltrlGround" runat="server"></asp:Literal></li>
            <li>容量:<asp:Literal ID="ltrlCapacity" runat="server"></asp:Literal></li>
        </ul>
    </div>
    <asp:Panel ID="pnlHistoryResult" CssClass="GameItemList CasinoSys_GameBet" runat="server">
        <ul>
            <li>比赛:<asp:Literal ID="ltrlMatchCount" runat="server"></asp:Literal></li>
            <li>胜:<asp:Literal ID="ltrlWonCount" runat="server"></asp:Literal></li>
            <li>平:<asp:Literal ID="ltrlDrawCount" runat="server"></asp:Literal></li>
            <li>负:<asp:Literal ID="ltrlLostCount" runat="server"></asp:Literal></li>
        </ul>
    </asp:Panel>
    <div class="Clear">
    </div>
</div>
