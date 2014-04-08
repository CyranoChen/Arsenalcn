<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PortalBulkOrderInfo.ascx.cs" Inherits="iArsenal.Web.Control.PortalBulkOrderInfo" %>
<asp:Panel ID="pnlBulkOrderInfo" CssClass="InfoPanel" runat="server">
    <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
        <a>当前活动信息栏</a></h3>
    <div class="Block">
        <p>(1). 活动所属期：<asp:Label ID="lblBulkOrderInfo_Period" runat="server"></asp:Label></p>
        <p>(2). 约定汇率：<asp:Label ID="lblBulkOrderInfo_ExchangeRate" runat="server"></asp:Label></p>
        <p>(3). 统计截止时间：<asp:Label ID="lblBulkOrderInfo_Deadline" runat="server"></asp:Label></p>
        <p>(4). 预计到货时间：<asp:Label ID="lblBulkOrderInfo_ArrivalDate" runat="server"></asp:Label></p>
    </div>
</asp:Panel>
