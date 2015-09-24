<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" CodeBehind="CasinoBetClubLog.aspx.cs"
    Inherits="Arsenalcn.CasinoSys.Web.CasinoBetClubLog" Title="投注中奖查询（球会）" %>

<%@ Register Src="Control/LeftPanel.ascx" TagName="LeftPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/FieldTooBar.ascx" TagName="FieldTooBar" TagPrefix="uc2" %>
<%@ Register Src="Control/MenuTabBar.ascx" TagName="MenuTabBar" TagPrefix="uc3" %>
<%@ Register Src="Control/CasinoHeader.ascx" TagName="CasinoHeader" TagPrefix="uc4" %>
<asp:Content ID="Content3" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:LeftPanel ID="ctrlLeftPanel" runat="server" />
    <div id="MainPanel">
        <uc2:FieldTooBar ID="ctrlFieldTooBar" runat="server" />
        <uc3:MenuTabBar ID="ctrlMenuTabBar" runat="server" />
        <uc4:CasinoHeader ID="ctrlCasinoHeader" runat="server" />
        <div class="FunctionBar">
            <div class="DivFloatLeft">
                <asp:DropDownList ID="ddlClub" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlClub_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:HyperLink ID="hlBetLog" runat="server" Text="比赛投注中奖查询" CssClass="LinkBtn"></asp:HyperLink>
            </div>
            <div class="DivFloatRight">
                猜输赢:<em><asp:Literal ID="ltrlSingleChoiceCount" runat="server"></asp:Literal></em>
                | 猜比分:<em><asp:Literal ID="ltrlMatchResultCount" runat="server"></asp:Literal></em>
                | 总投注金额:<em><asp:Literal ID="ltrlTotalBetCount" runat="server"></asp:Literal></em>
            </div>
            <div class="Clear">
            </div>
        </div>
        <asp:GridView ID="gvBet" runat="server" OnRowDataBound="gvBet_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="状态">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlBetResult" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="球会">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlClub" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="玩家">
                    <ItemTemplate>
                        <a href="MyBonusLog.aspx?userid=<%#DataBinder.Eval(Container.DataItem, "UserID") %>"
                            target="_blank">
                            <%#DataBinder.Eval(Container.DataItem, "UserName") %></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="BetTime" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" HeaderText="投注时间" />
                <asp:TemplateField HeaderText="预测结果">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlResult" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="EarningDesc" HeaderText="奖励" NullDisplayText="/" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
