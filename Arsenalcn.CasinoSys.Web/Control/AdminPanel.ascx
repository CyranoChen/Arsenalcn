<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminPanel.ascx.cs"
Inherits="Arsenalcn.CasinoSys.Web.Control.AdminPanel" %>
<div id="LeftPanel">
    <div class="InfoPanel">
        <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
            <a>后台管理菜单</a>
        </h3>
        <div class="Block">
            <ul>
                <li>
                    <a href="AdminConfig.aspx">全局配置</a>
                </li>
                <li>
                    <a href="AdminMatch.aspx">比赛管理</a>
                </li>
                <li>
                    <a href="AdminGambler.aspx">玩家管理</a>
                </li>
                <li>
                    <a href="AdminBet.aspx">投注日志</a>
                </li>
            </ul>
        </div>
    </div>
</div>