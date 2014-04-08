<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" CodeBehind="ManageMember.aspx.cs"
    Inherits="Arsenalcn.ClubSys.Web.ManageMember" Title="{0} 会员管理" %>

<%@ Register Src="Control/LeftPanel.ascx" TagName="LeftPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/FieldToolBar.ascx" TagName="FieldToolBar" TagPrefix="uc2" %>
<%@ Register Src="Control/ManageMenuTabBar.ascx" TagName="ManageMenuTabBar" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">

    <script type="text/javascript">
        function KickButtonClicked(userID) {
            if (confirm("确认要与此会员解约?")) {
                KickMember(userID);
            }
        }

        function GetResult(userName) {
            if (userName == "") {
                alert("该用户已不是该球会会员或者您没有操作权限！");
            }
            else {
                //kicked
                alert("球会已与此会员解约！");
                window.location.href = window.location.href;
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:LeftPanel ID="ctrlLeftPanel" runat="server" />
    <div id="MainPanel">
        <uc2:FieldToolBar ID="ctrlFieldToolBar" runat="server" />
        <uc4:ManageMenuTabBar ID="ctrlManageMenuTabBar" runat="server" />
        <asp:Panel ID="pnlInaccessible" runat="server" CssClass="ClubSys_Tip" Visible="false">
            <label>
                您不是该球会会长或干事，不得进入此页面。</label>
        </asp:Panel>
        <asp:PlaceHolder ID="phContent" runat="server">
            <div class="FunctionBar">
                <div class="DivFloatLeft">
                    <asp:DropDownList ID="ddlClub" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlClub_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
                <div class="Clear">
                </div>
            </div>
            <asp:GridView ID="gvClubMemberList" runat="server" OnRowDataBound="gvClubMemberList_RowDataBound"
                OnPageIndexChanging="gvClubMemberList_PageIndexChanging" PageSize="20">
                <Columns>
                    <asp:BoundField DataField="ID" Visible="false" />
                    <asp:TemplateField HeaderStyle-Width="40px" HeaderText="头像">
                        <ItemTemplate>
                            <asp:Image ID="imgAvatar" runat="server" Width="30" Height="30" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="用户名(职务)">
                        <ItemTemplate>
                            <a href="MyPlayerProfile.aspx?userID=<%#DataBinder.Eval(Container.DataItem, "UserID") %>"
                                target="_blank">
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
                    <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="贡献(天)">
                        <ItemTemplate>
                            <asp:Literal ID="ltrlContributeValue" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="操作">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnKick" runat="server" Text="解约" class="LinkBtn BackBtn" />
                            <asp:HyperLink ID="hlTransfer" runat="server" Text="转账" class="LinkBtn CashBtn" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </asp:PlaceHolder>
    </div>
</asp:Content>
