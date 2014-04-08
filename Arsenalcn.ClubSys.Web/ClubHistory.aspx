<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" CodeBehind="ClubHistory.aspx.cs"
    Inherits="Arsenalcn.ClubSys.Web.ClubHistory" Title="{0} 球会历史" EnableViewState="false" %>

<%@ Register Src="Control/LeftPanel.ascx" TagName="LeftPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/FieldToolBar.ascx" TagName="FieldToolBar" TagPrefix="uc2" %>
<%@ Register Src="Control/MenuTabBar.ascx" TagName="MenuTabBar" TagPrefix="uc3" %>
<%@ Register Src="Control/ClubSysHeader.ascx" TagName="ClubSysHeader" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:LeftPanel ID="ctrlLeftPanel" runat="server" />
    <div id="MainPanel">
        <uc2:FieldToolBar ID="ctrlFieldToolBar" runat="server" />
        <uc3:MenuTabBar ID="ctrlMenuTabBar" runat="server" />
        <uc4:ClubSysHeader ID="ctrlClubSysHeader" runat="server" />
        <asp:GridView ID="gvClubHistory" runat="server" OnPageIndexChanging="gvClubHistory_PageIndexChanging"
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
