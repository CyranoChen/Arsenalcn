<%@ Control Language="C#" CodeBehind="FieldToolBar.ascx.cs" Inherits="Arsenalcn.ClubSys.Web.Control.FieldToolBar"
    EnableViewState="false" %>
<div class="FieldToolBar">
    <div class="CtrlLeftPanelExp" onclick="$(this).toggleClass('CtrlLeftPanelExp'); $(this).toggleClass('CtrlLeftPanelCol'); $('#LeftPanel').toggle(); SwitchLeftPanel($(this).attr('class'))">
        <asp:Literal ID="ltrlToolBarTip" runat="server"></asp:Literal>
    </div>
    <asp:Panel ID="pnlFuncLink" runat="server" CssClass="HeaderBtnBar">
        <a href="ClubLuckyPlayerLog.aspx">幸运球员日志</a>
        <asp:PlaceHolder ID="phAdministrator" runat="server">
            <a href="AdminConfig.aspx">后台管理</a>
            <a href="MyCreateClub.aspx" style="display: none">创建新球会</a>
        </asp:PlaceHolder>
    </asp:Panel>
</div>
<asp:PlaceHolder ID="phLuckPlayer" runat="server">
    <div class="ClubSys_Tip">
        <label>
            幸运球员:<em><asp:Literal ID="ltrlLuckyPlayerName" runat="server"></asp:Literal></em>
            <asp:LinkButton ID="btnGetBonus" runat="server" CssClass="ClubSys_LuckyGet" Text="请领取"
                OnClick="btnGetBonus_Click"></asp:LinkButton>
            奖金共<em><asp:Literal ID="ltrlBonus" runat="server"></asp:Literal></em>枪手币</label>
    </div>
</asp:PlaceHolder>
