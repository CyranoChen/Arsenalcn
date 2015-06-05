<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="AdminPlayerView.aspx.cs" Inherits="Arsenal.Web.AdminPlayerView" Title="后台管理 添加/更新球员" Theme="Arsenalcn" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<asp:Content ID="cphhead" ContentPlaceHolderID="cphhead" runat="server">
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:AdminPanel ID="pnlAdmin" runat="server" />
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server" />
        <div class="Arsenal_MainInfo">
            <table class="DataView">
                <thead>
                    <tr class="Header">
                        <th colspan="2" style="text-align: left">
                            <input id="cbPlayerBasicInfo" type="checkbox" checked="checked" onclick="$('#phBasicInfo').toggle()" />
                            <label for="cbPlayerBasicInfo" title="展开球员基本信息" id="lblPlayerBasicInfo" runat="server" />
                        </th>
                    </tr>
                </thead>
                <tbody id="phBasicInfo">
                    <tr class="Row">
                        <td class="FieldHeader" style="width: 30%">球员GUID:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbPlayerGuid" runat="server" CssClass="TextBoxRead" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">First Name(名):
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbFirstName" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">Last Name(姓):
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbLastName" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="rfvLastName" ControlToValidate="tbLastName"
                                ErrorMessage="*" CssClass="ValiSpan" Display="Dynamic" />
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">球衣印字:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbPrintingName" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">球场位置:
                        </td>
                        <td class="FieldColumn">
                            <asp:DropDownList ID="ddlPosition" runat="server" Width="300px">
                                <asp:ListItem Text="--请选择球员位置--" Value="" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Goalkeeper" Value="Goalkeeper"></asp:ListItem>
                                <asp:ListItem Text="Defender" Value="Defender"></asp:ListItem>
                                <asp:ListItem Text="Midfielder" Value="Midfielder"></asp:ListItem>
                                <asp:ListItem Text="Forward" Value="Forward"></asp:ListItem>
                                <asp:ListItem Text="Coach" Value="Coach"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">球员号码:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbSquadNumber" runat="server" CssClass="TextBox" MaxLength="2" Width="40px"
                                Text="-1"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="rfvSquadNumber" ControlToValidate="tbSquadNumber"
                                ErrorMessage="*" CssClass="ValiSpan" Display="Dynamic" />
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">头像:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbFaceURL" runat="server" CssClass="TextBox" Width="300px" Text="UploadFiles/StripFace/Legend.jpg"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="rfvFaceURL" ControlToValidate="tbFaceURL"
                                ErrorMessage="*" CssClass="ValiSpan" Display="Dynamic" />
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">照片:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbPhotoURL" runat="server" CssClass="TextBox" Width="300px" Text="UploadFiles/StripPhoto/"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">偏移量:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbOffset" runat="server" CssClass="TextBox" Width="40px" Text="50"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="rfvOffset" ControlToValidate="tbOffset"
                                ErrorMessage="*" CssClass="ValiSpan" Display="Dynamic" />
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">是否离队:
                        </td>
                        <td class="FieldColumn">
                            <asp:CheckBox ID="cbLegend" Text="是否离队" ToolTip="现役为false, 离队为true" runat="server"
                                Checked="false" />
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">是否租借:
                        </td>
                        <td class="FieldColumn">
                            <asp:CheckBox ID="cbLoan" Text="是否租借" ToolTip="现役为false, 租借为true" runat="server"
                                Checked="false" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <table class="DataView">
                <thead>
                    <tr class="Header">
                        <th colspan="2" style="text-align: left">
                            <input id="cbPlayerDetailInfo" type="checkbox" checked="checked" onclick="$('#phDetailInfo').toggle()" />
                            <label for="cbPlayerDetailInfo" title="展开球员详细信息" id="lblPlayerDetailInfo" runat="server" />
                        </th>
                    </tr>
                </thead>
                <tbody id="phDetailInfo">
                    <tr class="Row">
                        <td class="FieldHeader" style="width: 30%">生日:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbBirthday" runat="server" CssClass="TextBox" Width="400px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">出生地:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbBorn" runat="server" CssClass="TextBox" Width="400px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">首发数:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbStarts" runat="server" CssClass="TextBox" Width="40px" Text="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">替补数:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbSubs" runat="server" CssClass="TextBox" Width="40px" Text="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">进球数:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbGoals" runat="server" CssClass="TextBox" Width="40px" Text="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">加盟时间:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbJoinDate" runat="server" CssClass="TextBox" Width="400px" ToolTip="yyyy-MM-dd"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">加盟(年):
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbJoined" runat="server" CssClass="TextBox" Width="400px" ToolTip="yyyy-MM-dd"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">离队(年):
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbLeft" runat="server" CssClass="TextBox" Width="400px" ToolTip="yyyy"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">首次上场:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbDebut" runat="server" CssClass="TextBox" Width="400px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">首次进球:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbFirstGoal" runat="server" CssClass="TextBox" Width="400px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">前球队:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbPreviousClubs" runat="server" CssClass="TextBox" Width="400px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">档案:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbProfile" runat="server" CssClass="TextBox" Width="400px" TextMode="MultiLine"
                                Rows="10"></asp:TextBox>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="FooterBtnBar">
                <asp:Button ID="btnSubmit" runat="server" CssClass="InputBtn Submit" Text="保存球员"
                    OnClick="btnSubmit_Click" OnClientClick="return confirm('保存该球员信息')" />
                <asp:Button ID="btnCancel" runat="server" CssClass="InputBtn" Text="返回" OnClick="btnCancel_Click" />
                <asp:Button ID="btnDelete" runat="server" CssClass="InputBtn" Text="删除球员" OnClick="btnDelete_Click"
                    OnClientClick="return confirm('删除该球员信息?(无法恢复)')" />
            </div>
        </div>
    </div>
</asp:Content>
