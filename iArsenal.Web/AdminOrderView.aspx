<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="AdminOrderView.aspx.cs" Inherits="iArsenal.Web.AdminOrderView"
    Title="后台管理 添加/更新订单" Theme="Arsenalcn" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<%@ Register Src="Control/CustomPagerInfo.ascx" TagName="CustomPagerInfo" TagPrefix="uc3" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphhead" runat="server">
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:AdminPanel ID="pnlAdmin" runat="server" />
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server" />
        <uc3:CustomPagerInfo ID="ctrlCustomPagerInfo" runat="server" />
        <div class="Arsenal_MainInfo">
            <table class="DataView">
                <thead>
                    <tr class="Header">
                        <th colspan="4">
                            <asp:Label ID="lblOrderInfo" runat="server"></asp:Label>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="Row">
                        <td class="FieldHeader">会员信息:
                        </td>
                        <td class="FieldColumn" id="tdMemberInfo">
                            <asp:TextBox ID="tbMemberID" runat="server" CssClass="TextBox MemberID" Width="50px"></asp:TextBox>
                            <asp:TextBox ID="tbMemberName" runat="server" CssClass="TextBoxRead MemberName"></asp:TextBox>
                            <a id="btnMemberCheck" class="LinkBtn" href="javascript:MemberCheck()">检查</a>
                        </td>
                        <td class="FieldHeader">有效:
                        </td>
                        <td class="FieldColumn">
                            <asp:CheckBox ID="cbIsActive" runat="server" Checked="true" Text="有效" />
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">状态:
                        </td>
                        <td class="FieldColumn">
                            <asp:DropDownList ID="ddlStatus" runat="server">
                                <asp:ListItem Value="1" Text="未提交"></asp:ListItem>
                                <asp:ListItem Value="2" Text="审核中"></asp:ListItem>
                                <asp:ListItem Value="21" Text="已审核"></asp:ListItem>
                                <asp:ListItem Value="3" Text="已确认"></asp:ListItem>
                                <asp:ListItem Value="4" Text="已下单"></asp:ListItem>
                                <asp:ListItem Value="5" Text="已发货"></asp:ListItem>
                                <asp:ListItem Value="0" Text="未知" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="FieldHeader">评价:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbRate" runat="server" CssClass="TextBox" Width="80px" Text="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">创建时间:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbCreateTime" runat="server" CssClass="TextBox" Width="180px"></asp:TextBox>
                        </td>
                        <td class="FieldHeader">更新时间:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbUpdateTime" runat="server" CssClass="TextBox" Width="180px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">收货手机:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbMobile" runat="server" CssClass="TextBox" Width="180px"></asp:TextBox>
                        </td>
                        <td class="FieldHeader">付款方式:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbPayment" runat="server" CssClass="TextBox" Width="180px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">收货地址:
                        </td>
                        <td class="FieldColumn" colspan="3">
                            <asp:TextBox ID="tbAddress" runat="server" CssClass="TextBox" Width="400px" TextMode="MultiLine"
                                Rows="3"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">定金:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbDeposit" runat="server" CssClass="TextBox" Width="180px"></asp:TextBox>
                        </td>
                        <td class="FieldHeader">邮费:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbPostage" runat="server" CssClass="TextBox" Width="180px" Text="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">总价:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbPrice" runat="server" CssClass="TextBox" Width="180px" Text="0"></asp:TextBox>
                        </td>
                        <td class="FieldHeader">优惠价:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbSale" runat="server" CssClass="TextBox" Width="180px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">描述:
                        </td>
                        <td class="FieldColumn" colspan="3">
                            <asp:TextBox ID="tbDescription" runat="server" CssClass="TextBox" Width="400px" TextMode="MultiLine"
                                Rows="3"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">备注:
                        </td>
                        <td class="FieldColumn" colspan="3">
                            <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBox" Width="400px" TextMode="MultiLine"
                                Rows="3"></asp:TextBox>
                        </td>
                    </tr>
                </tbody>
            </table>
            <asp:GridView ID="gvOrderItem" runat="server" DataKeyNames="ID" OnPageIndexChanging="gvOrderItem_PageIndexChanging"
                PageSize="10" OnSelectedIndexChanged="gvOrderItem_SelectedIndexChanged">
                <Columns>
                    <asp:BoundField HeaderText="编号" DataField="ID" />
                    <asp:BoundField HeaderText="创建时间" DataField="CreateTime" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" Visible="false" />
                    <asp:BoundField HeaderText="编码" DataField="Code" DataFormatString="<em>{0}</em>"
                        HtmlEncode="false" />
                    <asp:BoundField HeaderText="名称" DataField="ProductName" />
                    <asp:BoundField HeaderText="尺寸" DataField="Size" NullDisplayText="/" />
                    <asp:BoundField HeaderText="单价" DataField="UnitPrice" DataFormatString="{0:f2}" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField HeaderText="数量" DataField="Quantity" />
                    <asp:BoundField HeaderText="优惠" DataField="Sale" DataFormatString="{0:f2}" ItemStyle-HorizontalAlign="Right" NullDisplayText="/" />
                    <asp:BoundField HeaderText="状态" DataField="IsActive" />
                    <asp:CommandField ShowSelectButton="true" HeaderText="操作" EditText="修改" SelectText="详细"
                        UpdateText="保存" CancelText="取消" DeleteText="删除" ControlStyle-CssClass="LinkBtn" />
                </Columns>
            </asp:GridView>
            <div class="FooterBtnBar">
                <asp:Button ID="btnSubmit" runat="server" CssClass="InputBtn SubmitBtn" Text="保存订单"
                    OnClick="btnSubmit_Click" OnClientClick="return confirm('保存该订单信息')" />
                <asp:Button ID="btnCalc" runat="server" CssClass="InputBtn" Text="计算总价" OnClick="btnCalc_Click"
                    OnClientClick="return confirm('重新计算本订单所有许愿总价？')" />
                <asp:Button ID="btnCancel" runat="server" CssClass="InputBtn" Text="返回" OnClick="btnCancel_Click" />
                <asp:Button ID="btnDelete" runat="server" CssClass="InputBtn" Text="删除订单" OnClick="btnDelete_Click"
                    OnClientClick="return confirm('删除该订单与其关联的许愿信息?(无法恢复)')" />
            </div>
        </div>
    </div>
</asp:Content>
