<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="AdminLogSignIn.aspx.cs" Inherits="Arsenal.Web.AdminLogSignIn" Title="后台管理 签到管理" Theme="Arsenalcn" %>

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

            $(".DataView td.BtnColumn a.LinkBtn:contains('删除')").click(function () { return confirm('确认删除当前签到记录?') });
        });
    </script>
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:AdminPanel ID="pnlAdmin" runat="server" />
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server" />
        <div class="FunctionBar">
            <div class="DivFloatLeft">
                <asp:TextBox ID="tbUserName" runat="server" Text="--会员用户名--" CssClass="TextBox" Width="200px">
                </asp:TextBox>
                <asp:LinkButton ID="btnFilter" runat="server" Text="搜索签到" CssClass="LinkBtn" OnClick="btnFilter_Click"></asp:LinkButton>
            </div>
            <div class="Clear">
                <uc3:CustomPagerInfo ID="ctrlCustomPagerInfo" runat="server" />
            </div>
        </div>
        <asp:GridView ID="gvLogSignIn" runat="server" DataKeyNames="ID" OnPageIndexChanging="gvLogSignIn_PageIndexChanging"
            PageSize="20" OnRowDeleting="gvLogSignIn_RowDeleting">
            <Columns>
                <asp:BoundField HeaderText="标识" DataField="ID" />
                <asp:BoundField HeaderText="用户名" DataField="UserName" />
                <asp:BoundField HeaderText="签到时间" DataField="SignInTime" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" />
                <asp:BoundField HeaderText="奖励" DataField="Bonus" DataFormatString="<em>{0}</em>"
                    HtmlEncode="false" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField HeaderText="连续天数" DataField="SignInDays" />
                <asp:BoundField HeaderText="描述" DataField="Description" />
                <asp:CommandField ShowEditButton="false" ShowSelectButton="false" ShowDeleteButton="True"
                    HeaderText="操作" EditText="修改" UpdateText="保存" CancelText="取消" SelectText="详细"
                    DeleteText="删除" ControlStyle-CssClass="LinkBtn" ItemStyle-CssClass="BtnColumn" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
