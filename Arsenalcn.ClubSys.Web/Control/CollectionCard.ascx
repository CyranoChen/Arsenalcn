<%@ Control Language="C#" CodeBehind="CollectionCard.ascx.cs" Inherits="Arsenalcn.ClubSys.Web.Control.CollectionCard"
EnableViewState="false" %>
<div class="FunctionBar">
    <div class="DivFloatLeft">
        <asp:DropDownList ID="ddlCardOrder" runat="server" AutoPostBack="true">
            <asp:ListItem Text="按获得时间排序" Value="GainDate DESC" Selected="True"></asp:ListItem>
            <asp:ListItem Text="按球员号码排序" Value="SquadNumber"></asp:ListItem>
            <asp:ListItem Text="按激活时间排序" Value="ActiveDate DESC"></asp:ListItem>
            <asp:ListItem Text="按现役球员排序" Value="Legend, SquadNumber"></asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class="Clear">
    </div>
</div>
<div id="CollectionInfo" class="ClubSys_CollectionInfo">
    <asp:Repeater ID="rptCard" runat="server" OnItemCommand="rptCard_ItemCommand" OnItemDataBound="rptCard_ItemDataBound">
        <HeaderTemplate>
            <div class="Clear" style="border-bottom: 1px solid #000;">
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <div class="DataItem" title="PlayerCard">
                <asp:Label ID="lblPlayerCardID" CssClass="SwfID" runat="server"></asp:Label>
                <asp:Label ID="lblPlayerCardPath" CssClass="SwfSrc" runat="server"></asp:Label>
                <asp:Label ID="lblSetCurrent" runat="server" ToolTip="已使用" CssClass="CurrentStrip"></asp:Label>
                <asp:LinkButton ID="btnSetCurrent" runat="server" ToolTip="点击使用" CommandName="SetCurrent"
                                CssClass="BtnSetCurrent">
                </asp:LinkButton>
            </div>
        </ItemTemplate>
        <FooterTemplate>
            <div id="DataPlaceHolder"></div>
            <div id="DataPager" class="DataPager"></div>
        </FooterTemplate>
    </asp:Repeater>
</div>