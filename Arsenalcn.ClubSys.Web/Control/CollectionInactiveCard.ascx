<%@ Control Language="C#" CodeBehind="CollectionInactiveCard.ascx.cs" Inherits="Arsenalcn.ClubSys.Web.Control.CollectionInactiveCard"
    EnableViewState="false" %>
<div id="CollectionInfo" class="ClubSys_CollectionInfo">
    <asp:Repeater ID="rptCard" runat="server" OnItemDataBound="rptCard_ItemDataBound">
        <HeaderTemplate>
            <div class="ClubSys_Tip Clear">
                <asp:Label ID="lblTip" runat="server" Text="您需要一套<em>LV5</em>的球衣、球裤、球袜来激活一张球星卡，激活后<em>LV-5</em>。"></asp:Label>
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <div class="DataItem" title="PlayerInactiveCard">
                <asp:Label ID="lblPlayerInactiveCardID" CssClass="SwfID" runat="server"></asp:Label>
                <asp:Label ID="lblPlayerInactiveCardPath" CssClass="SwfSrc" runat="server"></asp:Label>
                <asp:LinkButton ID="btnActive" runat="server" CssClass="BtnActive" ToolTip="点击激活"></asp:LinkButton>
            </div>
        </ItemTemplate>
        <FooterTemplate>
            <div id="DataPlaceHolder"></div>
            <div id="DataPager" class="DataPager"></div>
        </FooterTemplate>
    </asp:Repeater>
</div>
