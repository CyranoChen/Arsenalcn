/* Javascript Version ClubSys */
/* Version: 1.7.0 || Date: 2014-08-21 || Author:cao262,Cyrano */
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
    $dataItem.append(obj.find("a.BtnCancelCurrent").clone(true));

    // Active Video

    $dataItem.append(obj.find("a.SwfViewBtn").clone(true));

    return $dataItem;
}

//VideoPreview API
function ShowVideoPreview(guid) {
    var $pnlVideo = $("#PageCoverLayoutAjax");

    if ($pnlVideo.length == 0) {
        $.getJSON("ServerVideoPreview.ashx", { VideoGuid: guid }, function (data, status, xhr) {
            if (status == "success" && data != null) {
                if (data.result != "error") {
                    if (data.Video.VideoType.toLowerCase() == "flv") {
                        var _url = "swf/ShowVideoRoom.swf?XMLURL=ServerXml.aspx%3FVideoGuid=" + data.Video.VideoGuid;

                        GenFlashFrame(_url, data.Video.VideoWidth, data.Video.VideoHeight, true);

                    } else {
                        GenVideoFrame(data);
                    }
                } else {
                    alert(data.error_msg);
                }
            } else {
                alert("调用数据接口失败(VideoPreview)");
            }
        });
    }
}

