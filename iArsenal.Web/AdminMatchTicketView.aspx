<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="AdminMatchTicketView.aspx.cs" Inherits="iArsenal.Web.AdminMatchTicketView"
    Title="后台管理 添加/更新球票" Theme="Arsenalcn" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphhead" runat="server">
    <link href="Scripts/jquery.ui/jquery-ui.min.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="Scripts/jquery-ui-1.10.4.min.js"></script>
    <style type="text/css">
        #tdMatchResult input.TextBox {
            text-align: center;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $.datepicker.setDefaults({ dateFormat: 'yy-mm-dd', changeMonth: true, changeYear: true });
            $("#tdDeadline .TextBox").datepicker({ defaultDate: $(this).val() });
        });
    </script>
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:AdminPanel ID="pnlAdmin" runat="server" />
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server" />
        <div class="Arsenal_MainInfo">
            <table class="DataView">
                <thead>
                    <tr class="Header">
                        <th colspan="4">添加/更新阿森纳比赛球票信息
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="CommandRow">
                        <td colspan="4">-- 比赛信息栏 --
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">比赛GUID:
                        </td>
                        <td class="FieldColumn" colspan="3">
                            <asp:Label ID="lblMatchGuid" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader" style="width: 15%">所属分类:
                        </td>
                        <td class="FieldColumn" style="width: 35%">
                            <asp:Label ID="lblLeagueName" runat="server"></asp:Label>
                        </td>
                        <td class="FieldHeader" style="width: 15%">对阵:
                        </td>
                        <td class="FieldColumn" style="width: 35%">
                            <asp:Label ID="lblTeamName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">主客场:
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblIsHome" runat="server"></asp:Label>
                        </td>
                        <td class="FieldHeader">轮次:
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblRound" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">比赛时间:
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblPlayTime" runat="server"></asp:Label>
                        </td>
                        <td class="FieldHeader">比赛结果(主v客):
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblResultInfo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="CommandRow">
                        <td colspan="4">-- 球票信息栏 --
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">比赛等级:
                        </td>
                        <td class="FieldColumn">
                            <asp:DropDownList ID="ddlProductCode" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td class="FieldHeader">适用会员:
                        </td>
                        <td class="FieldColumn">
                            <asp:DropDownList ID="ddlAllowMemberClass" runat="server">
                                <asp:ListItem Text="--全部--" Value="" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="普通会员" Value="1"></asp:ListItem>
                                <asp:ListItem Text="高级会员" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">截止时间:
                        </td>
                        <td class="FieldColumn" id="tdDeadline">
                            <asp:TextBox ID="tbDeadline" runat="server" CssClass="TextBox" Width="180px"></asp:TextBox>
                        </td>
                        <td class="FieldHeader">有效:
                        </td>
                        <td class="FieldColumn">
                            <asp:CheckBox ID="cbIsActive" runat="server" Text="有效" Checked="true" />
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">备注:
                        </td>
                        <td class="FieldColumn" colspan="3">
                            <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBox" Width="400px" TextMode="MultiLine"
                                Rows="4"></asp:TextBox>
                        </td>
                    </tr>
                </tbody>
            </table>
            <asp:GridView ID="gvMatchOrder" runat="server" DataKeyNames="ID" OnPageIndexChanging="gvMatchOrder_PageIndexChanging"
                OnSelectedIndexChanged="gvMatchOrder_SelectedIndexChanged" OnRowDataBound="gvMatchOrder_RowDataBound">
                <Columns>
                    <asp:HyperLinkField HeaderText="编号" DataTextField="ID" DataNavigateUrlFields="ID"
                        DataNavigateUrlFormatString="ServerOrderView.ashx?OrderID={0}" Target="_blank" />
                    <asp:TemplateField HeaderText="会员姓名">
                        <ItemTemplate>
                            <asp:HyperLink ID="hlName" runat="server"></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="手机" DataField="Mobile" />
                    <asp:BoundField HeaderText="创建时间" DataField="CreateTime" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" />
                    <asp:BoundField HeaderText="价格" DataField="Price" DataFormatString="<em>{0:f2}</em>"
                        HtmlEncode="false" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField HeaderText="优惠" DataField="Sale" NullDisplayText="/" DataFormatString="<em>{0:f2}</em>"
                        HtmlEncode="false" ItemStyle-HorizontalAlign="Right" />
                    <asp:TemplateField HeaderText="状态">
                        <ItemTemplate>
                            <asp:Label ID="lblOrderStatus" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="有效" DataField="IsActive" />
                    <asp:CommandField ShowSelectButton="true" HeaderText="操作" EditText="修改" SelectText="详细"
                        UpdateText="保存" CancelText="取消" DeleteText="删除" ControlStyle-CssClass="LinkBtn" />
                </Columns>
            </asp:GridView>
            <div class="FooterBtnBar">
                <asp:Button ID="btnSubmit" runat="server" CssClass="InputBtn SubmitBtn" Text="保存球票"
                    OnClick="btnSubmit_Click" OnClientClick="return confirm('保存球票信息')" />
                <asp:Button ID="btnCancel" runat="server" CssClass="InputBtn" Text="返回" OnClick="btnCancel_Click" />
                <asp:Button ID="btnDelete" runat="server" CssClass="InputBtn" Text="删除球票" OnClick="btnDelete_Click"
                    OnClientClick="return confirm('删除该球票信息?(无法恢复)')" />
            </div>
        </div>
    </div>
</asp:Content>
