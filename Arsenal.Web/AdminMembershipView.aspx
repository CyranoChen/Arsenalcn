<%@ Page Language="C#" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="AdminMembershipView.aspx.cs" Inherits="Arsenal.Web.AdminMembershipView"
    Title="后台管理 添加/更新会员" Theme="Arsenalcn" %>

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
                        <th colspan="4">添加/更新ACN统一用户认证
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="CommandRow">
                        <td colspan="4">-- 会员信息栏 --
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">用户标识:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbUserGuid" runat="server" CssClass="TextBoxRead" Width="200px"></asp:TextBox>
                        </td>
                        <td class="FieldHeader">用户名:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbUserName" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">手机:
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
                        <td class="FieldHeader">注册时间:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbCreateDate" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                        <td class="FieldHeader">密码修改时间:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbLastPasswordChangedDate" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">最近登录:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbLastLoginDate" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                        <td class="FieldHeader">最后活动:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbLastActivityDate" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">ACN信息:
                        </td>
                        <td class="FieldColumn" id="tdAcnInfo">
                            <asp:TextBox ID="tbAcnID" runat="server" CssClass="TextBox AcnID" Width="100px"></asp:TextBox>
                            <asp:TextBox ID="tbAcnName" runat="server" CssClass="TextBoxRead AcnName"></asp:TextBox>
                            <a id="btnAcnCheck" class="LinkBtn" href="javascript:AcnCheck()">检查</a>
                        </td>
                        <td class="FieldHeader">实名信息:
                        </td>
                        <td class="FieldColumn" id="tdMemberInfo">
                            <asp:TextBox ID="tbMemberID" runat="server" CssClass="TextBox MemberID" Width="100px"></asp:TextBox>
                            <asp:TextBox ID="tbMemberName" runat="server" CssClass="TextBoxRead MemberName"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">备注:
                        </td>
                        <td class="FieldColspan" colspan="3">
                            <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBox" Rows="3" Width="500px"
                                TextMode="MultiLine">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr class="CommandRow">
                        <td colspan="4">-- 微信信息栏 --
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">微信昵称:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbWeChatNickName" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                        <td class="FieldHeader">微信性别:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbGender" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">头像:
                        </td>
                        <td class="FieldColspan">
                            <asp:TextBox ID="tbHeadImgUrl" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                        <td class="FieldHeader">最新授权:
                        </td>
                        <td class="FieldColspan">
                            <asp:TextBox ID="tbLastAuthorizeDate" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">微信OpenID:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbWeChatOpenID" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                        <td class="FieldHeader">微信UnionID:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbUnionID" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">AccessToken:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbAccessToken" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                        <td class="FieldHeader">AccessToken过期:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbAccessTokenExpiredDate" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">RefreshToken:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbRefreshToken" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                        <td class="FieldHeader">RefreshToken过期:
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbRefreshTokenExpiredDate" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">所在地:
                        </td>
                        <td class="FieldColspan">
                            <asp:TextBox ID="tbProvince" runat="server" CssClass="TextBox" Width="80px"></asp:TextBox>
                            <asp:TextBox ID="tbCity" runat="server" CssClass="TextBox" Width="80px"></asp:TextBox>
                            <asp:TextBox ID="tbCountry" runat="server" CssClass="TextBox" Width="80px"></asp:TextBox>
                        </td>
                        <td class="FieldHeader">权限:
                        </td>
                        <td class="FieldColspan">
                            <asp:TextBox ID="tbPrivilege" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                </tbody>
            </table>
            <asp:GridView ID="gvAvatar" runat="server" DataKeyNames="ID" OnSelectedIndexChanged="gvAvatar_SelectedIndexChanged">
                <Columns>
                    <asp:BoundField DataField="ID" Visible="false" />
                    <asp:BoundField HeaderText="用户名" DataField="UserName" />
                    <asp:BoundField HeaderText="手机" DataField="Mobile" />
                    <asp:BoundField HeaderText="邮箱" DataField="Email" />
                    <asp:BoundField HeaderText="注册时间" DataField="CreateDate" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                    <asp:BoundField HeaderText="上次登录" DataField="LastLoginDate" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                    <asp:BoundField HeaderText="备注" DataField="Remark" />
                    <asp:CommandField ShowSelectButton="true" HeaderText="操作" EditText="修改" SelectText="详细"
                        UpdateText="保存" CancelText="取消" DeleteText="删除" ControlStyle-CssClass="LinkBtn" />
                </Columns>
            </asp:GridView>
            <div class="FooterBtnBar">
                <asp:Button ID="btnSubmit" runat="server" CssClass="InputBtn SubmitBtn" Text="保存用户"
                    OnClick="btnSubmit_Click" OnClientClick="return confirm('保存统一用户认证信息')" />
                <asp:Button ID="btnCancel" runat="server" CssClass="InputBtn" Text="返回" OnClick="btnCancel_Click" />
                <asp:Button ID="btnDelete" runat="server" CssClass="InputBtn" Text="删除用户" OnClick="btnDelete_Click"
                    OnClientClick="return confirm('删除统一用户认证信息?(无法恢复)')" />
            </div>
        </div>
    </div>
</asp:Content>
