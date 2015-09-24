<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" CodeBehind="AdminHistory.aspx.cs"
    Inherits="Arsenalcn.ClubSys.Web.AdminHistory" Title="后台管理 历史日志" EnableViewState="false" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:AdminPanel ID="pnlAdmin" runat="server" />
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server" />
        <div class="FunctionBar">
            <div class="DivFloatLeft">
                <asp:DropDownList ID="ddlClub" runat="server" AutoPostBack="true">
                </asp:DropDownList>
            </div>
            <div class="Clear">
            </div>
        </div>
        <asp:GridView ID="gvHistory" runat="server" OnPageIndexChanging="gvHistory_PageIndexChanging"
            PageSize="20">
            <Columns>
                <asp:TemplateField HeaderStyle-Width="40px" HeaderText="类型">
                    <ItemTemplate>
                        <a class="<%#DataBinder.Eval(Container.DataItem, "AdditionalData") %>" title="<%#DataBinder.Eval(Container.DataItem, "AdditionalData2") %>">
                        </a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="操作用户" DataField="OperatorUserName" NullDisplayText="/" />
                <asp:BoundField HeaderText="对象用户" DataField="ActionUserName" NullDisplayText="/" />
                <asp:BoundField HeaderText="操作内容" DataField="ActionDescription" NullDisplayText="/"
                    ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField HeaderText="操作时间" DataField="ActionDate" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
