<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="AdminGroup.aspx.cs" Inherits="Arsenal.Web.AdminGroup" Title="后台管理 分组管理" EnableViewState="true" Theme="Arsenalcn" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
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
                <asp:DropDownList ID="ddlLeague" runat="server" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlLeague_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:TextBox ID="tbGroupName" runat="server" Text="--分组名称--" CssClass="TextBox" Width="100px" />
                <asp:LinkButton ID="btnFilter" runat="server" Text="搜索分组" CssClass="LinkBtn" OnClick="btnFilter_Click"></asp:LinkButton>
            </div>
            <div class="DivFloatRight">
                <a href="AdminGroupView.aspx" class="LinkBtn">添加新分组</a>
            </div>
            <div class="Clear">
            </div>
        </div>
        <asp:GridView ID="gvGroup" runat="server" OnRowDataBound="gvGroup_RowDataBound" DataKeyNames="ID"
            PageSize="20" OnSelectedIndexChanged="gvGroup_SelectedIndexChanged" OnRowCommand="gvGroup_RowCommand">
            <Columns>
                <asp:BoundField DataField="ID" Visible="false" />
<%--                <asp:BoundField HeaderText="分类名称" DataField="LeagueName" />
                <asp:BoundField HeaderText="赛季" DataField="LeagueSeason" />--%>
                <asp:BoundField HeaderText="名称" DataField="GroupName" DataFormatString="<em>{0}</em>"
                    HtmlEncode="false" />
                <asp:TemplateField HeaderText="球队">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlGroupTeamInfo" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="比赛">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlGroupMatchInfo" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="排序" DataField="GroupOrder" />
                <asp:BoundField HeaderText="是否为积分榜" DataField="IsTable" />
                <asp:BoundField HeaderText="排序方式" DataField="RankMethod" />
                <asp:CommandField HeaderText="操作" ShowSelectButton="true" SelectText="详细" ShowDeleteButton="false"
                    DeleteText="删除" ControlStyle-CssClass="LinkBtn" />
                <asp:TemplateField HeaderText="功能">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnResetGroupTable" runat="server" Text="统计积分榜" CssClass="LinkBtn"
                            CommandName="ResetGroupTable" OnClientClick="return confirm('确认操作?')">
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnResetGroupMatch" runat="server" Text="绑定比赛" CssClass="LinkBtn"
                            CommandName="ResetGroupMatch" ToolTip="请确保该分类未有淘汰赛比赛" OnClientClick="return confirm('确认操作?')">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
