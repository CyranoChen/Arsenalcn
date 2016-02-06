<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" CodeBehind="ManageClub.aspx.cs"
Inherits="Arsenalcn.ClubSys.Web.ManageClub" Title="{0} 球会管理" EnableViewState="false" %>
<%@ Import Namespace="Arsenalcn.ClubSys.Service" %>

<%@ Register Src="Control/LeftPanel.ascx" TagName="LeftPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/FieldToolBar.ascx" TagName="FieldToolBar" TagPrefix="uc2" %>
<%@ Register Src="Control/ManageMenuTabBar.ascx" TagName="ManageMenuTabBar" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">

    <script type="text/javascript">
        var maxExecutorCount = <%= ClubLogic.GetClubExecutorQuota(ClubID).ToString() %>;

        function CheckExecutorCount() {
            var tb = document.getElementById("<%= tbExecutor.ClientID %>");

            if (tb != null && maxExecutorCount > 0) {
                var executors = tb.value;
                if (executors.split("|").length > maxExecutorCount) {
                    alert("干事数超过限额！");
                    return false;
                }
            }

            return true;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:LeftPanel ID="ctrlLeftPanel" runat="server"/>
    <div id="MainPanel">
        <uc2:FieldToolBar ID="ctrlFieldToolBar" runat="server"/>
        <uc4:ManageMenuTabBar ID="ctrlManageMenuTabBar" runat="server"/>
        <asp:Panel ID="pnlInaccessible" runat="server" CssClass="ClubSys_Tip" Visible="false">
            <label>
                您不是该球会会长，不得进入此页面。
            </label>
        </asp:Panel>
        <asp:PlaceHolder ID="phContent" runat="server">
            <table class="DataView" cellspacing="0" cellpadding="5">
                <tbody>
                <tr class="ClubSys_Tip">
                    <td colspan="2">
                        <label>
                            欢迎<em>
                                <asp:Literal ID="ltrlClubName" runat="server"></asp:Literal>
                            </em>的会长<em>
                                <asp:Literal
                                    ID="ltrlClubManagerName" runat="server">
                                </asp:Literal>
                            </em>进入球会管理(会长专用)。
                        </label>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="FieldHeader">
                        球会名称:
                    </td>
                    <td align="left">
                        <asp:Literal ID="ltrlFullName" runat="server"></asp:Literal>(<asp:Literal ID="ltrlShortName"
                                                                                                  runat="server">
                        </asp:Literal>)
                    </td>
                </tr>
                <tr class="Row">
                    <td class="FieldHeader">
                        球会状态:
                    </td>
                    <td align="left">
                        <asp:RadioButtonList ID="rblAppliable" runat="server" RepeatDirection="Horizontal"
                                             RepeatLayout="Flow">
                            <asp:ListItem Selected="True" Text="开放入会" Value="true"></asp:ListItem>
                            <asp:ListItem Text="禁止入会" Value="false"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="FieldHeader">
                        球会会长:
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbManager" runat="server" CssClass="TextBoxRead" Text="Cyrano" ToolTip="如果换成其他用户，原会长今后将无法对该球会进行管理，请慎重考虑。"></asp:TextBox>
                    </td>
                </tr>
                <asp:PlaceHolder ID="phExecutor" runat="server">
                    <tr class="Row">
                        <td class="FieldHeader">
                            球会干事:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbExecutor" runat="server" CssClass="TextBox" ToolTip="如有多个，请用|分割。"></asp:TextBox>
                            <span>*最多可以设置<em><%= ClubLogic.GetClubExecutorQuota(ClubID).ToString() %></em>个干事</span>
                        </td>
                    </tr>
                </asp:PlaceHolder>
                <tr class="Row">
                    <td class="FieldHeader">
                        球会标志:
                    </td>
                    <td align="left">
                        <asp:FileUpload ID="fuLogo" runat="server" CssClass="TextBox" Width="400px" ToolTip="推荐使用80*80px的GIF或JPG图片"/>
                        <asp:Literal ID="ltrlLogo" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="FieldHeader">
                        球会口号:
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbSlogan" runat="server" CssClass="TextBox" Width="400px" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="FieldHeader">
                        球会简介:
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbDesc" runat="server" CssClass="TextBox" Width="400px" TextMode="MultiLine"
                                     Rows="4" MaxLength="500">
                        </asp:TextBox>
                    </td>
                </tr>
                </tbody>
            </table>
            <div class="FooterBtnBar">
                <asp:LinkButton ID="linkButtonSave" runat="server" CssClass="LinkBtn SaveBtn" OnClientClick="return confirm('确认保存球会信息?');return CheckExecutorCount();"
                                OnClick="linkButtonSave_Click" Text="保存"/>
                <asp:LinkButton ID="linkButtonReset" runat="server" CssClass="LinkBtn ResetBtn" OnClick="linkButtonReset_Click"
                                Text="重置"/>
            </div>
        </asp:PlaceHolder>
    </div>
</asp:Content>