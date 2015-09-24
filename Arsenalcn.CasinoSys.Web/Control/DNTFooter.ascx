<%@ Control Language="C#" CodeBehind="DNTFooter.ascx.cs" Inherits="Arsenalcn.CasinoSys.Web.Control.DntFooter" %>
<%@ OutputCache Duration="3600" VaryByParam="none" %>
<div id="footer">
    <div id="footlinks">
        <p>
            <a href="http://www.arsenalcn.com" target="_blank">阿森纳中国官方球迷会</a> - 沪ICP备12045527号
            - <span class="scrolltop" onclick="window.scrollTo(0,0);">TOP</span>
        </p>
        <p>
            <script src="http://s117.cnzz.com/stat.php?id=1267977&web_id=1267977&show=pic1" type="text/javascript"
                charset="gb2312"></script>
            <a href="mailto:webmaster@arsenalcn.com" target="_blank">webmaster@arsenalcn.com</a>
            - <a href="/archiver/index.aspx" target="_blank">简洁版本</a>
        </p>
    </div>
    <img alt="ACN Logo" title="Powered by <%= PluginName %>" src="/App_Themes/arsenalcn/images/ACN_Logo.gif" />
    <p id="copyright">
        Powered by <a href="default.aspx" title="<%= PluginName %> <%=PluginVersion %> (.NET Framework 2.0)">
            <%= PluginName %></a> <em>
                <%=PluginVersion %></em> &copy; 2003-2014 <a href="http://www.arsenalcn.com" target="_blank">
                    ArsenalCN.com</a>
    </p>
    <p id="debuginfo">
        Discuz!NT v3.6.711 (Licensed) &copy; 2001-2012 Comsenz Inc.
    </p>
</div>
