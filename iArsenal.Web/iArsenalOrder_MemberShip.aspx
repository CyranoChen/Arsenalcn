<%@ Page Language="C#" MasterPageFile="iArsenalMaster.Master" AutoEventWireup="true"
    CodeBehind="iArsenalOrder_MemberShip.aspx.cs" Inherits="iArsenal.Web.iArsenalOrder_MemberShip"
    Title="ACN收费会员登记" Theme="iArsenal" %>

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
        });
    </script>
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <div id="banner">
        <a href="http://bbs.arsenalcn.com/showtopic.aspx?topicid=107269&postid=1795109#1795109" target="_blank">
            <img src="uploadfiles/banner/banner20120511.png" alt="球迷会收费会员说明与积分制度" /></a>
    </div>
    <div id="ACN_Main">
        <uc1:PortalSitePath ID="ucPortalSitePath" runat="server" />
        <div id="mainPanel">
            <table class="DataView">
                <thead>
                    <tr class="Header">
                        <th colspan="4" style="text-align: left">
                            <a name="anchorBack" id="anchorBack">欢迎进入ACN收费会员登记系统，请仔细确认并填写以下信息：</a>
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
                            <asp:TextBox ID="tbNation" runat="server" CssClass="TextBox OtherNation" Width="100px" ToolTip="请输入所在地区"></asp:TextBox>
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
                        <td colspan="4">-- 会籍信息栏 --
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">会籍等级：</td>
                        <td style="text-align: left" colspan="3">
                            <asp:Label ID="lblMemberClass" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">专属卡号：
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="tbMemberCardNo" runat="server" CssClass="TextBox" Width="50px" MaxLength="3"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvMemberCardNo" runat="server" ControlToValidate="tbMemberCardNo"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvMemberCardNo" runat="server" MinimumValue="100" MaximumValue="999"
                                ControlToValidate="tbMemberCardNo" Display="Static" ErrorMessage="提示：100~999" CssClass="ValiSpan"></asp:RangeValidator>
                        </td>
                        <td class="FieldHeader">会籍有效期：
                        </td>
                        <td style="text-align: left">
                            <asp:Label ID="lblEndDate" runat="server"></asp:Label>
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
                </div>
            </div>
            <div class="InfoPanel">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
                    <a>出行日程安排（暂定）</a></h3>
                <div class="Block">
                </div>
            </div>
        </div>
        <div class="Clear">
        </div>
    </div>
</asp:Content>
