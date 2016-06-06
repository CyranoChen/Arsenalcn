<%@ Control Language="C#" CodeBehind="LeftPanel.ascx.cs" Inherits="Arsenalcn.CasinoSys.Web.Control.LeftPanel" %>
<%@ Import Namespace="Arsenalcn.CasinoSys.Entity" %>
<%@ Register Src="TopContestList.ascx" TagName="TopContestList" TagPrefix="uc1" %>
<%@ Register Src="TopGamblerList.ascx" TagName="TopGamblerList" TagPrefix="uc2" %>
<%@ Register Src="TopMatchList.ascx" TagName="TopMatchList" TagPrefix="uc3" %>
<div id="LeftPanel">
    <asp:Panel ID="pnlNotice" runat="server" CssClass="InfoPanel">
        <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
            <a>ACN博彩公告</a>
        </h3>
        <div class="Block">
            <p class="CasinoSys_Notice">
                <%= ConfigGlobal.SysNotice %>
            </p>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlMyCasino" runat="server" CssClass="InfoPanel">
        <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
            <a>我的博彩面板</a>
        </h3>
        <div class="Block">
            <ul>
                <asp:Literal ID="ltrlMyGamblerInfo" runat="server"></asp:Literal>
                <li>
                    <a href="MyGambler.aspx">我的账户(充值|套现)</a>
                </li>
                <li>
                    <a href="MyCoupon.aspx">我的投注单</a>
                </li>
                <li>
                    <a href="MyBetLog.aspx">我的中奖查询</a>
                </li>
                <li>
                    <a href="MyBonusLog.aspx">我的盈亏情况</a>
                </li>
            </ul>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlWeChat" runat="server" CssClass="InfoPanel">
        <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
            <a>ACN微信公众号</a>
        </h3>
        <div class="Block" style="text-align: center; background: #ffffff">
            <img src="http://www.arsenalcn.com/arsenalcn-qrcode.png" alt="OfficialArsenalCN" />
        </div>
    </asp:Panel>
    <uc1:TopContestList ID="ctrlTopContestList" runat="server" />
    <uc2:TopGamblerList ID="ctrlTopGamblerList" runat="server" Visible="False" />
    <uc3:TopMatchList ID="ctrlTopMatchList" runat="server" Visible="False" />
    <asp:Panel ID="pnlDev" runat="server" CssClass="InfoPanel" Visible="False">
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
