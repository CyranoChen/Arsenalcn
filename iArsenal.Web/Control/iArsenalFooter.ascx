<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="iArsenalFooter.ascx.cs"
    Inherits="iArsenal.Web.Control.iArsenalFooter" %>
<%@ OutputCache Duration="3600" VaryByParam="none" %>
<div id="footer">
    <div id="footlinks">
        <p>
            <a href="http://www.arsenalcn.com" target="_blank">阿森纳中国官方球迷会</a> - 沪ICP备12045527号-1
            - <span class="scrolltop" onclick="window.scrollTo(0,0);">TOP</span>
        </p>
        <p>
            <script src="http://s22.cnzz.com/stat.php?id=4134306&web_id=4134306&show=pic1" type="text/javascript"
                charset="gb2312"></script>
            <a href="mailto:webmaster@arsenalcn.com" target="_blank">webmaster@arsenalcn.com</a>
        </p>
    </div>
    <img alt="ACN Logo" title="Powered by <%= pluginName %>" src="/App_Themes/arsenalcn/images/ACN_Logo.gif" />
    <p id="copyright">
        Powered by <a href="default.aspx" title="<%= pluginName %> <%=pluginVersion %> (.NET Framework 4.0)">
            <%= pluginName %></a> <em>
                <%=pluginVersion %></em> &copy; 2012 <a href="http://www.iArsenal.cn" target="_blank">
                    iArsenal.cn</a>
    </p>
    <p id="debuginfo">
        Arsenal China Official Supporters Club &copy; 2012 Willing co.,Ltd.
    </p>
</div>
