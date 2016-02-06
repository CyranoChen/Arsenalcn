<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" CodeBehind="Default.aspx.cs"
Inherits="Arsenalcn.ClubSys.Web._Default" Title="ACN球会系统" EnableViewState="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <div id="MainPanel" style="width: 100%">
        <div class="FieldToolBar">
            <asp:Literal ID="ltrlPluginName" runat="server"></asp:Literal>
            <!-- 如果是管理员显示后台管理 -->
            <asp:Panel ID="pnlAdmin" runat="server" CssClass="HeaderBtnBar">
                <a href="AdminConfig.aspx">后台管理</a>
            </asp:Panel>
        </div>
        <div class="ClubSys_Tip">
            <span>插件已关闭。请联系管理员。</span></div>
    </div>
    <div class="Clear">
    </div>
</asp:Content>