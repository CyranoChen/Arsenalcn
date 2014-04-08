<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopMatchList.ascx.cs"
    Inherits="Arsenalcn.CasinoSys.Web.Control.TopMatchList" %>
<%@ OutputCache Duration="3600" VaryByParam="none" %>
<asp:Panel ID="pnlMatchTop" runat="server" CssClass="InfoPanel">
    <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
        <a>庄家排行榜(月排行)</a>
    </h3>
    <div class="Block">
        <ul>
            <li class="LiTitle"><a onclick="$(this).parent().parent().siblings('ul').hide();$('ul#MatchEarning').show();">
                <asp:Literal ID="ltrlEarning" runat="server"></asp:Literal></a>|<a onclick="$(this).parent().parent().siblings('ul').hide();$('ul#MatchLoss').show();"><asp:Literal
                    ID="ltrlLoss" runat="server"></asp:Literal></a></li>
        </ul>
        <asp:Repeater ID="rptMatchEarning" runat="server">
            <HeaderTemplate>
                <ul id="MatchEarning">
            </HeaderTemplate>
            <ItemTemplate>
                <li class="IconTop<%#DataBinder.Eval(Container.DataItem, "Rank") %>"><a href="CasinoBetLog.aspx?Match=<%#DataBinder.Eval(Container.DataItem, "MatchGuid")%>"
                    title="<%#DataBinder.Eval(Container.DataItem, "LeagueName")%> <%#DataBinder.Eval(Container.DataItem, "PlayTime")%>">
                    <%#DataBinder.Eval(Container.DataItem, "HomeDisplay") %>
                    <em>vs</em><%#DataBinder.Eval(Container.DataItem, "AwayDisplay")%></a><em title="比赛盈余"
                        class="CasinoSys_TopRankEM">
                        <%# Convert.ToSingle(DataBinder.Eval(Container.DataItem, "Earning")).ToString("N2")%></em></li>
            </ItemTemplate>
            <FooterTemplate>
                </ul></FooterTemplate>
        </asp:Repeater>
        <asp:Repeater ID="rptMatchLoss" runat="server">
            <HeaderTemplate>
                <ul id="MatchLoss" style="display: none">
            </HeaderTemplate>
            <ItemTemplate>
                <li class="IconTop<%#DataBinder.Eval(Container.DataItem, "Rank") %>"><a href="CasinoBetLog.aspx?Match=<%#DataBinder.Eval(Container.DataItem, "MatchGuid")%>"
                    title="<%#DataBinder.Eval(Container.DataItem, "LeagueName")%> <%#DataBinder.Eval(Container.DataItem, "PlayTime")%>">
                    <%#DataBinder.Eval(Container.DataItem, "HomeDisplay") %>
                    <em>vs</em><%#DataBinder.Eval(Container.DataItem, "AwayDisplay")%></a><em title="比赛亏损"
                        class="CasinoSys_TopRankEM">
                        <%# Convert.ToSingle(DataBinder.Eval(Container.DataItem, "Earning")).ToString("N2")%></em></li>
            </ItemTemplate>
            <FooterTemplate>
                </ul></FooterTemplate>
        </asp:Repeater>
    </div>
</asp:Panel>
