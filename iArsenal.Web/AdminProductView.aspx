<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="AdminProductView.aspx.cs" Inherits="iArsenal.Web.AdminProductView"
    Title="后台管理 添加/更新商品" Theme="Arsenalcn" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<%@ Register Src="Control/CustomPagerInfo.ascx" TagName="CustomPagerInfo" TagPrefix="uc3" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphhead" runat="server">
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:AdminPanel ID="pnlAdmin" runat="server" />
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server" />
        <div class="Arsenal_MainInfo">
            <table class="DataView">
                <thead>
                    <tr class="Header">
                        <th colspan="4">添加/更新官方球迷会的团购商品名录
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="Row">
                        <td class="FieldHeader">商品GUID:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbProductGuid" runat="server" CssClass="TextBoxRead" Width="180px"></asp:TextBox>
                        </td>
                        <td class="FieldHeader">编码:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbCode" runat="server" CssClass="TextBox" Width="100px"></asp:TextBox>
                            <asp:CheckBox ID="cbIsActive" runat="server" Checked="true" Text="有效" />
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">名称:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbName" runat="server" CssClass="TextBox" Width="180px"></asp:TextBox>
                        </td>
                        <td class="FieldHeader">译名:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbDisplayName" runat="server" CssClass="TextBox" Width="180px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">类型:
                        </td>
                        <td class="FieldColumn">
                            <asp:DropDownList ID="ddlProductType" runat="server">
                                <asp:ListItem Value="" Text="--类型--" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="1" Text="主场球衣"></asp:ListItem>
                                <asp:ListItem Value="2" Text="客场球衣"></asp:ListItem>
                                <asp:ListItem Value="8" Text="杯赛球衣"></asp:ListItem>
                                <asp:ListItem Value="3" Text="印名字"></asp:ListItem>
                                <asp:ListItem Value="4" Text="印号码"></asp:ListItem>
                                <asp:ListItem Value="5" Text="特殊字体"></asp:ListItem>
                                <asp:ListItem Value="6" Text="英超袖标"></asp:ListItem>
                                <asp:ListItem Value="7" Text="欧冠袖标"></asp:ListItem>
                                <asp:ListItem Value="10" Text="观赛计划"></asp:ListItem>
                                <asp:ListItem Value="11" Text="观赛同伴"></asp:ListItem>
                                <asp:ListItem Value="20" Text="主场球票"></asp:ListItem>
                                <asp:ListItem Value="21" Text="友谊赛球票"></asp:ListItem>
                                <asp:ListItem Value="31" Text="会员费(Core)"></asp:ListItem>
                                <asp:ListItem Value="32" Text="会员费(Premier)"></asp:ListItem>
                                <asp:ListItem Value="0" Text="其他"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="FieldHeader">库存:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbStock" runat="server" CssClass="TextBox" Width="180px" Text="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">材料:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbMaterial" runat="server" CssClass="TextBox" Width="180px"></asp:TextBox>
                        </td>
                        <td class="FieldHeader">颜色:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbColour" runat="server" CssClass="TextBox" Width="180px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">尺寸:
                        </td>
                        <td class="FieldColumn">
                            <asp:DropDownList ID="ddlSize" runat="server">
                                <asp:ListItem Text="--请选择商品尺寸--" Value="" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="儿童尺寸" Value="Childrens"></asp:ListItem>
                                <asp:ListItem Text="成人尺寸" Value="Adults"></asp:ListItem>
                                <asp:ListItem Text="婴儿尺寸" Value="Infants"></asp:ListItem>
                                <asp:ListItem Text="小件尺寸" Value="MiniKit"></asp:ListItem>
                                <asp:ListItem Text="女性尺寸" Value="Ladies"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="FieldHeader">货币:
                        </td>
                        <td class="FieldColumn">
                            <asp:DropDownList ID="ddlCurrency" runat="server">
                                <asp:ListItem Text="英磅" Value="GBP" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="人民币" Value="CNY"></asp:ListItem>
                                <asp:ListItem Text="美元" Value="USD"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">价格:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbPrice" runat="server" CssClass="TextBox" Text="0"></asp:TextBox>
                        </td>
                        <td class="FieldHeader">促销:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbSale" runat="server" CssClass="TextBox"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">图片:
                        </td>
                        <td class="FieldColspan" colspan="3">
                            <asp:TextBox ID="tbImageURL" runat="server" CssClass="TextBox" Width="400px" Text="UploadFiles/Product/"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">描述:
                        </td>
                        <td class="FieldColspan" colspan="3">
                            <asp:TextBox ID="tbDescription" runat="server" CssClass="TextBox" Width="400px" TextMode="MultiLine"
                                Rows="4"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">备注:
                        </td>
                        <td class="FieldColspan" colspan="3">
                            <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBox" Width="400px" TextMode="MultiLine"
                                Rows="4"></asp:TextBox>
                        </td>
                    </tr>
                </tbody>
            </table>
            <asp:GridView ID="gvProductOrder" runat="server" DataKeyNames="ID" OnPageIndexChanging="gvProductOrder_PageIndexChanging"
                OnSelectedIndexChanged="gvProductOrder_SelectedIndexChanged" OnRowDataBound="gvProductOrder_RowDataBound" PageSize="5">
                <Columns>
                    <asp:HyperLinkField HeaderText="编号" DataTextField="ID" DataNavigateUrlFields="ID"
                        DataNavigateUrlFormatString="ServerOrderView.ashx?OrderID={0}" Target="_blank" />
                    <asp:HyperLinkField HeaderText="会员姓名" DataTextField="MemberName" DataTextFormatString="<em>{0}</em>"
                        DataNavigateUrlFields="MemberID" DataNavigateUrlFormatString="AdminOrder.aspx?MemberID={0}" />
                    <asp:BoundField HeaderText="手机" DataField="Mobile" />
                    <asp:BoundField HeaderText="创建时间" DataField="CreateTime" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" />
                    <asp:BoundField HeaderText="价格" DataField="Price" DataFormatString="<em>{0:f2}</em>"
                        HtmlEncode="false" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField HeaderText="优惠" DataField="Sale" NullDisplayText="/" DataFormatString="<em>{0:f2}</em>"
                        HtmlEncode="false" ItemStyle-HorizontalAlign="Right" />
                    <asp:TemplateField HeaderText="状态">
                        <ItemTemplate>
                            <asp:Label ID="lblOrderStatus" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="有效" DataField="IsActive" />
                    <asp:CommandField ShowSelectButton="true" HeaderText="操作" EditText="修改" SelectText="详细"
                        UpdateText="保存" CancelText="取消" DeleteText="删除" ControlStyle-CssClass="LinkBtn" />
                </Columns>
            </asp:GridView>
            <div class="Clear">
                <uc3:CustomPagerInfo ID="ctrlCustomPagerInfo" runat="server" />
            </div>
            <div class="FooterBtnBar">
                <asp:Button ID="btnSubmit" runat="server" CssClass="InputBtn SubmitBtn" Text="保存商品"
                    OnClick="btnSubmit_Click" OnClientClick="return confirm('保存该商品信息')" />
                <asp:Button ID="btnCancel" runat="server" CssClass="InputBtn" Text="返回" OnClick="btnCancel_Click" />
                <asp:Button ID="btnDelete" runat="server" CssClass="InputBtn" Text="删除商品" OnClick="btnDelete_Click"
                    OnClientClick="return confirm('删除该商品信息?(无法恢复)')" />
            </div>
        </div>
    </div>
</asp:Content>
