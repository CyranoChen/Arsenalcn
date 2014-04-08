<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="MyBetLog.aspx.cs" Inherits="Arsenalcn.CasinoSys.Web.MyBetLog" Title="我的中奖查询" %>

<%@ Register Src="Control/LeftPanel.ascx" TagName="LeftPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/FieldTooBar.ascx" TagName="FieldTooBar" TagPrefix="uc2" %>
<%@ Register Src="Control/MenuTabBar.ascx" TagName="MenuTabBar" TagPrefix="uc3" %>
<%@ Register Src="Control/GamblerHeader.ascx" TagName="GamblerHeader" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:LeftPanel ID="ctrlLeftPanel" runat="server" />
    <div id="MainPanel">
        <uc2:FieldTooBar ID="ctrlFieldTooBar" runat="server" />
        <uc3:MenuTabBar ID="ctrlMenuTabBar" runat="server" />
        <uc4:GamblerHeader ID="ctrlGamblerHeader" runat="server" />
        <asp:GridView ID="gvBetLog" runat="server" OnPageIndexChanging="gvBetLog_PageIndexChanging"
            OnRowDataBound="gvBetLog_RowDataBound" OnRowCommand="gvBetLog_RowCommand" PageSize="20">
            <Columns>
                <asp:BoundField DataField="ID" Visible="false" />
                <asp:TemplateField HeaderText="结果">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlBetResult" runat="server"></asp:Literal>
                        <asp:LinkButton ID="btnReturnBet" runat="server" Text="退注" CssClass="LinkBtn" CommandName="ReturnBet"
                            OnClientClick="return confirm('确认取消投注?')"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="主队" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <a class="StrongLink" href="CasinoTeam.aspx?Team=<%# DataBinder.Eval(Container.DataItem, "Home") %>">
                            <%#DataBinder.Eval(Container.DataItem, "HomeDisplay") %></a></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-Width="20">
                    <ItemTemplate>
                        <a href="CasinoTeam.aspx?Match=<%# DataBinder.Eval(Container.DataItem, "MatchGuid") %>">
                            <em title="<%# DataBinder.Eval(Container.DataItem, "Ground") %>(<%# DataBinder.Eval(Container.DataItem, "Capacity") %>)">
                                vs</em></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="客队" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <a class="StrongLink" href="CasinoTeam.aspx?Team=<%# DataBinder.Eval(Container.DataItem, "Away") %>">
                            <%#DataBinder.Eval(Container.DataItem, "AwayDisplay") %></a></ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="BetTime" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" HeaderText="投注时间" />
                <asp:TemplateField HeaderText="预测结果">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlResult" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Bet" HeaderText="投注金额" DataFormatString="{0:N0}" NullDisplayText="/" />
                <asp:TemplateField HeaderText="投注赔率">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlBetRate" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="EarningDesc" HeaderText="奖金" NullDisplayText="/" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
