/* Javascript Version iArsenal */
/* Version: 1.7.8 || Date: 2015-01-01 || Author: Cyrano */
/* type="text/javascript" */

$(document).ready(function () {
    var $pageInfo = $("#DataPagerInfo");
    var $gvPager = $(".DataView .Pager > td");

    if ($pageInfo.length > 0) {
        if ($gvPager.length > 0) {
            $pageInfo.appendTo($gvPager);
            $pageInfo.show();
        } else {
            $pageInfo.hide();
        }
    }

    if ($.cookie("leftPanel") != null) {
        $("#LeftPanel").hide();
        $("#MainPanel").width("100%");
        $("#MainPanel .FieldToolBar > div.CtrlLeftPanelExp").attr("class", "CtrlLeftPanelCol");
    } else {
        $("#LeftPanel").show();
        $("#MainPanel").width("78%");
    }
});

//function Logout() {
//    $.post("http://localhost/logout.aspx", { "confirm": "1" }, function (data) {
//        alert(data);
//    });
//}

// AdminOrder.aspx

function BulkOrderClickImpl(obj) {
    var $cblOrderID = obj.find("input:checked");

    if ($cblOrderID.length > 0) {
        var arraySelectedItems = new Array();

        $cblOrderID.each(function (i, entry) {
            arraySelectedItems.push($(entry).next("label").text());
        });

        $.getJSON("ServerBulkOrder.ashx",
            { SelectedOrderIDs: arraySelectedItems.join('|') }, function (data, status, xhr) {
                if (status == "success" && data != null) {
                    if (data.result != "error" && JSON.stringify(data) != "[]") {
                        alert($.format("选择订单共{0}个，其中下单成功{1}个，下单失败{2}个",
                            data.countTotal, data.countSucceed, data.countFailed));

                        window.location.href = window.location.href;
                    }
                } else {
                    alert("调用服务接口失败(BulkOrder)");
                }
            });
    }
    else {
        alert("请选择需要下单的订单");
    }
}


// AdminMemberView.aspx

function NationDataBindImpl(obj) {
    SwitchNationDataControl(obj);

    var $ddlNation = obj.find("select.Nation");
    //var $tbOtherNation = obj.find("input.OtherNation");
    var $ddlRegion1 = obj.find("select#ddlRegion1");
    var $ddlRegion2 = obj.find("select#ddlRegion2");
    var $tbRegion1 = obj.find("input.Region1");
    var $tbRegion2 = obj.find("input.Region2");

    $ddlNation.change(function () {
        SwitchNationDataControl($("#tdRegion"));
    });

    $.getJSON("ServerRegionCheck.ashx", { RegionID: "0" }, function (data, status, xhr) {
        if (status == "success" && data != null) {
            if (data.result != "error" && JSON.stringify(data) != "[]") {
                $ddlRegion1.empty();
                $.each(data, function (entryIndex, entry) {
                    $ddlRegion1.append($("<option>", { value: entry.ItemID }).text(entry.Name));
                });

                if ($.trim($tbRegion1.val()) != "") {
                    $ddlRegion1.val($.trim($tbRegion1.val()));
                    RegionDataBindImpl($.trim($tbRegion1.val()), $ddlRegion2, $.trim($tbRegion2.val()));
                } else {
                    $ddlRegion1.prepend($("<option>", { value: "" }).text("--请选择所在地区--"))
                }

            } else {
                $ddlRegion1.empty();
                $ddlRegion1.hide();
                $ddlRegion2.empty();
                $ddlRegion2.hide();
            }
        } else {
            alert("调用数据接口失败(Region)");
        }
    });

    $ddlRegion1.change(function () {
        $tbRegion1.val($.trim($(this).val()));
        $tbRegion2.val("");
        RegionDataBindImpl($.trim($(this).val()), $ddlRegion2, $.trim($tbRegion2.val()));
    });

    $ddlRegion2.change(function () {
        $tbRegion2.val($.trim($(this).val()));
    });
}

function RegionDataBindImpl(rid, obj, value) {
    $.getJSON("ServerRegionCheck.ashx", { RegionID: rid }, function (data, status, xhr) {
        if (status == "success" && data != null) {
            if (data.result != "error" && JSON.stringify(data) != "[]") {
                obj.empty();

                $.each(data, function (entryIndex, entry) {
                    obj.append($("<option>", { value: entry.ItemID }).text(entry.Name));
                });

                if (value != "") {
                    obj.val(value);

                } else {
                    obj.prepend($("<option>", { value: "" }).text("--请选择所在地区--"))
                }

                obj.show();

            } else {
                obj.empty();
                obj.hide();
            }
        } else {
            alert("调用数据接口失败(Region)");
        }
    });
}

