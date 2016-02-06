<%@ Control Language="C#" CodeBehind="CasinoHeader.ascx.cs" Inherits="Arsenalcn.CasinoSys.Web.Control.CasinoHeader" %>
<div class="CasinoSys_Header">
    <div class="Clear">
    </div>
    <div class="GameItemList">
        <div class="CasinoSys_CategoryName">
            <asp:Literal ID="ltrlLeagueSeason" runat="server"></asp:Literal>
        </div>
        <div class="CasinoSys_GameName">
            <a class="StrongLink" href="CasinoTeam.aspx?Team=<%= HomeTeam.ID %>" title="<%= HomeTeam.TeamEnglishName %>">
                <%= HomeTeam.TeamDisplayName %>
            </a>
            <img src="<%= HomeTeam.TeamLogo %>" alt="<%= HomeTeam.TeamEnglishName %>"/>
            <a href="CasinoTeam.aspx?Match=<%= MatchGuid %>" title="查看历史交战记录">
                <em>vs</em>
            </a>
            <img src="<%= AwayTeam.TeamLogo %>" alt="<%= AwayTeam.TeamEnglishName %>"/>
            <a class="StrongLink" href="CasinoTeam.aspx?Team=<%= AwayTeam.ID %>" title="<%= AwayTeam.TeamEnglishName %>">
                <%= AwayTeam.TeamDisplayName %>
            </a>
        </div>
        <div class="CasinoSys_GameGround">
            <%= HomeTeam.Ground %>
            (Capacity:<%= HomeTeam.Capacity %>)
        </div>
        <div class="CasinoSys_GameTime">
            <asp:Literal ID="ltrlPlayTime" runat="server"></asp:Literal>
        </div>
    </div>
    <div class="GameItemList CasinoSys_GameBet">
        <ul>
            <li>主队胜:<asp:Literal ID="ltrlWin" runat="server"></asp:Literal></li>
            <li>双方平:<asp:Literal ID="ltrlDraw" runat="server"></asp:Literal></li>
            <li>客队胜:<asp:Literal ID="ltrlLose" runat="server"></asp:Literal></li>
            <li>投注次数:<em>
                    <asp:Literal ID="ltrlMatchBetCount" runat="server"></asp:Literal>
                </em>
            </li>
            <li>投注总量:<em>
                    <asp:Literal ID="ltrlMatchTotalBet" runat="server"></asp:Literal>
                </em>
            </li>
        </ul>
    </div>
    <asp:Panel ID="pnlMatchResult" CssClass="GameItemList CasinoSys_GameBet" runat="server">
        <ul>
            <li>比赛结果:<em>
                    <asp:Literal ID="ltrlMatchResult" runat="server"></asp:Literal>
                </em>
            </li>
            <li>比赛盈亏:<em>
                    <asp:Literal ID="ltrlEarning" runat="server"></asp:Literal>
                </em>
            </li>
            <li>单场最大注:<em>
                    <asp:Literal ID="ltrlTopBet" runat="server"></asp:Literal>
                </em>
            </li>
            <li>单场最大奖金:<em>
                    <asp:Literal ID="ltrlTopEarning" runat="server"></asp:Literal>
                </em>
            </li>
        </ul>
    </asp:Panel>
    <asp:Panel ID="pnlHistoryResult" CssClass="GameItemList CasinoSys_GameBet" runat="server">
        <ul>
            <li>交战次数:<em>
                    <asp:Literal ID="ltrlMatchCount" runat="server"></asp:Literal>
                </em>
            </li>
            <li>
                <a href="CasinoTeam.aspx?Team=<%= HomeTeam.ID %>">
                    <%= HomeTeam.TeamDisplayName %>
                </a>胜:<em>
                    <asp:Literal ID="ltrlHomeWon" runat="server"></asp:Literal>
                </em>
            </li>
            <li>双方平:<em>
                    <asp:Literal ID="ltrlHomeDraw" runat="server"></asp:Literal>
                </em>
            </li>
            <li>
                <a href="CasinoTeam.aspx?Team=<%= AwayTeam.ID %>">
                    <%= AwayTeam.TeamDisplayName %>
                </a>胜:<em>
                    <asp:Literal ID="ltrlHomeLost" runat="server"></asp:Literal>
                </em>
            </li>
        </ul>
    </asp:Panel>
    <div class="Clear">
    </div>
</div>