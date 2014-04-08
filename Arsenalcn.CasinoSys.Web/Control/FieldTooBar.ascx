<%@ Control Language="C#" CodeBehind="FieldTooBar.ascx.cs" Inherits="Arsenalcn.CasinoSys.Web.Control.FieldTooBar" %>
<div class="FieldToolBar">
    <div class="CtrlLeftPanelExp" onclick="$(this).toggleClass('CtrlLeftPanelExp'); $(this).toggleClass('CtrlLeftPanelCol'); $('#LeftPanel').toggle(); SwitchLeftPanel($(this).attr('class'))">
        <asp:Literal ID="ltrlToolBarTip" runat="server"></asp:Literal>
    </div>
    <asp:Panel ID="pnlFuncLink" runat="server" CssClass="HeaderBtnBar">
        <a href="AdminConfig.aspx">后台管理</a> <a href="/plugin/Arsenal/AdminConfig.aspx">基础数据管理</a>
    </asp:Panel>
</div>
