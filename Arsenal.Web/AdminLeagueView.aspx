<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="AdminLeagueView.aspx.cs" Inherits="Arsenal.Web.AdminLeagueView" Title="后台管理 添加/更新分类" Theme="Arsenalcn" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:AdminPanel ID="pnlAdmin" runat="server" />
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server" />
        <div class="CasinoSys_MainInfo">
            <table class="DataView">
                <thead>
                    <tr class="Header">
                        <th colspan="2">添加比赛分类，标志请事先上传，并复制URL。
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="Row">
                        <td class="FieldHeader" style="width: 30%">分类GUID:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbLeagueGuid" runat="server" CssClass="TextBoxRead" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">分类名称：
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbLeagueName" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">分类英文名：
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbLeagueOrgName" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">赛季：
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbLeagueSeason" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">开始时间：
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbLeagueTime" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">标志：
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbLeagueLogo" runat="server" CssClass="TextBox" Text="UploadFiles/League/"
                                Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">排序：
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbLeagueOrder" runat="server" CssClass="TextBox" Width="50px" Text="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">状态：
                        </td>
                        <td class="FieldColumn">
                            <asp:CheckBox ID="cbIsActive" runat="server" Checked="true" Text="是否有效" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="FooterBtnBar">
                <asp:Button ID="btnSubmit" runat="server" CssClass="InputBtn Submit" Text="保存分类"
                    OnClick="btnSubmit_Click" OnClientClick="return confirm('保存该分类信息')" />
                <asp:Button ID="btnCancel" runat="server" CssClass="InputBtn" Text="返回" OnClick="btnCancel_Click" />
                <asp:Button ID="btnDelete" runat="server" CssClass="InputBtn" Text="删除分类" OnClick="btnDelete_Click"
                    OnClientClick="return confirm('删除该分类信息?(无法恢复)')" />
            </div>
        </div>
    </div>
</asp:Content>
