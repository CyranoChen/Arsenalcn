<%@ Page Language="C#" MasterPageFile="iArsenalMaster.Master" AutoEventWireup="true"
    CodeBehind="iArsenalOrder_Membership.aspx.cs" Inherits="iArsenal.Web.iArsenalOrder_Membership"
    Title="ACN收费会员登记" Theme="iArsenal" %>

<%@ Register Src="Control/PortalSitePath.ascx" TagName="PortalSitePath" TagPrefix="uc1" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
    <link href="Content/themes/base/all.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="Scripts/jquery-ui-1.12.1.min.js"></script>
    <style type="text/css">
        input.MemberClass, input.Sale, input.Region1, input.Region2 {
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
            <img src="uploadfiles/banner/banner20120511.png" alt="球迷会收费会员说明与积分制度" />
        </a>
    </div>
    <div id="ACN_Main">
        <uc1:PortalSitePath ID="ucPortalSitePath" runat="server" />
        <div id="mainPanel">
            <table class="DataView FormView">
                <thead>
                    <tr class="Header">
                        <th colspan="4" class="FieldColumn">
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
                        <td class="FieldColumn">
                            <asp:Label ID="lblMemberName" runat="server"></asp:Label>
                        </td>
                        <td class="FieldHeader" style="width: 110px;">ACN帐号：
                        </td>
                        <td class="FieldColumn">
                            <asp:Label ID="lblMemberACNInfo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">现居住地：
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
                        <td class="FieldHeader">身份证号：
                        </td>
                        <td class="FieldColspan" colspan="3">
                            <asp:TextBox ID="tbIDCardNo" runat="server" CssClass="TextBox" Width="400px" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvIDCardNo" runat="server" ControlToValidate="tbIDCardNo"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">护照编号：
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbPassportNo" runat="server" CssClass="TextBox" Width="150px" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvPassportNo" runat="server" ControlToValidate="tbPassportNo"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan">
                            </asp:RequiredFieldValidator>
                        </td>
                        <td class="FieldHeader">护照姓名：
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbPassportName" runat="server" CssClass="TextBox" Width="150px" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvPassportName" runat="server" ControlToValidate="tbPassportName"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">手机：
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbMobile" runat="server" CssClass="TextBox" Width="150px" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvMobile" runat="server" ControlToValidate="tbMobile"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan">
                            </asp:RequiredFieldValidator>
                        </td>
                        <td class="FieldHeader">微信：
                        </td>
                        <td class="FieldColumn">
                            <asp:TextBox ID="tbWeChat" runat="server" CssClass="TextBox" Width="150px" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvWeChat" runat="server" ControlToValidate="tbWeChat"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">邮箱：
                        </td>
                        <td class="FieldColspan" colspan="3">
                            <asp:TextBox ID="tbEmail" runat="server" CssClass="TextBox" Width="300px" MaxLength="40"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="tbEmail"
                                Display="Dynamic" ErrorMessage="*" CssClass="ValiSpan">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="CommandRow">
                        <td colspan="4">-- 会籍信息栏 --
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">会籍等级：</td>
                        <td class="FieldColspan" colspan="3">
                            <asp:TextBox ID="tbMemberClass" runat="server" CssClass="MemberClass"></asp:TextBox>
                            <asp:Label ID="lblMemberClass" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <asp:PlaceHolder ID="phSaleInfo" runat="server" Visible="false">
                        <tr class="Row">
                            <td class="FieldHeader">补充说明：</td>
                            <td class="FieldColspan" colspan="3">
                                <asp:TextBox ID="tbSale" runat="server" CssClass="Sale"></asp:TextBox>
                                <asp:Label ID="lblSaleInfo" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </asp:PlaceHolder>
                    <tr class="AlternatingRow">
                        <td class="FieldHeader">会籍有效期：
                        </td>
                        <td class="FieldColspan" colspan="3">
                            <asp:Label ID="lblDatePeriod" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">备注：
                        </td>
                        <td class="FieldColspan" colspan="3">
                            <asp:TextBox ID="tbOrderDescription" runat="server" CssClass="TextBox" Width="300px"
                                TextMode="MultiLine" Rows="4">
                            </asp:TextBox>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="FooterBtnBar">
                <asp:Button ID="btnSubmit" runat="server" Text="保存订单信息" CssClass="InputBtn SubmitBtn" OnClick="btnSubmit_Click" />
                <input id="btnReset" type="reset" value="重置表单" class="InputBtn" />
            </div>
        </div>
        <div id="rightPanel">
            <asp:Panel ID="pnlMemberCore" CssClass="InfoPanel" runat="server">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
                    <a>ACN(Core)会员福利</a>
                </h3>
                <div class="Block">
                    <p>(1). 由于球迷会球票预订者过多，导致每个赛季都有多场比赛供不应求。应俱乐部要求，官方球迷会应配合维持球场秩序，并严格确认购票者身份，特实行球迷会收费会员制度。</p>
                    <p>
                        (2). Arsenalcn Core Member作为普通收费会员，在实名登记真实信息后，可预订阿森纳主场<em>非</em>（A级或特定重要比赛，如收官站、旅游旺季比赛）的其他比赛球票。
                    </p>
                    <p>
                        (3). ACN(Core)会员年费(赛季)为<em>198元</em>。
                    </p>
                    <p>
                        (4). ACN(Core)会员可预订的比赛，原则上可<em>确保有票</em>。
                    </p>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlMemberPremier" CssClass="InfoPanel" runat="server">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
                    <a>ACN(Premier)会员福利</a>
                </h3>
                <div class="Block">
                    <p>(1). 由于球迷会球票预订者过多，导致每个赛季都有多场比赛供不应求。应俱乐部要求，官方球迷会应配合维持球场秩序，并严格确认购票者身份，特实行球迷会收费会员制度。</p>
                    <p>
                        (2). Arsenalcn Premier Member作为高级收费会员，在实名登记真实信息后，可预订<em>阿森纳主场所有比赛球票</em>。
                    </p>
                    <p>
                        (3). ACN(Premier)会员年费(赛季)为<em>480元</em>。
                    </p>
                    <p>
                        (4). ACN(Premier)会员可预订的<em>A级或特定重要比赛</em>，不确保一定有票。一旦球票紧张，将根据会员积分情况，球票付款确认的时间顺序优先分配。
                    </p>
                </div>
            </asp:Panel>
            <div class="InfoPanel">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
                    <a>申请方式与相关说明</a>
                </h3>
                <div class="Block">
                    <p>
                        (1). 球迷会会员实名登记，并缴纳一个赛季的年费，方有资格成为收费会员并实名登记真实信息后，<a href="iArsenalOrder_MatchList.aspx" target="_blank">
                            <em>可预订主场球票</em>
                        </a>。此收费会员与阿森纳官方俱乐部红会员(Red Member)等会员制无关。
                    </p>
                    <p>(2). 收费会员一旦确认支付年费并获得资格后，当前一个赛季均有效，可预订或购买多场比赛球票。每赛季联赛结束的月底，为会员资格到期时间。下个赛季需重新续费。</p>
                    <p>
                        (3). 收费会员在未成功购买球票之前，可随时选择放弃收费会员资格，我们会在每个季度末结算会费，并予以退还；但是，任何收费会员（包括普通与高级）只要通过球迷会渠道，成功购买过任何一场比赛球票，当赛季所交年费<em>恕不退还</em>。
                    </p>
                </div>
            </div>
        </div>
        <div class="Clear">
        </div>
    </div>
</asp:Content>
