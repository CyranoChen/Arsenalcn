<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="ClubLuckyPlayerLog.aspx.cs" Inherits="Arsenalcn.ClubSys.Web.ClubLuckyPlayerLog"
    Title="幸运球会日志" %>

<%@ Register Src="Control/LeftPanel.ascx" TagName="LeftPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/FieldToolBar.ascx" TagName="FieldToolBar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:LeftPanel ID="ctrlLeftPanel" runat="server" />
    <div id="MainPanel">
        <uc2:FieldToolBar ID="ctrlFieldToolBar" runat="server" />
        <asp:GridView ID="gvLuckyPlayerLog" runat="server" PageSize="30" OnPageIndexChanging="gvLuckyPlayerLog_PageIndexChanging">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        球员编号</HeaderTemplate>
                    <ItemTemplate>
                        <%#DataBinder.Eval(Container.DataItem, "PlayerID") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        用户名</HeaderTemplate>
                    <ItemTemplate>
                        <a href="MyPlayerProfile.aspx?userid=<%#DataBinder.Eval(Container.DataItem, "UserID") %>"
                            target="_blank">
                            <%#DataBinder.Eval(Container.DataItem, "UserName") %></a></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                    <HeaderTemplate>
                        金额</HeaderTemplate>
                    <ItemTemplate>
                        <em>
                            <%#DataBinder.Eval(Container.DataItem, "TotalBonus") %></em></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        获取时间</HeaderTemplate>
                    <ItemTemplate>
                        <%# ((DateTime)DataBinder.Eval(Container.DataItem, "Date")).ToString("yyyy-MM-dd") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        状态</HeaderTemplate>
                    <ItemTemplate>
                        <%# (bool)(DataBinder.Eval(Container.DataItem, "BonusGot")) ? "已领取" : "未领取"%></ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
