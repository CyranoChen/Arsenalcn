<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="AdminConfig.aspx.cs" Inherits="Arsenalcn.CasinoSys.Web.AdminConfig"
    Title="后台管理 全局配置" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:AdminPanel ID="pnlAdmin" runat="server" />
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server" />
        <div class="FunctionBar">
            <div class="DivFloatRight">
                <asp:LinkButton ID="btnRefreshCache" runat="server" Text="更新全部缓存" CssClass="LinkBtn"
                    OnClick="btnRefreshCache_Click" />
            </div>
            <div class="Clear"></div>
        </div>
        <asp:GridView ID="gvSysConfig" runat="server" OnRowUpdating="gvSysConfig_RowUpdating"
            DataKeyNames="ConfigKey" OnPageIndexChanging="gvSysConfig_PageIndexChanging"
            OnRowCancelingEdit="gvSysConfig_RowCancelingEdit" OnRowEditing="gvSysConfig_RowEditing"
            PageSize="50">
            <Columns>
                <asp:BoundField ReadOnly="true" DataField="ConfigKey" HeaderText="配置名" ItemStyle-HorizontalAlign="Right"
                    ItemStyle-Width="30%" />
                <asp:BoundField ConvertEmptyStringToNull="false" NullDisplayText="" DataField="ConfigValue"
                    HeaderText="配置值" ItemStyle-HorizontalAlign="Left" ControlStyle-CssClass="TextBox"
                    ControlStyle-Width="280px" />
                <asp:CommandField ShowEditButton="true" HeaderText="修改" EditText="修改" UpdateText="保存"
                    CancelText="取消" ControlStyle-CssClass="LinkBtn" ItemStyle-Width="100px" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
