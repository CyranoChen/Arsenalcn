<%@ Page Language="C#" MasterPageFile="iArsenalMaster.Master" AutoEventWireup="true"
    CodeBehind="iArsenalMemberRegister.aspx.cs" Inherits="iArsenal.Web.iArsenalMemberRegister"
    Title="球迷会实名会员注册" Theme="iArsenal" %>

<%@ Register Src="Control/PortalSitePath.ascx" TagName="PortalSitePath" TagPrefix="uc1" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <div id="banner">
        <a href="#">
            <img src="uploadfiles/banner/banner20120511.png" alt="阿森纳中国官方球迷会实名认证" /></a>
    </div>
    <div id="ACN_Main">
        <uc1:PortalSitePath ID="ucPortalSitePath" runat="server" />
        <div id="mainPanel">
            <table class="DataView">
                <thead>
                    <tr class="Header">
                        <th colspan="2" style="text-align: left">
                            <a name="anchorBack" id="anchorBack">请如实填写您的会员信息，谢谢配合：</a>
                            <asp:Label ID="lblACNInfo" runat="server"></asp:Label>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="Row">
                        <td class="FieldHeader">真实姓名：
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="tbName" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="tbName"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">性别：
                        </td>
                        <td style="text-align: left">
                            <asp:RadioButtonList ID="rblGender" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                                CssClass="RadioButtonList">
                                <asp:ListItem Text="男" Value="true" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="女" Value="false"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">身份证号：
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="tbIDCardNo" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">手机：
                        </td>
                        <td style="text-align: left" class="MemberInfo">
                            <asp:TextBox ID="tbMobile" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvMobile" runat="server" ControlToValidate="tbMobile"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">邮箱：
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="tbEmail" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="tbEmail"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">QQ：
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="tbQQ" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">地址：
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="tbAddress" runat="server" CssClass="TextBox" Width="300px" TextMode="MultiLine"
                                Rows="3"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">简介：
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="tbDescription" runat="server" CssClass="TextBox" Width="300px" TextMode="MultiLine"
                                Rows="3"></asp:TextBox>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="FooterBtnBar">
                <asp:Button ID="btnSubmit" runat="server" Text="提交会员信息" CssClass="InputBtn" OnClick="btnSubmit_Click" />
                <input id="btnReset" type="reset" value="重置表单" class="InputBtn" />
            </div>
        </div>
        <div id="rightPanel">
            <div class="InfoPanel">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
                    <a>会员中心</a></h3>
                <div class="Block">
                    <ul>
                        <li><a href="iArsenalMemberRegister.aspx">会员信息</a></li>
                        <li><a href="iArsenalMemberPeriod.aspx">会籍查询</a></li>
                        <li><a href="iArsenalOrder.aspx">订单查询</a></li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="Clear">
        </div>
    </div>
</asp:Content>
