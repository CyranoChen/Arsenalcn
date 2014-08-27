<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="AdminTeam.aspx.cs" Inherits="Arsenal.Web.AdminTeam" Title="后台管理 球队管理" Theme="Arsenalcn" %>

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
                <asp:DropDownList ID="ddlLeague" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLeague_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:TextBox ID="tbTeamName" runat="server" Text="--球队名称--" CssClass="TextBox" Width="100px"></asp:TextBox>
                <asp:LinkButton ID="btnFilter" runat="server" Text="搜索球队" CssClass="LinkBtn" OnClick="btnFilter_Click"></asp:LinkButton>
            </div>
            <div class="DivFloatRight">
                <a href="AdminTeamView.aspx" class="LinkBtn">添加新球队</a>
                <asp:LinkButton ID="btnRefreshCache" runat="server" Text="更新缓存" CssClass="LinkBtn"
                    OnClick="btnRefreshCache_Click" />
            </div>
            <div class="Clear">
                <uc3:CustomPagerInfo ID="ctrlCustomPagerInfo" runat="server" />
            </div>
        </div>
        <asp:GridView ID="gvTeam" runat="server" DataKeyNames="TeamGuid" OnPageIndexChanging="gvTeam_PageIndexChanging"
            PageSize="50" OnSelectedIndexChanged="gvTeam_SelectedIndexChanged" OnRowDeleting="gvTeam_RowDeleting">
            <Columns>
                <asp:BoundField DataField="TeamGuid" Visible="false" />
                <asp:BoundField HeaderText="球队英文名" DataField="TeamEnglishName" />
                <asp:BoundField HeaderText="球队显示名" DataField="TeamDisplayName" DataFormatString="<em>{0}</em>"
                    HtmlEncode="false" />
                <asp:BoundField HeaderText="主场" DataField="Ground" />
                <asp:BoundField HeaderText="分类数" DataField="LeagueCountInfo" />
                <asp:CommandField ShowEditButton="false" ShowSelectButton="true" ShowDeleteButton="true"
                    HeaderText="操作" EditText="修改" UpdateText="保存" CancelText="取消" SelectText="详细"
                    DeleteText="移除" ControlStyle-CssClass="LinkBtn" ItemStyle-CssClass="BtnColumn" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
