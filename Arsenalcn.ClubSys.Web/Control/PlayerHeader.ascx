<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlayerHeader.ascx.cs"
    Inherits="Arsenalcn.ClubSys.Web.Control.PlayerHeader" %>
<%@ Import Namespace="Arsenalcn.ClubSys.Service" %>
<div class="ClubSys_Header">
    <div class="Clear">
    </div>
    <asp:Panel CssClass="PlayerCard" ID="pnlSwf" runat="server">

        <script type="text/javascript">
                    GenSwfObject('UserStrip','swf/UserStrip.swf?XMLURL=ServerXml.aspx%3FUserID=<%=ProfileUserID.ToString() %>','180','120');
        </script>

    </asp:Panel>
    <div class="PlayerItemList">
        <ul>
            <li><span class="ClubSys_Strip Shirt">球衣:</span><em>LV<%=shirtLv %></em></li>
            <li><span class="ClubSys_Strip Shorts">球裤:</span><em>LV<%=shortsLv %></em></li>
            <li><span class="ClubSys_Strip Sock">球袜:</span><em>LV<%=sockLv %></em></li>
        </ul>
    </div>
    <div class="PlayerItemList">
        <ul>
            <li>尝试:<em title="今天(累计)"><%=PlayerStrip.GetUserBingoPlayCountToday(ProfileUserID).ToString()%>
                (<%=PlayerStrip.GetUserBingoPlayCount(ProfileUserID).ToString()%>)</em> </li>
            <li>获得:<em title="今天(累计)"><%=PlayerStrip.GetUserBingoGainCountToday(ProfileUserID).ToString()%>
                (<%=PlayerStrip.GetUserBingoGainCount(ProfileUserID).ToString()%>)</em> </li>
            <li>RP:<em title="抽中率"><asp:Literal ID="ltrlRP" runat="server"></asp:Literal></em></li>
        </ul>
    </div>
    <div class="ClubBtnGroup">
        <asp:LinkButton ID="btnCardFusion" runat="server" CssClass="LinkBtn GetBtn" Text="卡片融合"
            PostBackUrl="../ClubCardFusion.aspx"></asp:LinkButton>
        <asp:LinkButton ID="btnGetStrip" runat="server" CssClass="LinkBtn GetBtn" Text="获取装备"
            PostBackUrl="../ClubBingo.aspx"></asp:LinkButton>
        <asp:LinkButton ID="btnClub" runat="server" CssClass="LinkBtn ManageBtn"></asp:LinkButton>
    </div>
    <asp:Literal ID="ltrlPlayerLV" runat="server"></asp:Literal>
    <div class="Clear">
    </div>
</div>
