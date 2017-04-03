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
                            <asp:TextBox ID="tbQuantity" runat="server" CssClass="TextBox" Width="100px"></asp:TextBox>
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
                    <a>球票订购指南</a>
                </h3>
                <div class="Block">
                    <p>
                        (1). 阿森纳中国官方球迷会（ACN）的从俱乐部官方商店预订赛季球衣的团购服务，已经运行了多个赛季。在预订球衣的同时，会员还可选择自行心仪的<em>联赛</em>和<em>杯赛</em>两个字体。（杯赛字体又称阿森纳字体）
                    </p>
                    <p>
                        (2). Arsenal官方专卖店，印字印号效果体验： <a href="http://arsenaldirect.arsenal.com/icat/kitbuilder/" target="_blank">
                            <em>http://arsenaldirect.arsenal.com</em>
                        </a>
                    </p>
                    <p>
                        (3). 您可以下拉框中选择现役球员印字印号；如需自定义印字印号，请在下拉框选<em>自定义</em>，并出现的文本框中填写号码与印字即可。
                    </p>
                    <div class="Thumbnail">
                        <img src="uploadfiles/qrcode-wechat-iarsenalcn.gif" alt="iArsenalCN" />
                    </div>
                    <p>
                        (4). 【微信】订购操作中如有问题，请咨询官方球迷会客服微信号：<em>iArsenalCN</em>。加入时验证方式为您注册本服务中心的真实姓名和手机号码，并注明<em>【印字印号】</em>字样。
                    </p>
                </div>
            </div>
            <div class="InfoPanel">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
                    <a>补充说明</a>
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
