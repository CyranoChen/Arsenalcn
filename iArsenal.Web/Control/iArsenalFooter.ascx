﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="iArsenalFooter.ascx.cs" Inherits="iArsenal.Web.Control.iArsenalFooter" %>
<%@ OutputCache Duration="3600" VaryByParam="none" %>
<%@ Import Namespace="iArsenal.Service" %>
<div id="footer">
    <div id="footlinks">
        <p>
            <a href="http://www.arsenalcn.com" target="_blank">
                <%= ConfigGlobal.Assembly.Trademark %>
            </a>- 
            <a href="http://www.beian.miit.gov.cn/" target="_blank"><%= ConfigGlobal.Assembly.Description %></a> 
            - <span class="scrolltop" onclick="window.scrollTo(0, 0);">TOP</span>
        </p>
        <p>
            <a href="mailto:webmaster@arsenalcn.com" target="_blank"><%= ConfigGlobal.Assembly.Configuration %></a>
        </p>
    </div>
    <img alt="ACN Logo" title="<%= ConfigGlobal.Assembly.Company %>" src="/App_Themes/arsenalcn/images/ACN_Logo.gif" />
    <asp:Literal ID="ltrlCopyright" runat="server"></asp:Literal>
    <asp:Literal ID="ltrlDebugInfo" runat="server"></asp:Literal>
</div>
