<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="AdminAward.aspx.cs" Inherits="Arsenalcn.ClubSys.Web.AdminAward" Title="后台管理 发放奖励" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("td.tdVideo input:checkbox").click(function () { setVideoCardGuid(this.checked); });

            $("div.FooterBtnBar input:submit").click(function () {
                var $msg = '为 ### 发放奖励?';
                var $cash = $("td.tdCash input:text.TextBox").val();
                var $rp = $("td.tdRP input:text.TextBox").val();
                var $cardName = $("td.tdCard select option:selected").text();
                var cardActive = $("td.tdCard input:checkbox").get(0);
                var $videoName = $("td.tdVideo input:text.TextBox").val();
                var videoActive = $("td.tdVideo input:checkbox").get(0);

                $msg = $msg.replace('###', $("td.tdUser input:text.TextBoxRead").val());

                if ($cash != '') {
                    $msg += '\r\n-- 枪手币: ' + $cash;
                }

                if ($rp != '') {
                    $msg += '\r\n-- RP: ' + $rp;
                }

                if ($cardName != 'None') {
                    if (cardActive.checked) {
                        $msg += '\r\n-- 球星卡(激活): ' + $cardName;
                    } else {
                        $msg += '\r\n-- 球星卡(未激活): ' + $cardName;
                    }
                }

                if ($videoName != '') {
                    if (videoActive.checked) {
                        $msg += '\r\n-- 视频卡(激活): ' + $videoName;
                    } else {
                        $msg += '\r\n-- 视频卡(未激活)';
                    }
                }

                return confirm($msg);
            });
        });

        function setVideoCardGuid(flag) {
            var $tb = $("td.tdVideo input:text.TextBox");
            if (!flag) {
                $tb.val("<%= Guid.Empty %>");
                $tb.attr("readOnly", "readonly");
            } else {
                $tb.val("");
                $tb.attr("readOnly", "");
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:AdminPanel ID="pnlAdmin" runat="server" />
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server" />
        <div class="ClubSys_MainInfo">
            <table class="DataView" cellspacing="0" cellpadding="5">
                <thead>
                    <tr class="Header">
                        <th colspan="2">
                            为指定会员发放奖励
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="Row">
                        <td class="FieldHeader">
                            ID和用户名:
                        </td>
                        <td align="left" class="tdUser">
                            <asp:TextBox ID="tbUserID" runat="server" CssClass="TextBox" ToolTip="请输入会员UID" Width="50px"></asp:TextBox>
                            <asp:TextBox ID="tbUserName" runat="server" CssClass="TextBoxRead" Text="--用户名--"></asp:TextBox>
                            <asp:LinkButton ID="BtnCheckUserID" runat="server" Text="检查" CssClass="LinkBtn" OnClick="BtnCheckUserID_Click"></asp:LinkButton>
                            <asp:RequiredFieldValidator ID="rfvUserID" runat="server" ErrorMessage="*" ControlToValidate="tbUserID"
                                Display="Dynamic"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">
                            枪手币:
                        </td>
                        <td align="left" class="tdCash">
                            <asp:TextBox ID="tbCash" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">
                            RP:
                        </td>
                        <td align="left" class="tdRP">
                            <asp:TextBox ID="tbRP" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">
                            球星卡:
                        </td>
                        <td align="left" class="tdCard">
                            <asp:DropDownList ID="lstPlayer" runat="server" OnDataBound="lstPlayer_DataBound">
                            </asp:DropDownList>
                            <asp:CheckBox ID="cbCardActive" runat="server" Text="激活" Checked="true" />
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">
                            视频卡:
                        </td>
                        <td align="left" class="tdVideo">
                            <asp:TextBox ID="tbVideoGuid" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                            <asp:CheckBox ID="cbVideoActive" runat="server" Text="激活" Checked="true" />
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">
                            奖励理由:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbNotes" runat="server" CssClass="TextBox" Rows="4" TextMode="MultiLine"
                                Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="FooterBtnBar">
                <asp:Button ID="btnSubmit" runat="server" CssClass="InputBtn" Text="发出奖励" OnClick="btnSubmit_Click" />
                <asp:Button ID="btnReset" runat="server" CssClass="InputBtn" Text="重置" OnClick="btnReset_Click" />
            </div>
        </div>
    </div>
</asp:Content>