function SwitchNationDataControl(obj) {
    var $ddlNation = obj.find("select.Nation");
    var $tbOtherNation = obj.find("input.OtherNation");
    var $ddlRegion1 = obj.find("select#ddlRegion1");
    var $ddlRegion2 = obj.find("select#ddlRegion2");
    var $tbRegion1 = obj.find("input.Region1");
    var $tbRegion2 = obj.find("input.Region2");

    if ($.trim($ddlNation.val()) == "中国") {
        $tbOtherNation.hide();
        $ddlRegion1.show();

        if ($.trim($tbRegion2.val()) != "") {
            $ddlRegion2.show();
        }
        else {
            $ddlRegion2.hide();
        }

    } else if ($.trim($ddlNation.val()) == "其他") {
        $ddlRegion1.hide();
        $ddlRegion2.hide();
        $tbOtherNation.show();
    } else {
        $ddlRegion1.hide();
        $ddlRegion2.hide();
        $tbOtherNation.hide();
    }
}

// AdminMemberView.aspx

function AcnCheck() {
    var $tbAcnID = $("#tdAcnInfo .AcnID");
    var $tbAcnName = $("#tdAcnInfo .AcnName");
    var $tbAcnSessionKey = $("#tdAcnInfo .AcnSessionKey");
    var $btnSubmit = $(".FooterBtnBar .SubmitBtn");
    $btnSubmit.prop("disabled", true);

    if ($.trim($tbAcnID.val()) != "") {
        $.getJSON("ServerAcnCheck.ashx", { AcnID: $.trim($tbAcnID.val()), SessionKey: $.trim($tbAcnSessionKey.val()) }, function (data, status, xhr) {
            if (status == "success" && data != null) {
                if (data.result != "error" && JSON.stringify(data) != "{}") {
                    $tbAcnName.val(data.username);
                    $btnSubmit.prop("disabled", false);
                } else {
                    alert(data.error_msg);
                    $btnSubmit.prop("disabled", true);
                }
            } else {
                alert("调用数据接口失败(AcnInfo)");
            }
        });
    } else {
        alert("请输入本会员的AcnID");
    }
}

// AdminOrderItemView.aspx

function ProductCheck() {
    var $tbProductCode = $(".ProductInfo .ProductCode");
    var $tbProductGuid = $(".ProductInfo .ProductGuid");
    var $tbProductName = $(".ProductInfo .ProductName");
    var $tbProductUnitPrice = $(".ProductInfo .ProductUnitPrice");
    var $tbProductQuantity = $(".ProductInfo .ProductQuantity");
    var $lblProductTotalPrice = $(".ProductInfo .ProductTotalPrice");
    var $btnSubmit = $(".FooterBtnBar .SubmitBtn");
    $btnSubmit.prop("disabled", true);

    if ($.trim($tbProductCode.val()) != "") {
        $.getJSON("ServerProductCheck.ashx", { ProductCode: $.trim($tbProductCode.val()) }, function (data, status, xhr) {
            if (status == "success" && data != null) {
                if (data.result != "error" && JSON.stringify(data) != "{}") {
                    $tbProductCode.val(data.Code);
                    $tbProductGuid.val(data.ProductGuid);
                    $tbProductName.val($.format("{0} ({1})", data.DisplayName, data.Name));
                    $tbProductUnitPrice.val(data.PriceCNY.toLocaleString());

                    var totalPrice = Number(data.PriceCNY) * Number($.trim($tbProductQuantity.val()));
                    $lblProductTotalPrice.text(totalPrice.toLocaleString());

                    $btnSubmit.prop("disabled", false);
                } else {
                    alert(data.error_msg);
                    $btnSubmit.prop("disabled", true);
                }
            } else {
                alert("调用数据接口失败(Product)");
            }
        });
    } else {
        alert("请输入商品编号");
    }
}

// iArsenalOrder_ReplicaKit.aspx

function ProductCheckByID(pGuid) {
    $pnlProductImage = $("#pnlProductImage");
    $img = $("#pnlProductImage img");

    if (pGuid != "") {
        $.getJSON("ServerProductCheck.ashx", { ProductGuid: pGuid }, function (data, status, xhr) {
            if (status == "success" && data != null) {
                if (data.result != "error" && JSON.stringify(data) != "{}") {
                    $img.attr("src", data.ImageURL);
                    $img.attr("alt", data.DisplayName);
                    $pnlProductImage.show();
                } else {
                    alert(data.error_msg);
                }
            } else {
                alert("调用数据接口失败(ReplicaKit)");
            }
        });
    } else {
        //alert("请输入商品ID");
        $pnlProductImage.hide();
    }
}

