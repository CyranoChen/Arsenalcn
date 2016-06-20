<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="AdminMembership.aspx.cs" Inherits="Arsenal.Web.AdminMembership" Title="后台管理 会员管理" Theme="Arsenalcn" %>

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
                <asp:TextBox ID="tbUserName" runat="server" Text="--用户名--" CssClass="TextBox" Width="100px"></asp:TextBox>
                <asp:TextBox ID="tbMobile" runat="server" Text="--手机--" CssClass="TextBox" Width="100px"></asp:TextBox>
                <asp:TextBox ID="tbEmail" runat="server" Text="--邮箱--" CssClass="TextBox" Width="100px"></asp:TextBox>
                <asp:LinkButton ID="btnFilter" runat="server" Text="搜索会员" CssClass="LinkBtn" OnClick="btnFilter_Click"></asp:LinkButton>
            </div>
            <div class="Clear">
                <uc3:CustomPagerInfo ID="ctrlCustomPagerInfo" runat="server" />
            </div>
        </div>
        <asp:GridView ID="gvMembership" runat="server" DataKeyNames="ID" OnPageIndexChanging="gvMembership_PageIndexChanging"
            PageSize="20" OnSelectedIndexChanged="gvMembership_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="ID" Visible="false" />
                <asp:BoundField HeaderText="用户名" DataField="UserName" />
                <asp:BoundField HeaderText="手机" DataField="Mobile" />
                <asp:BoundField HeaderText="邮箱" DataField="Email" />
                <asp:BoundField HeaderText="注册时间" DataField="CreateDate" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                <asp:BoundField HeaderText="上次登录" DataField="LastLoginDate" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                <asp:BoundField HeaderText="备注" DataField="Remark" />
                <asp:CommandField ShowEditButton="false" ShowSelectButton="true" ShowDeleteButton="false"
                    HeaderText="操作" EditText="修改" UpdateText="保存" CancelText="取消" SelectText="详细"
                    DeleteText="删除" ControlStyle-CssClass="LinkBtn" ItemStyle-CssClass="BtnColumn" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
