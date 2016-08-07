<%@ Page Language="C#" MasterPageFile="iArsenalMaster.Master" AutoEventWireup="true"
    CodeBehind="iArsenalOrder_Showcase.aspx.cs" Inherits="iArsenal.Web.iArsenalOrder_Showcase" Title="阿森纳官方纪念品本季热推"
    Theme="iArsenal" %>

<%@ Register Src="Control/PortalSitePath.ascx" TagName="PortalSitePath" TagPrefix="uc1" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
    <script type="text/javascript">
        $(function () {
            var $items = $(".showcase .item-image");
            RefreshCart();

            if ($items.length > 0) {
                $.each($items, function (i, entry) {
                    $(entry).click(function () {
                        ItemPlace($(this).find("img").attr("alt"), $(this).parent().find("p:first").attr("title"));

                        RefreshCart();
                    });
                });
            }
        });

        function ItemPlace(code, price) {
            var item = {
                "code": code,
                "price": price,
                "quantity": 1
            };

            if (localStorage && localStorage.cart === "") {
                localStorage.cart = "{ \"items\": [] }";
            } else {
                $("#tbPreview").val("{ \"items\": [] }");
            }

            var json = JSON.parse(localStorage ? localStorage.cart : $("#tbPreview").val());
            json.items.push(item);

            if (localStorage) {
                localStorage.cart = JSON.stringify(json);
            } else {
                $("#tbPreview").val(JSON.stringify(json));
            }
        }

        function RefreshCart() {
            var $cart = $("a.Cart");
            var json = JSON.parse(localStorage ? localStorage.cart : $("#tbPreview").val());

            if (json.items.length > 0) {
                var totalPrice = 0;
                $.each(json.items, function (i, entry) {
                    totalPrice += Number(entry.price);
                });

                $cart.find("em.quanlity").html(json.items.length, "件商品");
                $cart.find("em.price").html("￡" + totalPrice);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <div id="banner" style="height: 250px">
        <a href="http://arsenaldirect.arsenal.com" target="_blank">
            <img src="uploadfiles/banner/banner20160805.png" alt="阿森纳官方纪念品本季热推" />
        </a>
    </div>
    <div id="ACN_Main">
        <uc1:PortalSitePath ID="ucPortalSitePath" runat="server" />
        <div class="FunctionBar">
            <div class="DivFloatLeft">
                <asp:TextBox ID="tbProductName" runat="server" Text="" CssClass="TextBox" Width="200px"></asp:TextBox>
                <asp:DropDownList ID="ddlIsSale" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlIsSale_SelectedIndexChanged">
                    <asp:ListItem Value="" Text="--本季促销--" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="true" Text="促销"></asp:ListItem>
                    <asp:ListItem Value="false" Text="普通"></asp:ListItem>
                </asp:DropDownList>
                <asp:LinkButton ID="btnFilter" runat="server" Text="搜索商品" CssClass="LinkBtn" OnClick="btnFilter_Click"></asp:LinkButton>
                <a href="iArsenalOrder_ArsenalDirect.aspx" class="LinkBtn"><i class="fa fa-hand-pointer-o" aria-hidden="true"></i>直接下单</a>
            </div>
            <div class="DivFloatRight">
                <asp:LinkButton ID="btnCart" runat="server" Text="查看购物车" CssClass="LinkBtn Cart" OnClick="btnCart_Click"></asp:LinkButton>
            </div>
            <div class="Clear">
                <input type="hidden" id="tbPreview" />
            </div>
            <div class="showcase">
                <asp:Repeater ID="rptShowcase" runat="server" OnItemDataBound="rptShowcase_ItemDataBound">
                    <ItemTemplate>
                        <div class="item">
                            <div class="item-image" title="加入购物车">
                                <asp:Image ID="imgItemThumbnail" runat="server" />
                            </div>
                            <asp:Literal ID="ltrlProductName" runat="server"></asp:Literal>
                            <asp:Literal ID="ltrlProductInfo" runat="server"></asp:Literal>
                            <asp:Literal ID="ltrlProductPrice" runat="server"></asp:Literal>
                        </div>
                    </ItemTemplate>
                    <FooterTemplate>
                        <div class="Clear"></div>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>
</asp:Content>
