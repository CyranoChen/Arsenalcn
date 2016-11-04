<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PortalProductQrCode.ascx.cs"
    Inherits="iArsenal.Web.Control.PortalProductQrCode" %>
<asp:Panel ID="pnlQrCode" CssClass="InfoPanel" runat="server">
    <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
        <asp:Literal ID="ltrlQrCodeProvider" runat="server"></asp:Literal>
    </h3>
    <div class="Block" style="text-align: center">
        <asp:Image ID="imgQrCode" runat="server" Width="250px" Height="250px" />
    </div>
</asp:Panel>
