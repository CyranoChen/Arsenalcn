<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminFieldToolBar.ascx.cs"
    Inherits="Arsenal.Web.Control.AdminFieldToolBar" %>
<div class="FieldToolBar">
    <div class="CtrlLeftPanelExp" onclick="$(this).toggleClass('CtrlLeftPanelExp'); $(this).toggleClass('CtrlLeftPanelCol'); $('#LeftPanel').toggle(); SwitchLeftPanel($(this).attr('class'))">
        <asp:Literal ID="ltrlAdminInfo" runat="server"></asp:Literal>
    </div>
    <div class="HeaderBtnBar">
        <asp:Literal ID="ltrlMyIPInfo" runat="server"></asp:Literal>
    </div>
</div>
