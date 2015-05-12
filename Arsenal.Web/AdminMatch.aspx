<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="AdminMatch.aspx.cs" Inherits="Arsenal.Web.AdminMatch" Title="后台管理 阿森纳比赛管理" Theme="Arsenalcn" %>

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

            $(".DataView td.BtnColumn a.LinkBtn:contains('移除')").click(function () { return confirm('确认从当前分类移除?') });
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
                <asp:DropDownList ID="ddlTeam" runat="server" AutoPostBack="true" OnSelectedIndexChanged="dllTeam_SelectedIndexChanged"
                    Visible="false">
                </asp:DropDownList>
                <asp:DropDownList ID="ddlIsHome" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlIsHome_SelectedIndexChanged">
                    <asp:ListItem Value="">--请选择主客场--</asp:ListItem>
                    <asp:ListItem Value="true">主场</asp:ListItem>
                    <asp:ListItem Value="false">客场</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="DivFloatRight">
                <a href="AdminMatchView.aspx" class="LinkBtn">添加新阿森纳比赛</a>
                <asp:LinkButton ID="btnRefreshCache" runat="server" Text="更新缓存" CssClass="LinkBtn"
                    OnClick="btnRefreshCache_Click" />
            </div>
            <div class="Clear">
                <uc3:CustomPagerInfo ID="ctrlCustomPagerInfo" runat="server" />
            </div>
        </div>
        <asp:GridView ID="gvMatch" runat="server" DataKeyNames="ID" OnPageIndexChanging="gvMatch_PageIndexChanging"
            PageSize="20" OnSelectedIndexChanged="gvMatch_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="ID" Visible="false" />
                <asp:BoundField DataField="LeagueName" HeaderText="分类" />
                <asp:BoundField DataField="Round" HeaderText="轮次" />
                <asp:TemplateField HeaderText="主客场">
                    <ItemTemplate>
                        <%# (bool)Eval("IsHome").Equals(true) ? "主场" : "客场"%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:HyperLinkField DataTextField="TeamName" HeaderText="对阵" DataTextFormatString="<em>{0}</em>"
                    DataNavigateUrlFields="TeamGuid" DataNavigateUrlFormatString="AdminTeamView.aspx?TeamGuid={0}"
                    Target="_blank" />
                <asp:BoundField DataField="PlayTime" HeaderText="比赛时间" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                <asp:BoundField DataField="ResultInfo" HeaderText="结果" DataFormatString="<em>{0}</em>"
                    HtmlEncode="false" />
                <asp:BoundField DataField="IsActive" HeaderText="状态" />
                <asp:CommandField ShowSelectButton="true" HeaderText="操作" EditText="修改" SelectText="详细"
                    UpdateText="保存" CancelText="取消" DeleteText="删除" ControlStyle-CssClass="LinkBtn" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
