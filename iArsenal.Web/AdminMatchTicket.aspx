<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="AdminMatchTicket.aspx.cs" Inherits="iArsenal.Web.AdminMatchTicket" Title="后台管理 球票管理" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<%@ Register Src="Control/CustomPagerInfo.ascx" TagName="CustomPagerInfo" TagPrefix="uc3" %>
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
    <uc1:AdminPanel ID="pnlAdmin" runat="server" />
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server" />
        <div class="FunctionBar">
            <div class="DivFloatLeft">
                <asp:DropDownList ID="ddlLeague" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLeague_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:DropDownList ID="ddlIsHome" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlIsHome_SelectedIndexChanged">
                    <asp:ListItem Value="">--请选择主客场--</asp:ListItem>
                    <asp:ListItem Value="true" Selected="True">主场</asp:ListItem>
                    <asp:ListItem Value="false">客场</asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="ddlProductCode" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlProductCode_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:DropDownList ID="ddlAllowMemberClass" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAllowMemberClass_SelectedIndexChanged">
                    <asp:ListItem value="" Selected="True">--全部--</asp:ListItem>
                    <asp:ListItem Text="普通会员" Value="1"></asp:ListItem>
                    <asp:ListItem Text="高级会员" Value="2"></asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="tbTeamName" runat="server" Text="--对阵球队--" CssClass="TextBox" Width="100px"></asp:TextBox>
                <asp:LinkButton ID="btnFilter" runat="server" Text="搜索比赛" CssClass="LinkBtn" OnClick="btnFilter_Click"></asp:LinkButton>
            </div>
            <div class="DivFloatRight">
                <asp:LinkButton ID="btnRefreshCache" runat="server" Text="更新缓存" CssClass="LinkBtn"
                    OnClick="btnRefreshCache_Click" />
            </div>
            <div class="Clear">
                <uc3:CustomPagerInfo ID="ctrlCustomPagerInfo" runat="server" />
            </div>
        </div>
        <asp:GridView ID="gvMatchTicket" runat="server" DataKeyNames="MatchGuid" OnPageIndexChanging="gvMatchTicket_PageIndexChanging"
            PageSize="20" OnSelectedIndexChanged="gvMatchTicket_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="MatchGuid" Visible="false" />
                <asp:BoundField DataField="LeagueName" HeaderText="分类" />
                <asp:BoundField DataField="Round" HeaderText="轮次" />
                <asp:TemplateField HeaderText="主客场">
                    <ItemTemplate>
                        <%# (bool)Eval("IsHome").Equals(true) ? "主场" : "客场"%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="TeamName" HeaderText="对阵" DataFormatString="<em>{0}</em>" HtmlEncode="false" />
                <asp:BoundField DataField="PlayTime" HeaderText="比赛时间" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                <asp:BoundField DataField="ResultInfo" HeaderText="结果" DataFormatString="<em>{0}</em>" HtmlEncode="false" />
                <asp:BoundField DataField="ProductInfo" HeaderText="比赛等级" />
                <asp:BoundField DataField="Deadline" HeaderText="截止时间" DataFormatString="<em>{0:yyyy-MM-dd}</em>" HtmlEncode="false" />
                <asp:BoundField DataField="IsActive" HeaderText="状态" />
                <asp:CommandField ShowSelectButton="true" HeaderText="操作" EditText="修改" SelectText="详细"
                    UpdateText="保存" CancelText="取消" DeleteText="删除" ControlStyle-CssClass="LinkBtn" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
