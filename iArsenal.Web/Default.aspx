<%@ Page Language="C#" MasterPageFile="iArsenalMaster.master" CodeBehind="Default.aspx.cs"
Inherits="iArsenal.Web._Default" Title="首页" Theme="iArsenal" %>

<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
    <script type="text/javascript" src="Scripts/jquery.slides.min.js"></script>
    <script type="text/javascript">
        $(function() {
            $("#banner").show();

            $("#slides").slidesjs({
                width: 980,
                height: 300,
                navigation: false,
                play: {
                    active: true,
                    auto: true,
                    interval: 4000,
                    swap: true
                }
            });
        });
    </script>
    <style type="text/css">
        .slidesjs-pagination, a.slidesjs-play, a.slidesjs-stop { display: none; }
    </style>
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <div id="banner" style="display: none; height: 300px;">
        <div id="slides">
            <a href="iArsenalMemberRegister.aspx">
                <img src="uploadfiles/banner/banner20120511.png" alt="阿森纳中国官方球迷会实名认证"/>
            </a>
            <a href="iArsenalOrder_ReplicaKit.aspx">
                <img src="uploadfiles/banner/banner20180524.png" alt="阿森纳新赛季主客场球衣许愿单"/>
            </a>
            <!-- a href="iArsenalOrder_AsiaTrophy2015.aspx">
            <img src="uploadfiles/banner/banner20150503.png" alt="2015英超亚洲杯阿森纳观赛团"/></a -->
            <!-- a href="iArsenalOrder_LondonTravel.aspx">
            <img src="uploadfiles/banner/banner20130713.png" alt="阿森纳新赛季伦敦行观战团预订"/></a -->
            <a href="iArsenalOrder_MatchList.aspx">
                <img src="uploadfiles/banner/banner20130714.png" alt="阿森纳新赛季比赛主场球票预订"/>
            </a>
        </div>
    </div>
    <div id="ACN_Main">
    </div>
</asp:Content>