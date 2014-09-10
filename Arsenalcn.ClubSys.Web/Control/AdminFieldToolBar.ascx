<%@ Control Language="C#" CodeBehind="AdminFieldToolBar.ascx.cs" Inherits="Arsenalcn.ClubSys.Web.Control.AdminFieldToolBar"
    EnableViewState="false" %>
<div class="FieldToolBar">
    <div class="CtrlLeftPanelExp" onclick="$(this).toggleClass('CtrlLeftPanelExp'); $(this).toggleClass('CtrlLeftPanelCol'); $('#LeftPanel').toggle(); SwitchLeftPanel($(this).attr('class'))">
        <asp:Literal ID="ltrlAdminInfo" runat="server"></asp:Literal>
    </div>
    <div class="HeaderBtnBar">
        <a href="http://www.arsenal.cn/arsenalportal.aspx">基础数据管理</a>
    </div>
</div>
