<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="AdminPlayer.aspx.cs" Inherits="Arsenalcn.ClubSys.Web.AdminPlayer"
    Title="后台管理 球员管理" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:AdminPanel ID="pnlAdmin" runat="server" />
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server" />
        <asp:GridView ID="gvPlayer" runat="server" DataSourceID="PlayerDataSouce" DataKeyNames="ID"
            OnPageIndexChanging="gvPlayer_PageIndexChanging" OnRowCancelingEdit="gvPlayer_RowCancelingEdit"
            OnRowEditing="gvPlayer_RowEditing" PageSize="20">
            <Columns>
                <asp:BoundField DataField="ID" HeaderText="编号" ReadOnly="true" />
                <asp:BoundField DataField="UserName" HeaderText="用户名" ReadOnly="true" />
                <asp:BoundField DataField="Shirt" HeaderText="球衣等级" ControlStyle-CssClass="TextBox"
                    DataFormatString="<em>{0}</em>" HtmlEncodeFormatString="false" />
                <asp:BoundField DataField="Shorts" HeaderText="球裤等级" ControlStyle-CssClass="TextBox"
                    DataFormatString="<em>{0}</em>" HtmlEncodeFormatString="false" />
                <asp:BoundField DataField="Sock" HeaderText="球袜等级" ControlStyle-CssClass="TextBox"
                    DataFormatString="<em>{0}</em>" HtmlEncodeFormatString="false" />
                <asp:BoundField DataField="DisplayName" HeaderText="当前球员" ReadOnly="true" />
                <asp:BoundField DataField="IsActive" HeaderText="打工开关" ControlStyle-CssClass="TextBox" />
                <asp:CommandField ShowEditButton="true" HeaderText="修改" EditText="修改" UpdateText="保存"
                    CancelText="取消" ControlStyle-CssClass="LinkBtn" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="PlayerDataSouce" SelectCommand="SELECT * From [dbo].[AcnClub_Player] LEFT OUTER JOIN Arsenal_Player ON AcnClub_Player.CurrentGuid = Arsenal_Player.PlayerGuid ORDER BY ID DESC"
            UpdateCommand="UPDATE [dbo].[AcnClub_Player] SET Shirt=@Shirt, Shorts=@Shorts, Sock=@Sock, IsActive=@IsActive WHERE [ID] = @ID" runat="server"></asp:SqlDataSource>
    </div>
</asp:Content>
