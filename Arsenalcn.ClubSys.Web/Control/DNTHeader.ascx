<%@ Control Language="C#" CodeBehind="DNTHeader.ascx.cs" Inherits="Arsenalcn.ClubSys.Web.Control.DNTHeader" %>
<div id="header">
    <h2>
        <a href="/index.aspx" title="阿森纳中国官方球迷会">
            <img src="/App_Themes/arsenalcn/images/logo.png" alt="阿森纳中国官方球迷会" /></a>
    </h2>
    <asp:PlaceHolder ID="phAnonymous" runat="server"><span class="avataonline"><a href="/login.aspx">
        登录</a> | <a href="/register.aspx">注册</a></span> </asp:PlaceHolder>
    <asp:PlaceHolder ID="phAthenticated" runat="server"><span class="avataonline">欢迎: <cite>
        <asp:Literal ID="ltrlUserName" runat="server"></asp:Literal></cite> |
        <asp:HyperLink ID="linkLogout" runat="server" NavigateUrl="/logout.aspx?userkey="
            Text="退出"></asp:HyperLink>
    </span></asp:PlaceHolder>
    <div class="Clear">
    </div>
</div>
<div id="menubar">
    <ul id="menu" class="cl">
        <li><a href="/plugin/acnclub/" class="current">球会</a></li>
        <li><a href="/plugin/acncasino/">博彩</a></li>
        <li><a href="http://arsenalcn.taobao.com" target="_blank">商城</a></li>
        <li><a href="http://www.weibo.com/arsenalcn/" target="_blank">微博</a></li>
        <li><a href="/showalbumlist.aspx">相册</a></li>
        <li><a href="/showuser.aspx">会员</a></li>
        <li><a href="/search.aspx" target="_blank">搜索</a></li>
        <li><a href="/help.aspx">帮助</a> </li>
    </ul>
</div>
<div id="nav">
    <div id="sitepath">
        <asp:Literal ID="ltrlTitle" runat="server"></asp:Literal>
    </div>
    <div id="stats">
        球会:<em><asp:Literal ID="ltrlClubCount" runat="server"></asp:Literal></em>, 会员:<em><asp:Literal
            ID="ltrlUserCount" runat="server"></asp:Literal></em>, 球员:<em><asp:Literal ID="ltrlPlayerCount"
                runat="server"></asp:Literal></em>
    </div>
    <div class="Clear">
    </div>
</div>
