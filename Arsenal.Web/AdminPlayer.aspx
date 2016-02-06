<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
CodeBehind="AdminPlayer.aspx.cs" Inherits="Arsenal.Web.AdminPlayer" Title="后台管理 球员管理" Theme="Arsenalcn" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
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
    <uc1:AdminPanel ID="pnlAdmin" runat="server"/>
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server"/>
        <div class="FunctionBar">
            <div class="DivFloatLeft">
                <asp:DropDownList ID="ddlSquadNumber" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSquadNumber_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:DropDownList ID="ddlPosition" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPosition_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:DropDownList ID="ddlIsLegend" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlIsLegend_SelectedIndexChanged">
                    <asp:ListItem Value="" Text="--状态--"></asp:ListItem>
                    <asp:ListItem Value="true" Text="离队"></asp:ListItem>
                    <asp:ListItem Value="false" Text="在队"></asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="tbDisplayName" runat="server" Text="--球员姓名--" CssClass="TextBox"
                             Width="100px">
                </asp:TextBox>
                <asp:LinkButton ID="btnFilter" runat="server" Text="搜索球员" CssClass="LinkBtn" OnClick="btnFilter_Click"></asp:LinkButton>
            </div>
            <div class="DivFloatRight">
                <a href="AdminPlayerView.aspx" class="LinkBtn">添加新球员</a>
                <asp:LinkButton ID="btnRefreshCache" runat="server" Text="更新缓存" CssClass="LinkBtn"
                                OnClick="btnRefreshCache_Click"/>
            </div>
            <div class="Clear">
                <uc3:CustomPagerInfo ID="ctrlCustomPagerInfo" runat="server"/>
            </div>
        </div>
        <asp:GridView ID="gvPlayer" runat="server" DataKeyNames="ID" OnPageIndexChanging="gvPlayer_PageIndexChanging"
                      PageSize="20" OnSelectedIndexChanged="gvPlayer_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="ID" Visible="false"/>
                <asp:BoundField DataField="SquadNumber" HeaderText="号码" DataFormatString="<em>{0}</em>"
                                HtmlEncode="false" ControlStyle-CssClass="TextBox" ControlStyle-Width="15px"/>
                <asp:BoundField DataField="DisplayName" HeaderText="球员名" ReadOnly="true"/>
                <asp:BoundField DataField="Position" HeaderText="位置" DataFormatString="<em>{0}</em>"
                                HtmlEncode="false" ControlStyle-CssClass="TextBox" ControlStyle-Width="80px"/>
                <asp:BoundField DataField="FaceUrl" HeaderText="头像" ControlStyle-CssClass="TextBox"
                                ControlStyle-Width="200px" ItemStyle-HorizontalAlign="Left"/>
                <asp:BoundField DataField="PhotoUrl" HeaderText="照片" ControlStyle-CssClass="TextBox"
                                ControlStyle-Width="200px" ItemStyle-HorizontalAlign="Left"/>
                <asp:BoundField DataField="Offset" HeaderText="偏移" ControlStyle-CssClass="TextBox"
                                ControlStyle-Width="20px"/>
                <asp:BoundField DataField="IsLegend" HeaderText="离队" ControlStyle-CssClass="TextBox"
                                ControlStyle-Width="30px"/>
                <asp:BoundField DataField="IsLoan" HeaderText="租借" ControlStyle-CssClass="TextBox"
                                ControlStyle-Width="30px"/>
                <asp:CommandField ShowEditButton="false" ShowSelectButton="true" ShowDeleteButton="false"
                                  HeaderText="操作" EditText="修改" UpdateText="保存" CancelText="取消" SelectText="详细"
                                  DeleteText="删除" ControlStyle-CssClass="LinkBtn" ItemStyle-CssClass="BtnColumn"/>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>