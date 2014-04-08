<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GoogleAdv.ascx.cs" Inherits="Arsenalcn.ClubSys.Web.Control.GoogleAdv" %>
<div id="GoogleAdv" class="ClubSys_GetStrip" style="display: <%=DisplayAdv %>">
    <div id="AdvContainer">
        <div id="AdvTips">
            提示：请将以下广告完全加载后点击，即可免费抽取与获得球会装备。</div>
        <div id="AdvLoading">
        </div>
        <div id="AdvFrame" style="display: none;">

            <script type="text/javascript"><!--
                google_ad_client = "pub-6225167962632465";
                /* Arsenal AdSense 728&#42;90 */
                google_ad_slot = "2050731143";
                google_ad_width = 728;
                google_ad_height = 90;
                //-->
            </script>

            <script type="text/javascript" src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
            </script>

        </div>
        <div id="AdvNewFrame" style="display: none;">
            <iframe id="GoogleAdvFrame" width="728" height="90" frameborder="0" scrolling="no"
                marginwidth="0" marginheight="0"></iframe>
        </div>
    </div>
</div>

<script type="text/javascript">
    $(document).ready(function() {
        if ($("#GoogleAdv:hidden").length == 0) {
            var google_ad_frame_url = $("#AdvFrame iframe").attr("src");
            var frameUrl = "ServerGoogleAdv.aspx?url=" + encodeURIComponent(google_ad_frame_url);
            $("#AdvNewFrame > iframe").attr("src", frameUrl);
            $("#AdvNewFrame > iframe").ready(function() {
                $("#AdvFrame").remove();
                $("#AdvLoading").hide();
                $("#AdvNewFrame").show();
            });
        } else {
            $("#AdvTips").remove();
            $("#AdvLoading").remove();
            $("#AdvNewFrame").remove();
        }
    });

    function GoogleAdvClick(url) {
        $.get("ServerGoogleAdv.aspx", {
            logAdv: "true", advURL: encodeURIComponent(url)
        }, function(data) {
            if (data == "success") {
                $("#GoogleAdv").hide();
                $("#pnlSwf").show();
            } else {
                window.location.href = window.location.href;
            }
        });
    }
</script>

