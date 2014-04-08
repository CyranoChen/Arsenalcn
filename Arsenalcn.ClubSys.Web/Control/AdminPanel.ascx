<%@ Control Language="C#" CodeBehind="AdminPanel.ascx.cs" Inherits="Arsenalcn.ClubSys.Web.Control.AdminPanel"
    EnableViewState="false" %>
<div id="LeftPanel">
    <div class="InfoPanel">
        <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
            <a>后台管理菜单</a></h3>
        <div class="Block">
            <ul>
                <li><a href="AdminConfig.aspx">全局配置</a></li>
                <li><a href="AdminAward.aspx">奖励发放</a></li>
                <li><a href="AdminClubInfo.aspx">球会管理</a></li>
                <li><a href="AdminRank.aspx">积分评价</a></li>
                <li><a href="AdminPlayer.aspx">会员管理</a></li>
                <li><a href="AdminStrip.aspx">装备日志</a></li>
                <li><a href="AdminHistory.aspx">历史日志</a></li>
                <li><a href="AdminEvent.aspx">任务日志</a></li>
            </ul>
        </div>
    </div>
</div>
