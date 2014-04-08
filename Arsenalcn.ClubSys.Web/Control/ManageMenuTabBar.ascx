<%@ Control Language="C#" CodeBehind="ManageMenuTabBar.ascx.cs" Inherits="Arsenalcn.ClubSys.Web.Control.ManageMenuTabBar"
    EnableViewState="false" %>
<div class="MenuTabBar">
    <ul>
        <li runat="server" id="liManageApplication"><a href="ManageApplication.aspx?ClubID=<%=ClubID %>">
            审核入会</a></li>
        <li runat="server" id="liManageClub"><a href="ManageClub.aspx?ClubID=<%=ClubID %>">球会管理</a></li>
        <li runat="server" id="liManageMemeber"><a href="ManageMember.aspx?ClubID=<%=ClubID %>">
            会员管理</a></li>
        <li><a href="ClubView.aspx?ClubID=<%=ClubID %>">返回球会首页</a></li>
    </ul>
</div>
