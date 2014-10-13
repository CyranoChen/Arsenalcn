﻿<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" CodeBehind="ArsenalPortal.aspx.cs"
    Inherits="Arsenal.Web.ArsenalPortal" Title="阿森纳中国官方球迷会" Theme="Arsenalcn" %>

<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <div id="MainPanel" style="width: 100%">
        <div class="FieldToolBar">
            <asp:Literal ID="ltrlPluginName" runat="server"></asp:Literal>
            <!-- 如果是管理员显示后台管理 -->
            <asp:Panel ID="pnlAdmin" runat="server" CssClass="HeaderBtnBar">
                <a href="AdminConfig.aspx">后台管理</a>
            </asp:Panel>
        </div>
        <div><span>系统已关闭。请联系管理员。</span></div>
    </div>
    <div class="Clear">
    </div>
</asp:Content>