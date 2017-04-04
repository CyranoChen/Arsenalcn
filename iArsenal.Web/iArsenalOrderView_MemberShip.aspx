<%@ Page Language="C#" MasterPageFile="iArsenalMaster.Master" AutoEventWireup="true"
    CodeBehind="iArsenalOrderView_Membership.aspx.cs" Inherits="iArsenal.Web.iArsenalOrderView_Membership"
    Title="订单信息查看" Theme="iArsenal" %>

<%@ Register Src="Control/PortalSitePath.ascx" TagName="PortalSitePath" TagPrefix="uc1" %>
<%@ Register Src="Control/PortalWorkflowInfo.ascx" TagName="PortalWorkflowInfo" TagPrefix="uc2" %>
<%@ Register Src="Control/PortalPaymentInfo.ascx" TagName="PortalPaymentInfo" TagPrefix="uc3" %>
<%@ Register Src="Control/PortalProductQrCode.ascx" TagName="PortalProductQrCode" TagPrefix="uc4" %>
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
                            <a name="anchorBack" id="anchorBack">感谢您登记注册为ACN收费会员，请仔细确认并提交登记信息：</a>
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
                        <td class="FieldHeader">会籍等级：</td>
                        <td class="FieldColspan" colspan="3">
                            <asp:Label ID="lblMemberClass" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">专属卡号：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblMemberCardNo" runat="server"></asp:Label>
                        </td>
                        <td class="FieldHeader">会籍有效期：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblEndDate" runat="server"></asp:Label>
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
                <asp:Button ID="btnGenMemberPeriod" runat="server" Text="关联会籍" CssClass="InputBtn" OnClick="btnGenMemberPeriod_Click"
                    Visible="false" OnClientClick="return confirm('关联此会员会籍?(关联后订单将自动完成)')" />
            </div>
        </div>
        <div id="rightPanel">
            <uc3:PortalPaymentInfo ID="ucPortalPaymentInfo" runat="server" />
            <uc4:PortalProductQrCode ID="ucPortalProductQrCode" runat="server" />
            <div class="InfoPanel">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
                    <a>会籍生效说明</a>
                </h3>
                <div class="Block">
                    <p>
                        (1). 请会员提交本会籍申请订单后，可扫描上方出现的<em>二维码</em>，通过微信钱包快捷付款。
                    </p>
                    <p>
                        (2). 您亦可前往<a href="http://arsenalcn.taobao.com" target="_blank">
                            <em>球迷会淘宝商城</em>
                        </a>拍下<a href="http://item.taobao.com/item.htm?id=35079525760" target="_blank">
                            <em>普通(Core)会员</em>
                        </a>，或<a href="http://item.taobao.com/item.htm?id=40209866226" target="_blank">
                            <em>高级(Premier)会员</em>
                        </a>的会员费，或通过支付宝和银行转账。如代付其他会员的会员费，请在<em>付款备注</em>中注明代付会员的真实姓名。
                    </p>
                    <p>
                        (3). 待管理员为您确认会籍信息后，会与您联系确认，即可预订或提交<a href="iArsenalOrder.aspx">
                            <em>主场球票订单</em>
                        </a>。
                    </p>
                    <p>
                        (4). 如您已经是本赛季的普通(Core)会员，需要订购A级或特定重要比赛球票，则需要升级成为高级(Premier)会员。您可在提交订单后，前往球迷会淘宝商城拍下<a href="http://item.taobao.com/item.htm?id=40251196901" target="_blank">
                            <em>升级会员费</em>
                        </a>即可。
                    </p>
                    <div class="Thumbnail">
                        <img src="uploadfiles/qrcode-iarsenalcn.gif" alt="iArsenalCN" />
                    </div>
                    <p>
                        (5). 【微信】申请操作中如有问题，请咨询官方球迷会服务微信号：<em>iArsenalCN</em>。加入时验证方式为您注册本服务中心的真实姓名和手机号码，并注明<em>【球票】</em>字样。
                    </p>
                </div>
            </div>
        </div>
        <div class="Clear">
        </div>
    </div>
</asp:Content>
