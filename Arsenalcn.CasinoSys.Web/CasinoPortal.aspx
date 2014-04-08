<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="CasinoPortal.aspx.cs" Inherits="Arsenalcn.CasinoSys.Web.CasinoPortal"
    Title="ACN CasinoSys 博彩系统" %>

<%@ Register Src="Control/LeftPanel.ascx" TagName="LeftPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/FieldTooBar.ascx" TagName="FieldTooBar" TagPrefix="uc2" %>
<%@ Register Src="Control/MenuTabBar.ascx" TagName="MenuTabBar" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:LeftPanel ID="ctrlLeftPanel" runat="server" />
    <div id="MainPanel">
        <uc2:FieldTooBar ID="ctrlFieldTooBar" runat="server" />
        <uc3:MenuTabBar ID="ctrlMenuTabBar" runat="server" />
        <div class="CasinoSys_Tip">
            <span>博彩只能使用博彩币；单场比赛可以多次投注；赔率固定不会变化。</span></div>
        <asp:GridView ID="gvMatch" runat="server" PageSize="100" OnRowDataBound="gvMatch_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="分类">
                    <ItemTemplate>
                        <a href="CasinoGame.aspx?League=<%#DataBinder.Eval(Container.DataItem, "LeagueGuid") %>"
                            title="<%#DataBinder.Eval(Container.DataItem, "League") %>">
                            <img src="<%#DataBinder.Eval(Container.DataItem, "LeagueLogo") %>" alt="<%#DataBinder.Eval(Container.DataItem, "League") %>"
                                class="CasinoSys_CategoryImg" /></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="主队" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <span class="CasinoSys_GameName"><a class="StrongLink" href="CasinoTeam.aspx?Team=<%# DataBinder.Eval(Container.DataItem, "Home") %>"
                            title="<%# DataBinder.Eval(Container.DataItem, "HomeEng") %>">
                            <%# DataBinder.Eval(Container.DataItem, "HomeDisplay") %></a>
                            <img src="<%# DataBinder.Eval(Container.DataItem, "HomeLogo") %>" alt="<%# DataBinder.Eval(Container.DataItem, "HomeEng") %>" />
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="vs" ItemStyle-Width="20">
                    <ItemTemplate>
                        <a href="CasinoTeam.aspx?Match=<%# DataBinder.Eval(Container.DataItem, "MatchGuid") %>">
                            <em title="<%# DataBinder.Eval(Container.DataItem, "Ground") %>(<%# DataBinder.Eval(Container.DataItem, "Capacity") %>)">
                                vs</em></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="客队" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <span class="CasinoSys_GameName">
                            <img src="<%# DataBinder.Eval(Container.DataItem, "AwayLogo") %>" alt="<%# DataBinder.Eval(Container.DataItem, "AwayEng") %>" />
                            <a class="StrongLink" href="CasinoTeam.aspx?Team=<%# DataBinder.Eval(Container.DataItem, "Away") %>"
                                title="<%# DataBinder.Eval(Container.DataItem, "AwayEng") %>">
                                <%# DataBinder.Eval(Container.DataItem, "AwayDisplay") %></a></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="PlayTime" HeaderText="比赛时间" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                <asp:TemplateField HeaderText="主队胜">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlWinRate" runat="server"></asp:Literal>
                        <asp:Label ID="lbWinInfo" runat="server" CssClass="CasinoSys_BetInfo" ToolTip="投注次数|金额总计"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="双方平">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlDrawRate" runat="server"></asp:Literal>
                        <asp:Label ID="lbDrawInfo" runat="server" CssClass="CasinoSys_BetInfo" ToolTip="投注次数|金额总计"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="客队胜">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlLoseRate" runat="server"></asp:Literal>
                        <asp:Label ID="lbLoseInfo" runat="server" CssClass="CasinoSys_BetInfo" ToolTip="投注次数|金额总计"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="投注">
                    <ItemTemplate>
                        <asp:HyperLink ID="BtnBet" runat="server" CssClass="LinkBtn" ToolTip="您的投注记录"></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
