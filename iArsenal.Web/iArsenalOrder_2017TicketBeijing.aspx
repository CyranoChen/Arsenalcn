<%@ Page Language="C#" MasterPageFile="iArsenalMaster.Master" AutoEventWireup="true"
    CodeBehind="iArsenalOrder_2017TicketBeijing.aspx.cs" Inherits="iArsenal.Web.iArsenalOrder_2017TicketBeijing"
    Title="阿森纳2017中国行国际冠军杯北京站-官方球迷会专属看台球票订购" Theme="iArsenal" %>

<%@ Register Src="Control/PortalSitePath.ascx" TagName="PortalSitePath" TagPrefix="uc1" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
    <style type="text/css">
        input.Region1, input.Region2 {
            display: none;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            NationDataBindImpl($("#tdRegion"));
            $(".idcardno").attr("placeholder", "领票时需核对证件信息，身份证/护照");
            $(".quantity").attr("placeholder", "1 ~ 6");
            $(".description").attr("placeholder", "可注明指定看台（123，124，125）");
        });
    </script>
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <div id="banner" style="height: 580px">
        <a href="http://bbs.arsenalcn.com/showtopic-108574.aspx" target="_blank">
            <img src="uploadfiles/banner/banner20170403.png" alt="阿森纳2017中国行国际冠军杯北京站球票预订指南" />
        </a>
    </div>
    <div id="ACN_Main">
        <uc1:PortalSitePath ID="ucPortalSitePath" runat="server" />
        <div id="mainPanel">
            <table class="DataView FormView">
                <thead>
                    <tr class="Header">
                        <th colspan="4" class="FieldColumn">
                            <a name="anchorBack" id="anchorBack">欢迎进入阿森纳2017中国行国际冠军杯北京站球票预订系统，请仔细确认并填写以下信息：</a>
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
                    <tr class="Row">
                        <td class="FieldHeader">身份证号：
                        </td>
                        <td class="FieldColumn" colspan="3">
                            <asp:TextBox ID="tbIDCardNo" runat="server" CssClass="TextBox idcardno" Width="300px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvIDCardNo" runat="server" ControlToValidate="tbIDCardNo"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">现居住地：
                        </td>
                        <td class="FieldColumn" id="tdRegion" colspan="3">
                            <asp:DropDownList ID="ddlNation" runat="server" CssClass="Nation">
                                <asp:ListItem Value="" Text="--请选择国家--" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="中国" Text="中国"></asp:ListItem>
                                <asp:ListItem Value="其他" Text="其他"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="tbNation" runat="server" CssClass="TextBox OtherNation" Width="150px"></asp:TextBox>
                            <select id="ddlRegion1">
                            </select>
                            <asp:TextBox ID="tbRegion1" runat="server" CssClass="TextBox Region1" Width="50px"></asp:TextBox>
                            <select id="ddlRegion2">
                            </select>
                            <asp:TextBox ID="tbRegion2" runat="server" CssClass="TextBox Region2" Width="50px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="CommandRow">
                        <td colspan="4">-- 商品信息栏 --
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">球票：</td>
                        <td class="FieldColspan" colspan="3">
                            <asp:RadioButtonList ID="rblTicket" runat="server"></asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">数量：
                        </td>
                        <td class="FieldColspan" colspan="3">
                            <asp:TextBox ID="tbQuantity" runat="server" CssClass="TextBox quantity" Width="100px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ControlToValidate="tbQuantity"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan">
                            </asp:RequiredFieldValidator>
                            <span>每个订单最多订 <em>6</em> 张同等价位的球票</span>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">备注：
                        </td>
                        <td class="FieldColspan" colspan="3">
                            <asp:TextBox ID="tbOrderDescription" runat="server" CssClass="TextBox description" Width="300px"
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
                    <a>球票订购指南</a>
                </h3>
                <div class="Block">
                    <p>
                        (1). 阿森纳2017中国行国际冠军杯北京站比赛， 将于7月22日在北京国家体育场（鸟巢）举行。阿森纳中国官方球迷会（ACN）和主办方合作推出球迷会专属看台，现在订购并付款，可选择具体看台（<em>123-124-125</em>）。
                    </p>
                    <p>
                        (2). 如需订购多张不同看台球票，请分开提交订单。每个订单对应一种类型的球票，并在同一看台联号。
                    </p>
                    <div class="Thumbnail">
                        <img src="uploadfiles/qrcode-iarsenalcn.gif" alt="iArsenalCN" />
                    </div>
                    <p>
                        (3). 【微信】订购操作中如有问题，请咨询官方球迷会客服微信号：<em>iArsenalCN</em>。加入时验证方式为您注册本服务中心的真实姓名和手机号码，并注明<em>【北京站球票】</em>。
                    </p>
                    <p>
                        (4). 如需大批量订购球票（<em>超过20张</em>），且希望选择看台并联号，可直接与球票负责人联系（<em>13818059707，陈先生</em>）。
                    </p>
                </div>
            </div>
            <div class="InfoPanel">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
                    <a>补充说明</a>
                </h3>
                <div class="Block">
                    <p>
                        (1). 各位如果购买公开渠道（大麦网）销售的球票，可能无法选坐，只能选票价，届时有可能无法保证你的看台，甚至无法保证所在球场的看台层次。我们的目标是为了让阿森纳球迷，特别是球迷会会员能集中坐在一起。所以向主办方申请了<em>专属看台</em>。
                    </p>
                    <p>
                        (2). 因本次团购活动完全由主办方给予支持，如在后续团购或出票过程中，可能会因各种原因产生的意外，导致座位名额减少、看台减少或出票延迟的情况。届时我们将尽全力保障各位球迷的球票费用可得到退款。
                    </p>
                    <p>
                        (3). 因球迷个人原因或行程改变，导致无法成行的，在正式出票前可申请<em>全额退款</em>。
                    </p>
                </div>
            </div>
        </div>
        <div class="Clear"></div>
    </div>
</asp:Content>
