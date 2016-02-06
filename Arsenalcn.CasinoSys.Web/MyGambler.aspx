<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" CodeBehind="MyGambler.aspx.cs"
Inherits="Arsenalcn.CasinoSys.Web.MyGambler" Title="我的帐户管理" %>
<%@ Import Namespace="Arsenalcn.CasinoSys.Entity" %>

<%@ Register Src="Control/LeftPanel.ascx" TagName="LeftPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/FieldTooBar.ascx" TagName="FieldTooBar" TagPrefix="uc2" %>
<%@ Register Src="Control/MenuTabBar.ascx" TagName="MenuTabBar" TagPrefix="uc3" %>
<%@ Register Src="Control/GamblerHeader.ascx" TagName="GamblerHeader" TagPrefix="uc4" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">

    <script type="text/javascript">
        $(function () {
            $(".DataView a.CashBtn").click(function() { return confirm('确认操作?') });
        });
    </script>

</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:LeftPanel ID="ctrlLeftPanel" runat="server"/>
    <div id="MainPanel">
        <uc2:FieldTooBar ID="ctrlFieldTooBar" runat="server"/>
        <uc3:MenuTabBar ID="ctrlMenuTabBar" runat="server"/>
        <uc4:GamblerHeader ID="ctrlGamblerHeader" runat="server"/>
        <div class="CasinoSys_MainInfo">
            <table class="DataView" cellspacing="0" cellpadding="5">
                <tbody>
                <tr class="CasinoSys_Tip">
                    <td colspan="2">
                        <span>您现有枪手币<em><asp:Literal ID="ltrlUserQSB" runat="server"></asp:Literal></em> | 博彩币<em><asp:Literal
                                ID="ltrlUserCash" runat="server"></asp:Literal></em> | 转换比率为<em>1:<%= ConfigGlobal.ExchangeRate %></em>
                                | 提现手续费<em><%= ConfigGlobal.ExchangeFee.ToString("p0") %></em></span>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="FieldHeader">
                        枪手币<em style="font-family: Impact">→</em>博彩币:
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbCash" runat="server" CssClass="TextBox" Text="请输入要买入的博彩币" Width="200px"></asp:TextBox>
                        <asp:LinkButton ID="btnToCash" runat="server" Text="充值" CssClass="LinkBtn CashBtn"
                                        OnClick="btnToCash_Click" ValidationGroup="ToCash"/>
                        <span>* 最多为可充值<em><asp:Literal ID="ltrlMaxCash" runat="server"></asp:Literal></em>博彩币，无需手续费</span>
                        <asp:RangeValidator ID="rvToCash" runat="server" ControlToValidate="tbCash" ErrorMessage="*请输入范围内的整数"
                                            MinimumValue="10" SetFocusOnError="True" Type="Integer" ValidationGroup="ToCash"
                                            EnableClientScript="true" Display="Dynamic">
                        </asp:RangeValidator>
                        <asp:RequiredFieldValidator ID="rfvToCash" runat="server" ControlToValidate="tbCash"
                                                    Display="Dynamic" ErrorMessage="*输入不能为空" ValidationGroup="ToCash">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="FieldHeader">
                        博彩币<em style="font-family: Impact">→</em>枪手币:
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbQSB" runat="server" CssClass="TextBox" Text="请输入要买入的枪手币" Width="200px"></asp:TextBox>
                        <asp:LinkButton ID="btnToQSB" runat="server" Text="提现" CssClass="LinkBtn CashBtn"
                                        OnClick="btnToQSB_Click" ValidationGroup="ToQSB"/>
                        <span>* 最多为可套现<em><asp:Literal ID="ltrlMaxQSB" runat="server"></asp:Literal></em>枪手币，2%手续费</span>
                        <asp:RangeValidator ID="rvMaxQSB" runat="server" ControlToValidate="tbQSB" ErrorMessage="*请输入范围内的整数"
                                            MinimumValue="1" SetFocusOnError="True" Type="Integer" ValidationGroup="ToQSB"
                                            EnableClientScript="true" Display="Dynamic">
                        </asp:RangeValidator>
                        <asp:RequiredFieldValidator ID="rfvToQSB" runat="server" ControlToValidate="tbQSB"
                                                    Display="Dynamic" ErrorMessage="*输入不能为空" ValidationGroup="ToQSB">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                </tbody>
            </table>
        </div>
    </div>
</asp:Content>