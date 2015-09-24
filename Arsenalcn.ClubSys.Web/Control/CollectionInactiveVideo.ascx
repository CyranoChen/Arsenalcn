<%@ Control Language="C#" CodeBehind="CollectionInactiveVideo.ascx.cs" Inherits="Arsenalcn.ClubSys.Web.Control.CollectionInactiveVideo"
    EnableViewState="false" %>
<div id="CollectionInfo" class="ClubSys_CollectionInfo">
    <asp:Repeater ID="rptVideo" runat="server" OnItemDataBound="rptVideo_ItemDataBound">
        <HeaderTemplate>
            <div class="ClubSys_Tip Clear">
                <asp:Label ID="lblTip" runat="server" Text="您需要一套<em>LV5</em>的球衣、球裤、球袜来激活一张视频卡，激活后<em>LV-5</em>。"></asp:Label>
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <div class="DataItem" title="PlayerInactiveVideo">
                <asp:Label ID="lblPlayerInactiveVideoID" CssClass="SwfID" runat="server"></asp:Label>
                <asp:Label ID="lblPlayerInactiveVideoPath" CssClass="SwfSrc" runat="server" Text="swf/PlayerCard.swf?XMLURL=Legend"></asp:Label>
                <asp:LinkButton ID="btnActive" runat="server" CssClass="BtnActive" ToolTip="点击激活"></asp:LinkButton>
            </div>
        </ItemTemplate>
        <FooterTemplate>
            <div id="DataPlaceHolder"></div>
            <div id="DataPager" class="DataPager"></div>
        </FooterTemplate>
    </asp:Repeater>
</div>
