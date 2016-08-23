<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="AdminGroupView.aspx.cs" Inherits="Arsenal.Web.AdminGroupView" Title="后台管理 添加/更新分组" Theme="Arsenalcn" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:AdminPanel ID="pnlAdmin" runat="server" />
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server" />
        <div class="Arsenal_MainInfo">
            <table class="DataView">
                <thead>
                    <tr class="Header">
                        <th colspan="2">添加/更新球队分组
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="Row">
                        <td class="FieldHeader" style="width: 30%">分类GUID:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbGroupGuid" runat="server" CssClass="TextBoxRead" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">分组名称：
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbGroupName" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">分组排序：
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbGroupOrder" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">排名方式：
                        </td>
                        <td class="FieldColumn">
                            <asp:RadioButtonList ID="rblRankMethod" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <asp:ListItem Text="对阵结果" Value="0"></asp:ListItem>
                                <asp:ListItem Text="净胜球" Value="1" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">性质：
                        </td>
                        <td class="FieldColumn">
                            <asp:CheckBox ID="cbIsTable" runat="server" Text="是否为积分榜" />
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">所属分类及球队：
                        </td>
                        <td class="FieldColumn">
                            <asp:DropDownList ID="ddlGroupLeague" runat="server" Width="300px" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlGroupLeague_SelectedIndexChanged">
                            </asp:DropDownList>
                            <br />
                            <asp:ListBox ID="lbLeagueTeam" Width="300px" Rows="20" runat="server" SelectionMode="Multiple"></asp:ListBox>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="FooterBtnBar">
                <asp:Button ID="btnSubmit" runat="server" CssClass="InputBtn Submit" Text="保存分组"
                    OnClick="btnSubmit_Click" OnClientClick="return confirm('保存该分组信息')" />
                <asp:Button ID="btnCancel" runat="server" CssClass="InputBtn" Text="返回" OnClick="btnCancel_Click" />
                <asp:Button ID="btnDelete" runat="server" CssClass="InputBtn" Text="删除分组" OnClick="btnDelete_Click"
                    OnClientClick="return confirm('删除该分组信息?(无法恢复)')" />
            </div>
        </div>
    </div>
</asp:Content>
