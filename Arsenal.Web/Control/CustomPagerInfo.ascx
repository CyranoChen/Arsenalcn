<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomPagerInfo.ascx.cs"
Inherits="Arsenal.Web.Control.CustomPagerInfo" %>
<div id="DataPagerInfo">
    <asp:Literal ID="ltrlPagerInfo" runat="server"></asp:Literal>
    <asp:TextBox ID="tbPagerIndex" runat="server" CssClass="GotoTxt"></asp:TextBox>
    <asp:LinkButton ID="tbPagerGoto" runat="server" Text="GO" OnClick="tbPagerGoto_Click" CssClass="GotoBtn"></asp:LinkButton>
</div>