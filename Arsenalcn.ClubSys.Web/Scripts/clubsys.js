/* Javascript Version ClubSys */
/* Version: 1.6.3 || Date: 2014-01-19 || Author:cao262,Cyrano */
/* type="text/javascript" */

$(function () {
    if ($.cookie("leftPanel") != null) {
        $("#LeftPanel").hide();
        $("#MainPanel").width("100%");
        $("#MainPanel .FieldToolBar > div.CtrlLeftPanelExp").attr("class", "CtrlLeftPanelCol");
    } else {
        $("#LeftPanel").show();
        $("#MainPanel").width("78%");
    }
});

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

// override jQuery Cookie

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

// Club Action API

function UserClubAction(clubID) {
    var statusID = "btnClub" + clubID + "Status";

    var status = document.getElementById(statusID).value;

    var param = clubID + ";" + status;

    if (status == "2") {
        if (window.confirm("您是否要退出该球会?"))
            Action(param);
    }
    else if (status == "1") {
        if (window.confirm("您是否要取消申请?"))
            Action(param);

    }
    else if (status == "0") {
        if (window.confirm("您是否要申请加入该球会?"))
            Action(param);
    }
}

function GetResult(res) {
    //alert(res);
    var arrParam = res.split(';');

    if (arrParam.length == 2) {
        var clubID = arrParam[0];
        var status = arrParam[1];

        var id = "btnClub" + clubID;
        var object = document.getElementById(id);

        ChangeButtonStyle(object, clubID, status);
    }
    else if (res == "Not Appliable") {
        alert("该球会暂时不接收申请！");
    }
    else {
        window.location.href = window.location.href;
    }
}

function ChangeButtonStyle(targetObject, clubID, status) {
    //2 = member 1; = applied; 0 = no

    var message = "";

    var statusID = "btnClub" + clubID + "Status";
    var preStatus = document.getElementById(statusID).value;

    document.getElementById(statusID).value = status;

    if (status == 2) {
        message = "您已加入球会";
        targetObject.value = "退出球会";
        alert(message);

        window.location.href = window.location.href;

        return;
    }
    else if (status == 1) {
        message = "您的申请已提交";
        targetObject.value = "取消申请";
    }
    else if (status == 0) {
        if (preStatus == "2") {
            message = "您已退出球会";
            alert(message);

            window.location.href = window.location.href;
            return;
        }
        else if (preStatus == "1")
            message = "您的申请已取消";

        targetObject.value = "申请加入";
    }

    if (message == "")
        window.location.href = window.location.href;
    else
        alert(message);
}


// CollectionInfo API
function GenSwfObject(swfID, swfSrc, swfWidth, swfHeight) {
    var swf = GenSwfString(swfID, swfSrc, swfWidth, swfHeight);
    document.write(swf);
}

function GenSwfString(swfID, swfSrc, swfWidth, swfHeight) {
    var swf = "";
    swf += '<object id="' + swfID + '" classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" width="' + swfWidth + '" height="' + swfHeight + '">';
    swf += '<param name="movie" value="' + swfSrc + '" />';
    swf += '<param name="quality" value="high" />';
    swf += '<param name="menu" value="false" />';
    swf += '<param name="wmode" value="transparent" />';
    swf += '<embed name="' + swfID + '" width="' + swfWidth + '" height="' + swfHeight + '" type="application/x-shockwave-flash" wmode="transparent" quality="high" src="' + swfSrc + '" />';
    swf += '</object>';
    return swf;
}

