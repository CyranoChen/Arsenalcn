<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="AdminOrder.aspx.cs" Inherits="iArsenal.Web.AdminOrder" Title="后台管理 订单管理" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<%@ Register Src="Control/CustomPagerInfo.ascx" TagName="CustomPagerInfo" TagPrefix="uc3" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
    <link href="Scripts/jquery.ui/jquery-ui.min.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="Scripts/jquery-ui-1.10.4.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $.datepicker.setDefaults({ dateFormat: 'yy-mm-dd', changeMonth: true, changeYear: true });
            $(".OrderDate").datepicker();

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
                <asp:TextBox ID="tbOrderID" runat="server" Text="--订单编号--" CssClass="TextBox" Width="100px"></asp:TextBox>
                <asp:TextBox ID="tbMemberName" runat="server" Text="--会员姓名--" CssClass="TextBox"
                    Width="100px"></asp:TextBox>
                <asp:TextBox ID="tbMobile" runat="server" Text="--手机--" CssClass="TextBox" Width="100px"></asp:TextBox>
                <asp:TextBox ID="tbOrderDate" runat="server" Text="--下单时间--" CssClass="TextBox OrderDate" Width="100px"></asp:TextBox>
                <asp:DropDownList ID="ddlProductType" runat="server">
                    <asp:ListItem Value="" Text="--类型--" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="ReplicaKit" Text="球衣"></asp:ListItem>
                    <asp:ListItem Value="Wish" Text="团购"></asp:ListItem>
                    <asp:ListItem Value="Ticket" Text="球票"></asp:ListItem>
                    <asp:ListItem Value="Travel" Text="观赛"></asp:ListItem>
                    <asp:ListItem Value="MemberShip" Text="会费"></asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="ddlStatus" runat="server">
                    <asp:ListItem Value="" Text="--状态--" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="1" Text="未提交"></asp:ListItem>
                    <asp:ListItem Value="2" Text="审核中"></asp:ListItem>
                    <asp:ListItem Value="21" Text="已审核"></asp:ListItem>
                    <asp:ListItem Value="3" Text="已确认"></asp:ListItem>
                    <asp:ListItem Value="4" Text="已下单"></asp:ListItem>
                    <asp:ListItem Value="5" Text="已发货"></asp:ListItem>
                    <asp:ListItem Value="0" Text="未知"></asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="ddlIsActive" runat="server">
                    <asp:ListItem Value="" Text="--有效--" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="true" Text="是"></asp:ListItem>
                    <asp:ListItem Value="false" Text="否"></asp:ListItem>
                </asp:DropDownList>
                <asp:LinkButton ID="btnFilter" runat="server" Text="搜索订单" CssClass="LinkBtn" OnClick="btnFilter_Click"></asp:LinkButton>
            </div>
            <div class="DivFloatRight">
                <a href="AdminOrderView.aspx" class="LinkBtn">添加新订单</a>
                <asp:LinkButton ID="btnExport" runat="server" CssClass="LinkBtn" Text="导出订单" OnClick="btnExport_Click"></asp:LinkButton>
            </div>
            <div class="Clear">
                <uc3:CustomPagerInfo ID="ctrlCustomPagerInfo" runat="server" />
            </div>
        </div>
        <asp:GridView ID="gvOrder" runat="server" DataKeyNames="OrderID" OnPageIndexChanging="gvOrder_PageIndexChanging"
            PageSize="10" OnSelectedIndexChanged="gvOrder_SelectedIndexChanged" OnRowDataBound="gvOrder_RowDataBound">
            <Columns>
                <asp:HyperLinkField HeaderText="编号" DataTextField="OrderID" DataNavigateUrlFields="OrderID"
                    DataNavigateUrlFormatString="ServerOrderView.ashx?OrderID={0}" Target="_blank" />
                <asp:HyperLinkField HeaderText="会员姓名" DataTextField="MemberName" DataTextFormatString="<em>{0}</em>"
                    DataNavigateUrlFields="MemberID" DataNavigateUrlFormatString="AdminOrder.aspx?MemberID={0}" />
                <asp:BoundField HeaderText="手机" DataField="Mobile" />
                <asp:BoundField HeaderText="创建时间" DataField="CreateTime" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" />
                <asp:BoundField HeaderText="价格" DataField="Price" DataFormatString="<em>{0:f2}</em>"
                    HtmlEncode="false" ItemStyle-HorizontalAlign="Right" />
                <asp:TemplateField HeaderText="类型">
                    <ItemTemplate>
                        <asp:Label ID="lblProductType" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="优惠" DataField="Sale" NullDisplayText="/" DataFormatString="<em>{0:f2}</em>"
                    HtmlEncode="false" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField HeaderText="定金" DataField="Deposit" NullDisplayText="/" DataFormatString="{0:f2}"
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
    </div>
</asp:Content>
