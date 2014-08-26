<%@ Control Language="C#" CodeBehind="DNTFooter.ascx.cs" Inherits="Arsenal.Web.Control.DNTFooter" %>
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
        </p>
    </div>
    <img alt="ACN Logo" title="Powered by <%= pluginName %>" src="/App_Themes/arsenalcn/images/ACN_Logo.gif" />
    <p id="copyright">
        Powered by <a href="default.aspx" title="<%= pluginName %> <%=pluginVersion %> (.NET Framework 4.0)">
            <%= pluginName %></a> <em>
                <%=pluginVersion %></em> &copy; 2014 <a href="http://www.Arsenal.cn" target="_blank">
                    Arsenal.cn</a>
    </p>
    <p id="debuginfo">
        Arsenal China Official Supporters Club &copy; 2012 - 2014 Willing co., ltd.
    </p>
</div>