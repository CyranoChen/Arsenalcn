<%@ Page Language="C#" MasterPageFile="iArsenalMaster.Master" AutoEventWireup="true"
    CodeBehind="iArsenalOrderView_Printing.aspx.cs" Inherits="iArsenal.Web.iArsenalOrderView_Printing"
    Title="订单信息查看" Theme="iArsenal" %>

<%@ Register Src="Control/PortalSitePath.ascx" TagName="PortalSitePath" TagPrefix="uc1" %>
<%@ Register Src="Control/PortalWorkflowInfo.ascx" TagName="PortalWorkflowInfo" TagPrefix="uc2" %>
<%@ Register Src="Control/PortalPaymentInfo.ascx" TagName="PortalPaymentInfo" TagPrefix="uc3" %>
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
                            <a name="anchorBack" id="anchorBack">感谢您申请阿森纳个性化印字印号服务，请仔细确认并提交订单信息：</a>
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
                    <tr class="Row">
                        <td class="FieldHeader">收货地址：
                        </td>
                        <td class="FieldColspan" colspan="3">
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
                        <td class="FieldHeader">类型：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblOrderItem_FontSelected" runat="server"></asp:Label>
                        </td>
                        <td class="FieldHeader">印字印号：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblOrderItem_PlayerDetail" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">快递单号与备注：
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
            <uc3:PortalPaymentInfo ID="ucPortalPaymentInfo" runat="server" />
            <div class="InfoPanel">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
                    <a>印制快递地址</a>
                </h3>
                <div class="Block">
                    <p>-- <em>上海市宝山区长江南路180号 长江软件园B幢506室</em></p>
                    <p>-- <em>陈先生 13818059707</em></p>
                    <p><em>请勿必在订单中填写印制衣物的快递编号</em></p>
                </div>
            </div>
            <uc5:PortalProductQrCode ID="ucPortalProductQrCode" runat="server" />
            <div class="InfoPanel">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
                    <a>印制步骤</a>
                </h3>
                <div class="Block">
                    <p>(1). 如您需购买阿森纳正品球衣，可在服务中心<a href="~/iArsenalOrder_ReplicaKit.aspx" target="_blank"><em>登记球衣订单</em></a>的同时，选择印字印号服务。</p>
                    <p>(2). 您可单独申请印字印号服务，请将球衣或同类衣物，通过快递方式发送到管理员提供的地址。衣物寄送的快递费用、安全保价，由会员自行承担。</p>
                    <p>(3). 在我们收到需要印制的衣物后，将在3个工作日内完成印制，并通过<em>包邮 - 顺丰快递</em>发还给会员。</p>
                    <p>(4). 印制价格以左侧订单结算的价格为准。</p>
                    <p>(5). 对于您寄送过来的衣物，我们会先进行拍照取证，并承诺对因印字印号过程中导致的损伤负责。</p>
                </div>
            </div>
        </div>
        <div class="Clear">
        </div>
    </div>
</asp:Content>
