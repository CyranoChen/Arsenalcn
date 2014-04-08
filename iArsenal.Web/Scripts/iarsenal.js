/* Javascript Version iArsenal */
/* Version: 1.7.5 || Date: 2014-04-08 || Author: Cyrano */
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

                if ($tbRegion1.val().trim() != "") {
                    $ddlRegion1.val($tbRegion1.val().trim());
                    RegionDataBindImpl($tbRegion1.val().trim(), $ddlRegion2, $tbRegion2.val().trim());
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
        $tbRegion1.val($(this).val().trim());
        $tbRegion2.val("");
        RegionDataBindImpl($(this).val().trim(), $ddlRegion2, $tbRegion2.val().trim());
    });

    $ddlRegion2.change(function () {
        $tbRegion2.val($(this).val().trim());
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

    if ($ddlNation.val().trim() == "中国") {
        $tbOtherNation.hide();
        $ddlRegion1.show();

        if ($tbRegion2.val().trim() != "") {
            $ddlRegion2.show();
        }
        else {
            $ddlRegion2.hide();
        }

    } else if ($ddlNation.val().trim() == "其他") {
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
    var $btnSubmit = $(".FooterBtnBar .Submit");
    $btnSubmit.prop("disabled", true);

    if ($tbAcnID.val().trim() != "") {
        $.getJSON("ServerAcnCheck.ashx", { AcnID: $tbAcnID.val().trim(), SessionKey: $tbAcnSessionKey.val().trim() }, function (data, status, xhr) {
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
    var $btnSubmit = $(".FooterBtnBar .Submit");
    $btnSubmit.prop("disabled", true);

    if ($tbProductCode.val().trim() != "") {
        $.getJSON("ServerProductCheck.ashx", { ProductCode: $tbProductCode.val().trim() }, function (data, status, xhr) {
            if (status == "success" && data != null) {
                if (data.result != "error" && JSON.stringify(data) != "{}") {
                    $tbProductCode.val(data.Code);
                    $tbProductGuid.val(data.ProductGuid);
                    $tbProductName.val($.format("{0} ({1})", data.DisplayName, data.Name));
                    $tbProductUnitPrice.val(data.PriceCNY.toLocaleString());

                    var totalPrice = Number(data.PriceCNY) * Number($tbProductQuantity.val().trim());
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
    var $btnSubmit = $(".FooterBtnBar .Submit");
    $btnSubmit.prop("disabled", true);

    if ($tbMemberID.val().trim() != "") {
        $.getJSON("ServerMemberCheck.ashx", { MemberID: $tbMemberID.val().trim() }, function (data, status, xhr) {
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
        if ($tbWishProductCode.val().trim() != "") {
            if (confirm($.format("移除当前许愿商品【{0}】?", $tbWishProductCode.val().trim()))) { obj.remove(); }
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
            if (ui.item.value.trim() != "") {
                $.getJSON("ServerProductCheck.ashx", { ProductCode: ui.item.value.trim() }, function (data, status, xhr) {
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
        if ($(this).val().trim() != "") {
            $.getJSON("ServerProductCheck.ashx", { ProductCode: $(this).val().trim() }, function (data, status, xhr) {
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

        var _pUnitPrice = $tbWishProductPrice.val().trim();
        var _pQuantity = $(this).val().trim();
        var _pPriceInfo = $lblWishProductPriceInfo.text().trim();

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

    if ($tbWishOrderItemListInfo.val().trim() != "") {
        var _jsonOrderItemList = JSON.parse($tbWishOrderItemListInfo.val().trim());
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

    if ($tbWishProductGuid.val().trim() == "" && $tbWishProductCode.val().trim() == "") {
        return "";
    }
    else {
        if (obj.find("input.ProductGuid").val().trim() != "")
        { _jsonOrderItem.ProductGuid = obj.find("input.ProductGuid").val().trim(); }

        if (obj.find("input.ProductCode").val().trim() != "")
        { _jsonOrderItem.Code = obj.find("input.ProductCode").val().trim(); }

        if (obj.find("input.ProductName").val().trim() != "")
        { _jsonOrderItem.ProductName = obj.find("input.ProductName").val().trim(); }

        if (obj.find("input.ProductSize").val().trim() != "")
        { _jsonOrderItem.Size = obj.find("input.ProductSize").val().trim(); }

        if (obj.find("input.ProductQuantity").val().trim() != "")
        { _jsonOrderItem.Quantity = obj.find("input.ProductQuantity").val().trim(); }

        if (obj.find("input.ProductPrice").val().trim() != "")
        { _jsonOrderItem.UnitPrice = obj.find("input.ProductPrice").val().trim(); }

        if (obj.find("span.ProductPriceInfo").text().trim() != "")
        { _jsonOrderItem.Remark = obj.find("span.ProductPriceInfo").text().trim(); }

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