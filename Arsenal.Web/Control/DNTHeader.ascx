<%@ Control Language="C#" CodeBehind="DNTHeader.ascx.cs" Inherits="Arsenal.Web.Control.DNTHeader" %>
<div id="header">
    <h2>
        <a href="http://www.arsenalcn.com/" title="阿森纳中国官方球迷会">
            <img src="http://www.arsenalcn.com/arsenalcn-logo.png" alt="阿森纳中国官方球迷会" />
        </a>
        <a href="#">
            <img src="http://bbs.arsenalcn.com/images/qrcode-arsenalcn.png" alt="OfficialArsenalCN">
        </a>
    </h2>
    <asp:PlaceHolder ID="phAnonymous" runat="server">
        <span class="avataonline">
            <asp:HyperLink ID="hlLogin" runat="server">登录</asp:HyperLink>
            | 
            <a href="http://bbs.arsenalcn.com/register.aspx">注册</a>
        </span>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phAthenticated" runat="server">
        <span class="avataonline">欢迎: <cite>
            <asp:Literal ID="ltrlUserName" runat="server"></asp:Literal></cite> | 
            <asp:LinkButton ID="btnLogout" runat="server" OnClick="btnLogout_Click">退出</asp:LinkButton>
        </span>
    </asp:PlaceHolder>
    <div class="Clear">
    </div>
</div>
<div id="menubar">
    <ul id="menu" class="cl">
        <li>
            <a href="http://bbs.arsenalcn.com/plugin/acnclub/">球会</a>
        </li>
        <li>
            <a href="http://bbs.arsenalcn.com/plugin/acncasino/">博彩</a>
        </li>
        <li>
            <a href="http://arsenalcn.taobao.com" target="_blank">商城</a>
        </li>
        <li>
            <a href="http://e.weibo.com/arsenalcn/" target="_blank">微博</a>
        </li>
        <li>
            <a href="http://bbs.arsenalcn.com/showuser.aspx">会员</a>
        </li>
        <li>
            <a href="http://bbs.arsenalcn.com/search.aspx" target="_blank">搜索</a>
        </li>
        <li>
            <a href="http://bbs.arsenalcn.com/help.aspx">帮助</a>
        </li>
    </ul>
</div>
<div id="nav">
    <div id="sitepath">
        <asp:Literal ID="ltrlTitle" runat="server"></asp:Literal>
    </div>
    <div id="stats">
        <script src="http://s22.cnzz.com/stat.php?id=4134306&web_id=4134306&online=1&show=line"
            type="text/javascript"></script>
    </div>
    <div class="Clear">
    </div>
</div>
