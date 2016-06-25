<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="AdminMember.aspx.cs" Inherits="iArsenal.Web.AdminMember" Title="后台管理 会员管理" Theme="Arsenalcn" %>

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
                <asp:TextBox ID="tbName" runat="server" Text="--会员姓名--" CssClass="TextBox" Width="100px"></asp:TextBox>
                <asp:TextBox ID="tbAcnName" runat="server" Text="--ACN会员名--" CssClass="TextBox" Width="100px"></asp:TextBox>
                <asp:TextBox ID="tbRegion" runat="server" Text="--所在地区--" CssClass="TextBox" Width="100px"></asp:TextBox>
                <asp:TextBox ID="tbMobile" runat="server" Text="--手机--" CssClass="TextBox" Width="100px"></asp:TextBox>
                <asp:DropDownList ID="ddlMemberType" runat="server">
                    <asp:ListItem Text="--全部--" Value="" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="团购" Value="1"></asp:ListItem>
                    <asp:ListItem Text="观赛" Value="2"></asp:ListItem>
                    <asp:ListItem Text="活动" Value="3"></asp:ListItem>
                    <asp:ListItem Text="干事" Value="4"></asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="ddlEvalution" runat="server">
                    <asp:ListItem Text="--全部--" Value="" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="黑名单" Value="1"></asp:ListItem>
                    <asp:ListItem Text="白名单" Value="2"></asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="ddlOfficialSync" runat="server">
                    <asp:ListItem Text="--全部--" Value="" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="未同步" Value="0000"></asp:ListItem>
                    <asp:ListItem Text="1617" Value="1617"></asp:ListItem>
                </asp:DropDownList>
                <asp:LinkButton ID="btnFilter" runat="server" Text="搜索会员" CssClass="LinkBtn" OnClick="btnFilter_Click"></asp:LinkButton>
            </div>
            <div class="DivFloatRight">
                <a href="AdminMemberView.aspx" class="LinkBtn">添加新会员</a>
                <asp:LinkButton ID="btnRefreshCache" runat="server" Text="更新缓存" CssClass="LinkBtn"
                    OnClick="btnRefreshCache_Click" />
            </div>
            <div class="Clear">
                <uc3:CustomPagerInfo ID="ctrlCustomPagerInfo" runat="server" />
            </div>
        </div>
        <asp:GridView ID="gvMember" runat="server" DataKeyNames="ID" OnPageIndexChanging="gvMember_PageIndexChanging"
            PageSize="10" OnSelectedIndexChanged="gvMember_SelectedIndexChanged" OnRowDataBound="gvMember_RowDataBound">
            <Columns>
                <asp:BoundField HeaderText="标识" DataField="ID" />
                <asp:TemplateField HeaderText="会员姓名">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlName" runat="server"></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="会员类型" DataField="MemberTypeInfo" DataFormatString="<em>{0}</em>"
                    HtmlEncode="false" />
                <asp:BoundField HeaderText="ACN会员" DataField="AcnName" />
                <asp:BoundField HeaderText="所在地区" DataField="RegionInfo" />
                <asp:BoundField HeaderText="手机" DataField="Mobile" />
                <asp:BoundField HeaderText="加入时间" DataField="JoinDate" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField HeaderText="邮箱" DataField="Email" />
                <asp:BoundField HeaderText="官方同步" DataField="OfficialSync" />
                <asp:CommandField ShowSelectButton="true" HeaderText="操作" EditText="修改" SelectText="详细"
                    UpdateText="保存" CancelText="取消" DeleteText="删除" ControlStyle-CssClass="LinkBtn" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>