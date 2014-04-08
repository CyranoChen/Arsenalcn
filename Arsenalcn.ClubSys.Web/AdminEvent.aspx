<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" CodeBehind="AdminEvent.aspx.cs"
    Inherits="Arsenalcn.ClubSys.Web.AdminEvent" Title="后台管理 任务日志" EnableViewState="false" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:AdminPanel ID="pnlAdmin" runat="server" />
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server" />
        <asp:GridView ID="gvEvent" runat="server" OnPageIndexChanging="gvEvent_PageIndexChanging"
            PageSize="20">
            <Columns>
                <asp:BoundField DataField="EventType" HeaderText="类型" />
                <asp:BoundField DataField="Message" HeaderText="提示" />
                <asp:BoundField DataField="ErrorStackTrace" DataFormatString="<em title='{0}' style='white-space:nowrap;'>错误跟踪信息</em>"
                    HeaderText="错误跟踪信息" HtmlEncode="false" />
                <asp:BoundField DataField="EventDate" HeaderText="时间" DataFormatString="<span style='white-space:nowrap;'>{0:yyyy-MM-dd HH:mm:ss}</span>"
                    HtmlEncode="false" />
                <asp:BoundField DataField="ErrorParam" HeaderText="错误信息" DataFormatString="<em title='{0}' style='white-space:nowrap;'>错误信息</em>"
                    HtmlEncode="false" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
