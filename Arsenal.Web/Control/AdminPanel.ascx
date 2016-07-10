<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminPanel.ascx.cs"
    Inherits="Arsenal.Web.Control.AdminPanel" %>
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
                    <a href="AdminSchedule.aspx">计划任务</a>
                </li>
                <li>
                    <a href="AdminLog.aspx">日志查询</a>
                </li>
            </ul>
        </div>
    </div>
    <div class="InfoPanel">
        <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
            <a>会员管理菜单</a>
        </h3>
        <div class="Block">
            <ul>
                <li>
                    <a href="AdminMembership.aspx">会员管理</a>
                </li>
                <li>
                    <a href="AdminLogSignIn.aspx">签到管理</a>
                </li>
            </ul>
        </div>
    </div>
    <div class="InfoPanel">
        <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
            <a>阿森纳球员管理菜单</a>
        </h3>
        <div class="Block">
            <ul>
                <li>
                    <a href="AdminPlayer.aspx">球员管理</a>
                </li>
                <li>
                    <a href="AdminVideo.aspx">视频管理</a>
                </li>
            </ul>
        </div>
    </div>
    <div class="InfoPanel">
        <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
            <a>阿森纳球队管理菜单</a>
        </h3>
        <div class="Block">
            <ul>
                <li>
                    <a href="AdminMatch.aspx">比赛管理</a>
                </li>
                <li>
                    <a href="AdminTeam.aspx">球队管理</a>
                </li>
                <li>
                    <a href="AdminGroup.aspx">分组管理</a>
                </li>
                <li>
                    <a href="AdminLeague.aspx">分类管理</a>
                </li>
            </ul>
        </div>
    </div>
</div>
