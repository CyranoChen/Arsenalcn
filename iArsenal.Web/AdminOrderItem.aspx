<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="AdminOrderItem.aspx.cs" Inherits="iArsenal.Web.AdminOrderItem" Title="后台管理 许愿管理" %>

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
                <asp:TextBox ID="tbOrderItemID" runat="server" Text="--许愿编号--" CssClass="TextBox"
                    Width="100px"></asp:TextBox>
                <asp:TextBox ID="tbMemberName" runat="server" Text="--会员姓名--" CssClass="TextBox"
                    Width="100px"></asp:TextBox>
                <asp:TextBox ID="tbCode" runat="server" Text="--编码--" CssClass="TextBox" Width="100px"></asp:TextBox>
                <asp:LinkButton ID="btnFilter" runat="server" Text="搜索许愿" CssClass="LinkBtn" OnClick="btnFilter_Click"></asp:LinkButton>
            </div>
            <div class="DivFloatRight">
                <a href="AdminOrderItemView.aspx" class="LinkBtn">添加新许愿</a>
            </div>
            <div class="Clear">
                <uc3:CustomPagerInfo ID="ctrlCustomPagerInfo" runat="server" />
            </div>
        </div>
        <asp:GridView ID="gvOrderItem" runat="server" DataKeyNames="OrderItemID" OnPageIndexChanging="gvOrderItem_PageIndexChanging"
            PageSize="10" OnSelectedIndexChanged="gvOrderItem_SelectedIndexChanged">
            <Columns>
                <asp:BoundField HeaderText="编号" DataField="OrderItemID" />
                <asp:BoundField HeaderText="会员姓名" DataField="MemberName" DataFormatString="<em>{0}</em>"
                    HtmlEncode="false" />
                <asp:BoundField HeaderText="创建时间" DataField="CreateTime" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" />
                <asp:BoundField HeaderText="编码" DataField="Code" DataFormatString="<em>{0}</em>"
                    HtmlEncode="false" />
                <asp:BoundField HeaderText="名称" DataField="ProductName" />
                <asp:BoundField HeaderText="尺寸" DataField="Size" NullDisplayText="/" />
                <asp:BoundField HeaderText="单价" DataField="UnitPrice" DataFormatString="{0:f2}" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField HeaderText="数量" DataField="Quantity" />
                <asp:BoundField HeaderText="状态" DataField="IsActive" />
                <asp:CommandField ShowSelectButton="true" HeaderText="操作" EditText="修改" SelectText="详细"
                    UpdateText="保存" CancelText="取消" DeleteText="删除" ControlStyle-CssClass="LinkBtn" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
