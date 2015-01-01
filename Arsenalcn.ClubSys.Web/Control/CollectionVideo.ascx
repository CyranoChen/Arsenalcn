<%@ Control Language="C#" CodeBehind="CollectionVideo.ascx.cs" Inherits="Arsenalcn.ClubSys.Web.Control.CollectionVideo"
    EnableViewState="false" %>
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
        <asp:DropDownList ID="ddlTeamRank" runat="server" AutoPostBack="true">
            <asp:ListItem Text="--配合星级--" Selected="True"></asp:ListItem>
            <asp:ListItem Text="★★★★★" Value="5"></asp:ListItem>
            <asp:ListItem Text="★★★★" Value="4"></asp:ListItem>
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
<div id="CollectionInfo" class="ClubSys_CollectionInfo">
    <asp:Repeater ID="rptVideo" runat="server" OnItemDataBound="rptVideo_ItemDataBound"
        OnItemCommand="rptVideo_ItemCommand">
        <HeaderTemplate>
            <div class="Clear" style="border-bottom: 1px solid #000;">
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <div class="DataItem" title="PlayerVideo">
                <asp:Label ID="lblPlayerVideoID" CssClass="SwfID" runat="server"></asp:Label>
                <asp:Label ID="lblPlayerVideoPath" CssClass="SwfSrc" runat="server"></asp:Label>
                <asp:Label ID="lblSetCurrent" runat="server" ToolTip="已使用" CssClass="CurrentStrip"></asp:Label>
                <asp:LinkButton ID="btnSetCurrent" ToolTip="点击使用" runat="server" CssClass="BtnSetCurrent"></asp:LinkButton>
                <asp:LinkButton ID="btnSwfView" ToolTip="点击播放" runat="server" CssClass="SwfViewBtn"></asp:LinkButton>
            </div>
        </ItemTemplate>
        <FooterTemplate>
            <div id="DataPlaceHolder"></div>
            <div id="DataPager" class="DataPager"></div>
        </FooterTemplate>
    </asp:Repeater>
</div>
