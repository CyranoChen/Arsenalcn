<%@ Page Language="C#" MasterPageFile="iArsenalMaster.Master" AutoEventWireup="true"
    CodeBehind="iArsenalOrderView_TicketBeijing.aspx.cs" Inherits="iArsenal.Web.iArsenalOrderView_TicketBeijing"
    Title="订单信息查看" Theme="iArsenal" %>

<%@ Register Src="Control/PortalSitePath.ascx" TagName="PortalSitePath" TagPrefix="uc1" %>
<%@ Register Src="Control/PortalWorkflowInfo.ascx" TagName="PortalWorkflowInfo" TagPrefix="uc2" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
    <style type="text/css">
        .OrderPrice, .OrderProductGuid
        {
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
                            <a name="anchorBack" id="anchorBack">感谢预订“阿森纳2012亚洲行北京圣殿杯比赛球票”，请仔细确认并提交订单信息：</a>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="CommandRow">
                        <td colspan="4">-- 请核对您的身份信息 --
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader" style="width: 100px">真实姓名：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblMemberName" runat="server"></asp:Label>
                        </td>
                        <td class="FieldHeader" style="width: 100px">身份证号：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblMemberIDCardNo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">邮箱：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblMemberEmail" runat="server"></asp:Label>
                        </td>
                        <td class="FieldHeader">手机：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblOrderMobile" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">来自于：
                        </td>
                        <td class="FieldColumn" colspan="3">
                            <asp:Label ID="lblMemberRegion" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">付款方式：
                        </td>
                        <td class="FieldColumn" colspan="3">
                            <asp:Label ID="lblOrderPayment" runat="server"></asp:Label>
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
                        <td class="FieldColumn" colspan="3">
                            <asp:Label ID="lblOrderItem_TicketBeijing" runat="server"></asp:Label>
                            <asp:TextBox ID="tbOrderItem_TicketBeijing" runat="server" CssClass="OrderProductGuid"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">订购数量：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblOrderItemQuantity" runat="server"></asp:Label>
                        </td>
                        <td class="FieldHeader">所在看台：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblOrderItemSize" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">活动情况：
                        </td>
                        <td class="FieldColumn" colspan="3">
                            <asp:Label ID="lblOrderItemRemak" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="Row">
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
            <div class="InfoPanel">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
                    <a>付款信息</a></h3>
                <div class="Block">
                    <p>
                        (1). 在此系统完成订票流程并确认后，会显示为“等待付款”状态。请通过相应的渠道方式进行全额付款。
                    </p>
                    <p>
                        (2). 方式一：支付宝转账账户：vickie_ling@hotmail.com，户名：凌薇。
                    </p>
                    <p>
                        (3). 方式二：银行转账账户：帐号6226 0902 1624 4489（开户地：招商银行上海分行闵行支行），户名：凌薇。
                    </p>
                    <p>
                        (4). 方式三：淘宝交易：<a href="http://item.taobao.com/item.htm?id=17861408168" target="_blank"><em>点击打开</em></a>，
                        请在买家留言中注明你在ACN球迷会服务中心的订票编号和ACN会员名。发货将以虚拟物品形式操作。之后请点击淘宝订单中“确认收货”，这样我们可以收到你的付款。
                    </p>
                    <p>
                        (5). 收到您的全额付款后，我们会将订单改为“确认付款”状态。
                    </p>
                </div>
            </div>
            <div class="InfoPanel">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
                    <a>取票信息</a></h3>
                <div class="Block">
                    <p>
                        (1). 在官方出票后，已确认付款的会员会以邮件方式收到一份<em>取票确认函</em>，大致注明会坐在几区几看台； 若多张票，会注明是否连号；以及相关的取票地点、时间和联系方式。（具体的几排几座，只有取票时才知道）
                    </p>
                    <p>
                        (2). 比赛当日开始前约1个小时，会员凭打印的<em>取票确认函</em>和自己的<em>居民身份证</em>前往北京国家体育场附近指定位置，找ACN干事取票。
                    </p>
                    <p>
                        (3). 原则上安排2名干事负责发票，或者亦可在比赛前1-2天的见面会活动或观看训练时也可找干事领取，但请事先自行与干事联系好。
                    </p>
                </div>
            </div>
        </div>
        <div class="Clear">
        </div>
    </div>
</asp:Content>
