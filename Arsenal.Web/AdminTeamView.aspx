<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="AdminTeamView.aspx.cs" Inherits="Arsenal.Web.AdminTeamView" Title="后台管理 添加/更新球队" Theme="Arsenalcn" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<asp:Content ID="cphhead" ContentPlaceHolderID="cphhead" runat="server">
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:AdminPanel ID="pnlAdmin" runat="server" />
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server" />
        <div class="Arsenal_MainInfo">
            <table class="DataView" cellspacing="0" >
                <thead>
                    <tr class="Header">
                        <th colspan="2">
                            添加球队，标志请事先上传，并复制URL。
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="Row">
                        <td class="FieldHeader" style="width: 30%">
                            球队GUID:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbTeamGuid" runat="server" CssClass="TextBoxRead" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">
                            球队英文名：
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbTeamEnglishName" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">
                            球队显示名：
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbTeamDisplayName" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">
                            球队标志：
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbTeamLogo" runat="server" CssClass="TextBox" Width="300px" Text="UploadFiles/Team/"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">
                            球队昵称：
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbTeamNickName" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">
                            主场：
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbGround" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">
                            创建年份：
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbTeamFounded" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">
                            容量：
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbCapacity" runat="server" CssClass="TextBox" Width="300px" Text="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">
                            主席：
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbChairMan" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">
                            主教练：
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbManager" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">
                            所属分类(添加)：
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlTeamLeague" runat="server" Width="300px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="FooterBtnBar">
                <asp:Button ID="btnSubmit" runat="server" CssClass="InputBtn Submit" Text="保存球队"
                    OnClick="btnSubmit_Click" OnClientClick="return confirm('保存该球队信息')" />
                <asp:Button ID="btnCancel" runat="server" CssClass="InputBtn" Text="返回" OnClick="btnCancel_Click" />
                <asp:Button ID="btnDelete" runat="server" CssClass="InputBtn" Text="删除球队" OnClick="btnDelete_Click"
                    OnClientClick="return confirm('删除该球队信息?(无法恢复，同时移除关联比赛分类)')" />
            </div>
        </div>
    </div>
</asp:Content>
