<%@ Page Language="C#" MasterPageFile="iArsenalMaster.Master" AutoEventWireup="true"
    CodeBehind="iArsenalOrderView_TicketFriendly.aspx.cs" Inherits="iArsenal.Web.iArsenalOrderView_TicketFriendly"
    Title="订单信息查看" Theme="iArsenal" %>

<%@ Register Src="Control/PortalSitePath.ascx" TagName="PortalSitePath" TagPrefix="uc1" %>
<%@ Register Src="Control/PortalWorkflowInfo.ascx" TagName="PortalWorkflowInfo" TagPrefix="uc2" %>
<%@ Register Src="Control/PortalPaymentInfo.ascx" TagName="PortalPaymentInfo" TagPrefix="uc4" %>
<%@ Register Src="Control/PortalProductQrCode.ascx" TagName="PortalProductQrCode" TagPrefix="uc5" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
    <style type="text/css">
        .OrderProductGuid {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <div id="ACN_Main">
        <uc1:PortalSitePath ID="ucPortalSitePath" runat="server" />
        <div id="mainPanel">
            <uc2:PortalWorkflowInfo ID="ucPortalWorkflowInfo" runat="server" />
            <table class="DataView FormView">
                <thead>
                    <tr class="Header">
                        <th colspan="4" class="FieldColspan">
                            <a name="anchorBack" id="anchorBack">感谢预订“阿森纳中国行官方球迷会专属看台球票”，请仔细确认并提交订单信息：</a>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="CommandRow">
                        <td colspan="4">-- 请核对您的身份信息 --
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader" style="width: 110px">真实姓名：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblMemberName" runat="server"></asp:Label>
                        </td>
                        <td class="FieldHeader" style="width: 110px">手机：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblOrderMobile" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">身份证号：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblMemberIDCardNo" runat="server"></asp:Label>
                        </td>
                        <td class="FieldHeader">现居住地：
                        </td>
                        <td class="FieldColspan">
                            <asp:Label ID="lblMemberRegion" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">邮箱：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblMemberEmail" runat="server"></asp:Label>
                        </td>
                        <td class="FieldHeader">微信：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblMemberWeChat" runat="server"></asp:Label>
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
                        <td class="FieldHeader">购买球票：
                        </td>
                        <td class="FieldColspan" colspan="3">
                            <asp:Label ID="lblOrderItem_TicketBeijing" runat="server"></asp:Label>
                            <asp:TextBox ID="tbOrderItem_TicketBeijing" runat="server" CssClass="OrderProductGuid"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">订购数量：
                        </td>
                        <td class="FieldColumn" colspan="3">
                            <asp:Label ID="lblOrderItemQuantity" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <asp:PlaceHolder ID="phOrderItemRemark" runat="server" Visible="false">
                        <tr class="Row">
                            <td class="FieldHeader">活动情况：
                            </td>
                            <td class="FieldColspan" colspan="3">
                                <asp:Label ID="lblOrderItemRemark" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </asp:PlaceHolder>
                    <tr class="Row">
                        <td class="FieldHeader">备注：
                        </td>
                        <td class="FieldColspan" colspan="3">
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
                            <td class="FieldColspan" colspan="3">
                                <asp:Label ID="lblOrderRemark" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </asp:PlaceHolder>
                    <tr class="CommandRow">
                        <td class="OrderPriceInfo" colspan="4">
                            <asp:Label ID="lblOrderPrice" runat="server" Text="? 元 (CNY)"></asp:Label>
                            <asp:TextBox ID="tbOrderPrice" runat="server"></asp:TextBox>
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
            <uc4:PortalPaymentInfo ID="ucPortalPaymentInfo" runat="server" />
            <uc5:PortalProductQrCode ID="ucPortalProductQrCode" runat="server" />
            <div class="InfoPanel">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
                    <a>取票信息</a>
                </h3>
                <div class="Block">
                    <p>
                        (1). 在主办方出票后，已确认付款的会员会以邮件方式收到一份<em>取票确认函</em>，会注明具体看台的编号， 若有多张票，会尽量联号，以及相关的取票地点、时间和联系方式。
                    </p>
                    <p>
                        (2). 比赛当日开始前约1个小时，请取票人凭打印的<em>取票确认函</em>和<em>居民身份证/护照</em>前往北京国家体育场附近指定位置，找ACN干事取票。
                    </p>
                    <p>
                        (3). 主办方出票后，我们也可提供球票快递服务（<em>仅限顺丰</em>）。但快递可能存在丢件和延迟送达的风险，请各位会员谨慎选择，快递费采用<em>到付</em>方式。
                    </p>
                    <p>
                        (4). 预订球票成功者，将优先获得<em>训练课门票</em>和<em>球员见面会门票</em>，数量有限，送完为止。
                    </p>
                    <p>
                        (5). 本次主办方提供的球票并无任何优惠和折扣，同时球迷会的所有干事（球票负责人、客服、发票人）均为<em>阿森纳球迷义务志愿服务</em>，不收取任何报酬。如在订购或咨询过程中，有响应较慢或差错，请各位理解原谅。
                    </p>
                </div>
            </div>
        </div>
        <div class="Clear">
        </div>
    </div>
</asp:Content>
