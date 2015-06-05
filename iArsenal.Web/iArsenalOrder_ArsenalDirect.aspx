<%@ Page Language="C#" MasterPageFile="iArsenalMaster.Master" AutoEventWireup="true"
    CodeBehind="iArsenalOrder_ArsenalDirect.aspx.cs" Inherits="iArsenal.Web.iArsenalOrder_ArsenalDirect"
    Title="阿森纳官方纪念品团购服务" Theme="iArsenal" %>

<%@ Register Src="Control/PortalSitePath.ascx" TagName="PortalSitePath" TagPrefix="uc1" %>
<%@ Register Src="Control/PortalBulkOrderInfo.ascx" TagName="PortalBulkOrderInfo" TagPrefix="uc2" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
    <link href="Scripts/jquery.ui/jquery-ui.min.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="Scripts/jquery-ui-1.10.4.min.js"></script>
    <script type="text/javascript" src="Scripts/json2.js"></script>
    <script type="text/javascript">
        var cacheProductCodeList = {};
        var cacheOrderItem = {};

        $(function () {
            var $btnSubmit = $(".FooterBtnBar .SubmitBtn");
            $btnSubmit.click(function () { return PackageWishOrderItemList($('tbody.ArsenalDirect_WishList')); });

            $.getJSON("ServerOrderItemCheck.ashx", { OrderItem: "0" }, function (data, status, xhr) {
                if (status == "success" && data != null) {
                    if (data.result == "error") { data = ""; }

                    cacheOrderItem = data;

                    $btnSubmit.prop("disabled", false);
                } else {
                    $btnSubmit.prop("disabled", true);
                }
            });

            $.getJSON("ServerProductCheck.ashx", { IsActive: "true", ProductType: "0" }, function (data, status, xhr) {
                if (status == "success" && data != null) {
                    if (data.result == "error") { data = ""; }

                    cacheProductCodeList = data;
                }

                UnPackageWishOrderItemList($("tbody.ArsenalDirect_WishList"));
            });
        });

    </script>
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <div id="banner" style="height: 400px">
        <a href="http://bbs.arsenalcn.com/showtopic-108067.aspx" target="_blank">
            <img src="uploadfiles/banner/banner20150601.png" alt="阿森纳官方纪念品团购服务" /></a>
    </div>
    <div id="ACN_Main">
        <uc1:PortalSitePath ID="ucPortalSitePath" runat="server" />
        <div id="mainPanel">
            <table class="DataView">
                <thead>
                    <tr class="Header">
                        <th colspan="4" class="FieldColumn">
                            <a name="anchorBack" id="anchorBack">欢迎进入阿森纳官方纪念品团购服务，请仔细确认并填写以下信息：</a>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="CommandRow">
                        <td colspan="4">-- 会员信息栏 --
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader" style="width: 110px;">真实姓名：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblMemberName" runat="server"></asp:Label>
                        </td>
                        <td class="FieldHeader" style="width: 110px;">ACN帐号：
                        </td>
                        <td class="FieldColumn">
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
                        <td class="FieldHeader">邮箱：
                        </td>
                        <td style="text-align: left; white-space: nowrap;">
                            <asp:TextBox ID="tbEmail" runat="server" CssClass="TextBox" Width="150px" MaxLength="40"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="tbEmail"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">收货地址：
                        </td>
                        <td class="FieldColumn" colspan="3">
                            <asp:TextBox ID="tbOrderAddress" runat="server" CssClass="TextBox" Width="300px"
                                TextMode="MultiLine" Rows="3" MaxLength="200"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvOrderAddress" runat="server" ControlToValidate="tbOrderAddress"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </tbody>
                <tbody class="ArsenalDirect_WishList">
                    <tr class="CommandRow">
                        <td colspan="3">-- 商品信息栏 --
                           
                            <asp:TextBox ID="tbWishOrderItemListInfo" runat="server" CssClass="WishOrderItemListInfo"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvWishOrderItemListInfo" runat="server" ControlToValidate="tbWishOrderItemListInfo"
                                Display="Dynamic" ErrorMessage="*请填写订购纪念品信息（商品编码和数量必填）" CssClass="ValiSpan"></asp:RequiredFieldValidator>
                        </td>
                        <td style="text-align: right">
                            <a class="LinkBtn AddBtn" onclick="InsertWishItem($('tbody.ArsenalDirect_WishList'));">添加商品</a>
                        </td>
                    </tr>
                    <tr class="SelectedRow WishItem">
                        <td>
                            <a class="DelBtn" onclick="DeleteWishItem($(this).parents('tr.WishItem'));"></a>
                            <input type="hidden" class="TextBox OrderItemID" />
                            <input type="hidden" class="TextBox ProductGuid" />
                            <input type="text" class="TextBox ProductCode" style="width: 60px" placeholder="商品编码" title="商品编码" maxlength="10" />
                        </td>
                        <td>
                            <input type="text" class="TextBox ProductName" style="width: 180px" placeholder="商品名称" title="商品名称" maxlength="50" />
                        </td>
                        <td>
                            <input type="text" class="TextBox ProductSize" style="width: 30px" placeholder="尺寸" title="尺寸" maxlength="20" />
                            <input type="text" class="TextBox ProductQuantity" style="width: 30px" placeholder="数量" title="数量" maxlength="3" />
                        </td>
                        <td style="text-align: right">
                            <input type="hidden" class="TextBox ProductPrice" />
                            <span class="ProductPriceInfo"></span>
                        </td>
                    </tr>
                    <tr class="AlternatingRow WishRemark">
                        <td class="FieldHeader">备注：
                        </td>
                        <td class="FieldColumn" colspan="3">
                            <asp:TextBox ID="tbOrderDescription" runat="server" CssClass="TextBox" Width="300px"
                                TextMode="MultiLine" Rows="4"></asp:TextBox>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="FooterBtnBar">
                <asp:Button ID="btnSubmit" runat="server" Text="保存订单信息" CssClass="InputBtn SubmitBtn"
                    OnClick="btnSubmit_Click" />
                <input id="btnReset" type="reset" value="重置表单" class="InputBtn" />
            </div>
        </div>
        <div id="rightPanel">
            <div class="InfoPanel">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
                    <a>ACN团代购说明</a></h3>
                <div class="Block">
                    <p>(1). Arsenal官方专卖店上的所有纪念品均可预订，团代购价格(CNY) = 纪念品正价(GBP) × 约定汇率。</p>
                    <p>(2). 约定汇率含全部国际运费和海关关税分摊；国内快递费自理（从中国上海发到国内各地的运费，只发<em>【顺丰速运】</em>）</p>
                    <p>(2). Arsenal官方专卖店网址： <a href="http://arsenaldirect.arsenal.com/" target="_blank"><em>http://arsenaldirect.arsenal.com</em></a></p>
                    <p>(3). 此功能入口负责除主客场球衣外，所有阿森纳纪念品的订购。若需订购各款球衣，请点击菜单中的<a href="iArsenalOrder_ReplicaKit.aspx" target="_blank"><em>【主客场球衣预订服务】</em></a>。</p>
                    <p>(4). 订购操作中如有问题，请咨询官方球迷会服务微信号：<em>iArsenalCN</em>。加入时验证方式为您注册本服务中心的真实姓名和手机号码，并注明<em>【团购】</em>字样。 </p>
                </div>
            </div>
            <uc2:PortalBulkOrderInfo ID="ucPortalBulkOrderInfo" runat="server" />
            <div class="InfoPanel">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
                    <a>纪念品填写说明</a></h3>
                <div class="Block">
                    <p>因系统中部分纪念品信息可能无法及时与官方商店同步更新，所以可能导致填写商品编号后无对应信息或信息有误。请按正确格式填写并提交即可，如有问题可在备注框中留言。后台管理员将会进行审核并补全纪念品信息。</p>
                </div>
            </div>
        </div>
        <div class="Clear">
        </div>
    </div>
</asp:Content>
