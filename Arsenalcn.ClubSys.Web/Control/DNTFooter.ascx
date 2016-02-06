<%@ Control Language="C#" CodeBehind="DNTFooter.ascx.cs" Inherits="Arsenalcn.ClubSys.Web.Control.DNTFooter" %>
<%@ OutputCache Duration="3600" VaryByParam="none" %>
<div id="footer">
    <div id="footlinks">
        <p>
            <a href="http://www.arsenalcn.com" target="_blank">ArsenalCN</a> - 沪ICP备12045527号
            - <span class="scrolltop" onclick="window.scrollTo(0, 0);">TOP</span>
        </p>
        <p>
            <script src="http://s117.cnzz.com/stat.php?id=1267977&web_id=1267977&show=pic1" type="text/javascript"
                    charset="gb2312"></script>
            <a href="mailto:webmaster@arsenalcn.com" target="_blank">webmaster@arsenalcn.com</a>
            - <a href="/archiver/index.aspx" target="_blank">简洁版本</a>
        </p>
    </div>
    <img alt="ACN Logo" title="Powered by <%= pluginName %>" src="/App_Themes/arsenalcn/images/ACN_Logo.gif"/>
    <p id="copyright">
        Powered by <a href="default.aspx" title="<%= pluginName %> <%= pluginVersion %> (.NET Framework 2.0)">
            <%= pluginName %>
        </a> <em>
            <%= pluginVersion %>
        </em> &copy; 2009 - 2015 <a href="http://www.arsenalcn.com" target="_blank">
            ArsenalCN.com
        </a>
    </p>
    <p id="debuginfo">
        Arsenal China Official Supporters Club &copy; 2003 - 2016 Willing co., ltd.
    </p>
</div>