<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" CodeBehind="ClubPortal.aspx.cs"
    Inherits="Arsenalcn.ClubSys.Web.ClubPortal" Title="ACN ClubSys 球会系统" EnableViewState="false" %>

<%@ Register Src="Control/LeftPanel.ascx" TagName="LeftPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/FieldToolBar.ascx" TagName="FieldToolBar" TagPrefix="uc2" %>
<%@ Register Src="Control/PortalClubList.ascx" TagName="PortalClubList" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:LeftPanel ID="ctrlLeftPanel" runat="server" />
    <div id="MainPanel">
        <uc2:FieldToolBar ID="ctrlFieldToolBar" runat="server" />
        <uc3:PortalClubList ID="ClubList" runat="server" />
    </div>
</asp:Content>
