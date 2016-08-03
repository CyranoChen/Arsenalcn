<%@ Page Language="C#" MasterPageFile="iArsenalMaster.Master" AutoEventWireup="true"
    CodeBehind="iArsenalOrder_Showcase.aspx.cs" Inherits="iArsenal.Web.iArsenalOrder_Showcase" Title="阿森纳官方纪念品本季热推"
    Theme="iArsenal" %>

<%@ Register Src="Control/PortalSitePath.ascx" TagName="PortalSitePath" TagPrefix="uc1" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
    <script type="text/javascript">
        $(function () {
            var $tbInfo = $(".DivFloatLeft > .TextBox");
            $tbInfo.each(function () {
                $(this).focus(function () {
                    $(this).val("");
                });
            });
        });
    </script>
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <div id="banner" style="height: 50px">
        <a href="http://bbs.arsenalcn.com/showtopic-107269.aspx" target="_blank">
            <img src="uploadfiles/banner/banner20130518.png" alt="阿森纳官方纪念品本季热推" />
        </a>
    </div>
    <div id="ACN_Main">
        <uc1:PortalSitePath ID="ucPortalSitePath" runat="server" />
        <div class="FunctionBar">
            <div class="DivFloatLeft">
                比赛查询：
                <asp:TextBox ID="tbTeamName" runat="server" Text="--对阵球队--" CssClass="TextBox" Width="200px"></asp:TextBox>
                <asp:LinkButton ID="btnFilter" runat="server" Text="搜索比赛" CssClass="LinkBtn" OnClick="btnFilter_Click"></asp:LinkButton>
            </div>
            <div class="DivFloatRight">
            </div>
            <div class="Clear">
            </div>
            <div class="showcase">
                <asp:Repeater ID="rptShowcase" runat="server" OnItemDataBound="rptShowcase_ItemDataBound">
                    <ItemTemplate>
                        <div class="item">
                            <div class="item-image" title="加入购物车">
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
