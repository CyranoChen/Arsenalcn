<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PortalWorkflowInfo.ascx.cs" Inherits="iArsenal.Web.Control.PortalWorkflowInfo" %>
<asp:Repeater ID="rptrWorkflowInfo" runat="server" OnItemDataBound="rptrWorkflowInfo_ItemDataBound">
    <HeaderTemplate>
        <div class="WorkflowInfo">
        <ul>
    </HeaderTemplate>
    <ItemTemplate>
        <asp:Literal ID="ltrlStateInfo" runat="server"></asp:Literal>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
        </div>
    </FooterTemplate>
</asp:Repeater>