<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" CodeBehind="MyHistoryLog.aspx.cs"
Inherits="Arsenalcn.ClubSys.Web.MyApplyLog" Title="我的球会历史日志" EnableViewState="false" %>

<%@ Register Src="Control/LeftPanel.ascx" TagName="LeftPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/FieldToolBar.ascx" TagName="FieldToolBar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:LeftPanel ID="ctrlLeftPanel" runat="server"/>
    <div id="MainPanel">
        <uc2:FieldToolBar ID="ctrlFieldToolBar" runat="server"/>
        <asp:GridView runat="server" ID="gvHistoryLog" OnPageIndexChanging="gvHistoryLog_PageIndexChanging"
                      OnRowDataBound="gvHistoryLog_RowDataBound" PageSize="20">
            <Columns>
                <asp:TemplateField HeaderStyle-Width="40px">
                    <HeaderTemplate>
                        类型
                    </HeaderTemplate>
                    <ItemTemplate>
                        <a class="<%#DataBinder.Eval(Container.DataItem, "AdditionalData") %>" title="<%#DataBinder.Eval(Container.DataItem, "AdditionalData2") %>">
                        </a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        所属球会
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Literal ID="ltrlClubName" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="操作内容" DataField="ActionDescription" NullDisplayText="/"
                                ItemStyle-HorizontalAlign="Left" HtmlEncode="false"/>
                <asp:BoundField HeaderText="操作时间" DataField="ActionDate" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}"/>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>