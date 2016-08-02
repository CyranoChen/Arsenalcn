<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="AdminShowcase.aspx.cs" Inherits="iArsenal.Web.AdminShowcase" Title="后台管理 橱窗管理" Theme="Arsenalcn" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<%@ Register Src="Control/CustomPagerInfo.ascx" TagName="CustomPagerInfo" TagPrefix="uc3" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
    <script type="text/javascript">
        $(function () {
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
                <asp:DropDownList ID="ddlIsActive" runat="server">
                    <asp:ListItem Value="" Text="--有效--" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="true" Text="是"></asp:ListItem>
                    <asp:ListItem Value="false" Text="否"></asp:ListItem>
                </asp:DropDownList>
                <asp:LinkButton ID="btnFilter" runat="server" Text="搜索橱窗" CssClass="LinkBtn" OnClick="btnFilter_Click"></asp:LinkButton>
            </div>
            <div class="DivFloatRight">
                <asp:LinkButton ID="btnRefreshCache" runat="server" Text="更新缓存" CssClass="LinkBtn" OnClick="btnRefreshCache_Click" />
            </div>
            <div class="Clear">
                <uc3:CustomPagerInfo ID="ctrlCustomPagerInfo" runat="server" />
            </div>
        </div>
        <asp:GridView ID="gvShowcase" runat="server" DataKeyNames="ID" OnPageIndexChanging="gvShowcase_PageIndexChanging"
            PageSize="20" OnRowDataBound="gvShowcase_RowDataBound" OnRowCancelingEdit="gvShowcase_RowCancelingEdit"
            OnRowEditing="gvShowcase_RowEditing" OnRowUpdating="gvShowcase_RowUpdating" OnRowDeleting="gvShowcase_RowDeleting">
            <Columns>
                <asp:BoundField DataField="ID" Visible="false" />
                <asp:BoundField DataField="OrderNum" HeaderText="序号" ControlStyle-CssClass="TextBox" ControlStyle-Width="30px" />
                <asp:HyperLinkField DataTextField="ProductCode" HeaderText="商品编码" DataTextFormatString="<em>{0}</em>"
                    DataNavigateUrlFields="ProductGuid" DataNavigateUrlFormatString="AdminProductView.aspx?ProductGuid={0}" Target="_blank" />
                <asp:TemplateField HeaderText="商品描述">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlProductName" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="商品价格">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlProductPrice" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="IsActive" HeaderText="状态" ControlStyle-CssClass="TextBox" ControlStyle-Width="30px" />
                <asp:CommandField ShowEditButton="true" ShowDeleteButton="True" HeaderText="操作" EditText="修改" SelectText="详细"
                    UpdateText="保存" CancelText="取消" DeleteText="删除" ControlStyle-CssClass="LinkBtn" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
