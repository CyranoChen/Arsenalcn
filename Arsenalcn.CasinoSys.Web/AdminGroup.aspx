<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="AdminGroup.aspx.cs" Inherits="Arsenalcn.CasinoSys.Web.AdminGroup"
    Title="后台管理 分组管理" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">

    <script type="text/javascript">
        $(document).ready(function() {
            $(".DataView td.BtnColumn a.LinkBtn:contains('删除')").click(function () { return confirm('确认删除?') });
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:AdminPanel ID="pnlAdmin" runat="server" />
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server" />
        <div class="FunctionBar">
            <div class="DivFloatLeft">
                <asp:DropDownList ID="ddlLeague" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLeague_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
            <div class="DivFloatRight">
                <asp:LinkButton ID="btnTeamAdd" CssClass="LinkBtn" Text="添加新分组" runat="server" OnClick="btnTeamAdd_Click"></asp:LinkButton>
            </div>
            <div class="Clear">
            </div>
        </div>
        <asp:Panel ID="pnlGroupView" CssClass="CasinoSys_MainInfo" runat="server">
            <table class="DataView" cellspacing="0" cellpadding="5">
                <thead>
                    <tr class="Header">
                        <th colspan="2">
                            <asp:Label ID="lblGroupAdd" runat="server" Text="分组标识："></asp:Label>
                            <asp:Label ID="lblGroupGuid" runat="server"></asp:Label>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="Row">
                        <td class="FieldHeader">
                            分组名称：
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbGroupName" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">
                            分组排序：
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbGroupOrder" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">
                            性质：
                        </td>
                        <td align="left">
                            <asp:CheckBox ID="cbIsTable" runat="server" Text="是否为积分榜" />
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">
                            所属分类及球队：
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlGroupLeague" runat="server" Width="300px" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlGroupLeague_SelectedIndexChanged">
                            </asp:DropDownList>
                            <br />
                            <asp:ListBox ID="lbLeagueTeam" Width="300px" Rows="20" runat="server" SelectionMode="Multiple">
                            </asp:ListBox>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="FooterBtnBar">
                <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="InputBtn" OnClick="btnSave_Click" OnClientClick="return confirm('确认保存分组信息?')" />
                <asp:Button ID="btnCancel" runat="server" Text="取消" CssClass="InputBtn" OnClick="btnCancel_Click" />
            </div>
        </asp:Panel>
        <asp:GridView ID="gvGroup" runat="server" OnRowDataBound="gvGroup_RowDataBound" DataKeyNames="GroupGuid"
            PageSize="10" OnSelectedIndexChanged="gvGroup_SelectedIndexChanged" OnRowDeleting="gvGroup_RowDeleting"
            OnRowCommand="gvGroup_RowCommand" OnPageIndexChanging="gvGroup_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="GroupGuid" Visible="false" />
                <asp:BoundField HeaderText="分类名称" DataField="LeagueName" />
                <asp:BoundField HeaderText="赛季" DataField="LeagueSeason" />
                <asp:BoundField HeaderText="名称" DataField="GroupName" DataFormatString="<em>{0}</em>"
                    HtmlEncode="false" />
                <asp:TemplateField HeaderText="球队">
                    <ItemTemplate>
                        <em title="<%#DataBinder.Eval(Container.DataItem, "GroupTeamList") %>">
                            <%#DataBinder.Eval(Container.DataItem, "GroupTeamCount") %></em>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="比赛" DataField="GroupMatchCount" DataFormatString="<em>{0}</em>"
                    HtmlEncode="false" />
                <asp:BoundField HeaderText="排序" DataField="GroupOrder" />
                <asp:BoundField HeaderText="是否为积分榜" DataField="IsTable" />
                <asp:CommandField HeaderText="编辑" ShowSelectButton="true" SelectText="编辑" ShowDeleteButton="true"
                    DeleteText="删除" ControlStyle-CssClass="LinkBtn" ItemStyle-CssClass="BtnColumn" />
                <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnResetGroupTable" runat="server" Text="统计积分榜" CssClass="LinkBtn"
                            CommandName="ResetGroupTable" OnClientClick="return confirm('确认操作?')"></asp:LinkButton>
                        <asp:LinkButton ID="btnResetGroupMatch" runat="server" Text="绑定比赛" CssClass="LinkBtn"
                            CommandName="ResetGroupMatch" ToolTip="请确保该分类未有淘汰赛比赛" OnClientClick="return confirm('确认操作?')"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
