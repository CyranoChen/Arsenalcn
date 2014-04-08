<%@ Page Language="C#" MasterPageFile="iArsenalMaster.Master" AutoEventWireup="true"
    CodeBehind="iArsenalOrderView_ArsenalDirect.aspx.cs" Inherits="iArsenal.Web.iArsenalOrderView_ArsenalDirect"
    Title="订单信息查看" Theme="iArsenal" %>

<%@ Register Src="Control/PortalSitePath.ascx" TagName="PortalSitePath" TagPrefix="uc1" %>
<%@ Register Src="Control/PortalWorkflowInfo.ascx" TagName="PortalWorkflowInfo" TagPrefix="uc2" %>
<%@ Register Src="Control/PortalBulkOrderInfo.ascx" TagName="PortalBulkOrderInfo" TagPrefix="uc3" %>
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
                        <th colspan="4" style="text-align: left">
                            <a name="anchorBack" id="anchorBack">感谢预订阿森纳官方纪念品，请仔细确认并提交订单信息：</a>
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
                        <td style="text-align: left">
                            <asp:Label ID="lblMemberName" runat="server"></asp:Label>
                        </td>
                        <td class="FieldHeader" style="width: 100px">手机：
                        </td>
                        <td style="text-align: left">
                            <asp:Label ID="lblOrderMobile" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <asp:PlaceHolder ID="phMemberInfo" runat="server">
                        <tr class="AlternatingRow">
                            <td class="FieldHeader">邮箱：
                            </td>
                            <td style="text-align: left">
                                <asp:Label ID="lblMemberEmail" runat="server"></asp:Label>
                            </td>
                            <td class="FieldHeader">QQ：
                            </td>
                            <td style="text-align: left">
                                <asp:Label ID="lblMemberQQ" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </asp:PlaceHolder>
                    <tr class="Row">
                        <td class="FieldHeader">收货地址：
                        </td>
                        <td style="text-align: left" colspan="3">
                            <asp:Label ID="lblOrderAddress" runat="server"></asp:Label>
                        </td>
                    </tr>
                </tbody>
                <tbody class="ArsenalDirect_WishList">
                    <tr class="CommandRow">
                        <td colspan="4">-- 请核对您的订单信息 --
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">订单号：
                        </td>
                        <td style="text-align: left">
                            <asp:Label ID="lblOrderID" runat="server"></asp:Label>
                        </td>
                        <td class="FieldHeader">创建时间：
                        </td>
                        <td style="text-align: left">
                            <asp:Label ID="lblOrderCreateTime" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="padding: 0px;">
                            <asp:GridView ID="gvWishItem" runat="server" DataKeyNames="OrderItemID" OnPageIndexChanging="gvWishItem_PageIndexChanging"
                                PageSize="10" OnRowDataBound="gvWishItem_RowDataBound">
                                <Columns>
                                    <asp:BoundField HeaderText="编号" DataField="OrderItemID" Visible="false" />
                                    <asp:BoundField HeaderText="编码" DataField="Code" DataFormatString="<em>{0}</em>"
                                        HtmlEncode="false" />
                                    <asp:BoundField HeaderText="名称" DataField="ProductName" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="尺寸" DataField="Size" NullDisplayText="/" />
                                    <asp:TemplateField HeaderText="单价" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWishPriceInfo" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="数量" DataField="Quantity" />
                                    <asp:TemplateField HeaderText="总价" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWishTotalPriceInfo" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">备注：
                        </td>
                        <td style="text-align: left" colspan="3">
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
                            <td style="text-align: left" colspan="3">
                                <asp:Label ID="lblOrderRemark" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </asp:PlaceHolder>
                    <tr class="CommandRow">
                        <td colspan="4" style="text-align: right; line-height: 1.8;">
                            <asp:Label ID="lblOrderPrice" runat="server" CssClass="OrderTotalPrice" Text="? 元 (CNY)"></asp:Label>
                            <asp:TextBox ID="tbOrderPrice" runat="server" CssClass="OrderPrice"></asp:TextBox>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="FooterBtnBar">
                <asp:Button ID="btnSubmit" runat="server" Text="提交" CssClass="InputBtn" OnClick="btnSubmit_Click"
                    OnClientClick="return confirm('确定提交此订单?(提交后将由管理员审核)')" />
                <asp:Button ID="btnModify" runat="server" Text="修改" CssClass="InputBtn" OnClick="btnModify_Click" />
                <asp:Button ID="btnConfirm" runat="server" Text="确认" CssClass="InputBtn" OnClick="btnConfirm_Click"
                    OnClientClick="return confirm('确认此订单信息?(确定后将无法修改与取消)')" />
                <asp:Button ID="btnCancel" runat="server" Text="取消" CssClass="InputBtn" OnClick="btnCancel_Click"
                    OnClientClick="return confirm('取消此订单?(取消后无法恢复)')" />
                <input id="btnPrint" type="button" value="打印" class="InputBtn" onclick="window.print()" />
                <input id="btnBack" type="button" value="返回列表" class="InputBtn" onclick="window.location.href = 'iArsenalOrder.aspx'" />
            </div>
        </div>
        <div id="rightPanel">
            <div class="InfoPanel">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
                    <a>订购流程</a></h3>
                <div class="Block">
                    <p>(1).【登记】在本功能入口中登记预订的纪念品信息并提交，其中商品编号和数量必填。</p>
                    <p>(2).【审核】后台管理员将对您提交的订单进行商品信息完整性与价格的审核，并将反馈订购确认信息，并结算出最终价格。</p>
                    <p>(3).【确认】查看后台反馈的已审核订单，确认无误后，点击<em>【确认按钮】</em>完成预订。</p>
                    <p>(4).【定金】对于订单总结算价格(CNY)超过500元以上的将收取100元定金，1000元以上的将收取200元定金，以此类推。同时对于订购冷门或特殊纪念品(如婴儿用品等)也将收取少量定金。如需支付定金，支付金额和方式将在审核确认单的备注框中反馈。支付的定金将在到货交易时进行全额抵扣。</p>
                </div>
            </div>
            <uc3:PortalBulkOrderInfo ID="ucPortalBulkOrderInfo" runat="server" />
            <div class="InfoPanel">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
                    <a>交易收货方式</a></h3>
                <div class="Block">
                    <p>(1).【流程】到货后通过淘宝会员专享店完成交易操作(选拍商品→买家付款→卖家发货→确认收货)。</p>
                    <p>(2).【小店】淘宝会员专享店地址： <a href="http://arsenalcn.taobao.com/" target="_blank"><em>http://arsenalcn.taobao.com</em></a></p>
                    <p>(3).【认领】按自己预订的商品进淘宝小店认领并交易。店中名称前带有<em>【*】</em>货品的为会员预定品，其他非预定者的商品如有看中，可以随意购买。</p>
                </div>
            </div>
        </div>
        <div class="Clear">
        </div>
    </div>
</asp:Content>
