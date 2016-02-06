<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
CodeBehind="AdminLogView.aspx.cs" Inherits="Arsenal.Web.AdminLogView" Title="后台管理 详细日志查看" Theme="Arsenalcn" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:AdminPanel ID="pnlAdmin" runat="server"/>
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server"/>
        <div class="CasinoSys_MainInfo">
            <table class="DataView">
                <thead>
                <tr class="Header">
                    <th colspan="4">
                        <asp:Literal ID="ltrlLogID" runat="server"></asp:Literal>
                    </th>
                </tr>
                </thead>
                <tbody>
                <tr class="Row">
                    <td class="FieldHeader">
                        日志类型:
                    </td>
                    <td class="FieldColumn">
                        <asp:TextBox ID="tbLogger" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                    </td>
                    <td class="FieldHeader">
                        创建时间:
                    </td>
                    <td class="FieldColumn">
                        <asp:TextBox ID="tbCreateTime" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                    </td>
                </tr>
                <tr class="AlternatingRow">
                    <td class="FieldHeader">
                        日志级别:
                    </td>
                    <td class="FieldColumn">
                        <asp:TextBox ID="tbLevel" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                    </td>
                    <td class="FieldHeader">
                        线程信息:
                    </td>
                    <td class="FieldColumn">
                        <asp:TextBox ID="tbThread" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="FieldHeader">
                        方法信息:
                    </td>
                    <td class="FieldColumn" colspan="3">
                        <asp:TextBox ID="tbMethod" runat="server" CssClass="TextBox" Width="400px"></asp:TextBox>
                    </td>
                </tr>
                <tr class="AlternatingRow">
                    <td class="FieldHeader">
                        日志消息:
                    </td>
                    <td class="FieldColumn" colspan="3">
                        <asp:TextBox ID="tbMessage" runat="server" CssClass="TextBox" Width="400px" TextMode="MultiLine"
                                     Rows="4">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="FieldHeader">
                        用户ID:
                    </td>
                    <td class="FieldColumn">
                        <asp:TextBox ID="tbUserID" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                    </td>
                    <td class="FieldHeader">
                        用户IP:
                    </td>
                    <td class="FieldColumn">
                        <asp:TextBox ID="tbUserIP" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                    </td>
                </tr>
                <tr class="AlternatingRow">
                    <td class="FieldHeader">
                        浏览器:
                    </td>
                    <td class="FieldColumn">
                        <asp:TextBox ID="tbUserBrowser" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                    </td>
                    <td class="FieldHeader">
                        操作系统:
                    </td>
                    <td class="FieldColumn">
                        <asp:TextBox ID="tbUserOS" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="FieldHeader">
                        出错信息:
                    </td>
                    <td class="FieldColumn" colspan="3">
                        <asp:TextBox ID="tbStackTrace" runat="server" CssClass="TextBox" Width="400px" TextMode="MultiLine"
                                     Rows="8">
                        </asp:TextBox>
                    </td>
                </tr>
                </tbody>
            </table>
            <div class="FooterBtnBar">
                <asp:Button ID="btnCancel" runat="server" CssClass="InputBtn" Text="返回" OnClick="btnCancel_Click"/>
            </div>
        </div>
    </div>
</asp:Content>