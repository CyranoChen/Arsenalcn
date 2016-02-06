<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
CodeBehind="AdminSchedule.aspx.cs" Inherits="iArsenal.Web.AdminSchedule"
Title="后台管理 计划任务" Theme="Arsenalcn" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:AdminPanel ID="pnlAdmin" runat="server"/>
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server"/>
        <asp:GridView ID="gvSchedule" runat="server" OnRowUpdating="gvSchedule_RowUpdating"
                      DataKeyNames="ScheduleKey" OnPageIndexChanging="gvSchedule_PageIndexChanging"
                      PageSize="50" OnRowCancelingEdit="gvSchedule_RowCancelingEdit"
                      OnRowEditing="gvSchedule_RowEditing" OnSelectedIndexChanged="gvSchedule_SelectedIndexChanged">
            <Columns>
                <asp:BoundField ReadOnly="true" HeaderText="任务名称" DataField="ScheduleKey"/>
                <asp:BoundField HeaderText="任务类型" DataField="ScheduleType"
                                ItemStyle-HorizontalAlign="Left" ControlStyle-CssClass="TextBox" ControlStyle-Width="300px"/>
                <asp:BoundField ReadOnly="true" HeaderText="执行方式" DataField="ExecuteTimeInfo"/>
                <asp:BoundField HeaderText="定时" DataField="DailyTime" ControlStyle-CssClass="TextBox" ControlStyle-Width="30px"/>
                <asp:BoundField HeaderText="轮询" DataField="Minutes" ControlStyle-CssClass="TextBox" ControlStyle-Width="30px"/>
                <asp:BoundField ReadOnly="true" HeaderText="上次执行时间" DataField="LastCompletedTime" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}"/>
                <asp:BoundField HeaderText="系统任务" DataField="IsSystem" ControlStyle-CssClass="TextBox" ControlStyle-Width="30px"/>
                <asp:BoundField HeaderText="状态" DataField="IsActive" ControlStyle-CssClass="TextBox" ControlStyle-Width="30px"/>
                <asp:CommandField ShowEditButton="true" ShowSelectButton="true" HeaderText="操作" EditText="修改" UpdateText="保存"
                                  CancelText="取消" SelectText="执行" ControlStyle-CssClass="LinkBtn" ItemStyle-Width="100px"/>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>