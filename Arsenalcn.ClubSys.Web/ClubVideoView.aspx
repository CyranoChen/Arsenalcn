<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" CodeBehind="ClubVideoView.aspx.cs"
Inherits="Arsenalcn.ClubSys.Web.ClubVideoView" Title="{0} 视频名人堂" EnableViewState="false" %>
<%@ Register Src="Control/LeftPanel.ascx" TagName="LeftPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/FieldToolBar.ascx" TagName="FieldToolBar" TagPrefix="uc2" %>
<%@ Register Src="Control/MenuTabBar.ascx" TagName="MenuTabBar" TagPrefix="uc3" %>
<%@ Register Src="Control/ClubSysHeader.ascx" TagName="ClubSysHeader" TagPrefix="uc4" %>
<%@ Register Src="Control/ClubVideo.ascx" TagName="ClubVideo" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <script type="text/javascript" src="Scripts/jquery.pagination.js"></script>
    <script type="text/javascript">
        $(function() {
            var $obj_entries = $("#CollectionInfo > div.DataItem").hide();
            var $dataPlaceHolder = $("#CollectionInfo > #DataPlaceHolder");
            var $dataPager = $("#CollectionInfo > #DataPager");

            var num_entries = $obj_entries.length;
            var items_per_page = 12;

            var initPagination = function() {
                $dataPager.pagination(num_entries, {
                    callback: PageSelectCallback,
                    items_per_page: items_per_page //每页显示数量
                });

                if (num_entries <= items_per_page) {
                    $dataPager.empty();
                    $dataPager.attr("class", "Clear");
                }
            }();

            function PageSelectCallback(page_index, jq) {
                var max_elem = Math.min((page_index + 1) * items_per_page, num_entries);
                $dataPlaceHolder.empty();

                for (var i = page_index * items_per_page; i < max_elem; i++) {
                    $dataPlaceHolder.append(
                        GenConllectInfoItem($obj_entries.eq(i))
                    );
                }

                return false;
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:LeftPanel ID="ctrlLeftPanel" runat="server"/>
    <div id="MainPanel">
        <uc2:FieldToolBar ID="ctrlFieldToolBar" runat="server"/>
        <uc3:MenuTabBar ID="ctrlMenuTabBar" runat="server"/>
        <uc4:ClubSysHeader ID="ctrlClubSysHeader" runat="server"/>
        <uc5:ClubVideo ID="ctrlClubVideo" runat="server"/>
    </div>
</asp:Content>