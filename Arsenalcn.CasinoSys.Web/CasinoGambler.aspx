<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="CasinoGambler.aspx.cs" Inherits="Arsenalcn.CasinoSys.Web.CasinoGambler"
    Title="博彩玩家" %>

<%@ Register Src="Control/LeftPanel.ascx" TagName="LeftPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/FieldTooBar.ascx" TagName="FieldTooBar" TagPrefix="uc2" %>
<%@ Register Src="Control/MenuTabBar.ascx" TagName="MenuTabBar" TagPrefix="uc3" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:LeftPanel ID="ctrlLeftPanel" runat="server" />
    <div id="MainPanel">
        <uc2:FieldTooBar ID="ctrlFieldTooBar" runat="server" />
        <uc3:MenuTabBar ID="ctrlMenuTabBar" runat="server" />
        <div class="FunctionBar">
            <div class="DivFloatLeft">
                <asp:DropDownList ID="ddlOrderClause" runat="server" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlOrderClause_SelectedIndexChanged">
                    <asp:ListItem Text="--请选择排名方式--" Value="" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="按盈亏总量排序" Value="Profit"></asp:ListItem>
                    <asp:ListItem Text="按盈亏率排序" Value="ProfitRate"></asp:ListItem>
                    <asp:ListItem Text="按投注总量排序" Value="TotalBet"></asp:ListItem>
                    <asp:ListItem Text="按RP奖励排序" Value="RPBonus"></asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="ddlContestArea" runat="server" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlContestArea_SelectedIndexChanged">
                    <asp:ListItem Text="--请选择赛区--" Value="" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="上半赛区" Value="Upper"></asp:ListItem>
                    <asp:ListItem Text="下半赛区" Value="Lower"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="DivFloatRight">
                <asp:HyperLink ID="hlGamblerLeagueView" runat="server" CssClass="LinkBtn"></asp:HyperLink>
            </div>
            <div class="Clear">
            </div>
        </div>
        <asp:GridView ID="gvGambler" runat="server" PageSize="20"
            OnPageIndexChanging="gvGambler_PageIndexChanging" OnRowDataBound="gvGambler_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="排行">
                    <ItemTemplate>
                        <asp:Label ID="lblRank" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:HyperLinkField HeaderText="用户名" DataTextField="UserName" DataNavigateUrlFields="UserID"
                    DataNavigateUrlFormatString="MyBonusLog.aspx?UserID={0}" Target="_blank" ControlStyle-CssClass="StrongLink" />
                <asp:BoundField DataField="Win" HeaderText="猜中" DataFormatString="{0}" />
                <asp:BoundField DataField="Lose" HeaderText="猜错" DataFormatString="{0}" />
                <asp:BoundField DataField="Profit" DataFormatString="<em>{0:N2}</em>" ItemStyle-HorizontalAlign="Right"
                    HeaderText="盈亏状况" HtmlEncode="false"></asp:BoundField>
                <asp:BoundField DataField="TotalBet" DataFormatString="<em>{0:N2}</em>" ItemStyle-HorizontalAlign="Right"
                    HeaderText="投注量" HtmlEncode="false"></asp:BoundField>
                <asp:BoundField DataField="ProfitRate" DataFormatString="<em>{0:N2}%</em>" ItemStyle-HorizontalAlign="Right"
                    HeaderText="盈亏率" HtmlEncode="false"></asp:BoundField>
                <asp:BoundField DataField="RPBonus" DataFormatString="<em>RP+{0}</em>" HeaderText="RP奖励"
                    HtmlEncode="false" NullDisplayText="/"></asp:BoundField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