function GenConllectInfoItem(obj) {
    var swf_id = obj.find("span.SwfID").text();
    var swf_src = obj.find("span.SwfSrc").text();
    var swf_type = obj.attr("title");
    var swf_width = 0;
    var swf_height = 0;

    if (swf_type == "PlayerVideo" || swf_type == "PlayerCard") {
        swf_width = 160;
        swf_height = 200;
    } else if (swf_type == "PlayerInactiveVideo" || swf_type == "PlayerInactiveCard") {
        swf_width = 120;
        swf_height = 150;
    }

    var $dataItem = $("<div class='ClubSys_ItemPH ClubSys_ItemPH_Border'></div>");
    var $swf = $(GenSwfString(swf_id, swf_src, swf_width, swf_height));

    $dataItem.mouseover(function () { $(this).find("a").show(); });
    $dataItem.mouseout(function () { $(this).find("a").hide(); });

    $dataItem.append($swf);

    // Inactive Video & Card
    $dataItem.append(obj.find("a.BtnActive").clone(true));

    // Active Video & Card

    $dataItem.append(obj.find("span.CurrentStrip").clone(true));
    $dataItem.append(obj.find("a.BtnSetCurrent").clone(true));

    // Active Video

    $dataItem.append(obj.find("a.SwfViewBtn").clone(true));

    return $dataItem;
}

function GenFrame(swfSrc, swfWidth, swfHeight, showClose) {
    //alert(swfSrc);
    if (document.getElementById("PageCoverLayout") == null) {
        divMask = document.createElement("div");
        divMask.setAttribute("id", "PageCoverLayout");
        divMask.style.height = document.documentElement.scrollHeight + "px";

        document.body.appendChild(divMask);
    }

    var divSwf = document.getElementById("PageCoverLayoutAjax");

    if (divSwf == null) {
        divSwf = document.createElement("div");
        divSwf.setAttribute("id", "PageCoverLayoutAjax");

        document.body.appendChild(divSwf);
    }

    var swf = '<object id="Frameswf" classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" width="' + swfWidth + '" height="' + swfHeight + '">';
    swf = swf + '<param name="movie" value="' + swfSrc + '" />';
    swf = swf + '<param name="quality" value="high" />';
    swf = swf + '<param name="menu" value="false" />';
    swf = swf + '<param name="wmode" value="transparent" />';
    swf = swf + '<embed name="Frameswf" width="' + swfWidth + '" height="' + swfHeight + '" type="application/x-shockwave-flash" wmode="transparent" quality="high" src="' + swfSrc + '" />';
    swf = swf + '</object>';

    divSwf.innerHTML = swf;

    if (showClose != null && showClose == true) {
        btnClose = document.createElement("a");
        btnClose.setAttribute("id", "btnClose");
        btnClose.innerHTML = "关闭";
        btnClose.href = "javascript:HideFrame();";

        divBtn = document.createElement("div");
        divBtn.setAttribute("style", "text-align:center;margin-top:10px;");

        divBtn.appendChild(btnClose);
        divSwf.appendChild(divBtn);
    }

    ShowFrame();
}

function HideFrame() {
    divMask = document.getElementById("PageCoverLayout");
    divSwf = document.getElementById("PageCoverLayoutAjax");

    if (divMask != null)
        divMask.style.display = "none";

    if (divSwf != null)
        document.body.removeChild(divSwf);
    //divSwf.style.display = "none";
}

function ShowFrame() {
    divMask = document.getElementById("PageCoverLayout");
    divSwf = document.getElementById("PageCoverLayoutAjax");
    divSwf.style.top = document.documentElement.scrollTop + "px";

    if (divMask != null)
        divMask.style.display = "";

    if (divSwf != null)
        divSwf.style.display = "";
}

// Rank Panel Switch 
function SwitchClubRank(showID) {
    var IDs = new Array();
    IDs.push("clubRpRank");
    IDs.push("clubLvRank");
    IDs.push("clubFortuneRank");

    for (var i = 0; i < IDs.length; i++) {
        var obj = document.getElementById(IDs[i]);

        if (obj != null) {
            if (IDs[i] == showID) {
                obj.style.display = "";
            }
            else {
                obj.style.display = "none";
            }
        }
    }
}

function SwitchPlayerRank(showID) {
    var IDs = new Array();
    IDs.push("rpRank");
    IDs.push("cardRank");
    IDs.push("videoRank");

    for (var i = 0; i < IDs.length; i++) {
        var obj = document.getElementById(IDs[i]);

        if (obj != null) {
            if (IDs[i] == showID) {
                obj.style.display = "";
            }
            else {
                obj.style.display = "none";
            }
        }
    }
}
