﻿<%@ Page Language="C#" MasterPageFile="iArsenalMaster.Master" AutoEventWireup="true"
    CodeBehind="iArsenalOrder_Showcase.aspx.cs" Inherits="iArsenal.Web.iArsenalOrder_Showcase" Title="阿森纳官方纪念品本季热推"
    Theme="iArsenal" %>

<%@ Register Src="Control/PortalSitePath.ascx" TagName="PortalSitePath" TagPrefix="uc1" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
    <script type="text/javascript">
        $(function () {
            var $items = $(".showcase .item-image");

            $.getJSON("ServerGetSession.ashx", function (data, status) {
                if (status === "success" && data != null) {
                    // 只能在登录后，才能看到购物车按钮
                    if (data.result !== "error" && JSON.stringify(data) !== "{}" && data.UserID > 0) {
                        var cart = "cart-" + data.UserID;
                        // 刷新当前用户的购物车状态
                        RefreshCart(cart);

                        // 设置商品图片的加入购物车按钮功能
                        if ($items.length > 0) {
                            $.each($items, function (i, entry) {
                                $(entry).click(function () {
                                    ItemPlace(cart, $(this).find("img").attr("alt"), $(this).parent().find("p:first").attr("title"));

                                    RefreshCart(cart);
                                });
                            });
                        }

                        // 设置清空购物车功能
                        $("a.ResetCart").click(function () {
                            if (localStorage && localStorage[cart] != null) {
                                localStorage.removeItem(cart);
                            } else {
                                $("#tbLocalStorage").val("");
                            }

                            RefreshCart(cart);
                        });

                        //设置结算购物车功能
                        $("a.Cart").click(function () {
                            PlaceCart(cart);
                        });
                    } else {
                        // 设置匿名用户状态
                        $("a.ResetCart").hide();
                        $("a.Cart").hide();
                        if ($items.length > 0) {
                            $.each($items, function (i, entry) {
                                $(entry).click(function () {
                                    window.location.href = $(".BtnLogin").attr("href");
                                });
                            });
                        }
                    }
                }
            });
        });

        // 商品加入购物车
        function ItemPlace(cart, code, price) {
            if (localStorage && localStorage[cart] == null) {
                localStorage[cart] = "{ \"items\": [] }";
            } else {
                $("#tbLocalStorage").val("{ \"items\": [] }");
            }

            var json = JSON.parse(localStorage ? localStorage[cart] : $("#tbLocalStorage").val());

            json.items.push({ "code": code, "price": price });

            if (localStorage) {
                localStorage[cart] = JSON.stringify(json);
            } else {
                $("#tbLocalStorage").val(JSON.stringify(json));
            }
        }

        // 刷新用户购物车状态
        function RefreshCart(cart) {
            var $cart = $("a.Cart");
            var $cartQuanlity = $cart.find("em.quanlity");
            var $cartPrice = $cart.find("em.price");
            var json = {};

            // 取得缓存区的数据
            if (localStorage && localStorage[cart] != null) {
                json = JSON.parse(localStorage[cart]);
            } else if ($("#tbLocalStorage").val() !== "") {
                json = JSON.parse($("#tbLocalStorage").val());
            }

            if (json.items != undefined && json.items.length > 0) {
                var totalPrice = 0;
                // 数量求和
                $.each(json.items, function (i, entry) {
                    totalPrice += Number(entry.price);
                });

                // 刷新状态
                $cartQuanlity.html("【" + json.items.length + "】");
                $cartPrice.html("￡" + totalPrice);
            } else {
                $cartQuanlity.html("");
                $cartPrice.html("");
            }
        }

        // 结算购物车
        function PlaceCart(cart) {
            if (localStorage ? localStorage[cart] != null : $("#tbLocalStorage").val() !== "") {
                if (confirm("查看并结算购物车?\r【您可在后续购物页面中添加或修改纪念品】")) {
                    var json = JSON.parse(localStorage ? localStorage[cart] : $("#tbLocalStorage").val());
                    var items = JSON.stringify(json.items);

                    // 向服务端post购物车数据
                    $.post("ServerPlaceOrder.ashx?OrderType=4", { "items": items }, function (data) {
                        if (data.result !== "error" && JSON.stringify(data) !== "{}" && data.OrderID > 0) {
                            // 清除本地缓存
                            if (localStorage) {
                                localStorage.removeItem(cart);
                            }
                            window.location.href = "iArsenalOrder_ArsenalDirect.aspx?OrderID=" + data.OrderID;
                        } else {
                            alert(data.error_msg);
                        }
                    });
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <div id="banner" style="height: 360px">
        <a href="https://arsenaldirect.arsenal.com/" target="_blank" title="访问英国官方商城">
            <img src="uploadfiles/banner/banner20200802.png" alt="阿森纳英国官方商城纪念品代购服务" />
        </a>
    </div>
    <div id="ACN_Main">
        <uc1:PortalSitePath ID="ucPortalSitePath" runat="server" />
        <div class="FunctionBar">
            <div class="DivFloatLeft">
                <asp:TextBox ID="tbProductName" runat="server" Text="" CssClass="TextBox" Width="200px"></asp:TextBox>
                <asp:DropDownList ID="ddlIsSale" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlIsSale_SelectedIndexChanged">
                    <asp:ListItem Value="" Text="--全部商品--" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="true" Text="本季促销"></asp:ListItem>
                </asp:DropDownList>
                <asp:LinkButton ID="btnFilter" runat="server" Text="搜索商品" CssClass="LinkBtn" OnClick="btnFilter_Click"></asp:LinkButton>
                <a href="iArsenalOrder_ArsenalDirect.aspx" class="LinkBtn"><i class="fa fa-hand-pointer-o" aria-hidden="true"></i>直接下单</a>
            </div>
            <div class="DivFloatRight">
                <a class="LinkBtn Cart">
                    <i class="fa fa-shopping-cart" aria-hidden="true"></i>
                    结算购物车<em class="quanlity"></em><em class="price"></em>
                </a>
                <a class="LinkBtn ResetCart"><i class="fa fa-refresh" aria-hidden="true"></i>清空购物车</a>
            </div>
            <div class="Clear">
                <input type="hidden" id="tbLocalStorage" />
            </div>
            <div class="showcase">
                <asp:Repeater ID="rptShowcase" runat="server" OnItemDataBound="rptShowcase_ItemDataBound">
                    <ItemTemplate>
                        <div class="item" title="加入购物车">
                            <div class="item-image">
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
