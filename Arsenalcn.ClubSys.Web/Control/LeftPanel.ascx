<%@ Control Language="C#" CodeBehind="LeftPanel.ascx.cs" Inherits="Arsenalcn.ClubSys.Web.Control.LeftPanel"
EnableViewState="false" %>
<%@ Import Namespace="Arsenalcn.ClubSys.Entity" %>
<%@ Register Src="TopPlayerList.ascx" TagName="TopPlayerList" TagPrefix="uc2" %>
<%@ Register Src="DailyVideoExhibit.ascx" TagName="DailyVideoExhibit" TagPrefix="uc3" %>
<div id="LeftPanel">
    <asp:Panel ID="pnlClubNotice" runat="server" CssClass="InfoPanel">
        <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
            <a>ACN球会公告</a>
        </h3>
        <div class="Block">
            <p class="ClubSys_Notice">
                <%= ConfigGlobal.SysNotice %>
            </p>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlMyClub" runat="server" CssClass="InfoPanel">
        <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
            <a>我的球会面板</a>
        </h3>
        <div class="Block">
            <ul>
                <li class="LiTitle">
                    <asp:PlaceHolder ID="phMyClub" runat="server">
                        <asp:HyperLink ID="hlMyClub" runat="server"/>
                        <asp:Literal ID="ltrlMyClubRankScore" runat="server"></asp:Literal>
                    </asp:PlaceHolder>
                    <asp:HyperLink ID="hlNoClub" runat="server" Text="您还未加入任何球会" NavigateUrl="../ClubPortal.aspx"/>
                </li>
                <asp:PlaceHolder ID="phCreateClub" runat="server">
                    <li>
                        <a href="MyCreateClub.aspx">创建我的新球会</a>
                    </li>
                </asp:PlaceHolder>
                <li>
                    <a href="MyHistoryLog.aspx">我的球会历史</a>
                </li>
                <li>
                    <a href="MyPlayerProfile.aspx">我的球员信息</a>
                </li>
                <li>
                    <a href="MyCollection.aspx">我的球员收藏</a>
                </li>
                <asp:PlaceHolder ID="phClubAdmin" runat="server">
                    <li>
                        <asp:HyperLink ID="hlMyAdminClub" runat="server" Text="管理我的球会"/>
                    </li>
                </asp:PlaceHolder>
            </ul>
        </div>
    </asp:Panel>
    <uc3:DailyVideoExhibit ID="ctrlDailyVideoExhibit" runat="server"/>
    <uc2:TopPlayerList ID="ctrlTopPlayerList" runat="server"/>
    <asp:Panel ID="pnlDev" runat="server" CssClass="InfoPanel" Visible="false">
        <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
            <a>程序开发与支持</a>
        </h3>
        <div class="Block">
            <ul>
                <li>开发: <a href="mailto:cao_yi_hui@hotmail.com">Cao262</a></li>
                <li>设计: <a href="mailto:cyrano@arsenalcn.com">Cyrano</a></li>
            </ul>
        </div>
    </asp:Panel>
</div>