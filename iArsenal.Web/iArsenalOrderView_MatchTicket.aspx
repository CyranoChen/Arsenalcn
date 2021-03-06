﻿<%@ Page Language="C#" MasterPageFile="iArsenalMaster.Master" AutoEventWireup="true"
    CodeBehind="iArsenalOrderView_MatchTicket.aspx.cs" Inherits="iArsenal.Web.iArsenalOrderView_MatchTicket"
    Title="订单信息查看" Theme="iArsenal" %>

<%@ Register Src="Control/PortalSitePath.ascx" TagName="PortalSitePath" TagPrefix="uc1" %>
<%@ Register Src="Control/PortalWorkflowInfo.ascx" TagName="PortalWorkflowInfo" TagPrefix="uc2" %>
<%@ Register Src="Control/PortalMatchInfo.ascx" TagName="PortalMatchInfo" TagPrefix="uc3" %>
<%@ Register Src="Control/PortalPaymentInfo.ascx" TagName="PortalPaymentInfo" TagPrefix="uc4" %>
<%@ Register Src="Control/PortalProductQrCode.ascx" TagName="PortalProductQrCode" TagPrefix="uc5" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <div id="ACN_Main">
        <uc1:PortalSitePath ID="ucPortalSitePath" runat="server" />
        <div id="mainPanel">
            <uc2:PortalWorkflowInfo ID="ucPortalWorkflowInfo" runat="server" />
            <table class="DataView FormView">
                <thead>
                    <tr class="Header">
                        <th colspan="4" class="FieldColumn">
                            <a name="anchorBack" id="anchorBack">感谢您预订本赛季的阿森纳主场球票，请仔细确认并提交预订信息：</a>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="CommandRow">
                        <td colspan="4">-- 请核对您的会员信息 --
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
                        <td class="FieldHeader">现居住地：
                        </td>
                        <td class="FieldColspan" colspan="3">
                            <asp:Label ID="lblMemberRegion" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">身份证号：
                        </td>
                        <td class="FieldColspan" colspan="3">
                            <asp:Label ID="lblMemberIDCardNo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">护照号码：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblMemberPassportNo" runat="server"></asp:Label>
                        </td>
                        <td class="FieldHeader">护照姓名：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblMemberPassportName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">微信：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblMemberWeChat" runat="server"></asp:Label>
                        </td>
                        <td class="FieldHeader">邮箱：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblMemberEmail" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="CommandRow">
                        <td colspan="4">-- 请核对您的订票信息 --
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
                        <td class="FieldHeader">比赛信息：
                        </td>
                        <td class="FieldColspan" colspan="3">
                            <asp:Label ID="lblMatchTicketInfo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">比赛时间：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblMatchTicketPlayTime" runat="server"></asp:Label>
                        </td>
                        <td class="FieldHeader">比赛等级：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblMatchTicketRank" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">预订限制：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblMatchTicketAllowMemberClass" runat="server"></asp:Label>
                        </td>
                        <td class="FieldHeader">出行时间：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblOrderItem_TravelDate" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
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
            <uc3:PortalMatchInfo ID="ucPortalMatchInfo" runat="server" />
            <uc4:PortalPaymentInfo ID="ucPortalPaymentInfo" runat="server" />
            <uc5:PortalProductQrCode ID="ucPortalProductQrCode" runat="server" />
            <div class="InfoPanel">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
                    <a>其他补充说明</a>
                </h3>
                <div class="Block">
                    <p>
                        (1). 由于一般订票时间与比赛时间的间隔时间较长，我们建议会员采用淘宝交易方式。您可通过扫描上方的<em>二维码</em>，
                        或访问<a href="http://arsenalcn.taobao.com" target="_blank"><em>球迷会淘宝商城</em></a>，拍下相应级别的球票，待出票后再确认收货。
                    </p>
                    <p>(2). 您亦可通过上方付款信息栏中的支付宝或银行转账。此方式钱款将直接到账。</p>
                    <p>(3). 订票者可在预订截止时间之前，申请退票，我们会全额退还球票款项（暂不包括收费会员年费）；截止时间之后，原则上不可退票，如有特殊需求，请与负责人联系。</p>
                    <div class="Thumbnail">
                        <img src="uploadfiles/qrcode-iarsenalcn.gif" alt="iArsenalCN" />
                    </div>
                    <p>
                        (4). 【微信】申请操作中如有问题，请咨询官方球迷会服务微信号：<em>iArsenalCN</em>。加入时验证方式为您注册本服务中心的真实姓名和手机号码，并注明<em>【球票】</em>字样。
                    </p>
                </div>
            </div>
        </div>
        <div class="Clear">
        </div>
    </div>
</asp:Content>
