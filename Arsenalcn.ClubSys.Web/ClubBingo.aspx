<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" CodeBehind="ClubBingo.aspx.cs"
    Inherits="Arsenalcn.ClubSys.Web.ClubGetStrip" Title="{0} 获取球员装备" EnableViewState="false" %>
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
        <div class="FunctionBar">
            <div class="DivFloatLeft">
                <asp:Label ID="lblGetStripNotAvailable" runat="server"></asp:Label>
                <asp:Label ID="lblGetStripUserInfo" runat="server"></asp:Label>
            </div>
            <div class="DivFloatRight">
                <asp:CheckBox ID="cbGoogleAdvActive" Text="打工模式" ToolTip="通过开启打工模式，使抽取与获取装备完全免费"
                    runat="server" AutoPostBack="true" />
            </div>
            <div class="Clear">
            </div>
        </div>
        <asp:Panel ID="pnlShowGetStrip" runat="server" CssClass="ClubSys_GetStrip">
            <div id="pnlSwf" style="display: <%=DisplaySwf %>">

                <script type="text/javascript">
                    GenSwfObject('ShowGetStrip', 'swf/ShowGetStrip.swf?IsAdv=<%=IsGoogleAdv %>', '500', '250');
                </script>

            </div>
        </asp:Panel>
    </div>
</asp:Content>