// AdminOrderView.aspx, AdminOrderItemView.aspx

function MemberCheck() {
    var $tbMemberID = $("#tdMemberInfo .MemberID");
    var $tbMemberName = $("#tdMemberInfo .MemberName");
    var $btnSubmit = $(".FooterBtnBar .SubmitBtn");
    $btnSubmit.prop("disabled", true);

    if ($.trim($tbMemberID.val()) != "") {
        $.getJSON("ServerMemberCheck.ashx", { MemberID: $.trim($tbMemberID.val()) }, function (data, status, xhr) {
            if (status == "success" && data != null) {
                if (data.result != "error" && JSON.stringify(data) != "{}") {
                    $tbMemberName.val(data.Name);
                    $btnSubmit.prop("disabled", false);
                } else {
                    alert(data.error_msg);
                    $btnSubmit.prop("disabled", true);
                }
            } else {
                alert("调用数据接口失败(Member)");
            }
        });
    } else {
        alert("请输入本实名会员ID");
    }
}

// iArsenalOrder_ArsenalDirect.aspx

function InsertWishItem(obj) {
    var $trWishItem = obj.find("tr.WishItem").first().clone();
    var $trWishRemark = obj.find("tr.WishRemark");

    $trWishItem.find("input.TextBox").each(function () { $(this).val(""); });
    $trWishItem.find("span").text("");

    AutoCompleteProductImpl($trWishItem);
    AutoCalculateProductImpl($trWishItem);

    obj.append($trWishItem);
    obj.append($trWishRemark);
}

function DeleteWishItem(obj) {
    var $trWishItem = $("tbody.ArsenalDirect_WishList tr.WishItem");
    var $tbWishProductCode = obj.find("input.ProductCode");

    if ($trWishItem.length > 1) {
        if ($.trim($tbWishProductCode.val()) != "") {
            if (confirm($.format("移除当前许愿商品【{0}】?", $.trim($tbWishProductCode.val())))) { obj.remove(); }
        }
        else { obj.remove(); }
    }
}

function AutoCompleteProductImpl(obj) {
    $tbWishProductCode = obj.find("input.ProductCode");

    $tbWishProductCode.autocomplete({
        source: cacheProductCodeList,
        minLength: 2,
        autoFocus: true,
        select: function (event, ui) {
            if ($.trim(ui.item.value) != "") {
                $.getJSON("ServerProductCheck.ashx", { ProductCode: $.trim(ui.item.value) }, function (data, status, xhr) {
                    if (status == "success" && data != null) {
                        AutoFillProductImpl(obj, data);
                    } else {
                        alert("调用数据接口失败(Product)");
                    }
                });
            }
        }
    });

    $tbWishProductCode.change(function () {
        if ($.trim($(this).val()) != "") {
            $.getJSON("ServerProductCheck.ashx", { ProductCode: $.trim($(this).val()) }, function (data, status, xhr) {
                if (status == "success" && data != null) {
                    AutoFillProductImpl(obj, data);
                } else {
                    alert("调用数据接口失败(Product)");
                }
            });
        }
    });

}

function AutoFillProductImpl(obj, data) {
    if (data.ProductGuid != undefined) {
        obj.find("input.ProductGuid").val(data.ProductGuid);
        obj.find("input.ProductCode").val(data.Code);
        obj.find("input.ProductName").val($.format("{0} ({1})", data.DisplayName, data.Name));
        obj.find("input.ProductSize").val("");
        obj.find("input.ProductQuantity").val("1");

        if (data.SaleInfo != "") {
            obj.find("input.ProductPrice").val(data.Sale);
            obj.find("span.ProductPriceInfo").text($.format("{0} × 1 = {0}", data.SaleInfo)).addClass("Sale");
        } else {
            obj.find("input.ProductPrice").val(data.Price);
            obj.find("span.ProductPriceInfo").text($.format("{0} × 1 = {0}", data.PriceInfo)).removeClass("Sale");
        }

    }
}

