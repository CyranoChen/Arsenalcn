<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" CodeBehind="CasinoGame.aspx.cs"
    Inherits="Arsenalcn.CasinoSys.Web.CasinoGame" Title="比赛结果" %>

<%@ Register Src="Control/LeftPanel.ascx" TagName="LeftPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/FieldTooBar.ascx" TagName="FieldTooBar" TagPrefix="uc2" %>
<%@ Register Src="Control/MenuTabBar.ascx" TagName="MenuTabBar" TagPrefix="uc3" %>
<%@ Register Src="Control/LeagueHeader.ascx" TagName="LeagueHeader" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:LeftPanel ID="ctrlLeftPanel" runat="server" />
    <div id="MainPanel">
        <uc2:FieldTooBar ID="ctrlFieldTooBar" runat="server" />
        <uc3:MenuTabBar ID="ctrlMenuTabBar" runat="server" />
        <uc4:LeagueHeader ID="ctrlLeagueHeader" runat="server" />
        <div class="FunctionBar">
            <div class="DivFloatLeft">
                <asp:DropDownList ID="ddlSeason" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSeason_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:DropDownList ID="ddlGameTip" runat="server" Visible="false">
                    <asp:ListItem Selected="True" Text="所有比赛"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="DivFloatRight">
                比赛场次:<em><asp:Literal ID="ltrlMatchCount" runat="server"></asp:Literal></em>
            </div>
            <div class="Clear">
            </div>
        </div>
        <asp:GridView ID="gvMatch" runat="server" OnPageIndexChanging="gvMatch_PageIndexChanging"
            OnRowDataBound="gvMatch_RowDataBound" PageSize="15">
            <Columns>
                <asp:BoundField DataField="PlayTime" DataFormatString="{0:yyyy-MM-dd HH:mm}" HeaderText="比赛时间" />
                <asp:BoundField DataField="Round" HeaderText="轮" NullDisplayText="/" />
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
                <asp:TemplateField HeaderText="结果">
                    <ItemTemplate>
                        <%#DataBinder.Eval(Container.DataItem, "ResultHome") %>：<%#DataBinder.Eval(Container.DataItem, "ResultAway") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="注数">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlTotalBetCount" runat="server"></asp:Literal></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="投注总量">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlTotalBetCash" runat="server"></asp:Literal></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="比赛盈亏">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlTotalWin" runat="server"></asp:Literal></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="中奖查询">
                    <ItemTemplate>
                        <a class="LinkBtn SelectBtn" href="CasinoBetLog.aspx?Match=<%#DataBinder.Eval(Container.DataItem, "MatchGuid") %>">
                            中奖查询</a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