function GenVideoFrame(jsonObject) {
    if (document.getElementById("PageCoverLayout") == null) {
        divMask = document.createElement("div");
        divMask.setAttribute("id", "PageCoverLayout");
        divMask.style.height = document.documentElement.scrollHeight + "px";

        document.body.appendChild(divMask);
    }

    var divVideo = document.getElementById("PageCoverLayoutAjax");

    if (divVideo == null) {
        divVideo = document.createElement("div");
        divVideo.setAttribute("id", "PageCoverLayoutAjax");

        document.body.appendChild(divVideo);
    }

    var _videoHtml = ' ' +
        '<div class="ClubSys_Video">' +
            '<div class="VideoToolbar">' +
                '<span></span>' +
                '<div class="BtnClose"></div>' +
                '<div class="BtnMin"></div>' +
                '<div class="Clear"></div>' +
            '</div>' +
            '<div class="VideoPoster">' +
                '<div class="GoalFrame">' +
                    '<div class="Star"><span></span></div>' +
                    '<div class="PlayerPhoto"><img /></div>' +
                    '<div class="PlayerName"><span></span></div>' +
                '</div>' +
                '<div class="MatchFrame">' +
                    '<div class="HomeTeamLogo"><img /></div>' +
                    '<div class="HomeTeamName"><span></span></div>' +
                    '<div class="MatchResult"><span>/span></div>' +
                    '<div class="MatchInfo"><span></span></div>' +
                    '<div class="AwayTeamName"><span></span></div>' +
                    '<div class="AwayTeamLogo"><img /></div>' +
                '</div>' +
                '<div class="TeamworkFrame">' +
                    '<div class="Star"><span></span></div>' +
                    '<div class="PlayerPhoto"><img  /></div>' +
                    '<div class="PlayerName"><span></span></div>' +
                '</div>' +
                '<div class="Clear"></div>' +
            '</div>' +
            '<div class="VideoFrame">' +
                '<video controls="controls" preload="auto">' +
                    '<source type="video/mp4">' +
                    'Your browser does not support the video tag.' +
                '</video>' +
            '</div>' +
        '</div>';

    var $pnlVideo = $(_videoHtml);
    var $pnlVideoToolbar = $pnlVideo.find(".VideoToolbar");
    var $pnlVideoPoster = $pnlVideo.find(".VideoPoster");
    var $pnlVideoFrame = $pnlVideo.find(".VideoFrame");

    $pnlVideo.mouseover(function () { $pnlVideoToolbar.show(); })
        .mouseout(function () { $pnlVideoToolbar.hide(); });

    // DIV VideoToolbar DataBind

    $pnlVideoToolbar.find("span").text(jsonObject.Video.VideoGuid);

    $pnlVideoToolbar.find(".BtnClose").click(function () {
        HideFrame();
    });

    $pnlVideoToolbar.find(".BtnMin").click(function () {
        $pnlVideoFrame.fadeOut(1000);
    });

    // DIV VideoPoster DataBind

    $pnlVideoPoster.click(function () {
        $pnlVideoFrame.fadeIn(1000);
    });

    if (jsonObject.GoalPlayer != "") {
        var $pnlGoalFrame = $pnlVideoPoster.find(".GoalFrame");

        $pnlGoalFrame.find(".Star > span")
            .css("width", (30 * jsonObject.Video.GoalRank).toString() + "px")
            .attr("title", "GoalRank " + jsonObject.Video.GoalRank.toString());

        $pnlGoalFrame.find(".PlayerPhoto > img")
            .attr("src", jsonObject.GoalPlayer.PhotoURL)
            .attr("alt", jsonObject.GoalPlayer.DisplayName);

        $pnlGoalFrame.find(".PlayerName > span").text(jsonObject.GoalPlayer.DisplayName);
    } else {
        $pnlVideo.find(".GoalFrame").remove();
    }

    if (jsonObject.AssistPlayer != "") {
        var $pnlTeamworkFrame = $pnlVideoPoster.find(".TeamworkFrame");

        $pnlTeamworkFrame.find(".Star > span")
            .css("width", (30 * jsonObject.Video.TeamworkRank).toString() + "px")
            .attr("title", "GoalRank " + jsonObject.Video.TeamworkRank.toString());

        $pnlTeamworkFrame.find(".PlayerPhoto > img")
            .attr("src", jsonObject.AssistPlayer.PhotoURL)
            .attr("alt", jsonObject.AssistPlayer.DisplayName);

        $pnlTeamworkFrame.find(".PlayerName > span").text(jsonObject.AssistPlayer.DisplayName);
    } else {
        $pnlVideo.find(".GoalFrame").remove();
    }

    if (jsonObject.Match != "") {
        var $pnlMatchFrame = $pnlVideoPoster.find(".MatchFrame");

        $pnlMatchFrame.find(".HomeTeamLogo > img")
            .attr("src", jsonObject.Match.HomeTeamLogo)
            .attr("title", jsonObject.Match.HomeTeam);

        $pnlMatchFrame.find(".HomeTeamName > span").text(jsonObject.Match.HomeTeam);

        $pnlMatchFrame.find(".MatchResult > span")
            .text(jsonObject.Match.ResultHome + " : " + jsonObject.Match.ResultAway);

        $pnlMatchFrame.find(".MatchInfo > span").text(jsonObject.Match.PlayTime);

        $pnlMatchFrame.find(".AwayTeamName > span").text(jsonObject.Match.AwayTeam);

        $pnlMatchFrame.find(".AwayTeamLogo > img")
            .attr("src", jsonObject.Match.AwayTeamLogo)
            .attr("title", jsonObject.Match.AwayTeam);

    } else {
        $pnlVideo.find(".MatchFrame").remove();
    }

    // DIV VideoFrame DataBind

    var $video = $pnlVideoFrame.find("video")
        .bind("contextmenu", function () { return false; })
        .attr("class", "DPI" + jsonObject.Video.VideoHeight.toString())
        .find("source")
            .attr("src", jsonObject.Video.VideoFilePath)
            .attr("type", "video/" + jsonObject.Video.VideoType);

    $(divVideo).append($pnlVideo).hide();

    ShowFrame();
}

function GenFlashFrame(swfSrc, swfWidth, swfHeight, showClose) {
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
    var $pnlMask = $("#PageCoverLayout").fadeOut(500);
    var $pnlFrame = $("#PageCoverLayoutAjax").remove();
}

function ShowFrame() {
    var $pnlMask = $("#PageCoverLayout").fadeIn(500);
    var $pnlFrame = $("#PageCoverLayoutAjax");

    var _offsetTop = 0;

    if ($(window).height() > $pnlFrame.height()) {
        _offsetTop = ($(window).height() - $pnlFrame.height()) / 2 + $(window).scrollTop();
    } else {
        _offsetTop = $(window).scrollTop();
    }

    $pnlFrame.css("top", _offsetTop + "px").fadeIn(1000);

    $(window).scroll(function () {
        $pnlFrame.css("top", ($(this).scrollTop() + 20) + "px");
    });
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
