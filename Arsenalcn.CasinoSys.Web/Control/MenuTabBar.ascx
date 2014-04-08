<%@ Control Language="C#" CodeBehind="MenuTabBar.ascx.cs"
    Inherits="Arsenalcn.CasinoSys.Web.Control.MenuTabBar" %>
<div class="MenuTabBar">
    <ul>
        <li id="CasinoPortal"><a href="CasinoPortal.aspx">可投注比赛</a></li>
        <li id="CasinoGame"><a href="CasinoGame.aspx">比赛结果</a></li>
        <li id="CasinoGroup"><a href="CasinoGroup.aspx">分组积分榜</a></li>
        <li id="CasinoGambler"><a href="CasinoGambler.aspx">博彩玩家</a></li>
        <li id="CasinoBetLog"><a href="CasinoBetLog.aspx">投注中奖查询</a></li>
        <li id="CasinoRank"><a href="CasinoRank.aspx">博彩排行榜</a></li>
    </ul>
    
    <script type="text/javascript">
        $("div.MenuTabBar li#<%=CurrentMenu.ToString() %>").addClass("Current");
    </script>
</div>
