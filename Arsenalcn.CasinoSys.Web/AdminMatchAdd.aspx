<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" CodeBehind="AdminMatchAdd.aspx.cs"
Inherits="Arsenalcn.CasinoSys.Web.AdminMatchAdd" Title="后台管理 比赛添加" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:AdminPanel ID="pnlAdmin" runat="server"/>
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server"/>
        <div class="CasinoSys_MainInfo">
            <table class="DataView" cellspacing="0" cellpadding="5">
                <thead>
                <tr class="Header">
                    <th colspan="2">
                        添加博彩比赛，请在比赛开始前一周左右添加为宜。
                    </th>
                </tr>
                </thead>
                <tbody>
                <tr class="Row">
                    <td class="FieldHeader">
                        所属分类:
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlLeague" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLeague_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr class="AlternatingRow">
                    <td class="FieldHeader">
                        轮次:
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbRound" runat="server" CssClass="TextBox"></asp:TextBox>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="FieldHeader">
                        主场球队:
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlHomeTeam" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr class="AlternatingRow">
                    <td class="FieldHeader">
                        客场球队:
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlAwayTeam" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="FieldHeader">
                        截止时间:
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbPlayTime" runat="server" CssClass="TextBox"></asp:TextBox>
                    </td>
                </tr>
                <tr class="AlternatingRow">
                    <td class="FieldHeader">
                        胜赔率:
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbWinRate" runat="server" CssClass="TextBox"></asp:TextBox>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="FieldHeader">
                        平赔率:
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbDrawRate" runat="server" CssClass="TextBox"></asp:TextBox>
                    </td>
                </tr>
                <tr class="AlternatingRow">
                    <td class="FieldHeader">
                        负赔率:
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbLoseRate" runat="server" CssClass="TextBox"></asp:TextBox>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="FieldHeader">
                        所属组别
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlLeagueGroup" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                </tbody>
            </table>
            <div class="FooterBtnBar">
                <asp:LinkButton ID="btnSave" runat="server" CssClass="LinkBtn" Text="保存" OnClick="btnSave_Click"
                                OnClientClick="return confirm('确认添加新比赛?')">
                </asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>