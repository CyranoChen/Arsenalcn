<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="AdminPlayer.aspx.cs" Inherits="Arsenalcn.ClubSys.Web.AdminPlayer"
    Title="后台管理 球员管理" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:AdminPanel ID="pnlAdmin" runat="server" />
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server" />
        <asp:GridView ID="gvPlayer" runat="server" DataKeyNames="ID"
            OnPageIndexChanging="gvPlayer_PageIndexChanging" PageSize="20">
            <Columns>
                <asp:BoundField DataField="ID" HeaderText="编号" ReadOnly="true" />
                <asp:BoundField DataField="UserID" HeaderText="ACN会员号" ReadOnly="true" />
                <asp:BoundField DataField="UserName" HeaderText="用户名" ReadOnly="true" />
                <asp:BoundField DataField="Shirt" HeaderText="球衣等级" ControlStyle-CssClass="TextBox"
                    DataFormatString="<em>{0}</em>" HtmlEncodeFormatString="false" />
                <asp:BoundField DataField="Shorts" HeaderText="球裤等级" ControlStyle-CssClass="TextBox"
                    DataFormatString="<em>{0}</em>" HtmlEncodeFormatString="false" />
                <asp:BoundField DataField="Sock" HeaderText="球袜等级" ControlStyle-CssClass="TextBox"
                    DataFormatString="<em>{0}</em>" HtmlEncodeFormatString="false" />
                <asp:BoundField DataField="IsActive" HeaderText="打工开关" ControlStyle-CssClass="TextBox" />
                <asp:CommandField ShowEditButton="true" HeaderText="修改" EditText="修改" UpdateText="保存"
                    CancelText="取消" ControlStyle-CssClass="LinkBtn" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
