<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminPanel.ascx.cs"
    Inherits="iArsenal.Web.Control.AdminPanel" %>
<div id="LeftPanel">
    <div class="InfoPanel">
        <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
            <a>后台管理菜单</a></h3>
        <div class="Block">
            <ul>
                <li><a href="AdminConfig.aspx">全局配置</a></li>
                <li><a href="AdminMember.aspx">会员管理</a></li>
                <li><a href="AdminMemberPeriod.aspx">会籍管理</a></li>
                <li><a href="AdminProduct.aspx">商品管理</a></li>
                <li><a href="AdminMatchTicket.aspx">球票管理</a></li>
                <li><a href="AdminOrderItem.aspx">许愿管理</a></li>
                <li><a href="AdminOrder.aspx">订单管理</a></li>
            </ul>
        </div>
    </div>
</div>
