<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="AdminMemberPeriodView.aspx.cs" Inherits="iArsenal.Web.AdminMemberPeriodView"
    Title="后台管理 添加/更新会籍" Theme="Arsenalcn" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphhead" runat="server">
    <link href="Content/themes/base/all.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="Scripts/jquery-ui-1.11.4.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $.datepicker.setDefaults({ dateFormat: 'yy-mm-dd', changeMonth: true, changeYear: true });
            var $tbStartDate = $("#tdStartDate .TextBox");
            var $tbEndDate = $("#tdEndDate .TextBox");

            $.datepicker.setDefaults({ dateFormat: 'yy-mm-dd', changeMonth: true, changeYear: true });
            $tbStartDate.datepicker({
                defaultDate: $(this).val(),
                changeMonth: true,
                numberOfMonths: 1,
                onClose: function (selectedDate) {
                    $tbEndDate.datepicker("option", "minDate", selectedDate);
                }
            });

            $tbEndDate.datepicker({
                defaultDate: $(this).val(),
                changeMonth: true,
                numberOfMonths: 1,
                onClose: function (selectedDate) {
                    $tbStartDate.datepicker("option", "maxDate", selectedDate);
                }
            });
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
                        <th colspan="4">添加/更新官方球迷会收费会籍信息
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="Row">
                        <td class="FieldHeader">会员信息:
                        </td>
                        <td class="FieldColumn" id="tdMemberInfo">
                            <asp:TextBox ID="tbMemberID" runat="server" CssClass="TextBox MemberID" Width="50px"></asp:TextBox>
                            <asp:TextBox ID="tbMemberName" runat="server" CssClass="TextBoxRead MemberName"></asp:TextBox>
                            <a id="btnMemberCheck" class="LinkBtn" href="javascript:MemberCheck()">检查</a>
                        </td>
                        <td class="FieldHeader">状态:
                        </td>
                        <td class="FieldColumn">
                            <asp:CheckBox ID="cbIsActive" runat="server" Checked="true" Text="有效" />
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">会员等级:
                        </td>
                        <td class="FieldColumn">
                            <asp:DropDownList ID="ddlMemberClass" runat="server">
                                <asp:ListItem Text="--请选择会员等级--" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="普通会员" Value="1"></asp:ListItem>
                                <asp:ListItem Text="高级会员" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="FieldHeader">会员卡号:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbMemberCardNo" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">订单信息:
                        </td>
                        <td class="FieldColumn" id="tdAcnInfo" colspan="3">
                            <asp:TextBox ID="tbOrderID" runat="server" CssClass="TextBox OrderID" Width="50px"></asp:TextBox>
                            <asp:TextBox ID="tbOrderPrice" runat="server" CssClass="TextBoxRead OrderPrice"></asp:TextBox>
                            <a id="btnOrderCheck" class="LinkBtn" href="javascript:OrderCheck()">检查</a>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">开始时间:
                        </td>
                        <td class="FieldColumn" id="tdStartDate">
                            <asp:TextBox ID="tbStartDate" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                        <td class="FieldHeader">结束时间:
                        </td>
                        <td class="FieldColumn" id="tdEndDate">
                            <asp:TextBox ID="tbEndDate" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
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
            <div class="FooterBtnBar">
                <asp:Button ID="btnSubmit" runat="server" CssClass="InputBtn SubmitBtn" Text="保存会籍"
                    OnClick="btnSubmit_Click" OnClientClick="return confirm('保存该会员收费会籍信息')" />
                <asp:Button ID="btnCancel" runat="server" CssClass="InputBtn" Text="返回" OnClick="btnCancel_Click" />
                <asp:Button ID="btnBackMember" runat="server" CssClass="InputBtn" Text="返回会员" OnClick="btnBackMember_Click" Visible="false" />
                <asp:Button ID="btnDelete" runat="server" CssClass="InputBtn" Text="删除会籍" OnClick="btnDelete_Click"
                    OnClientClick="return confirm('删除该会员收费会籍?(无法恢复)')" />
            </div>
        </div>
    </div>
</asp:Content>
