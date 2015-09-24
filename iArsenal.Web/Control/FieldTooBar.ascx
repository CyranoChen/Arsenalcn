<%@ Control Language="C#" CodeBehind="FieldTooBar.ascx.cs"
    Inherits="iArsenal.Web.Control.FieldTooBar" %>
<div class="FieldToolBar">
    <div class="CtrlLeftPanelExp" onclick="$(this).toggleClass('CtrlLeftPanelExp'); $(this).toggleClass('CtrlLeftPanelCol'); $('#LeftPanel').toggle(); SwitchLeftPanel($(this).attr('class'))">
        <asp:PlaceHolder ID="phAnonymous" runat="server"><strong>欢迎进入，请在<a href="/login.aspx"
            target="_self">登录</a>后使用全部功能</strong></asp:PlaceHolder>
    </div>
    <asp:Panel ID="pnlFuncLink" runat="server" CssClass="HeaderBtnBar">
        <a href="AdminConfig.aspx">后台管理</a>
    </asp:Panel>
</div>
