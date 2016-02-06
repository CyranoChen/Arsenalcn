<%@ Control Language="C#" CodeBehind="MenuTabBar.ascx.cs" Inherits="Arsenalcn.ClubSys.Web.Control.MenuTabBar"
EnableViewState="false" %>
<div class="MenuTabBar">
    <ul>
        <li runat="server" id="liClubInfo">
            <a href="ClubView.aspx?ClubID=<%= ClubID %>">基本信息</a>
        </li>
        <li runat="server" id="liClubPlayer">
            <a href="ClubPlayer.aspx?ClubID=<%= ClubID %>">现役球员</a>
        </li>
        <li runat="server" id="liClubMember">
            <a href="ClubMember.aspx?ClubID=<%= ClubID %>">球会成员</a>
        </li>
        <li runat="server" id="liClubRank">
            <a href="ClubRank.aspx?ClubID=<%= ClubID %>">等级评价</a>
        </li>
        <li runat="server" id="liClubVideo">
            <a href="ClubVideo.aspx?ClubID=<%= ClubID %>">视频名人堂</a>
        </li>
        <li runat="server" id="liClubStrip">
            <a href="ClubStrip.aspx?ClubID=<%= ClubID %>">装备日志</a>
        </li>
        <li runat="server" id="liClubHistory">
            <a href="ClubHistory.aspx?ClubID=<%= ClubID %>">
                历史日志
            </a>
        </li>
    </ul>
</div>