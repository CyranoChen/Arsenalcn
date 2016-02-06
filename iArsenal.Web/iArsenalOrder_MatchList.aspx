<%@ Page Language="C#" MasterPageFile="iArsenalMaster.Master" AutoEventWireup="true"
CodeBehind="iArsenalOrder_MatchList.aspx.cs" Inherits="iArsenal.Web.iArsenalOrder_MatchList" Title="阿森纳新赛季比赛主场球票预订"
Theme="iArsenal" %>

<%@ Register Src="Control/PortalSitePath.ascx" TagName="PortalSitePath" TagPrefix="uc1" %>
<%@ Register Src="Control/CustomPagerInfo.ascx" TagName="CustomPagerInfo" TagPrefix="uc3" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
    <script type="text/javascript">
        $(function () {
            var $tbInfo = $(".DivFloatLeft > .TextBox");
            $tbInfo.each(function() {
                $(this).focus(function() {
                    $(this).val("");
                });
            });
        });
    </script>
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <div id="banner" style="height: 250px">
        <a href="http://bbs.arsenalcn.com/showtopic-107269.aspx" target="_blank">
            <img src="uploadfiles/banner/banner20130518.png" alt="阿森纳新赛季比赛主场球票预订"/>
        </a>
    </div>
    <div id="ACN_Main">
        <uc1:PortalSitePath ID="ucPortalSitePath" runat="server"/>
        <div class="FunctionBar">
            <div class="DivFloatLeft">
                比赛查询：
                <asp:DropDownList ID="ddlProductCode" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlProductCode_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:TextBox ID="tbTeamName" runat="server" Text="--对阵球队--" CssClass="TextBox" Width="200px"></asp:TextBox>
                <asp:LinkButton ID="btnFilter" runat="server" Text="搜索比赛" CssClass="LinkBtn" OnClick="btnFilter_Click"></asp:LinkButton>
            </div>
            <div class="DivFloatRight">
            </div>
            <div class="Clear">
                <uc3:CustomPagerInfo ID="ctrlCustomPagerInfo" runat="server"/>
            </div>
        </div>
        <asp:GridView ID="gvMatch" runat="server" DataKeyNames="ID" OnPageIndexChanging="gvMatch_PageIndexChanging"
                      PageSize="20" OnRowDataBound="gvMatch_RowDataBound">
            <Columns>
                <asp:BoundField DataField="ID" Visible="false"/>
                <asp:BoundField DataField="LeagueName" HeaderText="分类"/>
                <asp:BoundField DataField="Round" HeaderText="轮次"/>
                <asp:TemplateField HeaderText="标志">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlTeamInfo" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="TeamName" HeaderText="对阵" DataFormatString="<em>{0}</em>" HtmlEncode="false"/>
                <asp:BoundField DataField="PlayTimeLocal" HeaderText="比赛时间（伦敦）" DataFormatString="{0:yyyy-MM-dd HH:mm}"/>
                <asp:TemplateField HeaderText="比赛等级">
                    <ItemTemplate>
                        <asp:Label ID="lblMatchTicketRank" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="截止时间|比赛结果">
                    <ItemTemplate>
                        <asp:Label ID="lblMatchDeadlineOrResult" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="预订限制">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlAllowMemberClass" runat="server" Target="_blank"></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlTicketApply" runat="server" Text="订票" CssClass="LinkBtn"/>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>