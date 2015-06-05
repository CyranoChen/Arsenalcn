<%@ Page Language="C#" MasterPageFile="iArsenalMaster.Master" AutoEventWireup="true"
    CodeBehind="iArsenalOrderView_MatchTicket.aspx.cs" Inherits="iArsenal.Web.iArsenalOrderView_MatchTicket"
    Title="订单信息查看" Theme="iArsenal" %>

<%@ Register Src="Control/PortalSitePath.ascx" TagName="PortalSitePath" TagPrefix="uc1" %>
<%@ Register Src="Control/PortalWorkflowInfo.ascx" TagName="PortalWorkflowInfo" TagPrefix="uc2" %>
<%@ Register Src="Control/PortalMatchInfo.ascx" TagName="PortalMatchInfo" TagPrefix="uc3" %>
<%@ Register Src="Control/PortalPaymentInfo.ascx" TagName="PortalPaymentInfo" TagPrefix="uc4" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
    <style type="text/css">
        .OrderPrice {
            display: none;
        }
    </style>
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
                            <a name="anchorBack" id="anchorBack">感谢您预订阿森纳新赛季比赛主场球票，请仔细确认并提交预订信息：</a>
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
                        <td class="FieldColumn" colspan="3">
                            <asp:Label ID="lblMemberRegion" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">身份证号：
                        </td>
                        <td class="FieldColumn" colspan="3">
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
                        <td class="FieldHeader">微信/QQ：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblMemberQQ" runat="server"></asp:Label>
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
                        <td class="FieldColumn" colspan="3">
                            <asp:Label ID="lblMatchTicketInfo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">比赛时间：
                        </td>
                        <td style="text-align: left; white-space: nowrap;">
                            <asp:Label ID="lblMatchTicketPlayTime" runat="server"></asp:Label>
                        </td>
                        <td class="FieldHeader">比赛等级：
                        </td>
                        <td style="text-align: left; white-space: nowrap;">
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
            <uc3:PortalMatchInfo ID="ctrlPortalMatchInfo" runat="server" />
            <uc4:PortalPaymentInfo ID="ucPortalPaymentInfo" runat="server" />
            <div class="InfoPanel">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
                    <a>其他补充说明</a></h3>
                <div class="Block">
                    <p>(1). 由于一般订票时间与比赛时间的间隔时间较长，我们建议会员对于A/B/C级比赛球票用淘宝交易方式，可通过访问<a href="http://arsenalcn.taobao.com" target="_blank"><em>球迷会淘宝商城</em></a>，拍下相应级别的球票。</p>

                    <p>(2). 特价球票（非A/B/C级比赛，如联赛杯）建议用支付宝或银行转帐交易。对于普通比赛球票，如没有淘宝帐号，亦可采用支付宝或银行转账交易。</p>

                    <p>(3). 球票原则上只采用人民币交易，如需通过英磅交易或有其他需求，请与负责人联系。</p>

                    <p>(4). 订票者可在预订截止时间之前，申请退票，我们会全额退还球票款项（不包括收费会员年费）；截止时间之后，原则上不可退票，如有特殊需求，请与负责人联系。</p>

                    <p>
                        (5). 票务负责人联系方式：<br />
                        微信号：<em>iArsenalcn【推荐】加入时请注明<em>【球票】</em>字样</em>。<br />
                        QQ：<em>22287842，Cyrano Chen</em><br />
                        Email：<a href="mailto:webmaster@arsenalcn.com"><em>webmaster@arsenalcn.com</em></a>
                    </p>
                </div>
            </div>
        </div>
        <div class="Clear">
        </div>
    </div>
</asp:Content>
