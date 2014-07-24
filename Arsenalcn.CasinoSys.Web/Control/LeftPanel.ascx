<%@ Control Language="C#" CodeBehind="LeftPanel.ascx.cs" Inherits="Arsenalcn.CasinoSys.Web.Control.LeftPanel" %>
<%@ Register Src="TopContestList.ascx" TagName="TopContestList" TagPrefix="uc1" %>
<%@ Register Src="TopGamblerList.ascx" TagName="TopGamblerList" TagPrefix="uc2" %>
<div id="LeftPanel">
    <asp:Panel ID="pnlNotice" runat="server" CssClass="InfoPanel">
        <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
            <a>ACN博彩公告</a>
        </h3>
        <div class="Block">
            <p class="CasinoSys_Notice">
                <%=Arsenalcn.CasinoSys.Entity.ConfigGlobal.SysNotice %>
            </p>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlMyCasino" runat="server" CssClass="InfoPanel">
        <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
            <a>我的博彩面板</a>
        </h3>
        <div class="Block">
            <ul>
                <li class="LiTitle">博彩币:<em style="font-size: 12px; margin: 0px 2px">
                    <asp:Literal ID="ltrlCash" runat="server"></asp:Literal>
                    (RP:<asp:Literal ID="ltrlUserRP" runat="server"></asp:Literal>)</em></li>
                <li><a href="MyGambler.aspx">我的账户(充值|套现)</a></li>
                <li><a href="MyCoupon.aspx">我的投注单</a></li>
                <li><a href="MyBetLog.aspx">我的中奖查询</a></li>
                <li><a href="MyBonusLog.aspx">我的盈亏情况</a></li>
            </ul>
        </div>
    </asp:Panel>
    <uc1:TopContestList ID="ctrlTopContestList" runat="server" Visible="false" />
    <uc2:TopGamblerList ID="ctrlTopGamblerList" runat="server" />
    <asp:Panel ID="pnlDev" runat="server" CssClass="InfoPanel">
        <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
            <a>程序开发与支持</a>
        </h3>
        <div class="Block">
            <ul>
                <li>开发: <a href="mailto:cao_yi_hui@hotmail.com">Cao262</a></li>
                <li>设计: <a href="mailto:cyrano@arsenalcn.com">Cyrano</a></li>
            </ul>
        </div>
    </asp:Panel>
</div>
