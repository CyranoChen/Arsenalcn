<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="AdminMemberView.aspx.cs" Inherits="iArsenal.Web.AdminMemberView"
    Title="后台管理 添加/更新会员" Theme="Arsenalcn" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphhead" runat="server">
    <link href="Content/themes/base/all.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="Scripts/jquery-ui-1.11.4.min.js"></script>
    <style type="text/css">
        input.AcnSessionKey, input.Region1, input.Region2 {
            display: none;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $.datepicker.setDefaults({ dateFormat: 'yy-mm-dd', changeMonth: true, changeYear: true });
            $("#tdBirthday .TextBox").datepicker();
            $("#tdJoinDate .TextBox").datepicker();

            NationDataBindImpl($("#tdRegion"));
        });
    </script>
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:AdminPanel ID="pnlAdmin" runat="server" />
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server" />
        <div class="ASC_MainInfo">
            <table class="DataView">
                <thead>
                    <tr class="Header">
                        <th colspan="4">添加/更新官方球迷会实名会员信息
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="CommandRow">
                        <td colspan="4">-- 会员信息栏 --
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">
                            <asp:Label ID="lblMemberInfo" runat="server"></asp:Label>
                        </td>
                        <td class="FieldColumn MemberInfo">
                            <asp:TextBox ID="tbName" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                        <td class="FieldHeader">状态:
                        </td>
                        <td class="FieldColumn">
                            <asp:CheckBox ID="cbIsActive" runat="server" Checked="true" Text="有效" />
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">会员类型:
                        </td>
                        <td class="FieldColumn">
                            <asp:DropDownList ID="ddlMemberType" runat="server">
                                <asp:ListItem Text="--请选择类型--" Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="观赛" Value="1"></asp:ListItem>
                                <asp:ListItem Text="VIP" Value="2"></asp:ListItem>
                                <asp:ListItem Text="干事" Value="3"></asp:ListItem>
                                <asp:ListItem Text="团购" Value="4"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlEvalution" runat="server">
                                <asp:ListItem Text="--请选择评价--" Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="黑名单" Value="1"></asp:ListItem>
                                <asp:ListItem Text="白名单" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="FieldHeader">ACN信息:
                        </td>
                        <td class="FieldColumn" id="tdAcnInfo">
                            <asp:TextBox ID="tbAcnID" runat="server" CssClass="TextBox AcnID" Width="50px"></asp:TextBox>
                            <asp:TextBox ID="tbAcnName" runat="server" CssClass="TextBoxRead AcnName"></asp:TextBox>
                            <asp:TextBox ID="tbAcnSessionKey" runat="server" CssClass="TextBoxRead AcnSessionKey" Width="50px"></asp:TextBox>
                            <a id="btnAcnCheck" class="LinkBtn" href="javascript:AcnCheck()">检查</a>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">会员卡号:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbMemberCardNo" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                        <td class="FieldHeader">淘宝帐号:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbTaobaoName" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">IP地址:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbIP" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                        <td class="FieldHeader">IP信息:
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblIPInfo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="CommandRow">
                        <td colspan="4">-- 身份信息栏 --
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">身份证号:
                        </td>
                        <td class="FieldColspan" colspan="3">
                            <asp:TextBox ID="tbIDCardNo" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">加入时间:
                        </td>
                        <td class="FieldColumn" id="tdJoinDate">
                            <asp:TextBox ID="tbJoinDate" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                        <td class="FieldHeader">上次活动:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbLastLoginTime" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">性别:
                        </td>
                        <td class="FieldColumn">
                            <asp:RadioButtonList ID="rblGender" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <asp:ListItem Text="男" Value="true" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="女" Value="false"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td class="FieldHeader">护照编号:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbPassportNo" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">生日:
                        </td>
                        <td class="FieldColumn" id="tdBirthday">
                            <asp:TextBox ID="tbBirthday" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                        <td class="FieldHeader">护照姓名:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbPassportName" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">MSN:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbMSN" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                        <td class="FieldHeader">微信/QQ:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbQQ" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="CommandRow">
                        <td colspan="4">-- 联系信息栏 --
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">所在国家/地区:
                        </td>
                        <td class="FieldColumn" id="tdRegion" colspan="3">
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
                        <td class="FieldHeader">职业:
                        </td>
                        <td class="FieldColspan" colspan="3">
                            <asp:TextBox ID="tbCareer" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">移动电话:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbMobile" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                        <td class="FieldHeader">邮箱:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbEmail" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">固定电话:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbTelephone" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                        <td class="FieldHeader">邮编:
                        </td>
                        <td class="FieldColspan" colspan="3">
                            <asp:TextBox ID="tbZipCode" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">地址:
                        </td>
                        <td class="FieldColspan" colspan="3">
                            <asp:TextBox ID="tbAddress" runat="server" CssClass="TextBox" Rows="4" Width="500px"
                                TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="CommandRow">
                        <td colspan="4">-- 其他信息栏 --
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">简介:
                        </td>
                        <td class="FieldColspan" colspan="3">
                            <asp:TextBox ID="tbDescription" runat="server" CssClass="TextBox" Rows="5" Width="500px"
                                TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">备注:
                        </td>
                        <td class="FieldColspan" colspan="3">
                            <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBox" Rows="5" Width="500px"
                                TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                </tbody>
            </table>
            <asp:GridView ID="gvMemberPeriod" runat="server" DataKeyNames="ID" OnSelectedIndexChanged="gvMemberPeriod_SelectedIndexChanged">
                <Columns>
                    <asp:BoundField HeaderText="标识" DataField="ID" />
                    <asp:BoundField HeaderText="会员编号" DataField="MemberID" DataFormatString="<em>{0}</em>"
                        HtmlEncode="false" />
                    <asp:HyperLinkField HeaderText="会员姓名" DataTextField="MemberName" DataTextFormatString="<em>{0}</em>"
                        DataNavigateUrlFields="MemberID" DataNavigateUrlFormatString="AdminMemberView.aspx?MemberID={0}" />
                    <asp:BoundField HeaderText="卡号" DataField="MemberCardNo" />
                    <asp:BoundField HeaderText="开始时间" DataField="StartDate" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField HeaderText="结束时间" DataField="EndDate" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:HyperLinkField HeaderText="订单编号" DataTextField="OrderID"
                        DataNavigateUrlFields="OrderID" DataNavigateUrlFormatString="AdminOrderView.aspx?OrderID={0}" />
                    <asp:BoundField HeaderText="状态" DataField="IsActive" />
                    <asp:CommandField ShowSelectButton="true" HeaderText="操作" EditText="修改" SelectText="详细"
                        UpdateText="保存" CancelText="取消" DeleteText="删除" ControlStyle-CssClass="LinkBtn" />
                </Columns>
            </asp:GridView>
            <div class="FooterBtnBar">
                <asp:Button ID="btnSubmit" runat="server" CssClass="InputBtn SubmitBtn" Text="保存会员"
                    OnClick="btnSubmit_Click" OnClientClick="return confirm('保存该会员实名信息')" />
                <input id="btnAcnCheckSubmit" type="button" value="检查ACN信息" class="InputBtn" onclick="AcnCheck()" />
                <asp:Button ID="btnCancel" runat="server" CssClass="InputBtn" Text="返回" OnClick="btnCancel_Click" />
                <asp:Button ID="btnDelete" runat="server" CssClass="InputBtn" Text="删除会员" OnClick="btnDelete_Click"
                    OnClientClick="return confirm('删除该会员实名信息?(无法恢复)')" />
            </div>
        </div>
    </div>
</asp:Content>
