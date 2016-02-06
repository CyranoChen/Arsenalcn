<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
CodeBehind="AdminMatchView.aspx.cs" Inherits="Arsenal.Web.AdminMatchView" Title="后台管理 添加/更新阿森纳比赛" Theme="Arsenalcn" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<asp:Content ID="cphhead" ContentPlaceHolderID="cphhead" runat="server">
    <style type="text/css">
        #tdMatchResult input.TextBox { text-align: center; }
    </style>
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:AdminPanel ID="pnlAdmin" runat="server"/>
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server"/>
        <div class="Arsenal_MainInfo">
            <table class="DataView">
                <thead>
                <tr class="Header">
                    <th colspan="2" class="FieldColumn">
                        <input id="cbMatchBasicInfo" type="checkbox" checked="checked" onclick="$('#phBasicInfo').toggle()"/>
                        <label for="cbMatchDetailInfo" title="展开比赛基本信息" id="lblPlayerBasicInfo" runat="server">
                            比赛基本信息
                        </label>
                    </th>
                </tr>
                </thead>
                <tbody id="phBasicInfo">
                <tr class="Row">
                    <td class="FieldHeader" style="width: 30%">
                        比赛GUID:
                    </td>
                    <td class="FieldColumn">
                        <asp:TextBox ID="tbMatchGuid" runat="server" CssClass="TextBoxRead" Width="300px"></asp:TextBox>
                    </td>
                </tr>
                <tr class="AlternatingRow">
                    <td class="FieldHeader">
                        所属分类:
                    </td>
                    <td class="FieldColumn">
                        <asp:DropDownList ID="ddlLeague" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLeague_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="FieldHeader">
                        对阵:
                    </td>
                    <td class="FieldColumn">
                        <asp:DropDownList ID="ddlTeam" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr class="AlternatingRow">
                    <td class="FieldHeader">
                        主客场:
                    </td>
                    <td class="FieldColumn">
                        <asp:CheckBox ID="cbIsHome" runat="server" Text="主场" Checked="true"/>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="FieldHeader">
                        轮次:
                    </td>
                    <td class="FieldColumn">
                        <asp:TextBox ID="tbRound" runat="server" CssClass="TextBox" Width="20px"></asp:TextBox>
                    </td>
                </tr>
                <tr class="AlternatingRow">
                    <td class="FieldHeader">
                        比赛时间:
                    </td>
                    <td class="FieldColumn">
                        <asp:TextBox ID="tbPlayTime" runat="server" CssClass="TextBox" Width="180px"></asp:TextBox>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="FieldHeader">
                        有效:
                    </td>
                    <td class="FieldColumn">
                        <asp:CheckBox ID="cbIsActive" runat="server" Text="有效" Checked="true"/>
                    </td>
                </tr>
                </tbody>
            </table>
            <table class="DataView" cellspacing="0">
                <thead>
                <tr class="Header">
                    <th colspan="2" class="FieldColumn">
                        <input id="cbMatchDetailInfo" type="checkbox" checked="checked" onclick="$('#phDetailInfo').toggle()"/>
                        <label for="cbMatchDetailInfo" title="展开比赛详细信息" id="lblPlayerDetailInfo" runat="server">
                            比赛详细信息
                        </label>
                    </th>
                </tr>
                </thead>
                <tbody id="phDetailInfo">
                <tr class="Row">
                    <td class="FieldHeader" style="width: 30%">
                        比赛结果(主v客):
                    </td>
                    <td class="FieldColumn" id="tdMatchResult">
                        <asp:TextBox ID="tbResultHome" runat="server" CssClass="TextBox" Width="20px"></asp:TextBox>
                        <em>:</em>
                        <asp:TextBox ID="tbResultAway" runat="server" CssClass="TextBox" Width="20px"></asp:TextBox>
                    </td>
                </tr>
                <tr class="AlternatingRow">
                    <td class="FieldHeader">
                        博彩比赛GUID:
                    </td>
                    <td class="FieldColumn">
                        <asp:TextBox ID="tbCasinoMatchGuid" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="FieldHeader">
                        分组GUID:
                    </td>
                    <td class="FieldColumn">
                        <asp:TextBox ID="tbGroupGuid" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                    </td>
                </tr>
                <tr class="AlternatingRow">
                    <td class="FieldHeader">
                        比赛图片地址:
                    </td>
                    <td class="FieldColumn">
                        <asp:TextBox ID="tbReportImageURL" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="FieldHeader">
                        比赛报告地址:
                    </td>
                    <td class="FieldColumn">
                        <asp:TextBox ID="tbReportURL" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                    </td>
                </tr>
                <tr class="AlternatingRow">
                    <td class="FieldHeader">
                        比赛讨论贴地址:
                    </td>
                    <td class="FieldColumn">
                        <asp:TextBox ID="tbTopicURL" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="FieldHeader">
                        备注:
                    </td>
                    <td class="FieldColumn" colspan="3">
                        <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBox" Width="400px" TextMode="MultiLine"
                                     Rows="4">
                        </asp:TextBox>
                    </td>
                </tr>
                </tbody>
            </table>
            <div class="FooterBtnBar">
                <asp:Button ID="btnSubmit" runat="server" CssClass="InputBtn Submit" Text="保存比赛"
                            OnClick="btnSubmit_Click" OnClientClick="return confirm('保存该比赛信息')"/>
                <asp:Button ID="btnCancel" runat="server" CssClass="InputBtn" Text="返回" OnClick="btnCancel_Click"/>
                <asp:Button ID="btnDelete" runat="server" CssClass="InputBtn" Text="删除比赛" OnClick="btnDelete_Click"
                            OnClientClick="return confirm('删除该比赛信息?(无法恢复)')"/>
            </div>
        </div>
    </div>
</asp:Content>