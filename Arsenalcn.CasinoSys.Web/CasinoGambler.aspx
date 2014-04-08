<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="CasinoGambler.aspx.cs" Inherits="Arsenalcn.CasinoSys.Web.CasinoGambler"
    Title="博彩玩家" %>

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
        <div class="FunctionBar">
            <div class="DivFloatLeft">
                <asp:DropDownList ID="ddlOrderClause" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlOrderClause_SelectedIndexChanged">
                    <asp:ListItem Text="按盈亏总量排序" Value="Profit" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="按盈亏率排序" Value="ProfitRate"></asp:ListItem>
                    <asp:ListItem Text="按投注总量排序" Value="TotalBet"></asp:ListItem>
                    <asp:ListItem Text="按RP奖励排序" Value="RPBonus"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="DivFloatRight">
                <asp:HyperLink ID="hlGamblerLeagueView" runat="server" CssClass="LinkBtn"></asp:HyperLink>
            </div>
            <div class="Clear">
            </div>
        </div>
        <asp:GridView ID="gvGamlber" runat="server" PageSize="20" OnPageIndexChanging="gvGamlber_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="Rank" HeaderText="排行" DataFormatString="<em>{0}</em>"
                    HtmlEncode="false" />
                <asp:TemplateField>
                    <HeaderTemplate>
                        用户名</HeaderTemplate>
                    <ItemTemplate>
                        <a href="MyBonusLog.aspx?UserID=<%#DataBinder.Eval(Container.DataItem, "UserID") %>"
                            target="_blank" class="StrongLink">
                            <%#DataBinder.Eval(Container.DataItem, "UserName") %></a>
                    </ItemTemplate>
                </asp:TemplateField>
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
