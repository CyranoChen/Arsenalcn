<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CollectionTabBar.ascx.cs"
Inherits="Arsenalcn.ClubSys.Web.Control.CollectionTabBar" %>
<div class="MenuTabBar">
    <ul>
        <li id="liVideo" runat="server">
            <a href="MyCollection.aspx?type=Video<%= QueryStringUserID %>">视频集锦(<%= ActiveVideoCount %>)</a>
        </li>
        <li id="liCard" runat="server">
            <a href="MyCollection.aspx?type=Card<%= QueryStringUserID %>">球星卡(<%= ActiveCardCount %>)</a>
        </li>
        <li id="liInactiveCard" runat="server">
            <a href="MyCollection.aspx?type=InactiveCard<%= QueryStringUserID %>">未激活的球星卡(<%= InactiveCardCount %>)</a>
        </li>
        <li id="liInactiveVideo" runat="server">
            <a href="MyCollection.aspx?type=InactiveVideo<%= QueryStringUserID %>">未激活的视频卡(<%= InactiveVideoCount %>)</a>
        </li>
    </ul>
</div>