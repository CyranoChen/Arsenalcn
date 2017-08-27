<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="AdminLeagueTeamView.aspx.cs" Inherits="Arsenal.Web.AdminLeagueTeamView" Title="后台管理 添加/更新分类球队" Theme="Arsenalcn" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
    <link href="Content/themes/base/all.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="Scripts/jquery-ui-1.12.1.min.js"></script>
    <script type="text/javascript">
        $(function () {
            var $tbTeamFilter = $("td#tdLeagueTeam").find("input.tbTeamFilter");
            var $tbTeamSelected = $("td#tdLeagueTeam").find("input.tbTeamSelected");

            var cacheTeamList = [];

            $.getJSON("ServerLeagueTeamHandler.ashx", function (data, status) {
                if (status === "success" && data != null) {
                    if (data.result === "error") {
                        data = [];
                    }

                    cacheTeamList = data;

                    $tbTeamFilter.autocomplete({
                        source: cacheTeamList,
                        minLength: 1,
                        autoFocus: true,
                        select: function (event, ui) {
                            $(this).val(ui.item.label);
                            $tbTeamSelected.val(ui.item.value);

                            event.preventDefault();
                        }
                    }).focus(function () {
                        $(this).val("");
                    });
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:AdminPanel ID="pnlAdmin" runat="server" />
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server" />
        <div class="Arsenal_MainInfo">
            <table class="DataView">
                <thead>
                    <tr class="Header">
                        <th colspan="2">添加当前分类中的对应球队</th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="Row">
                        <td class="FieldHeader" style="width: 30%">分类GUID:
                        </td>
                        <td class="FieldColumn">
                            <asp:Label runat="server" ID="lblLeagueGuid"></asp:Label>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">分类名称：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label runat="server" ID="lblLeagueName"></asp:Label>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">所属球队：
                        </td>
                        <td class="FieldColumn" id="tdLeagueTeam">
                            <asp:TextBox runat="server" ID="tbTeamFilter" CssClass="TextBox tbTeamFilter" Width="200px"></asp:TextBox>
                            <asp:TextBox runat="server" ID="tbTeamSelected" CssClass="TextBox tbTeamSelected hidden"></asp:TextBox>
                            <asp:LinkButton runat="server" ID="btnAddTeam" CssClass="LinkBtn" Text="增加" OnClick="btnAddTeam_Click"></asp:LinkButton>
                            <asp:LinkButton runat="server" ID="btnRemoveTeam" CssClass="LinkBtn" Text="移除" OnClick="btnRemoveTeam_Click"></asp:LinkButton>
                            <br />
                            <asp:ListBox ID="lbLeagueTeam" CssClass="lbLeagueTeam" Width="300px" Rows="32" runat="server" SelectionMode="Multiple"></asp:ListBox>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="FooterBtnBar">
                <asp:Button ID="btnCancel" runat="server" CssClass="InputBtn" Text="返回分类" OnClick="btnCancel_Click" />
                <asp:Button ID="btnDelete" runat="server" CssClass="InputBtn" Text="删除全部所属球队" OnClick="btnDelete_Click"
                    OnClientClick="return confirm('删除该分类下全部所属球队?(无法恢复)')" />
            </div>
        </div>
    </div>
</asp:Content>
