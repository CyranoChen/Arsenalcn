<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="iArsenalHeader.ascx.cs"
    Inherits="iArsenal.Web.Control.iArsenalHeader" %>
<div id="header">
    <h1>
        <a href="http://www.arsenalcn.com/">
            <img src="uploadfiles/logo_ArsenalChina.png" alt="阿森纳中国官方球迷会" /></a>
    </h1>
    <div id="userPanel">
        <h2 onclick="window.location.href='default.aspx'"></h2>
        <asp:Panel ID="pnlLoginUser" CssClass="UserInfo" runat="server">
            <asp:Label ID="lblUserInfo" runat="server"></asp:Label> - 
            <a href="http://bbs.arsenalcn.com/usercpinbox.aspx" target="_blank">短消息</a> - 
            <a href="iArsenalMemberRegister.aspx">会员中心</a> - 
            <asp:Literal ID="ltrlAdminConfig" runat="server"></asp:Literal>
            <asp:LinkButton ID="btnLogout" runat="server" OnClick="btnLogout_Click">退出</asp:LinkButton>
        </asp:Panel>
        <asp:Panel ID="pnlAnonymousUser" CssClass="UserInfo" runat="server">
            <asp:HyperLink ID="hlLogin" runat="server">登录</asp:HyperLink>
            - <a href="http://bbs.arsenalcn.com/register.aspx">注册</a> - <a href="#">帮助中心</a>
        </asp:Panel>
    </div>
</div>
<div id="menubar">
    <ul>
        <li class="nol"><a href="default.aspx">首页</a></li>
        <li><a href="iArsenalOrder_ReplicaKit.aspx?Type=Home">2015/16赛季主场球衣预定</a></li>
        <li style="display: none"><a href="iArsenalOrder_ReplicaKit.aspx?Type=Away">1415客场球衣</a></li>
        <li style="display: none"><a href="iArsenalOrder_ReplicaKit.aspx?Type=Cup">1415杯赛球衣</a></li>
        <li style="display: none"><a href="iArsenalOrder_LondonTravel.aspx">伦敦行</a></li>
        <li><a href="iArsenalOrder_AsiaTrophy2015.aspx">2015英超亚洲杯观赛团</a></li>
        <li><a href="iArsenalOrder_MatchList.aspx">阿森纳主场球票预定</a></li>
        <li><a href="iArsenalOrder_ArsenalDirect.aspx">官方纪念品团购</a></li>
        <li class="nor"><a href="iArsenalOrder.aspx">订单查询</a></li>
    </ul>
</div>
