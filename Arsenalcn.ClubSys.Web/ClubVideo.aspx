<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" CodeBehind="ClubVideo.aspx.cs"
    Inherits="Arsenalcn.ClubSys.Web.ClubVideo" Title="{0} 视频名人堂" EnableViewState="false" %>

<%@ Import Namespace="Arsenalcn.ClubSys.Service" %>
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
                <asp:DropDownList ID="ddlGoalRank" runat="server" AutoPostBack="true">
                    <asp:ListItem Text="--进球星级--" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="★★★★★" Value="5" Enabled="false"></asp:ListItem>
                    <asp:ListItem Text="★★★★" Value="4" Enabled="false"></asp:ListItem>
                    <asp:ListItem Text="★★★" Value="3"></asp:ListItem>
                    <asp:ListItem Text="★★" Value="2"></asp:ListItem>
                    <asp:ListItem Text="★" Value="1"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="DivFloatRight">
                <asp:Literal ID="ltlVideoCount" runat="server"></asp:Literal>
            </div>
            <div class="Clear">
            </div>
        </div>
        <asp:GridView ID="gvVideo" runat="server" OnPageIndexChanging="gvVideo_PageIndexChanging"
            OnRowDataBound="gvVideo_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="视频卡">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlVideo" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:HyperLinkField HeaderText="最先获得" DataTextField="UserName" Target="_blank" DataNavigateUrlFields="UserID"
                    DataNavigateUrlFormatString="MyCollection.aspx?type=Video&userid={0}" ControlStyle-CssClass="StrongLink" />
                <asp:BoundField HeaderText="获得时间" DataField="ActiveDate" DataFormatString="<em>{0:yyyy-MM-dd HH:mm:ss}</em>"
                    HtmlEncode="false" />
                <asp:BoundField HeaderText="球员名" DataField="GoalPlayerName" />
                <asp:TemplateField HeaderText="视频等级">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlGoalRankInfo" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="查看视频">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnSwfView" ToolTip="点击播放视频卡" Text="播放" runat="server" CssClass="LinkBtn SelectBtn"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
