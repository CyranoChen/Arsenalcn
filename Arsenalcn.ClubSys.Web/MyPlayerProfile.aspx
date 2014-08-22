<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" CodeBehind="MyPlayerProfile.aspx.cs"
    Inherits="Arsenalcn.ClubSys.Web.MyPlayerProfile" Title="我的球员信息" EnableViewState="false" %>

<%@ Import Namespace="Arsenalcn.ClubSys.Service" %>
<%@ Register Src="Control/LeftPanel.ascx" TagName="LeftPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/FieldToolBar.ascx" TagName="FieldToolBar" TagPrefix="uc2" %>
<%@ Register Src="Control/MenuTabBar.ascx" TagName="MenuTabBar" TagPrefix="uc3" %>
<%@ Register Src="Control/PlayerHeader.ascx" TagName="PlayerHeader" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:LeftPanel ID="ctrlLeftPanel" runat="server" />
    <div id="MainPanel">
        <uc2:FieldToolBar ID="ctrlFieldToolBar" runat="server" />
        <uc4:PlayerHeader ID="ctrlPlayerHeader" runat="server" />
        <div class="ClubSys_CollectionInfo">
            <div class="ClubSys_Tip">
                <asp:Label ID="lblClubTip" runat="server"></asp:Label>
            </div>
            <div class="ClubSys_ProfileInfo">
                <asp:LinkButton ID="btnVideoActive" runat="server" CssClass="BtnVideoActive"></asp:LinkButton>
                <asp:LinkButton ID="btnCardActive" runat="server" CssClass="BtnCardActive"></asp:LinkButton>
                <asp:Label ID="lblVideoActiveCount" runat="server" CssClass="VideoActiveCount"></asp:Label>
                <asp:Label ID="lblVideoCount" runat="server" CssClass="VideoCount"></asp:Label>
                <asp:Label ID="lblCardActiveCount" runat="server" CssClass="CardActiveCount"></asp:Label>
                <asp:Label ID="lblCardCount" runat="server" CssClass="CardCount"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>
