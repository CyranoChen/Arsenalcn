<%@ Page Language="C#" MasterPageFile="iArsenalMaster.Master" AutoEventWireup="true"
    CodeBehind="iArsenalOrder_Printing.aspx.cs" Inherits="iArsenal.Web.iArsenalOrder_Printing"
    Title="阿森纳个性化印字印号服务" Theme="iArsenal" %>

<%@ Register Src="Control/PortalSitePath.ascx" TagName="PortalSitePath" TagPrefix="uc1" %>
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
    <div id="banner" style="height: 170px">
        <a href="http://bbs.arsenalcn.com/showtopic-108435.aspx" target="_blank">
            <img src="uploadfiles/banner/banner20160717.png" alt="阿森纳个性化印字印号服务" />
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
                        <td class="FieldHeader">快递单号与备注：
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
                    <a>印字印号说明</a>
                </h3>
                <div class="Block">
                    <p>
                        (1). 阿森纳中国官方球迷会（ACN）的从俱乐部官方商店预订赛季球衣的团购服务，已经运行了多个赛季。在预订球衣的同时，会员还可选择自行心仪的<em>联赛</em>和<em>杯赛</em>两个字体。（杯赛字体又称阿森纳字体）
                    </p>
                    <p>
                        (2). 您可以下拉框中选择现役球员印字印号；如需自定义印字印号，请在下拉框选<em>自定义</em>，并出现的文本框中填写号码与印字即可。
                    </p>
                    <div class="Thumbnail">
                        <img src="uploadfiles/qrcode-iarsenalcn.gif" alt="iArsenalCN" />
                    </div>
                    <p>
                        (3). 【微信】订购操作中如有问题，请咨询官方球迷会客服微信号：<em>iArsenalCN</em>。加入时验证方式为您注册本服务中心的真实姓名和手机号码，并注明<em>【印字印号】</em>字样。
                    </p>
                </div>
            </div>
            <div class="InfoPanel">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
                    <a>个性化印制</a>
                </h3>
                <div class="Block">
                    <p>
                        (1). 阿森纳字体（杯赛字体）印字印号服务，可在会员提供的本赛季各种球衣款式上印制，包括主场和即将推出的<em>客场、杯赛球衣</em>。
                    </p>
                    <p>
                        (2). 阿森纳字体（杯赛字体）个性化服务，可根据会员要求，自行拼接印字名称、印号数字等，数字最多为2位(00-99)；名字最多为11个英文字母。（根据球衣大小，如婴儿版、儿童版，印字印号的字母数量可能会有所不同）
                    </p>
                    <p>
                        (3). 字体印制服务，可印制联赛字体、杯赛字体于各种球衣、T恤或其他非阿森纳的衣服上。（视具体印制条件评估）
                    </p>
                </div>
            </div>
        </div>
        <div class="Clear"></div>
    </div>
</asp:Content>
