<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopClubList.ascx.cs"
    Inherits="Arsenalcn.ClubSys.Web.Control.TopClubList" %>
<%@ OutputCache Duration="3600" VaryByParam="none" %>
<asp:Panel ID="pnlClubTop" runat="server" CssClass="InfoPanel">
    <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
        <a>球会排行榜</a>
    </h3>
    <div class="Block">
        <ul>
            <li class="LiTitle"><a onclick="javascript:SwitchClubRank('clubLvRank');">等级</a> | <a
                onclick="javascript:SwitchClubRank('clubRpRank');">评价</a> | <a onclick="javascript:SwitchClubRank('clubFortuneRank');">
                    财富</a></li>
        </ul>
        <asp:Repeater ID="rptTopClubs" runat="server">
            <HeaderTemplate>
                <ul id="clubRpRank" style="display: none;">
            </HeaderTemplate>
            <ItemTemplate>
                <li class="IconTop<%#DataBinder.Eval(Container.DataItem, "AdditionalData2") %>"><em>
                    RP:<%#DataBinder.Eval(Container.DataItem, "RankScore") %></em><a href="ClubView.aspx?ClubID=<%#DataBinder.Eval(Container.DataItem, "ID") %>"><%#DataBinder.Eval(Container.DataItem, "FullName") %></a></li>
            </ItemTemplate>
            <FooterTemplate>
                </ul></FooterTemplate>
        </asp:Repeater>
        <asp:Repeater ID="rptLv" runat="server">
            <HeaderTemplate>
                <ul id="clubLvRank">
            </HeaderTemplate>
            <ItemTemplate>
                <li class="IconTop<%#DataBinder.Eval(Container.DataItem, "AdditionalData2") %>"><em>
                    Lv<%#DataBinder.Eval(Container.DataItem, "RankLevel")%></em><a href="ClubView.aspx?ClubID=<%#DataBinder.Eval(Container.DataItem, "ID") %>"><%#DataBinder.Eval(Container.DataItem, "FullName")%></a></li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
        <asp:Repeater ID="rptFortune" runat="server">
            <HeaderTemplate>
                <ul id="clubFortuneRank" style="display: none;">
            </HeaderTemplate>
            <ItemTemplate>
                <li class="IconTop<%#DataBinder.Eval(Container.DataItem, "AdditionalData2") %>"><em>
                    $<%#DataBinder.Eval(Container.DataItem, "Fortune")%></em><a href="ClubView.aspx?ClubID=<%#DataBinder.Eval(Container.DataItem, "ID") %>"><%#DataBinder.Eval(Container.DataItem, "FullName")%></a></li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
    </div>
</asp:Panel>
