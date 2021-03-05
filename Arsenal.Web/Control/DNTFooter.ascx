<%@ Control Language="C#" CodeBehind="DNTFooter.ascx.cs" Inherits="Arsenal.Web.Control.DNTFooter" %>
<%@ OutputCache Duration="3600" VaryByParam="none" %>
<%@ Import Namespace="Arsenal.Service" %>
<div id="footer">
    <div id="footlinks">
        <p>
            <a href="http://www.arsenalcn.com" target="_blank">
                <%= ConfigGlobal_Arsenal.Assembly.Trademark %>
            </a> - 
            <span class="scrolltop" onclick="window.scrollTo(0, 0);">TOP</span>
        </p>
        <p>
            <a href="mailto:webmaster@arsenalcn.com" target="_blank"><%= ConfigGlobal_Arsenal.Assembly.Configuration %></a>
        </p>
    </div>
    <img alt="ACN Logo" title="<%= ConfigGlobal_Arsenal.Assembly.Company %>" src="/App_Themes/arsenalcn/images/ACN_Logo.gif" />
    <asp:Literal ID="ltrlCopyright" runat="server"></asp:Literal>
    <asp:Literal ID="ltrlDebugInfo" runat="server"></asp:Literal>
</div>
