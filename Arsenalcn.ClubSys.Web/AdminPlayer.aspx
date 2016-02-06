<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
CodeBehind="AdminPlayer.aspx.cs" Inherits="Arsenalcn.ClubSys.Web.AdminPlayer"
Title="后台管理 球员管理" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:AdminPanel ID="pnlAdmin" runat="server"/>
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server"/>
        <asp:GridView ID="gvPlayer" runat="server" OnRowDataBound="gvPlayer_RowDataBound" DataKeyNames="ID"
                      OnRowCancelingEdit="gvPlayer_RowCancelingEdit" OnRowEditing="gvPlayer_RowEditing"
                      OnRowUpdating="gvPlayer_RowUpdating" OnPageIndexChanging="gvPlayer_PageIndexChanging"
                      PageSize="20">
            <Columns>
                <asp:BoundField DataField="ID" HeaderText="编号" ReadOnly="true"/>
                <asp:BoundField DataField="UserID" HeaderText="ACN会员号" ReadOnly="true"/>
                <asp:BoundField DataField="UserName" HeaderText="用户名" ReadOnly="true"/>
                <asp:TemplateField HeaderText="球衣">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Shirt", "<em>{0}</em>") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="tbShirt" runat="server" CssClass="TextBox" Width="40px"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="球裤">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Shorts", "<em>{0}</em>") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="tbShorts" runat="server" CssClass="TextBox" Width="40px"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="球袜">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Sock", "<em>{0}</em>") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="tbSock" runat="server" CssClass="TextBox" Width="40px"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="球袜">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "IsActive") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlIsActive" runat="server">
                            <asp:ListItem Value="true" Text="true"></asp:ListItem>
                            <asp:ListItem Value="false" Text="false"></asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:CommandField HeaderText="编辑" ShowEditButton="true" ShowDeleteButton="false" EditText="编辑"
                                  UpdateText="保存" CancelText="取消" DeleteText="删除" ButtonType="Link" ControlStyle-CssClass="LinkBtn"
                                  ItemStyle-CssClass="BtnColumn"/>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>