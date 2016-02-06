<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" CodeBehind="MyCollection.aspx.cs"
Inherits="Arsenalcn.ClubSys.Web.MyCollection" Title="我的球员收藏" EnableViewState="false" %>

<%@ Register Src="Control/LeftPanel.ascx" TagName="LeftPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/FieldToolBar.ascx" TagName="FieldToolBar" TagPrefix="uc2" %>
<%@ Register Src="Control/PlayerHeader.ascx" TagName="PlayerHeader" TagPrefix="uc4" %>
<%@ Register Src="Control/CollectionTabBar.ascx" TagName="CollectionTabBar" TagPrefix="uc3" %>
<%@ Register Src="Control/CollectionCard.ascx" TagName="CollectionCard" TagPrefix="uc5" %>
<%@ Register Src="Control/CollectionInactiveCard.ascx" TagName="CollectionInactiveCard"
TagPrefix="uc6" %>
<%@ Register Src="Control/CollectionInactiveVideo.ascx" TagName="CollectionInactiveVideo"
TagPrefix="uc7" %>
<%@ Register Src="Control/CollectionVideo.ascx" TagName="CollectionVideo" TagPrefix="uc8" %>
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
        <uc4:PlayerHeader ID="ctrlPlayerHeader" runat="server"/>
        <uc3:CollectionTabBar ID="ctrlTabBar" runat="server"/>
        <!-- Panel可实现显示哪个类型的功能，不管是通过JS，还是通过页面QS -->
        <uc8:CollectionVideo ID="ctrlVideo" runat="server" Visible="false"/>
        <uc7:CollectionInactiveVideo ID="ctrlInvalidVideo" runat="server" Visible="false"/>
        <uc6:CollectionInactiveCard ID="ctrlInvalidCard" runat="server" Visible="false"/>
        <uc5:CollectionCard ID="ctrlCard" runat="server" Visible="false"/>
    </div>
</asp:Content>