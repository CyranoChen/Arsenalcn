<%@ Page Language="C#" MasterPageFile="iArsenalMaster.Master" AutoEventWireup="true"
    CodeBehind="iArsenalOrder_ReplicaKit.aspx.cs" Inherits="iArsenal.Web.iArsenalOrder_ReplicaKit"
    Title="阿森纳新赛季球衣许愿单" Theme="iArsenal" %>

<%@ Register Src="Control/PortalSitePath.ascx" TagName="PortalSitePath" TagPrefix="uc1" %>
<%@ Register Src="Control/PortalBulkOrderInfo.ascx" TagName="PortalBulkOrderInfo" TagPrefix="uc2" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
    <script type="text/javascript">
        $(function () {
            var $rblPayment = $("#tdPaymentInfo .RadioButtonList");
            $rblPayment.click(function () {
                var payment = $(this).children("input:radio:checked").val();
                if (payment != "") {
                    $(".PaymentInfo").hide();
                    $("." + payment).show();
                }
            });

            var $ddlReplicaKit = $("#tdReplicaKit select");
            $ddlReplicaKit.change(function () {
                ProductCheckByID($(this).val());
            });

            var $ddlPlayerDetail = $("#tdPrinting select");
            var $trCustomPrinting = $(".CustomPrinting");
            $ddlPlayerDetail.change(function () {
                if ($(this).val() == "custom")
                    $trCustomPrinting.show();
                else
                    $trCustomPrinting.hide();
            });

            var $rbPremierPatch = $("#tdPremierPatch input:radio");
            var $rbChampionPatch = $("#tdChampionPatch input:radio");

            $rbPremierPatch.last().click(function () {
                $rbChampionPatch.eq(0).prop("checked", true);
            });

            $rbPremierPatch.eq(1).click(function () {
                if ($rbChampionPatch.last().prop("checked") == true) {
                    $rbChampionPatch.eq(1).prop("checked", true);
                }
            });

            $rbChampionPatch.last().click(function () {
                $rbPremierPatch.eq(0).prop("checked", true);
            });

            $rbChampionPatch.eq(1).click(function () {
                if ($rbPremierPatch.last().prop("checked") == true) {
                    $rbPremierPatch.eq(1).prop("checked", true);
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <div id="banner" style="height: 250px">
        <a href="http://bbs.arsenalcn.com/showtopic-107237.aspx" target="_blank">
            <asp:Literal ID="ltrlBannerImage" runat="server"></asp:Literal>
        </a>
    </div>
    <div id="ACN_Main">
        <uc1:PortalSitePath ID="ucPortalSitePath" runat="server" />
        <div id="mainPanel">
            <table class="DataView">
                <thead>
                    <tr class="Header">
                        <th colspan="4" style="text-align: left">
                            <a name="anchorBack" id="anchorBack">欢迎进入新赛季阿森纳球衣预订系统，请仔细确认并填写以下信息：</a>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="CommandRow">
                        <td colspan="4">-- 会员信息栏 --
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader" style="width: 100px">真实姓名：
                        </td>
                        <td style="text-align: left">
                            <asp:Label ID="lblMemberName" runat="server"></asp:Label>
                        </td>
                        <td class="FieldHeader" style="width: 100px">ACN帐号：
                        </td>
                        <td style="text-align: left">
                            <asp:Label ID="lblMemberACNInfo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">手机：
                        </td>
                        <td style="text-align: left; white-space: nowrap;">
                            <asp:TextBox ID="tbOrderMobile" runat="server" CssClass="TextBox" Width="150px" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvOrderMobile" runat="server" ControlToValidate="tbOrderMobile"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan"></asp:RequiredFieldValidator>
                        </td>
                        <td class="FieldHeader">付款方式：
                        </td>
                        <td style="text-align: left" id="tdPaymentInfo">
                            <asp:RadioButtonList ID="rblOrderPayment" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" CssClass="RadioButtonList">
                                <asp:ListItem Text="支付宝" Value="Alipay" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="银行转帐" Value="Bank"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr class="Row PaymentInfo Alipay" id="trAlipay" runat="server">
                        <td class="FieldHeader">支付宝帐号：
                        </td>
                        <td style="text-align: left" colspan="3">
                            <asp:TextBox ID="tbAlipay" runat="server" CssClass="TextBox" Width="300px" ToolTip="请输入支付宝帐号" MaxLength="40"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row PaymentInfo Bank" id="trBank" runat="server" style="display: none">
                        <td class="FieldHeader">银行账户：
                        </td>
                        <td style="text-align: left" colspan="3">
                            <asp:TextBox ID="tbBankName" runat="server" CssClass="TextBox" Width="100px" ToolTip="请输入银行名称" Text="-银行名称-" MaxLength="10"></asp:TextBox>
                            <asp:TextBox ID="tbBankAccount" runat="server" CssClass="TextBox" Width="250px" ToolTip="请输入银行帐号" Text="-银行帐号-" MaxLength="30"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">收货地址：
                        </td>
                        <td style="text-align: left" colspan="3">
                            <asp:TextBox ID="tbOrderAddress" runat="server" CssClass="TextBox" Width="300px"
                                TextMode="MultiLine" Rows="3" MaxLength="200"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvOrderAddress" runat="server" ControlToValidate="tbOrderAddress"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">快递方式：
                        </td>
                        <td style="text-align: left" colspan="3">
                            <asp:RadioButtonList ID="rblOrderPostage" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" CssClass="RadioButtonList">
                                <asp:ListItem Text="12元（江浙沪 - 顺丰速运）" Value="12"></asp:ListItem>
                                <asp:ListItem Text="18元（全国其他地区 - 顺丰速运）" Value="18" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr class="CommandRow">
                        <td colspan="4">-- 商品信息栏 --
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">球衣类型：
                        </td>
                        <td style="text-align: left" colspan="3" id="tdReplicaKit">
                            <asp:DropDownList ID="ddlReplicaKit" runat="server" OnDataBound="ddlReplicaKit_DataBound">
                            </asp:DropDownList>
                            <asp:HyperLink ID="hlReplicaKitPage" runat="server" Text="【官网链接】" Target="_blank"></asp:HyperLink>
                            <asp:RequiredFieldValidator ID="rfvReplicaKit" runat="server" ControlToValidate="ddlReplicaKit"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">球衣尺寸：
                        </td>
                        <td style="text-align: left">
                            <asp:DropDownList ID="ddlOrderItemSize" runat="server" Visible="false">
                                <asp:ListItem Value="0" Text="--请选择球衣尺寸--"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="tbOrderItemSize" runat="server" CssClass="TextBox" Width="40px" MaxLength="10"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvOrderItemSize" runat="server" ControlToValidate="tbOrderItemSize"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan"></asp:RequiredFieldValidator>
                            <a href="http://bbs.arsenalcn.com/showtopic.aspx?topicid=107237&postid=1794623#1794623" target="_blank">【欧版尺码表】</a>
                        </td>
                        <td class="FieldHeader">印字印号：
                        </td>
                        <td style="text-align: left" id="tdPrinting">
                            <asp:DropDownList ID="ddlPlayerDetail" runat="server" OnDataBound="ddlPlayerDetail_DataBound" Width="140px">
                            </asp:DropDownList>
                            <asp:Label ID="lblPricePlayerDetail" runat="server" CssClass="PricePlayerDetail"></asp:Label>
                        </td>
                    </tr>
                    <tr class="Row CustomPrinting" id="trCustomPrinting" runat="server" style="display: none">
                        <td class="FieldHeader">个性化印号：
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="tbPlayerNumber" runat="server" CssClass="TextBox" Width="150px" MaxLength="5"></asp:TextBox>
                        </td>
                        <td class="FieldHeader">个性化印字：
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="tbPlayerName" runat="server" CssClass="TextBox" Width="150px" MaxLength="20"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">特殊字体：
                        </td>
                        <td style="text-align: left" colspan="3" id="tdArsenalFont">
                            <asp:CheckBox ID="cbArsenalFont" runat="server" Text="阿森纳杯赛字体" ToolTip="只限部分现役球员" />
                            <a href="http://arsenaldirect.arsenal.com/icat/kitbuilder/" target="_blank">【效果体验】</a>
                            <asp:Label ID="lblPriceArsenalFont" runat="server" CssClass="PriceArsenalFont"></asp:Label>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">英超袖标：
                        </td>
                        <td style="text-align: left" colspan="3" id="tdPremierPatch">
                            <asp:RadioButtonList ID="rblPremierPatch" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" CssClass="RadioButtonList">
                                <asp:ListItem Text="无需" Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="1个" Value="1"></asp:ListItem>
                                <asp:ListItem Text="2个" Value="2"></asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:Label ID="lblPricePremierPatch" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">欧冠袖标：
                        </td>
                        <td style="text-align: left" colspan="3" id="tdChampionPatch">
                            <asp:RadioButtonList ID="rblChampionPatch" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" CssClass="RadioButtonList">
                                <asp:ListItem Text="无需" Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="1个" Value="1"></asp:ListItem>
                                <asp:ListItem Text="2个（R章）" Value="2"></asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:Label ID="lblPriceChampionPatch" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">备注：
                        </td>
                        <td style="text-align: left" colspan="3">
                            <asp:TextBox ID="tbOrderDescription" runat="server" CssClass="TextBox" Width="300px"
                                TextMode="MultiLine" Rows="4"></asp:TextBox>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="FooterBtnBar">
                <asp:Button ID="btnSubmit" runat="server" Text="保存订单信息" CssClass="InputBtn SubmitBtn" OnClick="btnSubmit_Click" />
                <input id="btnReset" type="reset" value="重置表单" class="InputBtn" />
            </div>
        </div>
        <div id="rightPanel">
            <div class="InfoPanel" id="pnlProductImage" style="display: none">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
                    <a>商品图片</a></h3>
                <div class="Block">
                    <img src="uploadfiles/logo_ArsenalChina.png" alt="" style="width: 250px; margin: 2px;" />
                </div>
            </div>
            <div class="InfoPanel">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
                    <a>ACN团购说明</a></h3>
                <div class="Block">
                    <p>(1). Arsenal官方专卖店上的各款主客场球衣均可预订，团代购价格(CNY) = 纪念品正价(GBP) × 约定汇率 + 国内快递费（从中国上海发到国内各地的运费，只发<em>【顺丰速运】</em>）。</p>
                    <p>(2). Arsenal官方专卖店网址： <a href="http://arsenaldirect.arsenal.com/" target="_blank"><em>http://arsenaldirect.arsenal.com</em></a></p>
                    <p>(3). 此功能入口只负责阿森纳各款正品球衣的订购。若需订购其他纪念品，请点击菜单中的<a href="iArsenalOrder_ArsenalDirect.aspx" target="_blank"><em>【纪念品团购服务】</em></a>。</p>
                    <p>(4). 订购操作中如有问题，请咨询官方球迷会服务微信号：<em>iArsenalCN</em>。加入时验证方式为您注册本服务中心的真实姓名和手机号码，并注明<em>【团购】</em>字样。 </p>
                </div>
            </div>
            <uc2:PortalBulkOrderInfo ID="ucPortalBulkOrderInfo" runat="server" />
            <div class="InfoPanel">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
                    <a>球衣印字印号说明</a></h3>
                <div class="Block">
                    <p>(1). 您可以下拉框中选择现役球员印字印号；如需自定义印字印号，请在下拉框选<em>自定义</em>，并出现的文本框中填写号码与印字即可。</p>
                    <p style="display: none">(2). 特殊字体“阿森纳字体”为胶印，具体效果可<a href="http://arsenaldirect.arsenal.com/icat/kitbuilder/" target="_blank">【点击体验】</a>，阿森纳字体不接受自定义印字印号。选择了阿森纳字体后，印字印号会自动按阿森纳字体的费用结算。</p>
                    <p>(2). 因球衣定制的特殊性（选印名字、号码、袖标等个性化选择），应提交订单后尽快付款；若拒付全额款项的，我们将视为无效的订单，敬请配合和谅解。</p>
                </div>
            </div>
        </div>
        <div class="Clear">
        </div>
    </div>
</asp:Content>
