<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" CodeBehind="ClubMember.aspx.cs"
    Inherits="Arsenalcn.ClubSys.Web.ClubMember" Title="{0} 球会成员" EnableViewState="false" %>

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
        <div class="FunctionBar">
            <div class="DivFloatRight">
                共有<em><asp:Literal ID="ltrlMemberCount" runat="server"></asp:Literal></em>个会员，每天总贡献值为<em><asp:Literal
                    ID="ltrlTotalContribution" runat="server"></asp:Literal></em>枪手币。</div>
            <div class="Clear">
            </div>
        </div>
        <asp:GridView ID="gvClubMemberList" runat="server" OnRowDataBound="gvClubMemberList_RowDataBound"
            OnPageIndexChanging="gvClubMemberList_PageIndexChanging" PageSize="20">
            <Columns>
                <asp:TemplateField HeaderStyle-Width="40px" HeaderText="头像" Visible="false">
                    <ItemTemplate>
                        <asp:Image ID="imgAvatar" runat="server" Width="30" Height="30" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="ID" DataField="UserID" DataFormatString="<em>{0}</em>"
                    HtmlEncode="false" />
                <asp:TemplateField HeaderText="用户名(职务)">
                    <ItemTemplate>
                        <a href="MyPlayerProfile.aspx?userid=<%#DataBinder.Eval(Container.DataItem, "UserID") %>"
                            target="_blank" class="StrongLink">
                            <%#DataBinder.Eval(Container.DataItem, "UserName") %></a>
                        <asp:Literal ID="ltrlResponsibility" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="组别">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlUserGroup" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="金钱">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlUserFortune" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="发帖数">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlUserPosts" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="入会天数">
                    <ItemTemplate>
                        <em title="自<%# ((DateTime)DataBinder.Eval(Container.DataItem, "FromDate")).ToString("yyyy-MM-dd") %>入会以来">
                            <asp:Literal ID="ltrlDays" runat="server"></asp:Literal>天</em>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="贡献财富(天)">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlContributeValue" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
