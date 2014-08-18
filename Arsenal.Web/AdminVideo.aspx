<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="AdminVideo.aspx.cs" Inherits="Arsenal.Web.AdminVideo" Title="后台管理 视频管理" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<%@ Register Src="Control/CustomPagerInfo.ascx" TagName="CustomPagerInfo" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:AdminPanel ID="pnlAdmin" runat="server" />
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server" />
        <div class="FunctionBar">
            <div class="DivFloatLeft">
                <asp:DropDownList ID="ddlGoalYear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlGoalYear_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:DropDownList ID="ddlGoalRank" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlGoalRank_SelectedIndexChanged">
                    <asp:ListItem Text="--进球星级--" Value="" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="★★★★★" Value="5"></asp:ListItem>
                    <asp:ListItem Text="★★★★" Value="4"></asp:ListItem>
                    <asp:ListItem Text="★★★" Value="3"></asp:ListItem>
                    <asp:ListItem Text="★★" Value="2"></asp:ListItem>
                    <asp:ListItem Text="★" Value="1"></asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="ddlTeamworkRank" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTeamworkRank_SelectedIndexChanged">
                    <asp:ListItem Text="--配合星级--" Value="" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="★★★★★" Value="5"></asp:ListItem>
                    <asp:ListItem Text="★★★★" Value="4"></asp:ListItem>
                    <asp:ListItem Text="★★★" Value="3"></asp:ListItem>
                    <asp:ListItem Text="★★" Value="2"></asp:ListItem>
                    <asp:ListItem Text="★" Value="1"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="DivFloatRight">
                <a href="AdminVideoView.aspx" class="LinkBtn">添加新视频</a>
                <asp:LinkButton ID="btnRefreshCache" runat="server" Text="更新缓存" CssClass="LinkBtn"
                    OnClick="btnRefreshCache_Click" />
            </div>
            <div class="Clear">
                <uc3:CustomPagerInfo ID="ctrlCustomPagerInfo" runat="server" />
            </div>
        </div>
        <asp:GridView ID="gvVideo" runat="server" DataKeyNames="VideoGuid" OnPageIndexChanging="gvVideo_PageIndexChanging"
            PageSize="20" OnSelectedIndexChanged="gvVideo_SelectedIndexChanged" OnRowDataBound="gvVideo_RowDataBound">
            <Columns>
                <asp:BoundField DataField="VideoGuid" Visible="false" />
                <asp:BoundField DataField="GoalYear" HeaderText="年份" ControlStyle-CssClass="TextBox"
                    ControlStyle-Width="30px" />
                <asp:BoundField DataField="GoalPlayerName" HeaderText="进球球员" ReadOnly="true" />
                <asp:BoundField DataField="AssistPlayerName" HeaderText="助攻球员" ReadOnly="true" NullDisplayText="/" />
                <asp:TemplateField HeaderText="比赛对阵">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlMatchOpponentInfo" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:HyperLinkField DataTextField="VideoType" HeaderText="文件路径" DataTextFormatString="点击浏览视频"
                    DataNavigateUrlFields="VideoFilePath" Target="_blank" />
                <asp:BoundField DataField="GoalRank" DataFormatString="<em>{0}</em>" HtmlEncodeFormatString="false"
                    HeaderText="G" ControlStyle-CssClass="TextBox" ControlStyle-Width="10px" />
                <asp:BoundField DataField="TeamworkRank" DataFormatString="<em>{0}</em>" HtmlEncodeFormatString="false"
                    HeaderText="T" ControlStyle-CssClass="TextBox" ControlStyle-Width="10px" />
                <asp:BoundField DataField="VideoLength" HeaderText="长度" ControlStyle-CssClass="TextBox"
                    ControlStyle-Width="30px" />
                <asp:CommandField ShowEditButton="false" ShowSelectButton="true" ShowDeleteButton="false"
                    HeaderText="操作" EditText="修改" UpdateText="保存" CancelText="取消" SelectText="详细"
                    DeleteText="删除" ControlStyle-CssClass="LinkBtn" ItemStyle-CssClass="BtnColumn" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
