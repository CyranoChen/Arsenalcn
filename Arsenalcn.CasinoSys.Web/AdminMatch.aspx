<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" CodeBehind="AdminMatch.aspx.cs"
    Inherits="Arsenalcn.CasinoSys.Web.AdminMatch" Title="后台管理 比赛管理" AutoEventWireup="true" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
    <style type="text/css">
        td.ResultColumn, td.BtnColumn {
            white-space: nowrap;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".DataView td.BtnColumn a.LinkBtn:contains('删除')").click(function () { return confirm('确认删除?') });

            var $tbResult = $(".DataView td.ResultColumn > .TextBox");
            $tbResult.each(function () {
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
        <div class="CasinoSys_Tip" style="text-align: right;">
            <a href="AdminMatchAdd.aspx" class="LinkBtn">添加新比赛</a>
        </div>
        <asp:GridView ID="gvMatch" runat="server" OnRowDataBound="gvMatch_RowDataBound" DataKeyNames="MatchGuid"
            OnRowCancelingEdit="gvMatch_RowCancelingEdit" OnRowEditing="gvMatch_RowEditing"
            OnRowUpdating="gvMatch_RowUpdating" OnRowDeleting="gvMatch_RowDeleting" OnRowCommand="gvMatch_RowCommand"
            OnPageIndexChanging="gvMatch_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="MatchGuid" Visible="false" />
                <asp:BoundField DataField="LeagueName" HeaderText="分类" ReadOnly="true" />
                <asp:BoundField DataField="HomeDisplay" HeaderText="主队" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                <asp:TemplateField HeaderText="vs" ItemStyle-Width="20">
                    <ItemTemplate>
                        <em>vs</em>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="AwayDisplay" HeaderText="客队" ReadOnly="true" ItemStyle-HorizontalAlign="Left" />
                <asp:TemplateField HeaderText="轮">
                    <ItemTemplate>
                        <%#DataBinder.Eval(Container.DataItem, "Round") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="tbRound" runat="server" CssClass="TextBox" Width="20"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="比赛时间">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "PlayTime","{0:yyyy-MM-dd HH:mm}") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="tbPlayTime" runat="server" CssClass="TextBox" Width="100px"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="比赛结果" ItemStyle-CssClass="ResultColumn">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlMatchResult" runat="server"></asp:Literal>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="tbHome" runat="server" Text="主队" Width="20" CssClass="TextBox"></asp:TextBox>
                        <em>vs</em>
                        <asp:TextBox ID="tbAway" runat="server" Text="客队" Width="20" CssClass="TextBox"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:CommandField HeaderText="编辑" ShowEditButton="true" ShowDeleteButton="true" EditText="编辑"
                    UpdateText="保存" CancelText="取消" DeleteText="删除" ButtonType="Link" ControlStyle-CssClass="LinkBtn"
                    ItemStyle-CssClass="BtnColumn" />
                <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                        <asp:Label ID="lblBonus" runat="server" Text="/"></asp:Label>
                        <asp:LinkButton ID="btnReturnBet" runat="server" Text="退还投注" CssClass="LinkBtn" CommandName="ReturnBet"
                            OnClientClick="return confirm('确认操作?')"></asp:LinkButton>
                        <asp:LinkButton ID="btnCalcBonus" runat="server" Text="发放奖励" CssClass="LinkBtn" CommandName="CalcBonus"
                            OnClientClick="return confirm('确认操作?')"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
