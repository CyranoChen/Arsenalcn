<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="AdminVideoView.aspx.cs" Inherits="Arsenal.Web.AdminVideoView" Title="后台管理 添加/更新视频" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphhead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:AdminPanel ID="pnlAdmin" runat="server" />
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server" />
        <div class="Arsenal_MainInfo">
            <table class="DataView" cellspacing="0" cellpadding="5">
                <thead>
                    <tr class="Header">
                        <th colspan="4">添加/更新视频集锦，添加后请将视频COPY到服务器上。
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="Row">
                        <td class="FieldHeader">视频文件GUID:
                        </td>
                        <td style="text-align: left;" colspan="3">
                            <asp:TextBox ID="tbVideoGuid" runat="server" CssClass="TextBoxRead" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">路径(留空为默认):
                        </td>
                        <td style="text-align: left;" colspan="3">
                            <asp:TextBox ID="tbFileName" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">分类:
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList ID="ddlLeague" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLeague_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="FieldHeader">比赛:
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList ID="ddlMatch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMatch_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">年份:
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="tbGoalYear" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                        <td class="FieldHeader">对阵:
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="tbOpponent" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">进球者:
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList ID="ddlGoalPlayer" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td class="FieldHeader">助攻者:
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList ID="ddlAssistPlayer" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">GRank:
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="tbGoalRank" runat="server" CssClass="TextBox" MaxLength="1" Width="30px"
                                Text="0"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="rfvGRank" ControlToValidate="tbGoalRank"
                                ErrorMessage="*" CssClass="ValiSpan" Display="Dynamic" />
                        </td>
                        <td class="FieldHeader">TRank:
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="tbTeamworkRank" runat="server" CssClass="TextBox" MaxLength="1" Width="30px"
                                Text="0"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="rfvTRank" ControlToValidate="tbTeamworkRank"
                                ErrorMessage="*" CssClass="ValiSpan" Display="Dynamic" />
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">格式:
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList ID="ddlVideoType" runat="server">
                                <asp:ListItem Value="mp4" Text="video/mp4" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="flv" Text="video/flv"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="rfvExt" ControlToValidate="ddlExt"
                                ErrorMessage="*" CssClass="ValiSpan" Display="Dynamic" />
                        </td>
                        <td class="FieldHeader">长度:
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="tbVideoLength" runat="server" CssClass="TextBox" Text="60" Width="30px"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="rfvVideoLength" ControlToValidate="tbVideoLength"
                                ErrorMessage="*" CssClass="ValiSpan" Display="Dynamic" />
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">宽度:
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="tbVideoWidth" runat="server" CssClass="TextBox" Text="640" Width="30px"></asp:TextBox>
                        </td>
                        <td class="FieldHeader">高度:
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="tbVideoHeight" runat="server" CssClass="TextBox" Text="360" Width="30px"></asp:TextBox>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="FooterBtnBar">
                <asp:Button ID="btnSubmit" runat="server" CssClass="InputBtn Submit" Text="保存视频"
                    OnClick="btnSubmit_Click" OnClientClick="return confirm('保存该视频信息')" />
                <asp:Button ID="btnCancel" runat="server" CssClass="InputBtn" Text="返回" OnClick="btnCancel_Click" />
                <asp:Button ID="btnDelete" runat="server" CssClass="InputBtn" Text="删除视频" OnClick="btnDelete_Click"
                    OnClientClick="return confirm('删除该视频信息?(无法恢复)')" />
            </div>
        </div>
    </div>
</asp:Content>
