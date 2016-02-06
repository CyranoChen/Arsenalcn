<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="iArsenalFooter.ascx.cs" Inherits="iArsenal.Web.Control.iArsenalFooter" %>
<%@ OutputCache Duration="3600" VaryByParam="none" %>
<%@ Import Namespace="iArsenal.Service" %>
<div id="footer">
    <div id="footlinks">
        <p>
            <a href="http://www.arsenalcn.com" target="_blank">
                <%= ConfigGlobal.Assembly.Trademark %>
            </a>- <%= ConfigGlobal.Assembly.Description %>
            - <span class="scrolltop" onclick="window.scrollTo(0, 0);">TOP</span>
        </p>
        <p>
            <script src="http://s22.cnzz.com/stat.php?id=4134306&web_id=4134306&show=pic1" type="text/javascript"
                    charset="gb2312"></script>
            <a href="mailto:webmaster@arsenalcn.com" target="_blank"><%= ConfigGlobal.Assembly.Configuration %></a>
        </p>
    </div>
    <img alt="ACN Logo" title="<%= ConfigGlobal.Assembly.Company %>" src="/App_Themes/arsenalcn/images/ACN_Logo.gif"/>
    <p id="copyright">
        Powered by <a href="default.aspx" title="<%= ConfigGlobal.Assembly.Title %> <%= ConfigGlobal.Assembly.Version %> (.NET Framework 4.0)">
            <%= ConfigGlobal.Assembly.Title %>
        </a> <em>
            <%= ConfigGlobal.Assembly.FileVersion %>
        </em> <%= ConfigGlobal.Assembly.Copyright %> - <%= DateTime.Now.Year %>
        <a href="http://www.iArsenal.com" target="_blank"><%= ConfigGlobal.Assembly.Product %></a>
    </p>
    <p id="debuginfo">
        <%= ConfigGlobal.Assembly.Company %> &copy; 2003 - <%= DateTime.Now.Year %> Willing co., ltd.
    </p>
</div>