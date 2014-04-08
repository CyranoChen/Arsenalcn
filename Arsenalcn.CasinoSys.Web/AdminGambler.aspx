<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="AdminGambler.aspx.cs" Inherits="Arsenalcn.CasinoSys.Web.AdminGambler"
    Title="后台管理 玩家管理" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:AdminPanel ID="pnlAdmin" runat="server" />
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server" />
        <div class="CasinoSys_Tip">
            <asp:TextBox ID="tbUserName" runat="server" Text="-请输入用户名-" CssClass="TextBox"></asp:TextBox>
            <asp:LinkButton ID="btnQuery" runat="server" Text="搜索" CssClass="LinkBtn" OnClick="btnQuery_Click"></asp:LinkButton></div>
        <asp:GridView ID="gvGambler" runat="server" OnRowDataBound="gvGambler_RowDataBound"
            DataKeyNames="UserID" OnRowCancelingEdit="gvGambler_RowCancelingEdit" OnRowEditing="gvGambler_RowEditing"
            OnRowUpdating="gvGambler_RowUpdating" OnRowCommand="gvGambler_RowCommand" OnPageIndexChanging="gvGambler_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="UserID" Visible="false" />
                <asp:BoundField DataField="ID" HeaderText="编号" ReadOnly="true" />
                <asp:BoundField DataField="UserName" HeaderText="用户名" ReadOnly="true" />
                <asp:BoundField DataField="QSB" HeaderText="枪手币" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="RP" HeaderText="RP" ReadOnly="true" />
                <asp:TemplateField HeaderText="现金(博彩币)" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <em>
                            <%# ((double)DataBinder.Eval(Container.DataItem, "Cash")).ToString("N2") %></em>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="tbCash" runat="server" CssClass="TextBox" Width="80"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="猜对">
                    <ItemTemplate>
                        <em>
                            <%#DataBinder.Eval(Container.DataItem, "Win") %></em>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="tbWin" runat="server" CssClass="TextBox" Width="40"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="猜错">
                    <ItemTemplate>
                        <em>
                            <%#DataBinder.Eval(Container.DataItem, "Lose") %></em>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="tbLose" runat="server" CssClass="TextBox" Width="40"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="TotalBet" HeaderText="总投注量" ReadOnly="true" ItemStyle-HorizontalAlign="Right"
                    DataFormatString="{0:N2}" />
                <asp:CommandField HeaderText="编辑" ShowEditButton="true" EditText="编辑" UpdateText="保存"
                    CancelText="取消" ButtonType="Link" ControlStyle-CssClass="LinkBtn" />
                <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnResetGambler" runat="server" Text="统计信息" CssClass="LinkBtn"
                            CommandName="ResetGambler" OnClientClick="return confirm('确认操作?')"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
