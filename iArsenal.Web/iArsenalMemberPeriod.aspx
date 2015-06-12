<%@ Page Language="C#" MasterPageFile="iArsenalMaster.Master" AutoEventWireup="true"
    CodeBehind="iArsenalMemberPeriod.aspx.cs" Inherits="iArsenal.Web.iArsenalMemberPeriod" Title="会籍查询"
    Theme="iArsenal" %>

<%@ Register Src="Control/PortalSitePath.ascx" TagName="PortalSitePath" TagPrefix="uc1" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var $tbInfo = $(".DivFloatLeft > .TextBox");
            $tbInfo.each(function () {
                $(this).focus(function () {
                    $(this).val("");
                });
            });
        });
    </script>
    <style type="text/css">
    </style>
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <div id="banner">
        <a href="http://bbs.arsenalcn.com/showtopic.aspx?topicid=107269&postid=1795109#1795109" target="_blank">
            <img src="uploadfiles/banner/banner20120511.png" alt="球迷会收费会员说明与积分制度" /></a>
    </div>
    <div id="ACN_Main">
        <uc1:PortalSitePath ID="ucPortalSitePath" runat="server" />
        <div id="mainPanel">
            <div class="FunctionBar" style="display: none">
                <div class="DivFloatLeft">
                    会籍查询：                  
                    <asp:TextBox ID="tbMemberCardNo" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                    <asp:LinkButton ID="btnFilter" runat="server" Text="搜索会籍" CssClass="LinkBtn" OnClick="btnFilter_Click"></asp:LinkButton>
                </div>
                <div class="DivFloatRight">
                </div>
                <div class="Clear">
                </div>
            </div>
            <div class="MemberPeriod_Entrance">
                <div class="ApplyContainer">
                    <a class="Core" href="iArsenalOrder_MemberShip.aspx?Type=Core" title="注册本赛季ACN普通(Core)会员"></a>
                </div>
                <div class="ApplyContainer">
                    <a class="Premier" href="iArsenalOrder_MemberShip.aspx?Type=Premier" title="注册本赛季ACN高级(Premier)会员"></a>
                </div>
            </div>
            <asp:GridView ID="gvMemberPeriod" runat="server" OnRowDataBound="gvMemberPeriod_RowDataBound"
                DataKeyNames="ID">
                <Columns>
                    <asp:BoundField HeaderText="标识" DataField="ID" Visible="false" />
                    <asp:BoundField HeaderText="卡号" DataField="MemberCardNo" />
                    <asp:BoundField HeaderText="等级" DataField="MemberClass" DataFormatString="<em>{0}</em>" HtmlEncode="false" />
                    <asp:BoundField HeaderText="开始时间" DataField="StartDate" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField HeaderText="结束时间" DataField="EndDate" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:HyperLinkField HeaderText="订单编号" DataTextField="OrderID"
                        DataNavigateUrlFields="OrderID" DataNavigateUrlFormatString="ServerOrderView.ashx?OrderID={0}" />
                    <asp:TemplateField HeaderText="操作">
                        <ItemTemplate>
                            <asp:HyperLink ID="btnUpgrade" runat="server" Text="升级" CssClass="LinkBtn"
                                OnClientClick="return confirm('确认将升级为Premier会员?')" NavigateUrl="iArsenalOrder_MemberShip.aspx?Type=Premier"></asp:HyperLink>
                            <asp:HyperLink ID="btnRenew" runat="server" Text="续期" CssClass="LinkBtn"
                                OnClientClick="return confirm('确认在本赛季续期此会员资格?')" NavigateUrl="iArsenalOrder_MemberShip.aspx?Type=Premier"></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
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
            <div class="InfoPanel">
                <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
                    <a>收费会员注册说明</a></h3>
                <div class="Block">
                    <p>(1). Arsenalcn Core会员作为普通收费会员，在实名登记真实信息后，可预订阿森纳主场<em>非</em>（A级或特定比赛）的其他比赛球票。</p>
                    <p>(2). Arsenalcn Premier会员作为高级收费会员，在实名登记真实信息后，可预订<em>阿森纳主场所有比赛球票</em>。</p>
                    <p>(3). Core会员在赛季中补足差额，可升级为Premier会员，以预定A级或特定比赛球票。</p>
                    <p>(4). 上个赛季已经成为Premier的高级会员，如在本赛季续费<em>88折</em>优惠，即以<em>422元</em>续约一个赛季会员资格。</p>
                    <p>
                        (5). 球迷会客服联系方式：<br />
                        微信号：<em>iArsenalcn【推荐】</em><br />
                        Email：<a href="mailto:webmaster@arsenalcn.com"><em>webmaster@arsenalcn.com</em></a>
                    </p>
                </div>
            </div>
        </div>
        <div class="Clear">
        </div>
    </div>
</asp:Content>
