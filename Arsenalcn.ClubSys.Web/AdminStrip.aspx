<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" CodeBehind="AdminStrip.aspx.cs"
Inherits="Arsenalcn.ClubSys.Web.AdminStrip" Title="后台管理 装备日志" EnableViewState="false" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:AdminPanel ID="pnlAdmin" runat="server"/>
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server"/>
        <asp:GridView ID="gvClubStrip" runat="server" PageSize="20" OnPageIndexChanging="gvClubStrip_PageIndexChanging"
                      OnRowDataBound="gvClubStrip_RowDataBound">
            <Columns>
                <asp:BoundField HeaderText="用户名" DataField="UserName"/>
                <asp:TemplateField>
                    <HeaderTemplate>
                        装备
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Literal ID="ltrlStrip" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="获取时间" DataField="ActionDate" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}"/>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>