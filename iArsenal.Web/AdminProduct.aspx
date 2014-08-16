<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="AdminProduct.aspx.cs" Inherits="iArsenal.Web.AdminProduct" Title="后台管理 商品管理" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<%@ Register Src="Control/CustomPagerInfo.ascx" TagName="CustomPagerInfo" TagPrefix="uc3" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var $tbInfo = $(".DivFloatLeft > .TextBox");
            $tbInfo.each(function () {
                $(this).focus(function () {
                    $(this).val("");
                });
            });
        });
    </script>
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:AdminPanel ID="pnlAdmin" runat="server" />
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server" />
        <div class="FunctionBar">
            <div class="DivFloatLeft">
                <asp:TextBox ID="tbCode" runat="server" Text="--编码--" CssClass="TextBox" Width="100px"></asp:TextBox>
                <asp:TextBox ID="tbName" runat="server" Text="--名称--" CssClass="TextBox" Width="100px"></asp:TextBox>
                <asp:TextBox ID="tbDisplayName" runat="server" Text="--译名--" CssClass="TextBox" Width="100px"></asp:TextBox>
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
                <asp:DropDownList ID="ddlIsActive" runat="server">
                    <asp:ListItem Value="" Text="--有效--" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="true" Text="是"></asp:ListItem>
                    <asp:ListItem Value="false" Text="否"></asp:ListItem>
                </asp:DropDownList>
                <asp:LinkButton ID="btnFilter" runat="server" Text="搜索商品" CssClass="LinkBtn" OnClick="btnFilter_Click"></asp:LinkButton>
            </div>
            <div class="DivFloatRight">
                <a href="AdminProductView.aspx" class="LinkBtn">添加新商品</a>
                <asp:LinkButton ID="btnRefreshCache" runat="server" Text="更新缓存" CssClass="LinkBtn"
                    OnClick="btnRefreshCache_Click" />
            </div>
            <div class="Clear">
                <uc3:CustomPagerInfo ID="ctrlCustomPagerInfo" runat="server" />
            </div>
        </div>
        <asp:GridView ID="gvProduct" runat="server" DataKeyNames="ProductGuid" OnPageIndexChanging="gvProduct_PageIndexChanging"
            PageSize="10" OnSelectedIndexChanged="gvProduct_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="ProductGuid" Visible="false" />
                <asp:BoundField DataField="Code" HeaderText="编码" DataFormatString="<em>{0}</em>"
                    HtmlEncode="false" />
                <asp:BoundField DataField="Name" HeaderText="名称" />
                <asp:BoundField DataField="DisplayName" HeaderText="译名" />
                <asp:BoundField DataField="PriceInfo" HeaderText="价格" DataFormatString="<em>{0}</em>"
                    HtmlEncode="false" />
                <asp:BoundField DataField="SaleInfo" HeaderText="促销" DataFormatString="<em>{0}</em>"
                    HtmlEncode="false" NullDisplayText="/" />
                <asp:BoundField DataField="ProductTypeInfo" HeaderText="类型" />
                <asp:BoundField DataField="IsActive" HeaderText="状态" />
                <asp:CommandField ShowSelectButton="true" HeaderText="操作" EditText="修改" SelectText="详细"
                    UpdateText="保存" CancelText="取消" DeleteText="删除" ControlStyle-CssClass="LinkBtn" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
