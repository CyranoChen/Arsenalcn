<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" CodeBehind="CasinoGroup.aspx.cs"
    Inherits="Arsenalcn.CasinoSys.Web.CasinoGroup" Title="分组积分榜" %>

<%@ Register Src="Control/LeftPanel.ascx" TagName="LeftPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/FieldTooBar.ascx" TagName="FieldTooBar" TagPrefix="uc2" %>
<%@ Register Src="Control/MenuTabBar.ascx" TagName="MenuTabBar" TagPrefix="uc3" %>
<%@ Register Src="Control/LeagueHeader.ascx" TagName="LeagueHeader" TagPrefix="uc4" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:LeftPanel ID="ctrlLeftPanel" runat="server" />
    <div id="MainPanel">
        <uc2:FieldTooBar ID="ctrlFieldTooBar" runat="server" />
        <uc3:MenuTabBar ID="ctrlMenuTabBar" runat="server" />
        <uc4:LeagueHeader ID="ctrlLeagueHeader" runat="server" />
        <div class="FunctionBar">
            <div class="DivFloatLeft">
                <asp:DropDownList ID="ddlSeason" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSeason_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:DropDownList ID="ddlGroup" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
            <div class="DivFloatRight">
                <asp:LinkButton ID="btnGroupMatch" runat="server" CssClass="LinkBtn SelectBtn" Text="查看本组比赛"
                    OnClick="btnGroupMatch_Click"></asp:LinkButton>
            </div>
            <div class="Clear">
            </div>
        </div>
        <asp:Panel ID="pnlGroupList" CssClass="CasinoSys_GroupList" runat="server">
            <asp:Repeater ID="rptGroup" runat="server" OnItemDataBound="rptGroup_ItemDataBound">
                <ItemTemplate>
                    <asp:GridView ID="gvGroupTeam" runat="server" PageSize="100"
                        OnRowDataBound="gvGroupTeam_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="PositionNo" HeaderText="排行" NullDisplayText="/" DataFormatString="<em>{0}</em>"
                                HtmlEncode="false" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Literal ID="ltrlTeamLogo" runat="server"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="球队" ItemStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hlTeamInfo" runat="server" CssClass="StrongLink"></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="TotalPlayed" HeaderText="场次" NullDisplayText="0" />
                            <asp:BoundField DataField="TotalWon" HeaderText="胜" NullDisplayText="0" />
                            <asp:BoundField DataField="TotalDraw" HeaderText="平" NullDisplayText="0" />
                            <asp:BoundField DataField="TotalLost" HeaderText="负" NullDisplayText="0" />
                            <asp:BoundField DataField="TotalPoints" HeaderText="积分" NullDisplayText="0" DataFormatString="<em>{0}</em>"
                                HtmlEncode="false" />
                        </Columns>
                    </asp:GridView>
                </ItemTemplate>
            </asp:Repeater>
            <div class="Clear">
            </div>
        </asp:Panel>
        <asp:GridView ID="gvGroupTable" runat="server" PageSize="100" OnRowDataBound="gvGroupTable_RowDataBound">
            <Columns>
                <asp:BoundField DataField="PositionNo" HeaderText="排行" NullDisplayText="/" DataFormatString="<em>{0}</em>"
                    HtmlEncode="false" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Literal ID="ltrlTeamLogo" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="球队">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlTeamInfo" runat="server" CssClass="StrongLink"></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="TotalPlayed" HeaderText="场次" NullDisplayText="0" />
                <asp:BoundField DataField="TotalWon" HeaderText="胜" NullDisplayText="0" />
                <asp:BoundField DataField="TotalDraw" HeaderText="平" NullDisplayText="0" />
                <asp:BoundField DataField="TotalLost" HeaderText="负" NullDisplayText="0" />
                <asp:BoundField DataField="TotalGoalFor" HeaderText="进球" NullDisplayText="0" />
                <asp:BoundField DataField="TotalGoalAgainst" HeaderText="失球" NullDisplayText="0" />
                <asp:TemplateField HeaderText="净胜球">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlGoalDiff" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="TotalPoints" HeaderText="积分" NullDisplayText="0" DataFormatString="<em>{0}</em>"
                    HtmlEncode="false" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
