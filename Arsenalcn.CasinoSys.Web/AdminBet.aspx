<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="AdminBet.aspx.cs" Inherits="Arsenalcn.CasinoSys.Web.AdminBet" Title="后台管理 投注日志" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:AdminPanel ID="pnlAdmin" runat="server" />
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server" />
        <div class="FunctionBar">
            <div class="DivFloatLeft">
                <asp:DropDownList ID="ddlTimeDiff" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTimeDiff_SelectedIndexChanged">
                    <asp:ListItem Text="最近一天" Value="1" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="最近一周" Value="7"></asp:ListItem>
                    <asp:ListItem Text="最近一月" Value="30"></asp:ListItem>
                    <asp:ListItem Text="当前赛季" Value="365"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="Clear">
            </div>
        </div>
        <asp:GridView ID="gvBet" runat="server" OnRowDataBound="gvBet_RowDataBound" OnPageIndexChanging="gvBet_PageIndexChanging"
            PageSize="20" OnRowCommand="gvBet_RowCommand">
            <Columns>
                <asp:BoundField DataField="ID" Visible="false" />
                <asp:TemplateField HeaderText="玩家">
                    <ItemTemplate>
                        <a href="MyBonusLog.aspx?userid=<%#DataBinder.Eval(Container.DataItem, "UserID") %>"
                            target="_blank">
                            <%#DataBinder.Eval(Container.DataItem, "UserName") %></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="主队" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <a class="StrongLink">
                            <asp:Literal ID="ltrlHome" runat="server"></asp:Literal></a></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="vs" ItemStyle-Width="20">
                    <ItemTemplate>
                        <em>vs</em></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="客队" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <a class="StrongLink">
                            <asp:Literal ID="ltrlAway" runat="server"></asp:Literal></a></ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="BetTime" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" HeaderText="投注时间" />
                <asp:TemplateField>
                    <HeaderTemplate>
                        预测结果</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Literal ID="ltrlResult" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="BetAmount" HeaderText="投注金额" DataFormatString="{0:N0}"
                    NullDisplayText="/" />
                <asp:TemplateField HeaderText="投注赔率">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlBetRate" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="EarningDesc" HeaderText="奖金" NullDisplayText="/" />
                <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnReturnBet" runat="server" Text="退注" CssClass="LinkBtn" CommandName="ReturnBet"
                            OnClientClick="return confirm('确认操作?')"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
