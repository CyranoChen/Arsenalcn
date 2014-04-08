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
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <div id="banner">
        <a href="#">
            <img src="uploadfiles/banner/banner20120511.png" alt="阿森纳中国官方球迷会会籍管理" /></a>
    </div>
    <div id="ACN_Main">
        <uc1:PortalSitePath ID="ucPortalSitePath" runat="server" />
        <div id="mainPanel">
            <div class="FunctionBar">
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
            <asp:GridView ID="gvMemberPeriod" runat="server" DataKeyNames="MemberPeriodID">
                <Columns>
                    <asp:BoundField HeaderText="标识" DataField="MemberPeriodID" />
                    <asp:BoundField HeaderText="卡号" DataField="MemberCardNo" />
                    <asp:BoundField HeaderText="开始时间" DataField="StartDate" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField HeaderText="结束时间" DataField="EndDate" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:HyperLinkField HeaderText="订单编号" DataTextField="OrderID"
                        DataNavigateUrlFields="OrderID" DataNavigateUrlFormatString="ServerOrderView.ashx?OrderID={0}" />
                    <asp:BoundField HeaderText="状态" DataField="IsActive" />
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
        </div>
        <div class="Clear">
        </div>
    </div>
</asp:Content>
