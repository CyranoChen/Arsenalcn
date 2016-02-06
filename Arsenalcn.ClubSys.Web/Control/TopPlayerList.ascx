<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopPlayerList.ascx.cs"
Inherits="Arsenalcn.ClubSys.Web.Control.TopPlayerList" %>
<%@ OutputCache Duration="3600" VaryByParam="none" %>
<asp:Panel ID="pnlPlayerTop" runat="server" CssClass="InfoPanel">
    <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
        <a>球员排行榜</a>
    </h3>
    <div class="Block">
        <ul>
            <li class="LiTitle">
                <a onclick="javascript:SwitchPlayerRank('rpRank');">获得</a> | <a
                                                                                 onclick="javascript:SwitchPlayerRank('cardRank');">
                    卡片
                </a> | <a onclick="javascript:SwitchPlayerRank('videoRank');">
                    视频
                </a>
            </li>
        </ul>
        <asp:Repeater ID="rptRP" runat="server">
            <HeaderTemplate>
                <ul id="rpRank">
            </HeaderTemplate>
            <ItemTemplate>
                <li class="IconTop<%#DataBinder.Eval(Container.DataItem, "Rank") %>">
                    <em>
                        <%#DataBinder.Eval(Container.DataItem, "countGot") %>
                    </em><em>(<%#DataBinder.Eval(Container.DataItem, "rp") %>%)</em><a
                                                                                       href="MyPlayerProfile.aspx?UserID=<%#DataBinder.Eval(Container.DataItem, "userid") %>">
                        <%#DataBinder.Eval(Container.DataItem, "username").ToString().Trim() %>
                    </a>
                </li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
        <asp:Repeater ID="rptCard" runat="server">
            <HeaderTemplate>
                <ul id="cardRank" style="display: none;">
            </HeaderTemplate>
            <ItemTemplate>
                <li class="IconTop<%#DataBinder.Eval(Container.DataItem, "Rank") %>">
                    <em>Card:<%#DataBinder.Eval(Container.DataItem, "cardCount") %></em><a
                                                                                           href="MyPlayerProfile.aspx?UserID=<%#DataBinder.Eval(Container.DataItem, "userid") %>">
                        <%#DataBinder.Eval(Container.DataItem, "username").ToString().Trim() %>
                    </a>
                </li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
        <asp:Repeater ID="rptVideo" runat="server">
            <HeaderTemplate>
                <ul id="videoRank" style="display: none;">
            </HeaderTemplate>
            <ItemTemplate>
                <li class="IconTop<%#DataBinder.Eval(Container.DataItem, "Rank") %>">
                    <em>Video:<%#DataBinder.Eval(Container.DataItem, "videoCount") %></em><a
                                                                                             href="MyPlayerProfile.aspx?UserID=<%#DataBinder.Eval(Container.DataItem, "userid") %>">
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