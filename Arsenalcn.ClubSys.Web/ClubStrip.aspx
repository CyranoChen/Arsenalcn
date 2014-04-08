<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" CodeBehind="ClubStrip.aspx.cs"
    Inherits="Arsenalcn.ClubSys.Web.ClubStrip" Title="{0} 球员装备" EnableViewState="false" %>

<%@ Import Namespace="Arsenalcn.ClubSys.DataAccess" %>
<%@ Register Src="Control/LeftPanel.ascx" TagName="LeftPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/FieldToolBar.ascx" TagName="FieldToolBar" TagPrefix="uc2" %>
<%@ Register Src="Control/MenuTabBar.ascx" TagName="MenuTabBar" TagPrefix="uc3" %>
<%@ Register Src="Control/ClubSysHeader.ascx" TagName="ClubSysHeader" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:LeftPanel ID="ctrlLeftPanel" runat="server" />
    <div id="MainPanel">
        <uc2:FieldToolBar ID="ctrlFieldToolBar" runat="server" />
        <uc3:MenuTabBar ID="ctrlMenuTabBar" runat="server" />
        <uc4:ClubSysHeader ID="ctrlClubSysHeader" runat="server" />
        <div class="FunctionBar">
            <div class="DivFloatLeft">
                <asp:Literal ID="ltlClubBingoStrip" runat="server"></asp:Literal>
            </div>
            <div class="DivFloatRight">
                <asp:Literal ID="ltlClubStripCount" runat="server"></asp:Literal>
            </div>
            <div class="Clear">
            </div>
        </div>
        <asp:GridView ID="gvClubStrip" runat="server" PageSize="20" OnPageIndexChanging="gvClubStrip_PageIndexChanging">
            <Columns>
                <asp:BoundField HeaderText="用户名" DataField="UserName" />
                <asp:BoundField HeaderText="装备信息" DataField="AdditionalData" HtmlEncode="false" />
                <asp:BoundField HeaderText="获取时间" DataField="ActionDate" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
