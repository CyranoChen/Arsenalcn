<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" CodeBehind="ManageExtcredit.aspx.cs"
    Inherits="Arsenalcn.ClubSys.Web.ManageExtcredit" Title="{0} 转账给{1}" EnableViewState="false" %>

<%@ Register Src="Control/LeftPanel.ascx" TagName="LeftPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/FieldToolBar.ascx" TagName="FieldToolBar" TagPrefix="uc2" %>
<%@ Register Src="Control/ManageMenuTabBar.ascx" TagName="ManageMenuTabBar" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:LeftPanel ID="ctrlLeftPanel" runat="server" />
    <div id="MainPanel">
        <uc2:FieldToolBar ID="ctrlFieldToolBar" runat="server" />
        <uc4:ManageMenuTabBar ID="ctrlManageMenuTabBar" runat="server" />
        <asp:Panel ID="pnlInaccessible" runat="server" CssClass="ClubSys_Tip" Visible="false">
            <asp:Label ID="lblTips" runat="server" Text="您不是该球会会长或干事，不得进入此页面。" />
        </asp:Panel>
        <asp:PlaceHolder ID="phContent" runat="server">
            <table class="DataView" cellspacing="0" cellpadding="5">
                <tbody>
                    <tr class="ClubSys_Tip">
                        <td colspan="2">
                            <asp:Label ID="lblTransferInfo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">转账人:
                        </td>
                        <td align="left">
                            <asp:Literal ID="ltrlFromUserInfo" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">转账给:
                        </td>
                        <td align="left">
                            <asp:Literal ID="ltrlToUserInfo" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">转账金额:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbCash" runat="server" CssClass="TextBox" Text="请输入需要转账的金额" Width="200px"></asp:TextBox>
                            <asp:Label ID="lblMaxTransfer" runat="server"></asp:Label>
                            <asp:RangeValidator ID="rvMaxCash" runat="server" ControlToValidate="tbCash" ErrorMessage="*请输入范围内的整数 "
                                MinimumValue="1" SetFocusOnError="True" Type="Integer" ValidationGroup="CashTransfer"
                                EnableClientScript="true" Display="Dynamic"></asp:RangeValidator>
                            <asp:RequiredFieldValidator ID="rfvCash" runat="server" ControlToValidate="tbCash"
                                Display="Dynamic" ErrorMessage="*输入不能为空 " ValidationGroup="CashTransfer"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="FooterBtnBar">
                <asp:LinkButton ID="btnSave" runat="server" CssClass="LinkBtn SaveBtn" OnClick="btnSave_Click"
                    ValidationGroup="CashTransfer" OnClientClick="return confirm('确认转账信息?')">保存</asp:LinkButton>
                <asp:LinkButton ID="btnReset" runat="server" CssClass="LinkBtn ResetBtn" OnClick="btnReset_Click">重置</asp:LinkButton>
            </div>
        </asp:PlaceHolder>
    </div>
</asp:Content>