function AutoCalculateProductImpl(obj) {
    var $tbWishProductQuantity = obj.find("input.ProductQuantity");

    $tbWishProductQuantity.change(function () {
        var $tbWishProductPrice = $(this).parents("tr.WishItem").find("input.ProductPrice");
        var $lblWishProductPriceInfo = $(this).parents("tr.WishItem").find("span.ProductPriceInfo");

        var _pUnitPrice = $.trim($tbWishProductPrice.val());
        var _pQuantity = $.trim($(this).val());
        var _pPriceInfo = $.trim($lblWishProductPriceInfo.text());

        if (_pUnitPrice != "" && _pQuantity > 0) {
            var _strCurrency = _pPriceInfo.substr(0, 1);
            var _strPriceInfo = $.format("{0}{1} × {2} = {0}{3}", _strCurrency, _pUnitPrice, _pQuantity, (_pUnitPrice * _pQuantity));

            $lblWishProductPriceInfo.text(_strPriceInfo);
        }
    });
}

function UnPackageWishOrderItemList(obj) {
    var $tbWishOrderItemListInfo = obj.find("tr.CommandRow input.WishOrderItemListInfo");
    var $trWishItem = obj.find("tr.WishItem").first();

    if ($.trim($tbWishOrderItemListInfo.val()) != "") {
        var _jsonOrderItemList = JSON.parse($.trim($tbWishOrderItemListInfo.val()));
        var $trWishRemark = obj.find("tr.WishRemark");

        $.each(_jsonOrderItemList, function (entryIndex, entry) {
            var $trWishItemClone = $trWishItem.clone();

            UnPackageWishOrderItem($trWishItemClone, entry);

            AutoCompleteProductImpl($trWishItemClone);
            AutoCalculateProductImpl($trWishItemClone);

            obj.append($trWishItemClone);
        });

        obj.append($trWishRemark);
        $trWishItem.remove();

    } else {
        AutoCompleteProductImpl($trWishItem);
        AutoCalculateProductImpl($trWishItem);
    }
}

function UnPackageWishOrderItem(obj, entry) {
    obj.find("input.OrderItemID").val(entry.OrderItemID);
    obj.find("input.ProductGuid").val(entry.ProductGuid);
    obj.find("input.ProductCode").val(entry.Code);
    obj.find("input.ProductName").val(entry.ProductName);
    obj.find("input.ProductSize").val(entry.Size);
    obj.find("input.ProductQuantity").val(entry.Quantity);

    $.getJSON("ServerProductCheck.ashx", { ProductCode: entry.Code }, function (data, status, xhr) {
        if (status == "success" && data != null) {
            if (data.ProductGuid != undefined) {
                if (data.SaleInfo != "") {
                    obj.find("input.ProductPrice").val(data.Sale);
                    obj.find("span.ProductPriceInfo").text($.format("{0} × {1} = {2}{3}",
                        data.SaleInfo, entry.Quantity, data.CurrencyInfo, (data.Sale * entry.Quantity))).addClass("Sale");
                } else {
                    obj.find("input.ProductPrice").val(data.Price);
                    obj.find("span.ProductPriceInfo").text($.format("{0} × {1} = {2}{3}",
                        data.PriceInfo, entry.Quantity, data.CurrencyInfo, (data.Price * entry.Quantity))).removeClass("Sale");
                }
            } else {
                obj.find("input.ProductPrice").val("");
                obj.find("span.ProductPriceInfo").text("");
            }
        } else {
            alert("调用数据接口失败(Product)");
        }
    });
}

function PackageWishOrderItemList(obj) {
    var $trWishItem = obj.find("tr.WishItem");
    var $tbWishOrderItemListInfo = obj.find("tr.CommandRow input.WishOrderItemListInfo");
    var $rfvWishOrderItemListInfo = obj.find("tr.CommandRow span.ValiSpan");

    var _arrayOrderItemList = new Array();
    var _strOrderItem = "";

    $trWishItem.each(function () {
        _strOrderItem = PackageWishOrderItem($(this));

        if (_strOrderItem != "") {
            _arrayOrderItemList.push(_strOrderItem);
        }
    });

    if (_arrayOrderItemList.length > 0) {
        //alert(_arrayOrderItemList);
        $tbWishOrderItemListInfo.val(_arrayOrderItemList);
        $rfvWishOrderItemListInfo.hide();

        return confirm($.format("您将预订【{0}】件商品，是否提交订单信息？", _arrayOrderItemList.length));
    } else {
        $tbWishOrderItemListInfo.val("");
        $rfvWishOrderItemListInfo.show();

        return false;
    }
}

