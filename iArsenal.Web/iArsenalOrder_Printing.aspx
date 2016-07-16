<%@ Page Language="C#" MasterPageFile="iArsenalMaster.Master" AutoEventWireup="true"
    CodeBehind="iArsenalOrder_Printing.aspx.cs" Inherits="iArsenal.Web.iArsenalOrder_Printing"
    Title="阿森纳个性化印字印号服务" Theme="iArsenal" %>

<%@ Register Src="Control/PortalSitePath.ascx" TagName="PortalSitePath" TagPrefix="uc1" %>
<%@ Register Src="Control/PortalBulkOrderInfo.ascx" TagName="PortalBulkOrderInfo" TagPrefix="uc2" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
    <script type="text/javascript">
        $(function () {
            var $ddlPlayerDetail = $("#tdPrinting select");
            var $trCustomPrinting = $(".CustomPrinting");

            $ddlPlayerDetail.change(function () {
                if ($(this).val() === "custom")
                    $trCustomPrinting.show();
                else
                    $trCustomPrinting.hide();
            });
        });
    </script>
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <div id="banner" style="height: 270px">
        <a href="http://arsenaldirect.arsenal.com/" target="_blank">
            <img src="uploadfiles/banner/banner20160528.png" alt="阿森纳个性化印字印号服务" />
        </a>
    </div>
    <div id="ACN_Main">
        <uc1:PortalSitePath ID="ucPortalSitePath" runat="server" />
        <div id="mainPanel">
            <table class="DataView FormView">
                <thead>
                    <tr class="Header">
                        <th colspan="4" class="FieldColumn">
                            <a name="anchorBack" id="anchorBack">欢迎进入阿森纳个性化印字印号服务系统，请仔细确认并填写以下信息：</a>
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
                        <td class="FieldColumn">
                            <asp:Label ID="lblMemberName" runat="server"></asp:Label>
                        </td>
                        <td class="FieldHeader" style="width: 100px">ACN帐号：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblMemberACNInfo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">手机：
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbOrderMobile" runat="server" CssClass="TextBox" Width="150px" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvOrderMobile" runat="server" ControlToValidate="tbOrderMobile"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan">
                            </asp:RequiredFieldValidator>
                        </td>
                        <td class="FieldHeader">微信：
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbMemberWeChat" runat="server" CssClass="TextBox" Width="150px" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvMemberWeChat" runat="server" ControlToValidate="tbMemberWeChat"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">收货地址：
                        </td>
                        <td class="FieldColspan" colspan="3">
                            <asp:TextBox ID="tbOrderAddress" runat="server" CssClass="TextBox" Width="300px"
                                TextMode="MultiLine" Rows="3" MaxLength="200">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvOrderAddress" runat="server" ControlToValidate="tbOrderAddress"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="CommandRow">
                        <td colspan="4">-- 商品信息栏 --
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">类型：
                        </td>
                        <td class="FieldColspan" colspan="3">
                            <asp:RadioButtonList ID="rblFontSelected" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" CssClass="RadioButtonList">
                                <asp:ListItem Text="<em>联赛字体 - 128.00元</em>" Value="PremierFont"></asp:ListItem>
                                <asp:ListItem Text="<em>阿森纳字体 - 158.00元</em>" Value="ArsenalFont" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>
                            <a href="http://arsenaldirect.arsenal.com/icat/kitbuilder/" target="_blank">【效果体验】</a>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">印字印号：
                        </td>
                        <td class="FieldColspan" colspan="3" id="tdPrinting">
                            <asp:DropDownList ID="ddlPlayerDetail" runat="server" OnDataBound="ddlPlayerDetail_DataBound">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="Row CustomPrinting" id="trCustomPrinting" runat="server" style="display: none">
                        <td class="FieldHeader">个性化印号：
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbPlayerNumber" runat="server" CssClass="TextBox" Width="150px" MaxLength="5"></asp:TextBox>
                        </td>
                        <td class="FieldHeader">个性化印字：
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbPlayerName" runat="server" CssClass="TextBox" Width="150px" MaxLength="20"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">备注：
                        </td>
                        <td class="FieldColspan" colspan="3">
                            <asp:TextBox ID="tbOrderDescription" runat="server" CssClass="TextBox" Width="300px"
                                TextMode="MultiLine" Rows="4">
                            </asp:TextBox>
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
            <div class="InfoPanel">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
                    <a>ACN团购说明</a>
                </h3>
                <div class="Block">
                    <p>
                        (1). Arsenal官方专卖店上的各款主客场球衣均可预订，团代购价格(CNY) = 纪念品正价(GBP) × 约定汇率 + 国内快递费（从中国上海发到国内各地的运费，只发<em>【顺丰速运】</em>）。
                    </p>
                    <p>
                        (2). Arsenal官方专卖店网址： <a href="http://arsenaldirect.arsenal.com/" target="_blank">
                            <em>http://arsenaldirect.arsenal.com</em>
                        </a>
                    </p>
                    <p>
                        (3). 此功能入口只负责阿森纳各款正品球衣的订购。若需订购其他纪念品，请点击菜单中的<a href="iArsenalOrder_ArsenalDirect.aspx" target="_blank">
                            <em>【纪念品团购服务】</em>
                        </a>。
                    </p>
                    <p>
                        (4). 订购操作中如有问题，请咨询官方球迷会服务微信号：<em>iArsenalCN</em>。加入时验证方式为您注册本服务中心的真实姓名和手机号码，并注明<em>【团购】</em>字样。
                    </p>
                </div>
            </div>
            <uc2:PortalBulkOrderInfo ID="ucPortalBulkOrderInfo" runat="server" />
            <div class="InfoPanel">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
                    <a>球衣印字印号说明</a>
                </h3>
                <div class="Block">
                    <p>
                        (1). 您可以下拉框中选择现役球员印字印号；如需自定义印字印号，请在下拉框选<em>自定义</em>，并出现的文本框中填写号码与印字即可。
                    </p>
                    <p style="display: none">
                        (2). 特殊字体“阿森纳字体”为胶印，具体效果可<a href="http://arsenaldirect.arsenal.com/icat/kitbuilder/" target="_blank">【点击体验】</a>，阿森纳字体不接受自定义印字印号。选择了阿森纳字体后，印字印号会自动按阿森纳字体的费用结算。
                    </p>
                    <p>(2). 因球衣定制的特殊性（选印名字、号码、袖标等个性化选择），应提交订单后尽快付款；若拒付全额款项的，我们将视为无效的订单，敬请配合和谅解。</p>
                </div>
            </div>
        </div>
        <div class="Clear"></div>
    </div>
</asp:Content>
