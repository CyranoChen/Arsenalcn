<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopGamblerList.ascx.cs"
Inherits="Arsenalcn.CasinoSys.Web.Control.TopGamblerList" %>
<%@ OutputCache Duration="3600" VaryByParam="none" %>
<asp:Panel ID="pnlGamblerTop" runat="server" CssClass="InfoPanel">
    <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
        <a>玩家排行榜(月排行)</a>
    </h3>
    <div class="Block">
        <ul>
            <li class="LiTitle">
                <a onclick="$(this).parent().parent().siblings('ul').hide();$('ul#GamblerProfit').show();">
                    <asp:Literal ID="ltrlProfit" runat="server"></asp:Literal>
                </a> | <a onclick="$(this).parent().parent().siblings('ul').hide();$('ul#GamblerRP').show();">
                    <asp:Literal
                        ID="ltrlRP" runat="server">
                    </asp:Literal>
                </a> | <a onclick="$(this).parent().parent().siblings('ul').hide();$('ul#GamblerBet').show();">
                    <asp:Literal
                        ID="ltrlBet" runat="server">
                    </asp:Literal>
                </a>
            </li>
        </ul>
        <asp:Repeater ID="rptGamblerProfit" runat="server">
            <HeaderTemplate>
                <ul id="GamblerProfit">
            </HeaderTemplate>
            <ItemTemplate>
                <li class="IconTop<%#DataBinder.Eval(Container.DataItem, "Rank") %>">
                    <em title="博彩收益">
                        <%# Convert.ToSingle(DataBinder.Eval(Container.DataItem, "Profit")).ToString("N2") %>
                    </em><a
                             href="MyBonusLog.aspx?UserID=<%#DataBinder.Eval(Container.DataItem, "userid") %>">
                        <%#DataBinder.Eval(Container.DataItem, "username").ToString().Trim() %>
                    </a>
                </li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
        <asp:Repeater ID="rptGamblerRP" runat="server">
            <HeaderTemplate>
                <ul id="GamblerRP" style="display: none">
            </HeaderTemplate>
            <ItemTemplate>
                <li class="IconTop<%#DataBinder.Eval(Container.DataItem, "Rank") %>">
                    <em title="RP奖励">
                        RP+<%#DataBinder.Eval(Container.DataItem, "RPBonus") %>:
                    </em><a href="MyBonusLog.aspx?UserID=<%#DataBinder.Eval(Container.DataItem, "userid") %>"><%#DataBinder.Eval(Container.DataItem, "username").ToString().Trim() %></a>
                </li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
        <asp:Repeater ID="rptGamblerBet" runat="server">
            <HeaderTemplate>
                <ul id="GamblerBet" style="display: none">
            </HeaderTemplate>
            <ItemTemplate>
                <li class="IconTop<%#DataBinder.Eval(Container.DataItem, "Rank") %>">
                    <em title="投注总量">
                        Bet:<%# Convert.ToSingle(DataBinder.Eval(Container.DataItem, "TotalBetMonthly")).ToString("N0") %>
                    </em><a
                             href="MyBetLog.aspx?UserID=<%#DataBinder.Eval(Container.DataItem, "userid") %>">
                        <%#DataBinder.Eval(Container.DataItem, "username").ToString().Trim() %>
                    </a>
                </li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
    </div>
</asp:Panel>