<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" CodeBehind="CasinoBetLog.aspx.cs"
    Inherits="Arsenalcn.CasinoSys.Web.CasinoBetLog" Title="投注中奖查询" %>

<%@ Register Src="Control/LeftPanel.ascx" TagName="LeftPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/FieldTooBar.ascx" TagName="FieldTooBar" TagPrefix="uc2" %>
<%@ Register Src="Control/MenuTabBar.ascx" TagName="MenuTabBar" TagPrefix="uc3" %>
<%@ Register Src="Control/CasinoHeader.ascx" TagName="CasinoHeader" TagPrefix="uc4" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:LeftPanel ID="ctrlLeftPanel" runat="server" />
    <div id="MainPanel">
        <uc2:FieldTooBar ID="ctrlFieldTooBar" runat="server" />
        <uc3:MenuTabBar ID="ctrlMenuTabBar" runat="server" />
        <uc4:CasinoHeader ID="ctrlCasinoHeader" runat="server" />
        <div class="FunctionBar">
            <div class="DivFloatLeft">
                <asp:DropDownList ID="ddlTimeDiff" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTimeDiff_SelectedIndexChanged">
                    <asp:ListItem Text="最近一天" Value="1" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="最近一周" Value="7"></asp:ListItem>
                    <asp:ListItem Text="最近一月" Value="30"></asp:ListItem>
                    <asp:ListItem Text="当前赛季" Value="365"></asp:ListItem>
                </asp:DropDownList>
                <asp:HyperLink ID="hlBetClubLog" runat="server" Text="球会投注中奖查询" CssClass="LinkBtn"></asp:HyperLink>
            </div>
            <div class="DivFloatRight">
                猜输赢:<em><asp:Literal ID="ltrlSingleChoiceCount" runat="server"></asp:Literal></em>
                | 猜比分:<em><asp:Literal ID="ltrlMatchResultCount" runat="server"></asp:Literal></em>
                | 总投注金额:<em><asp:Literal ID="ltrlTotalBetCount" runat="server"></asp:Literal></em>
            </div>
            <div class="Clear">
            </div>
        </div>
        <asp:GridView ID="gvBet" runat="server" OnRowDataBound="gvBet_RowDataBound" OnPageIndexChanging="gvBet_PageIndexChanging"
            PageSize="20">
            <Columns>
                <asp:TemplateField HeaderText="状态">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlBetResult" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="玩家">
                    <ItemTemplate>
                        <a href="MyBonusLog.aspx?userid=<%#DataBinder.Eval(Container.DataItem, "UserID") %>"
                            target="_blank">
                            <%#DataBinder.Eval(Container.DataItem, "UserName") %></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="主队" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlHome" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="vs" ItemStyle-Width="20">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlVS" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="客队" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlAway" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="BetTime" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" HeaderText="投注时间" />
                <asp:TemplateField HeaderText="预测结果">
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
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
