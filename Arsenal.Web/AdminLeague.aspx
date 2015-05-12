<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true" CodeBehind="AdminLeague.aspx.cs"
    Inherits="Arsenal.Web.AdminLeague" Title="后台管理 分类管理" EnableViewState="true" Theme="Arsenalcn" %>

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
                <asp:TextBox ID="tbLeagueName" runat="server" Text="--分类名称--" CssClass="TextBox"
                    Width="100px"></asp:TextBox>
                <asp:TextBox ID="tbLeagueSeason" runat="server" Text="--所属赛季--" CssClass="TextBox"
                    Width="100px"></asp:TextBox>
                <asp:DropDownList ID="ddlIsActive" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlIsActive_SelectedIndexChanged">
                    <asp:ListItem Value="" Text="--状态--"></asp:ListItem>
                    <asp:ListItem Value="true" Text="有效"></asp:ListItem>
                    <asp:ListItem Value="false" Text="无效"></asp:ListItem>
                </asp:DropDownList>
                <asp:LinkButton ID="btnFilter" runat="server" Text="搜索分类" CssClass="LinkBtn" OnClick="btnFilter_Click"></asp:LinkButton>
            </div>
            <div class="DivFloatRight">
                <a href="AdminLeagueView.aspx" class="LinkBtn">添加新分类</a>
                <asp:LinkButton ID="btnRefreshCache" runat="server" Text="更新缓存" CssClass="LinkBtn"
                    OnClick="btnRefreshCache_Click" />
            </div>
            <div class="Clear">
                <uc3:CustomPagerInfo ID="ctrlCustomPagerInfo" runat="server" />
            </div>
        </div>
        <asp:GridView ID="gvLeague" runat="server" DataKeyNames="ID" OnPageIndexChanging="gvLeague_PageIndexChanging"
            PageSize="20" OnSelectedIndexChanged="gvLeague_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="ID" Visible="false" />
                <asp:BoundField DataField="LeagueName" HeaderText="分类名称" DataFormatString="<em>{0}</em>"
                    HtmlEncode="false" ControlStyle-CssClass="TextBox" ControlStyle-Width="120px" />
                <asp:BoundField DataField="LeagueOrgName" HeaderText="英文名" ControlStyle-CssClass="TextBox"
                    ControlStyle-Width="120px" />
                <asp:BoundField DataField="LeagueSeason" HeaderText="赛季" DataFormatString="<em>{0}</em>"
                    HtmlEncode="false" ControlStyle-CssClass="TextBox" ControlStyle-Width="40px" />
                <asp:BoundField DataField="LeagueTime" HeaderText="开始时间" DataFormatString="{0:yyyy-MM-dd}"
                    ControlStyle-CssClass="TextBox" ControlStyle-Width="80px" />
                <asp:BoundField DataField="LeagueLogo" HeaderText="标志" ControlStyle-CssClass="TextBox"
                    ControlStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="LeagueOrder" HeaderText="排序" ControlStyle-CssClass="TextBox"
                    ControlStyle-Width="40px" />
                <asp:TemplateField HeaderText="球队数">
                    <ItemTemplate>
                        <asp:Label ID="lblTeamCountInfo" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="IsActive" HeaderText="有效" ControlStyle-CssClass="TextBox"
                    ControlStyle-Width="40px" />
                <asp:CommandField ShowEditButton="false" ShowSelectButton="true" ShowDeleteButton="false"
                    HeaderText="操作" EditText="修改" UpdateText="保存" CancelText="取消" SelectText="详细"
                    DeleteText="删除" ControlStyle-CssClass="LinkBtn" ItemStyle-CssClass="BtnColumn" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
