<%@ Page Language="C#" MasterPageFile="iArsenalMaster.Master" AutoEventWireup="true"
    CodeBehind="iArsenalOrder_MatchTicket.aspx.cs" Inherits="iArsenal.Web.iArsenalOrder_MatchTicket"
    Title="阿森纳新赛季比赛主场球票预订" Theme="iArsenal" %>

<%@ Register Src="Control/PortalSitePath.ascx" TagName="PortalSitePath" TagPrefix="uc1" %>
<%@ Register Src="Control/PortalMatchInfo.ascx" TagName="PortalMatchInfo" TagPrefix="uc2" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
    <link href="Scripts/jquery.ui/jquery-ui.min.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="Scripts/jquery-ui-1.10.4.min.js"></script>
    <style type="text/css">
        input.Region1, input.Region2
        {
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
        <a href="http://bbs.arsenalcn.com/showtopic-104999.aspx" target="_blank">
            <img src="uploadfiles/banner/banner20130518.png" alt="阿森纳新赛季比赛主场球票预订" /></a>
    </div>
    <div id="ACN_Main">
        <uc1:PortalSitePath ID="ucPortalSitePath" runat="server" />
        <div id="mainPanel">
            <table class="DataView">
                <thead>
                    <tr class="Header">
                        <th colspan="4" style="text-align: left">
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
                        <td class="FieldHeader">QQ：
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
                        <td colspan="4">-- 订票信息栏 --
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">比赛信息：
                        </td>
                        <td style="text-align: left" colspan="3">
                            <asp:Label ID="lblMatchTicketInfo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">比赛时间：
                        </td>
                        <td style="text-align: left; white-space: nowrap;">
                            <asp:Label ID="lblMatchTicketPlayTime" runat="server"></asp:Label>
                        </td>
                        <td class="FieldHeader">比赛等级：
                        </td>
                        <td style="text-align: left; white-space: nowrap;">
                            <asp:Label ID="lblMatchTicketRank" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">球票数量：
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="tbQuantity" runat="server" CssClass="TextBoxRead" Width="50px" Text="1" MaxLength="3"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ControlToValidate="tbQuantity"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan"></asp:RequiredFieldValidator>
                        </td>
                        <td class="FieldHeader">出行时间：
                        </td>
                        <td style="text-align: left;" id="tdTravelDate">
                            <asp:TextBox ID="tbTravelDate" runat="server" CssClass="TextBox" Width="150px" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
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
                <input id="btnBack" type="button" value="返回比赛列表" class="InputBtn" onclick="window.location.href = 'iArsenalOrder_MatchList.aspx';" />
            </div>
        </div>
        <div id="rightPanel">
            <uc2:PortalMatchInfo ID="ctrlPortalMatchInfo" runat="server" />
            <div class="InfoPanel">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
                    <a>订票须知</a></h3>
                <div class="Block">
                    <p>
                        (1). 阿森纳主场球票预订，可以在无需注册成Arsenal Red Member的情况下，<em>通过官方球迷会的渠道以平价买到主场球票</em>。所有比赛列表上的球票在截止时间之前，均可有效预订，待确认后确保有票。
                    </p>
                    <p>
                        (2). 订票需详细填写左侧表单，并在提交后完成付款。待球票确认后，在比赛当日凭<em>护照</em>去酋长球场的<em>西南取票处</em>直接取票即可。部分重要场次，我们会安排专人发票或专属活动。
                    </p>
                    <p>
                        (3). 由于存在汇率转换手续费和英国俱乐部收取的预订费用，球迷会按<em>票面平价价格</em>×略高的英磅汇率（<em>现设定为10.5</em>）的人民币价格，销售给订票会员，交易方式为<em>支付宝或淘宝</em>。
                    </p>
                </div>
            </div>
            <div class="InfoPanel">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
                    <a>球票信息</a></h3>
                <div class="Block">
                    <p>
                        (1).球票座位为著名的南看台（<em>海布里大钟 Clock End</em>）的前排球票，官方球迷会的球票都分布在这里，即电视直播镜头的右侧球门后上层看台。阿森纳主场气氛热烈，我们提供的座位也相对靠前，视野非常清晰。
                    </p>
                    <p>
                        (2).根据比赛的重要程度，一般分为<em>A\B\C</em>三个等级的比赛。<em>A级比赛票为89.5磅，B级比赛票为52磅，C级比赛票为36.5磅</em>。
                    </p>
                    <p>
                        (3).酋长杯、联赛杯、足总杯的球票价格可能不按以上的标准划分，一般比联赛球票便宜不少，故在订票系统中将此设定为<em>特价球票</em>。具体以订票表单上的价格为准。
                    </p>
                </div>
            </div>
        </div>
        <div class="Clear">
        </div>
    </div>
</asp:Content>
