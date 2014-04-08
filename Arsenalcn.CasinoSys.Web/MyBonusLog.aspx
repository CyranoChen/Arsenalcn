<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="MyBonusLog.aspx.cs" Inherits="Arsenalcn.CasinoSys.Web.MyBonusLog"
    Title="我的盈亏情况" %>

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
        <asp:GridView ID="gvBonusLog" runat="server" OnPageIndexChanging="gvBonusLog_PageIndexChanging"
            OnRowDataBound="gvBonusLog_RowDataBound" PageSize="20">
            <Columns>
                <asp:TemplateField HeaderText="标识">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlResult" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="PlayTime" DataFormatString="{0:yyyy-MM-dd HH:mm}" HeaderText="比赛时间" />
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
                <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                    <HeaderTemplate>
                        投注金额</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Literal ID="ltrlBetCount" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                    <HeaderTemplate>
                        盈亏</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Literal ID="ltrWinLose" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        额外奖励</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Literal ID="ltrlExtraBonus" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
