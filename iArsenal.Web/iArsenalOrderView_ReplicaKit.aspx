<%@ Page Language="C#" MasterPageFile="iArsenalMaster.Master" AutoEventWireup="true"
    CodeBehind="iArsenalOrderView_ReplicaKit.aspx.cs" Inherits="iArsenal.Web.iArsenalOrderView_ReplicaKit"
    Title="订单信息查看" Theme="iArsenal" %>

<%@ Register Src="Control/PortalSitePath.ascx" TagName="PortalSitePath" TagPrefix="uc1" %>
<%@ Register Src="Control/PortalWorkflowInfo.ascx" TagName="PortalWorkflowInfo" TagPrefix="uc2" %>
<%@ Register Src="Control/PortalPaymentInfo.ascx" TagName="PortalPaymentInfo" TagPrefix="uc3" %>
<%@ Register Src="Control/PortalBulkOrderInfo.ascx" TagName="PortalBulkOrderInfo" TagPrefix="uc4" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
    <style type="text/css">
        .OrderPrice, .OrderProductGuid
        {
            display: none;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            var $tbOrderItem = $(".OrderProductGuid");
            if ($tbOrderItem.val() != undefined) {
                ProductCheckByID($tbOrderItem.val());
            }
        });
    </script>
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <div id="ACN_Main">
        <uc1:PortalSitePath ID="ucPortalSitePath" runat="server" />
        <div id="mainPanel">
            <uc2:PortalWorkflowInfo ID="ucPortalWorkflowInfo" runat="server" />
            <table class="DataView">
                <thead>
                    <tr class="Header">
                        <th colspan="4" class="FieldColumn">
                            <a name="anchorBack" id="anchorBack">感谢预订新赛季阿森纳球衣，请仔细确认并提交订单信息：</a>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="CommandRow">
                        <td colspan="4">-- 请核对您的收货信息 --
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader" style="width: 100px">真实姓名：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblMemberName" runat="server"></asp:Label>
                        </td>
                        <td class="FieldHeader" style="width: 100px">手机：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblOrderMobile" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">付款方式：
                        </td>
                        <td class="FieldColumn" colspan="3">
                            <asp:Label ID="lblOrderPayment" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">收货地址：
                        </td>
                        <td class="FieldColumn" colspan="3">
                            <asp:Label ID="lblOrderAddress" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="CommandRow">
                        <td colspan="4">-- 请核对您的订单信息 --
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">订单号：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblOrderID" runat="server"></asp:Label>
                        </td>
                        <td class="FieldHeader">创建时间：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblOrderCreateTime" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">购买球衣：
                        </td>
                        <td class="FieldColumn" colspan="3">
                            <asp:Label ID="lblOrderItem_ReplicaKit" runat="server"></asp:Label>
                            <asp:TextBox ID="tbOrderItem_ReplicaKit" runat="server" CssClass="OrderProductGuid"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">尺寸：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblOrderItem_ReplicaKitSize" runat="server"></asp:Label>
                        </td>
                        <td class="FieldHeader">印字印号：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblOrderItem_PlayerDetail" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">袖标：
                        </td>
                        <td class="FieldColumn" colspan="3">
                            <asp:Label ID="lblOrderItem_Patch" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">备注：
                        </td>
                        <td class="FieldColumn" colspan="3">
                            <asp:Label ID="lblOrderDescription" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <asp:PlaceHolder ID="phOrderRemark" runat="server" Visible="false">
                        <tr class="CommandRow">
                            <td colspan="4">-- 请留意给您的反馈信息 --
                            </td>
                        </tr>
                        <tr class="AlternatingRow">
                            <td class="FieldHeader">订单反馈：
                            </td>
                            <td class="FieldColumn" colspan="3">
                                <asp:Label ID="lblOrderRemark" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </asp:PlaceHolder>
                    <tr class="CommandRow">
                        <td colspan="4" style="text-align: right; white-space: nowrap; line-height: 1.8;">
                            <asp:Label ID="lblOrderPrice" runat="server" CssClass="OrderTotalPrice" Text="? 元 (CNY)"></asp:Label>
                            <asp:TextBox ID="tbOrderPrice" runat="server" CssClass="OrderPrice"></asp:TextBox>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="FooterBtnBar">
                <asp:Button ID="btnSubmit" runat="server" Text="确认并提交" CssClass="InputBtn SubmitBtn" OnClick="btnSubmit_Click"
                    OnClientClick="return confirm('确定提交此订单?(提交后无法修改)')" />
                <asp:Button ID="btnModify" runat="server" Text="修改" CssClass="InputBtn" OnClick="btnModify_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="取消" CssClass="InputBtn" OnClick="btnCancel_Click"
                    OnClientClick="return confirm('取消此订单?(取消后无法恢复)')" />
                <input id="btnPrint" type="button" value="打印" class="InputBtn" onclick="window.print()" />
                <input id="btnBack" type="button" value="返回列表" class="InputBtn" onclick="window.location.href = 'iArsenalOrder.aspx'" />
            </div>
        </div>
        <div id="rightPanel">
            <div class="InfoPanel" id="pnlProductImage" style="display: none">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
                    <a>商品图片</a></h3>
                <div class="Block">
                    <img src="" alt="" style="width: 250px; margin: 2px;" />
                </div>
            </div>
            <uc3:PortalPaymentInfo ID="ucPortalPaymentInfo" runat="server" />
            <uc4:PortalBulkOrderInfo ID="ucPortalBulkOrderInfo" runat="server" />
            <div class="InfoPanel">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
                    <a>收货方式信息</a></h3>
                <div class="Block">
                    <p>
                        英国官方预订的球衣和纪念品：<br />
                        -【下单】统计截止后2天内发单英国官方；<br />
                        -【到货】下单成功后20天左右到货中国上海；<br />
                        -【快递】到货后1周内分批陆续发快递。快递单号，可以通过<a href="iArsenalOrder.aspx" target="_blank"><em>【订单查询】</em></a>获得。
                    </p>
                </div>
            </div>
        </div>
        <div class="Clear">
        </div>
    </div>
</asp:Content>
