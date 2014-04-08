/* Javascript Version Arsenal */
/* Version: 1.1 || Date: 2013-09-27 || Author: Cyrano */
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