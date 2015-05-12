<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="AdminMemberPeriod.aspx.cs" Inherits="iArsenal.Web.AdminMemberPeriod" Title="后台管理 会籍管理" Theme="Arsenalcn" %>

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
                <asp:TextBox ID="tbMemberID" runat="server" Text="--会员编号--" CssClass="TextBox" Width="100px"></asp:TextBox>
                <asp:TextBox ID="tbMemberName" runat="server" Text="--会员姓名--" CssClass="TextBox" Width="100px"></asp:TextBox>
                <asp:TextBox ID="tbMemberCardNo" runat="server" Text="--会员卡号--" CssClass="TextBox" Width="100px"></asp:TextBox>
                <asp:DropDownList ID="ddlMemberClass" runat="server">
                    <asp:ListItem Text="--全部--" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="普通会员" Value="1"></asp:ListItem>
                    <asp:ListItem Text="高级会员" Value="2"></asp:ListItem>
                </asp:DropDownList>
                <asp:LinkButton ID="btnFilter" runat="server" Text="搜索会籍" CssClass="LinkBtn" OnClick="btnFilter_Click"></asp:LinkButton>
            </div>
            <div class="DivFloatRight">
                <a href="AdminMemberPeriodView.aspx" class="LinkBtn">添加新会籍</a>
            </div>
            <div class="Clear">
                <uc3:CustomPagerInfo ID="ctrlCustomPagerInfo" runat="server" />
            </div>
        </div>
        <asp:GridView ID="gvMemberPeriod" runat="server" DataKeyNames="MemberPeriodID" OnPageIndexChanging="gvMemberPeriod_PageIndexChanging"
            PageSize="10" OnSelectedIndexChanged="gvMemberPeriod_SelectedIndexChanged" OnRowDataBound="gvMemberPeriod_RowDataBound">
            <Columns>
                <asp:BoundField HeaderText="标识" DataField="MemberPeriodID" />
                <asp:BoundField HeaderText="会员编号" DataField="MemberID" DataFormatString="<em>{0}</em>"
                    HtmlEncode="false" />
                <asp:TemplateField HeaderText="会员姓名">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlName" runat="server"></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="卡号" DataField="MemberCardNo" />
                <asp:BoundField HeaderText="等级" DataField="MemberClass" />
                <asp:BoundField HeaderText="开始时间" DataField="StartDate" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField HeaderText="结束时间" DataField="EndDate" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:HyperLinkField HeaderText="订单编号" DataTextField="OrderID"
                    DataNavigateUrlFields="OrderID" DataNavigateUrlFormatString="AdminOrderView.aspx?OrderID={0}" />
                <asp:BoundField HeaderText="状态" DataField="IsActive" />
                <asp:CommandField ShowSelectButton="true" HeaderText="操作" EditText="修改" SelectText="详细"
                    UpdateText="保存" CancelText="取消" DeleteText="删除" ControlStyle-CssClass="LinkBtn" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
