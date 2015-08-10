<%@ Page Language="C#" MasterPageFile="iArsenalMaster.Master" AutoEventWireup="true"
    CodeBehind="iArsenalOrder_MatchTicket.aspx.cs" Inherits="iArsenal.Web.iArsenalOrder_MatchTicket"
    Title="阿森纳新赛季比赛主场球票预订" Theme="iArsenal" %>

<%@ Register Src="Control/PortalSitePath.ascx" TagName="PortalSitePath" TagPrefix="uc1" %>
<%@ Register Src="Control/PortalMatchInfo.ascx" TagName="PortalMatchInfo" TagPrefix="uc2" %>
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
            $.datepicker.setDefaults({ dateFormat: 'yy-mm-dd', changeMonth: true, changeYear: true });
            $("#tdTravelDate .TextBox").datepicker();

            NationDataBindImpl($("#tdRegion"));
        });
    </script>
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <div id="banner" style="height: 250px">
        <a href="http://bbs.arsenalcn.com/showtopic-107269.aspx" target="_blank">
            <img src="uploadfiles/banner/banner20130518.png" alt="阿森纳新赛季比赛主场球票预订" /></a>
    </div>
    <div id="ACN_Main">
        <uc1:PortalSitePath ID="ucPortalSitePath" runat="server" />
        <div id="mainPanel">
            <table class="DataView FormView">
                <thead>
                    <tr class="Header">
                        <th colspan="4" class="FieldColumn">
                            <a name="anchorBack" id="anchorBack">欢迎进入新赛季比赛主场球票预订系统，请仔细确认并填写以下信息：</a>
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
                    <tr class="Row">
                        <td class="FieldHeader">身份证号：
                        </td>
                        <td class="FieldColspan" colspan="3">
                            <asp:TextBox ID="tbIDCardNo" runat="server" CssClass="TextBox" Width="400px" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvIDCardNo" runat="server" ControlToValidate="tbIDCardNo"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">护照编号：
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbPassportNo" runat="server" CssClass="TextBox" Width="150px" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvPassportNo" runat="server" ControlToValidate="tbPassportNo"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan"></asp:RequiredFieldValidator>
                        </td>
                        <td class="FieldHeader">护照姓名：
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbPassportName" runat="server" CssClass="TextBox" Width="150px" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvPassportName" runat="server" ControlToValidate="tbPassportName"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">手机：
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbMobile" runat="server" CssClass="TextBox" Width="150px" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvMobile" runat="server" ControlToValidate="tbMobile"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan"></asp:RequiredFieldValidator>
                        </td>
                        <td class="FieldHeader">微信/QQ：
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbQQ" runat="server" CssClass="TextBox" Width="150px" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvQQ" runat="server" ControlToValidate="tbQQ"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">邮箱：
                        </td>
                        <td class="FieldColspan" colspan="3">
                            <asp:TextBox ID="tbEmail" runat="server" CssClass="TextBox" Width="300px" MaxLength="40"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="tbEmail"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="CommandRow">
                        <td colspan="4">-- 订票信息栏 --
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">比赛信息：
                        </td>
                        <td class="FieldColspan" colspan="3">
                            <asp:Label ID="lblMatchTicketInfo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">比赛时间：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblMatchTicketPlayTime" runat="server"></asp:Label>
                        </td>
                        <td class="FieldHeader">比赛等级：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblMatchTicketRank" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">预订限制：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblAllowMemberClass" runat="server"></asp:Label>
                        </td>
                        <td class="FieldHeader">出行时间：
                        </td>
                        <td class="FieldColumn" id="tdTravelDate">
                            <asp:TextBox ID="tbTravelDate" runat="server" CssClass="TextBox" Width="150px" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">备注：
                        </td>
                        <td class="FieldColspan" colspan="3">
                            <asp:TextBox ID="tbOrderDescription" runat="server" CssClass="TextBox" Width="300px"
                                TextMode="MultiLine" Rows="4"></asp:TextBox>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="FooterBtnBar">
                <asp:Button ID="btnSubmit" runat="server" Text="保存订单信息" CssClass="InputBtn SubmitBtn" OnClick="btnSubmit_Click" />
                <input id="btnReset" type="reset" value="重置表单" class="InputBtn" />
                <input id="btnBack" type="button" value="返回比赛列表" class="InputBtn" onclick="window.location.href = 'iArsenalOrder_MatchList.aspx';" />
            </div>
        </div>
        <div id="rightPanel">
            <uc2:PortalMatchInfo ID="ctrlPortalMatchInfo" runat="server" />
            <div class="InfoPanel">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
                    <a>订票须知</a></h3>
                <div class="Block">
                    <p>1. 由于球迷会预订平价球票供不应求，从本赛季开始所有预订联赛、足总杯、欧冠比赛的主场球票，均需升级为<a href="iArsenalMemberPeriod.aspx" target="_blank"><em>【收费会员】</em></a>。</p>
                    <p>2. 根据俱乐部要求，为防止黄牛倒票现象，每一个预订球票会员必须以实名身份登记备案，<em>【每一个会员资格只能买一张票】</em>。同行者也需登记备案，并升级为<a href="iArsenalMemberPeriod.aspx" target="_blank"><em>【收费会员】</em></a>方能预订购买。</p>
                    <p>3. 所有球票仍按票面平价*汇率（比市场汇率略高，现为<em>11.5</em>）销售，此价格已包括送票费、汇率手续费等，统一采用人民币结算。结算确认成功后，原则上直至比赛日取票、看球，不再额外收取任何费用。</p>
                </div>
            </div>
            <div class="InfoPanel">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
                    <a>球票信息</a></h3>
                <div class="Block">

                    <p>(1).球票座位为著名的南看台（海布里大钟 Clock End）的上层前排球票，即电视直播镜头的<em>右侧球门后上层看台</em>。</p>

                    <p>(2).根据比赛的重要程度，一般分为A\B\C三个等级的比赛。本赛季A级比赛票为92磅，B级比赛票为53.5磅，C级比赛票为37.5磅。</p>

                    <p>(3).订票需详细填写左侧表单，并在提交后完成付款。待球票确认后，在比赛当日凭护照去酋长球场指定地点取票入场。</p>

                    <p>(4).普通收费会员可预订的所有场次球票（即非A级或非重要比赛），在截止时间之前预订<em>确保有票</em>。但对于高级收费会员可预订的A级或特定重要比赛，<em>无法确保一定有票</em>。</p>
                </div>
            </div>
        </div>
        <div class="Clear">
        </div>
    </div>
</asp:Content>
