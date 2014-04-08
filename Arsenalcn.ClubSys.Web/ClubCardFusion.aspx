<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="ClubCardFusion.aspx.cs" Inherits="Arsenalcn.ClubSys.Web.ClubCardFusion"
    Title="卡片融合" %>

<%@ Register Src="Control/LeftPanel.ascx" TagName="LeftPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/FieldToolBar.ascx" TagName="FieldToolBar" TagPrefix="uc2" %>
<%@ Register Src="Control/PlayerHeader.ascx" TagName="PlayerHeader" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:LeftPanel ID="ctrlLeftPanel" runat="server" />
    <div id="MainPanel">
        <uc2:FieldToolBar ID="ctrlFieldToolBar" runat="server" />
        <uc4:PlayerHeader ID="ctrlPlayerHeader" runat="server" />
        <asp:Panel ID="pnlSwf" runat="server" CssClass="ClubSys_GetStrip">

            <script type="text/javascript">
                GenSwfObject('ShowCardFusion', 'swf/ShowCardFusion.swf', '500', '250');
            </script>

        </asp:Panel>
    </div>
</asp:Content>
