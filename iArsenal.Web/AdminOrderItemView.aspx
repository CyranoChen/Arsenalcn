<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
CodeBehind="AdminOrderItemView.aspx.cs" Inherits="iArsenal.Web.AdminOrderItemView"
Title="后台管理 添加/更新许愿单" Theme="Arsenalcn" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphhead" runat="server">
    <style type="text/css">
        input.ProductGuid { display: none; }
    </style>

</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:AdminPanel ID="pnlAdmin" runat="server"/>
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server"/>
        <div class="Arsenal_MainInfo">
            <table class="DataView">
                <thead>
                <tr class="Header">
                    <th colspan="4">
                        <asp:Label ID="lblOrderItemInfo" runat="server"></asp:Label>
                    </th>
                </tr>
                </thead>
                <tbody>
                <tr class="CommandRow">
                    <td colspan="4">
                        -- 许愿信息栏 --
                    </td>
                </tr>
                <tr class="Row">
                    <td class="FieldHeader">
                        会员信息:
                    </td>
                    <td class="FieldColumn" id="tdMemberInfo">
                        <asp:TextBox ID="tbMemberID" runat="server" CssClass="TextBox MemberID" Width="50px"></asp:TextBox>
                        <asp:TextBox ID="tbMemberName" runat="server" CssClass="TextBoxRead MemberName"></asp:TextBox>
                        <a id="btnMemberCheck" class="LinkBtn" href="javascript:MemberCheck()">检查</a>
                    </td>
                    <td class="FieldHeader">
                        创建时间:
                    </td>
                    <td class="FieldColumn">
                        <asp:TextBox ID="tbCreateTime" runat="server" CssClass="TextBoxRead" Width="180px"></asp:TextBox>
                    </td>
                </tr>
                <tr class="AlternatingRow">
                    <td class="FieldHeader">
                        关联订单:
                    </td>
                    <td class="FieldColumn">
                        <asp:TextBox ID="tbOrderID" runat="server" CssClass="TextBox" Width="180px"></asp:TextBox>
                    </td>
                    <td class="FieldHeader">
                        状态:
                    </td>
                    <td class="FieldColumn">
                        <asp:CheckBox ID="cbIsActive" runat="server" Checked="true" Text="有效"/>
                    </td>
                </tr>
                <tr class="CommandRow">
                    <td colspan="4">
                        -- 商品信息栏 --
                    </td>
                </tr>
                <tr class="Row">
                    <td class="FieldHeader">
                        商品编码:
                    </td>
                    <td class="FieldColumn ProductInfo">
                        <asp:TextBox ID="tbCode" runat="server" CssClass="TextBox ProductCode" Width="100px"></asp:TextBox>
                        <a id="btnProductCheck" class="LinkBtn" href="javascript:ProductCheck()">检查</a>
                    </td>
                    <td class="FieldHeader">
                        商品名称:
                    </td>
                    <td class="FieldColumn ProductInfo">
                        <asp:TextBox ID="tbProductGuid" runat="server" CssClass="TextBox ProductGuid"></asp:TextBox>
                        <asp:TextBox ID="tbProductName" runat="server" CssClass="TextBox ProductName" Width="180px"></asp:TextBox>
                    </td>
                </tr>
                <tr class="AlternatingRow">
                    <td class="FieldHeader">
                        尺寸:
                    </td>
                    <td class="FieldColumn ProductInfo">
                        <asp:TextBox ID="tbSize" runat="server" CssClass="TextBox" Width="180px"></asp:TextBox>
                    </td>
                    <td class="FieldHeader">
                        单价:
                    </td>
                    <td class="FieldColumn ProductInfo">
                        <asp:TextBox ID="tbUnitPrice" runat="server" CssClass="TextBox ProductUnitPrice"
                                     Width="100px" Text="0">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="FieldHeader">
                        数量:
                    </td>
                    <td class="FieldColumn ProductInfo">
                        <asp:TextBox ID="tbQuantity" runat="server" CssClass="TextBox ProductQuantity" Width="100px"
                                     Text="0">
                        </asp:TextBox>
                    </td>
                    <td class="FieldHeader">
                        优惠价/总价:
                    </td>
                    <td class="FieldColumn ProductInfo">
                        <asp:TextBox ID="tbSale" runat="server" CssClass="TextBox" Width="100px"></asp:TextBox>
                        <span>/</span>
                        <asp:Label ID="lblPrice" runat="server" CssClass="ProductTotalPrice" Text="?"></asp:Label>
                    </td>
                </tr>
                <tr class="AlternatingRow">
                    <td class="FieldHeader">
                        备注:
                    </td>
                    <td class="FieldColspan" colspan="3">
                        <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBox" Width="400px" TextMode="MultiLine"
                                     Rows="4">
                        </asp:TextBox>
                    </td>
                </tr>
                </tbody>
            </table>
            <div class="FooterBtnBar">
                <asp:Button ID="btnSubmit" runat="server" CssClass="InputBtn SubmitBtn" Text="保存许愿"
                            OnClick="btnSubmit_Click" OnClientClick="return confirm('保存该许愿信息')"/>
                <asp:Button ID="btnCancel" runat="server" CssClass="InputBtn" Text="返回" OnClick="btnCancel_Click"/>
                <asp:Button ID="btnBackOrder" runat="server" CssClass="InputBtn" Text="返回订单" OnClick="btnBackOrder_Click" Visible="false"/>
                <asp:Button ID="btnDelete" runat="server" CssClass="InputBtn" Text="删除许愿" OnClick="btnDelete_Click"
                            OnClientClick="return confirm('删除该许愿信息?(无法恢复)')"/>
            </div>
        </div>
    </div>
</asp:Content>