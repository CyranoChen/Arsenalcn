<%@ Page Language="C#" MasterPageFile="iArsenalMaster.Master" AutoEventWireup="true"
    CodeBehind="iArsenalOrder_AsiaTrophy2015.aspx.cs" Inherits="iArsenal.Web.iArsenalOrder_AsiaTrophy2015"
    Title="2015英超亚洲杯阿森纳观战团报名" Theme="iArsenal" %>

<%@ Register Src="Control/PortalSitePath.ascx" TagName="PortalSitePath" TagPrefix="uc1" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
    <link href="Scripts/jquery.ui/jquery-ui.min.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="Scripts/jquery-ui-1.10.4.min.js"></script>
    <style type="text/css">
        input.Region1, input.Region2 {
            display: none;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            NationDataBindImpl($("#tdRegion"));

            var $cbPartner = $("#tdPartner input:checkbox");

            if ($cbPartner.attr("checked") == "checked") {
                $(".Partner").show();
            } else {
                $(".Partner").hide();
            }

            $cbPartner.click(function () {
                $(".Partner").toggle('normal');
            });
        });
    </script>
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <div id="banner" style="height: 400px">
        <a href="#" target="_blank">
            <img src="uploadfiles/banner/banner20150406.png" alt="2015英超亚洲杯阿森纳观战团报名" /></a>
    </div>
    <div id="ACN_Main">
        <uc1:PortalSitePath ID="ucPortalSitePath" runat="server" />
        <div id="mainPanel">
            <table class="DataView">
                <thead>
                    <tr class="Header">
                        <th colspan="4" style="text-align: left">
                            <a name="anchorBack" id="anchorBack">欢迎进入2015英超亚洲杯观赛团报名系统，请仔细确认并填写以下信息：</a>
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
                        <td style="text-align: left">
                            <asp:Label ID="lblMemberName" runat="server"></asp:Label>
                        </td>
                        <td class="FieldHeader" style="width: 110px;">ACN帐号：
                        </td>
                        <td style="text-align: left">
                            <asp:Label ID="lblMemberACNInfo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">现居住地：
                        </td>
                        <td style="text-align: left" id="tdRegion" colspan="3">
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
                    <tr class="Row">
                        <td class="FieldHeader">身份证号：
                        </td>
                        <td style="text-align: left" colspan="3">
                            <asp:TextBox ID="tbIDCardNo" runat="server" CssClass="TextBox" Width="400px" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvIDCardNo" runat="server" ControlToValidate="tbIDCardNo"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">护照编号：
                        </td>
                        <td style="text-align: left; white-space: nowrap;">
                            <asp:TextBox ID="tbPassportNo" runat="server" CssClass="TextBox" Width="150px" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvPassportNo" runat="server" ControlToValidate="tbPassportNo"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan"></asp:RequiredFieldValidator>
                        </td>
                        <td class="FieldHeader">护照姓名：
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="tbPassportName" runat="server" CssClass="TextBox" Width="150px" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvPassportName" runat="server" ControlToValidate="tbPassportName"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">手机：
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="tbMobile" runat="server" CssClass="TextBox" Width="150px" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvMobile" runat="server" ControlToValidate="tbMobile"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan"></asp:RequiredFieldValidator>
                        </td>
                        <td class="FieldHeader">微信/QQ：
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="tbQQ" runat="server" CssClass="TextBox" Width="150px" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvQQ" runat="server" ControlToValidate="tbQQ"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">邮箱：
                        </td>
                        <td style="text-align: left" colspan="3">
                            <asp:TextBox ID="tbEmail" runat="server" CssClass="TextBox" Width="300px" MaxLength="40"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="tbEmail"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="CommandRow">
                        <td colspan="4">-- 旅行信息栏 --
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">观看场次：
                        </td>
                        <td style="text-align: left" colspan="3"></td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">同伴信息：
                        </td>
                        <td style="text-align: left; font-weight: bold;" id="tdPartner">
                            <asp:CheckBox ID="cbPartner" runat="server" Text="限定登记1名出行同伴" ToolTip="如需更多人数出行，请重新注册登记。" />
                        </td>
                        <td class="FieldHeader">
                            <span class="Partner">同伴姓名：</span>
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="tbPartnerName" runat="server" CssClass="TextBox Partner" Width="150px" MaxLength="20"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row Partner">
                        <td class="FieldHeader">同伴关系：
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList ID="ddlPartnerRelation" runat="server">
                                <asp:ListItem Text="--请选择同伴关系--" Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="亲属" Value="1"></asp:ListItem>
                                <asp:ListItem Text="朋友" Value="2"></asp:ListItem>
                                <asp:ListItem Text="其他" Value="3"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="FieldHeader">同伴性别：
                        </td>
                        <td style="text-align: left">
                            <asp:RadioButtonList ID="rblPartnerGender" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" CssClass="RadioButtonList">
                                <asp:ListItem Text="男" Value="true" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="女" Value="false"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr class="AlternatingRow Partner">
                        <td class="FieldHeader">同伴身份证：
                        </td>
                        <td style="text-align: left" colspan="3">
                            <asp:TextBox ID="tbPartnerIDCardNo" runat="server" CssClass="TextBox" Width="400px" MaxLength="20"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row Partner">
                        <td class="FieldHeader">同伴护照号码：
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="tbPartnerPassportNo" runat="server" CssClass="TextBox" Width="150px" MaxLength="20"></asp:TextBox>
                        </td>
                        <td class="FieldHeader">同伴护照姓名：
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="tbPartnerPassportName" runat="server" CssClass="TextBox" Width="150px" MaxLength="20"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">报名方式：</td>
                        <td style="text-align: left;" colspan="3">
                            <asp:RadioButtonList ID="rblIsTicketOnly" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" CssClass="RadioButtonList">
                                <asp:ListItem Text="观赛团报名" Value="Tour" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="仅预定比赛门票（未开通）" Value="Ticket" Enabled="false"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">出行选项：
                        </td>
                        <td style="text-align: left; line-height: 2;" colspan="3">
                            <asp:CheckBoxList ID="cblTravelOption" runat="server" RepeatDirection="Vertical" RepeatLayout="Flow">
                                <asp:ListItem Text="统一购买机票前往新加坡" Value="FLIGHT" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="统一预订宾馆并安排入住" Value="HOTEL" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="参观赛季训练课" Value="TRAINING" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="参加球员见面会（暂未确定）" Value="PARTY"></asp:ListItem>
                                <asp:ListItem Text="参加新加坡当地团一日游" Value="SINGAPORE"></asp:ListItem>
                            </asp:CheckBoxList>
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
                <asp:Button ID="btnSubmit" runat="server" Text="保存订单信息" CssClass="InputBtn" OnClick="btnSubmit_Click" />
                <input id="btnReset" type="reset" value="重置表单" class="InputBtn" />
            </div>
        </div>
        <div id="rightPanel">
            <div class="InfoPanel">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
                    <a>预约报名须知</a></h3>
                <div class="Block">
                    <p>
                        (1). 第一轮报名截止时间至新赛季阿森纳联赛赛程公布之日，约在2013年7月中下旬。
                    </p>
                    <p>
                        (2). 请完整填写出行会员的个人信息，每位会员可以有一个同伴，同伴信息也请填写完整。我们会为会员的个人信息进行保密。
                    </p>
                    <p>
                        (3). 请填写希望出行的时间段，并勾选相关出行选项，如有特殊需求，请填写在备注栏中。
                    </p>
                    <p>
                        (4). 我们会在收到预订报名表后，核对身份信息，并与您取得联系，然后邀请您加入出行专用的QQ群。
                    </p>
                    <p>
                        (5). 如有问题，可与ACN负责人Cyrano联系。QQ：<em>22287842</em>，<a href="http://bbs.arsenalcn.com/usercppostpm.aspx" target="_blank">ACN论坛短消息</a>，Email：<a href="mailto:cyrano@arsenalcn.com"><em>cyrano@arsenalcn.com</em>。</a>
                    </p>
                </div>
            </div>
            <div class="InfoPanel">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
                    <a>出行日程安排（暂定）</a></h3>
                <div class="Block">
                    <p style="font-weight: bold;">注：观战团行程大约在5~7天左右，具体安排预计如下，详见<a href="http://bbs.arsenalcn.com/showtopic.aspx?topicid=104630&postid=1767535#1767535" target="_blank"><em>点击查看</em></a>。</p>
                    <p>(1). <em>DAY1 (周五)</em> 各自出行前往英国伦敦，航班约在10个小时左右。北京时间上午出发，英国时间当天下午能到，自行到酒店办理住宿。（时差为8个小时，余下行程开始以英国当地时间为准）</p>
                    <p>(2). <em>DAY2 (周六)</em> 阿森纳地铁站集合，开始比赛日当天活动与观战。比赛之后，可自由组合去酒吧、共进晚餐等。</p>
                    <p>(3). <em>DAY3 (周日)</em> 非比赛日的参观酋长球场和博物馆的活动。半天自由活动。</p>
                    <p>(4). <em>DAY4 (周一)</em> 伦敦一日自由行。不参加的会员也可自行活动，或自由组合。</p>
                    <p>(5). <em>DAY5 (周二)</em> 阿森纳地铁站集合，开始比赛日当天活动与观战，一般是晚上19:45或者是20:45的比赛。</p>
                    <p>(6). <em>DAY6 (周三)</em> 自由活动，或可安排回国航班。（考虑到时差，能到中国大概已经是北京时间周四了）</p>
                </div>
            </div>
        </div>
        <div class="Clear">
        </div>
    </div>
</asp:Content>