function PackageWishOrderItem(obj) {
    var _jsonOrderItem = JSON.parse(JSON.stringify(cacheOrderItem));
    var $tbWishProductGuid = obj.find("input.ProductGuid");
    var $tbWishProductCode = obj.find("input.ProductCode");

    if ($.trim($tbWishProductGuid.val()) == "" && $.trim($tbWishProductCode.val()) == "") {
        return "";
    }
    else {
        if ($.trim(obj.find("input.ProductGuid").val()) != "")
        { _jsonOrderItem.ProductGuid = $.trim(obj.find("input.ProductGuid").val()); }

        if ($.trim(obj.find("input.ProductCode").val()) != "")
        { _jsonOrderItem.Code = $.trim(obj.find("input.ProductCode").val()); }

        if ($.trim(obj.find("input.ProductName").val()) != "")
        { _jsonOrderItem.ProductName = $.trim(obj.find("input.ProductName").val()); }

        if ($.trim(obj.find("input.ProductSize").val()) != "")
        { _jsonOrderItem.Size = $.trim(obj.find("input.ProductSize").val()); }

        if ($.trim(obj.find("input.ProductQuantity").val()) != "")
        { _jsonOrderItem.Quantity = $.trim(obj.find("input.ProductQuantity").val()); }

        if ($.trim(obj.find("input.ProductPrice").val()) != "")
        { _jsonOrderItem.UnitPrice = $.trim(obj.find("input.ProductPrice").val()); }

        if ($.trim(obj.find("span.ProductPriceInfo").text()) != "")
        { _jsonOrderItem.Remark = $.trim(obj.find("span.ProductPriceInfo").text()); }

        _jsonOrderItem.CreateTime = $.datenow();
        _jsonOrderItem.IsActive = true;

        return JSON.stringify(_jsonOrderItem);
    }
}

// Control.AdminFieldToolBar

function SwitchLeftPanel(className) {
    if (className == "CtrlLeftPanelCol") {
        $('#MainPanel').width('100%');
        $.cookie('leftPanel', 'hidden', { expires: 30 });
    }
    else {
        $('#MainPanel').width('78%');
        $.cookie('leftPanel', null);
    }
}

/* Javascript Version jquery extend */
/* Version: 1.0 || Date: 2014-3-1 || Author: Cyrano */
/* type="text/javascript" */

// override jQuery get date now 

jQuery.datenow = function () {
    var date = new Date();
    var seperator1 = "-";
    var seperator2 = ":";
    var month = date.getMonth() + 1;
    var strDate = date.getDate();
    if (month >= 1 && month <= 9) {
        month = "0" + month;
    }
    if (strDate >= 0 && strDate <= 9) {
        strDate = "0" + strDate;
    }
    var currentdate = date.getYear() + seperator1 + month + seperator1 + strDate
            + " " + date.getHours() + seperator2 + date.getMinutes()
            + seperator2 + date.getSeconds();
    return currentdate;
}

// override jQuery string format

jQuery.format = function (source, params) {
    if (arguments.length == 1)
        return function () {
            var args = $.makeArray(arguments);
            args.unshift(source);
            return $.format.apply(this, args);
        };
    if (arguments.length > 2 && params.constructor != Array) {
        params = $.makeArray(arguments).slice(1);
    }
    if (params.constructor != Array) {
        params = [params];
    }
    $.each(params, function (i, n) {
        source = source.replace(new RegExp("\\{" + i + "\\}", "g"), n);
    });
    return source;
};

// override jQuery cookie

jQuery.cookie = function (name, value, options) {
    if (typeof value != 'undefined') {
        options = options || {};
        if (value === null) {
            value = '';
            options = $.extend({}, options);
            options.expires = -1;
        }
        var expires = '';
        if (options.expires && (typeof options.expires == 'number' || options.expires.toUTCString)) {
            var date;
            if (typeof options.expires == 'number') {
                date = new Date();
                date.setTime(date.getTime() + (options.expires * 24 * 60 * 60 * 1000));
            } else {
                date = options.expires;
            }
            expires = '; expires=' + date.toUTCString();
        }
        var path = options.path ? '; path=' + (options.path) : '';
        var domain = options.domain ? '; domain=' + (options.domain) : '';
        var secure = options.secure ? '; secure' : '';
        document.cookie = [name, '=', encodeURIComponent(value), expires, path, domain, secure].join('');
    } else {
        var cookieValue = null;
        if (document.cookie && document.cookie != '') {
            var cookies = document.cookie.split(';');
            for (var i = 0; i < cookies.length; i++) {
                var cookie = jQuery.trim(cookies[i]);
                if (cookie.substring(0, name.length + 1) == (name + '=')) {
                    cookieValue = decodeURIComponent(cookie.substring(name.length + 1));
                    break;
                }
            }
        }
        return cookieValue;
    }
};