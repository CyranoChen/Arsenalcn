<%@ Page Language="C#" MasterPageFile="iArsenalMaster.Master" AutoEventWireup="true"
    CodeBehind="iArsenalOrder_TicketBeijing.aspx.cs" Inherits="iArsenal.Web.iArsenalOrder_TicketBeijing"
    Title="阿森纳2012亚洲行北京圣殿杯比赛球票团购" Theme="iArsenal" %>

<%@ Register Src="Control/PortalSitePath.ascx" TagName="PortalSitePath" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <style type="text/css">
        #tdRegion select
        {
            float: left;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $rblPayment = $("#tdPaymentInfo .RadioButtonList");
            $rblPayment.click(function () {
                var payment = $(this).children("input:radio:checked").val();
                if (payment != "") {
                    $(".PaymentInfo").hide();
                    $("#tr" + payment).show();
                }
            });

            $ddlTicketBeijing = $("#tdTicketBeijing select");
            $ddlTicketBeijing.change(function () {
                ProductCheckByID($(this).val());
            });

            var $ddlNation = $("#tdRegion .Nation");
            var $tbNation = $("#tdRegion .OtherNation");
            var $tbRegion1 = $("#tdRegion .Region1");
            var $tbRegion2 = $("#tdRegion .Region2");
            var $ddlRegion1 = $("#ddlRegion1");
            var $ddlRegion2 = $("#ddlRegion2");

            $("#tdRegion .TextBox").hide();

            $ddlNation.change(function () {
                NationDataBind($ddlNation, $ddlRegion1, $ddlRegion2, $tbNation, $tbRegion1, $tbRegion2);
            });

            $ddlRegion1.change(function () {
                if ($(this).val() != "") {
                    $tbRegion1.val($(this).val());
                } else {
                    $tbRegion1.val("");
                }
                $ddlRegion2.show();
                RegionDataBind($ddlRegion2, $(this).val(), "");
                $tbRegion2.val("");
            });

            $ddlRegion2.change(function () {
                if ($(this).val() != "") {
                    $tbRegion2.val($(this).val());
                } else {
                    $tbRegion2.val("");
                }
            });

            NationDataBind($ddlNation, $ddlRegion1, $ddlRegion2, $tbNation, $tbRegion1, $tbRegion2);
            RegionDataBind($ddlRegion1, "0", $tbRegion1.val());
            RegionDataBind($ddlRegion2, $tbRegion1.val(), $tbRegion2.val());
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <div id="banner" style="height: 800px">
        <a href="#">
            <img src="uploadfiles/banner/banner20120531.png" alt="阿森纳2012亚洲行北京圣殿杯比赛座位图" />
        </a>
    </div>
    <div id="ACN_Main">
        <uc1:PortalSitePath ID="ctrlSitePath" runat="server" />
        <div id="mainPanel">
            <table class="DataView">
                <thead>
                    <tr class="Header">
                        <th colspan="4" style="text-align:left">
                            <a name="anchorBack" id="anchorBack">欢迎进入“阿森纳2012亚洲行北京圣殿杯比赛球票团购”，请仔细确认并填写以下信息：</a>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="CommandRow">
                        <td colspan="4">
                            -- 会员信息栏 --
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader" style="width: 100px">
                            真实姓名：
                        </td>
                        <td align="left">
                            <asp:Label ID="lblMemberName" runat="server"></asp:Label>
                        </td>
                        <td class="FieldHeader" style="width: 100px">
                            ACN帐号：
                        </td>
                        <td align="left">
                            <asp:Label ID="lblMemberACNInfo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">
                            身份证号：
                        </td>
                        <td align="left" colspan="3">
                            <asp:TextBox ID="tbIDCardNo" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvIDCardNo" runat="server" ControlToValidate="tbIDCardNo"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">
                            来自于：
                        </td>
                        <td align="left" id="tdRegion" colspan="3">
                            <asp:DropDownList ID="ddlNation" runat="server" CssClass="Nation">
                                <asp:ListItem Value="" Text="--请选择国家--"></asp:ListItem>
                                <asp:ListItem Value="中国" Text="中国"></asp:ListItem>
                                <asp:ListItem Value="其他" Text="其他"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="tbNation" runat="server" CssClass="TextBox OtherNation" Width="100px"></asp:TextBox>
                            <select id="ddlRegion1">
                            </select>
                            <asp:TextBox ID="tbRegion1" runat="server" CssClass="TextBox Region1" Width="50px"></asp:TextBox>
                            <select id="ddlRegion2">
                            </select>
                            <asp:TextBox ID="tbRegion2" runat="server" CssClass="TextBox Region2" Width="50px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">
                            手机：
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbOrderMobile" runat="server" CssClass="TextBox" Width="150px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvOrderMobile" runat="server" ControlToValidate="tbOrderMobile"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan"></asp:RequiredFieldValidator>
                        </td>
                        <td class="FieldHeader">
                            付款方式：
                        </td>
                        <td align="left" id="tdPaymentInfo">
                            <asp:RadioButtonList ID="rblOrderPayment" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" CssClass="RadioButtonList">
                                <asp:ListItem Text="支付宝" Value="Alipay" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="银行转帐" Value="Bank"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr class="Row PaymentInfo" id="trAlipay">
                        <td class="FieldHeader">
                            支付宝帐号：
                        </td>
                        <td align="left" colspan="3">
                            <asp:TextBox ID="tbAlipay" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row PaymentInfo" id="trBank" style="display: none">
                        <td class="FieldHeader">
                            银行帐号：
                        </td>
                        <td align="left" colspan="3">
                            <asp:TextBox ID="tbBankName" runat="server" CssClass="TextBox" Width="100px" ToolTip="请输入银行名称"></asp:TextBox>
                            <asp:TextBox ID="tbBankAccount" runat="server" CssClass="TextBox" Width="250px" ToolTip="请输入银行帐号"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="CommandRow">
                        <td colspan="4">
                            -- 商品信息栏 --
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">
                            球票类型：
                        </td>
                        <td align="left" colspan="3" id="tdTicketBeijing">
                            <asp:DropDownList ID="ddlTicketBeijing" runat="server" OnDataBound="ddlTicketBeijing_DataBound">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvTicketBeijing" runat="server" ControlToValidate="ddlTicketBeijing"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader" title="">
                            订购数量：
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbQuantity" runat="server" CssClass="TextBox" Width="50px" Text="1"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ControlToValidate="tbQuantity"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan"></asp:RequiredFieldValidator>
                        </td>
                        <td class="FieldHeader">
                            所在看台：
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlSeatLevel" runat="server">
                                <asp:ListItem Text="不介意" Value="0"></asp:ListItem>
                                <asp:ListItem Text="一层" Value="1"></asp:ListItem>
                                <asp:ListItem Text="二层" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">
                            球迷见面会：
                        </td>
                        <td align="left">
                            <asp:CheckBox ID="cbDaytimeEvent" ToolTip="初拟7月26日下午举行" Text="希望另行购买见面会门票" runat="server" />
                        </td>
                        <td class="FieldHeader">
                            公开训练课：
                        </td>
                        <td align="left">
                            <asp:CheckBox ID="cbEveningEvent" ToolTip="初拟7月26日晚上举行" Text="希望另行购买训练课门票" runat="server" />
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">
                            备注：
                        </td>
                        <td align="left" colspan="3">
                            <asp:TextBox ID="tbOrderDescription" runat="server" CssClass="TextBox" Width="300px" TextMode="MultiLine"
                                Rows="4"></asp:TextBox>
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
            <div class="InfoPanel" id="pnlProductImage" style="display: none">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle();">
                    <a>商品图片</a></h3>
                <div class="Block">
                    <img src="" alt="" style="width: 250px; margin: 2px;" />
                </div>
            </div>
            <div class="InfoPanel">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle();">
                    <a>球票信息</a></h3>
                <div class="Block">
                    <p>
                        (1). ACN将帮助各位会员预订阿森纳北京行2012.7.27圣殿杯的比赛球票，我们将坐在主办方指定的阿森纳球迷看台区。为球场示意图右侧的C区、D区。</p>
                    <p>
                        (2). 座位：图中的球场右侧CD区的119通道到123通道的一层看台，219通道到223通道的二层看台均可预订。</p>
                    <p>
                        (3). 顺序：我们会视预订的先后次序，为大家分配尽量靠前排的座位。如有特殊要求，请在备注中留言，如无留言我们将尽量安排大家坐在同一看台。</p>
                    <p>
                        (4). 请准确填写身份证号码、真实姓名、手机号码和邮箱地址，届时将必须凭取票确认函和身份证取票，不得代领。</p>
                </div>
            </div>
            <div class="InfoPanel">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle();">
                    <a>票价信息</a></h3>
                <div class="Block">
                    <p>
                        (1). 票价：提供的球票覆盖3个价位，如图所示，E-580元，D-880元，C-1280元。</p>
                    <p>
                        (2). 优惠：可以优惠价2012元一次预订两张票价为1280元的C级球票。</p>
                </div>
            </div>
        </div>
        <div class="Clear">
        </div>
    </div>
</asp:Content>
