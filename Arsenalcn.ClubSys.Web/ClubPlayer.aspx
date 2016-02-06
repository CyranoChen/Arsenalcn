<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" CodeBehind="ClubPlayer.aspx.cs"
Inherits="Arsenalcn.ClubSys.Web.ClubPlayer" Title="{0} 现役球员" EnableViewState="false" %>
<%@ Register Src="Control/LeftPanel.ascx" TagName="LeftPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/FieldToolBar.ascx" TagName="FieldToolBar" TagPrefix="uc2" %>
<%@ Register Src="Control/MenuTabBar.ascx" TagName="MenuTabBar" TagPrefix="uc3" %>
<%@ Register Src="Control/ClubSysHeader.ascx" TagName="ClubSysHeader" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:LeftPanel ID="ctrlLeftPanel" runat="server"/>
    <div id="MainPanel">
        <uc2:FieldToolBar ID="ctrlFieldToolBar" runat="server"/>
        <uc3:MenuTabBar ID="ctrlMenuTabBar" runat="server"/>
        <uc4:ClubSysHeader ID="ctrlClubSysHeader" runat="server"/>
        <div class="FunctionBar">
            <div class="DivFloatLeft">
                <asp:Literal ID="ltlPlayerCount" runat="server"></asp:Literal>
            </div>
            <div class="DivFloatRight">
                <asp:Literal ID="ltlPlayerLv" runat="server"></asp:Literal>
            </div>
            <div class="Clear">
            </div>
        </div>
        <asp:GridView ID="gvPlayers" runat="server" OnPageIndexChanging="gvPlayers_PageIndexChanging"
                      OnRowDataBound="gvPlayers_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="球星卡">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlNum" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="编号" DataField="ID" DataFormatString="<em>{0}</em>" HtmlEncode="false"/>
                <asp:TemplateField HeaderText="用户名">
                    <ItemTemplate>
                        <a href="MyPlayerProfile.aspx?userid=<%#DataBinder.Eval(Container.DataItem, "UserID") %>"
                           target="_blank" class="StrongLink">
                            <%#DataBinder.Eval(Container.DataItem, "UserName") %>
                        </a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="等级">
                    <ItemTemplate>
                        <div class="ClubSys_PlayerLV" style="width: <%#DataBinder.Eval(Container.DataItem, "AdditionalData") %>px;"
                             title="球员等级">
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="球衣">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlShirtLV" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="球裤">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlShortsLV" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="球袜">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlSockLV" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>