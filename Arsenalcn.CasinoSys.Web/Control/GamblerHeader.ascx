<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GamblerHeader.ascx.cs"
    Inherits="Arsenalcn.CasinoSys.Web.Control.GamblerHeader" %>
<div class="CasinoSys_Header">
    <div class="Clear">
    </div>
    <div class="GamblerItemList NoList">
        <ul>
            <li><asp:HyperLink ID="hlUserName" runat="server"></asp:HyperLink></li>
            <li><asp:Image ID="imgAvatar" runat="server" Height="80px" /></li>
        </ul>
    </div>
    <div class="GamblerItemList">
        <ul>
            <li>投注总额:<em><asp:Literal ID="ltrlTotalBet" runat="server"></asp:Literal></em></li>
            <li>奖金总额:<em><asp:Literal ID="ltrlEarning" runat="server"></asp:Literal></em></li>
            <li>猜中注数:<em><asp:Literal ID="ltrlWin" runat="server"></asp:Literal></em></li>
            <li>猜错注数:<em><asp:Literal ID="ltrlLose" runat="server"></asp:Literal></em></li>
        </ul>
    </div>
    <div class="GamblerItemList">
        <ul>
            <li>枪手币:<em><asp:Literal ID="ltrlQSB" runat="server"></asp:Literal></em></li>
            <li>博彩币:<em><asp:Literal ID="ltrlCash" runat="server"></asp:Literal></em></li>
            <li>RP:<em><asp:Literal ID="ltrlRP" runat="server"></asp:Literal></em></li>
        </ul>
    </div>
    <div class="ClubBtnGroup">
        <a class="LinkBtn CashBtn" href="../CasinoPortal.aspx" runat="server" id="BtnBet">我要投注</a>
        <asp:LinkButton ID="BtnViewBet" runat="server" CssClass="LinkBtn SelectBtn" Text="查看中奖记录"></asp:LinkButton>
        <asp:LinkButton ID="BtnViewBonus" runat="server" CssClass="LinkBtn SelectBtn" Text="查看盈余情况"></asp:LinkButton>
    </div>
    <div class="Clear">
    </div>
</div>
