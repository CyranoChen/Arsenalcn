<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" CodeBehind="ManageApplication.aspx.cs"
Inherits="Arsenalcn.ClubSys.Web.ManageApplication" Title="{0} 审核入会" EnableViewState="false" %>

<%@ Register Src="Control/LeftPanel.ascx" TagName="LeftPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/FieldToolBar.ascx" TagName="FieldToolBar" TagPrefix="uc2" %>
<%@ Register Src="Control/ManageMenuTabBar.ascx" TagName="ManageMenuTabBar" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">

    <script type="text/javascript">
        function ActionClicked(applyHistoryID, approved) {
            var confirmMsg;
            if (approved == true)
                confirmMsg = "通过该用户加入球会吗？";
            else
                confirmMsg = "驳回该用户的加入请求吗？";

            if (window.confirm(confirmMsg)) {
                var arg = applyHistoryID + ';' + approved;
                ApproveJoin(arg);
            }
        }

        function GetResult(result) {
            if (result == "") {
                alert("球会的会员已达到上限！");
            } else if (result == "true") {
                alert("会员已加入球会！");
                window.location.href = window.location.href;
            } else if (result == "false") {
                alert("会员申请已被驳回！");
                window.location.href = window.location.href;
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:LeftPanel ID="ctrlLeftPanel" runat="server"/>
    <div id="MainPanel">
        <uc2:FieldToolBar ID="ctrlFieldToolBar" runat="server"/>
        <uc4:ManageMenuTabBar ID="ctrlManageMenuTabBar" runat="server"/>
        <asp:Panel ID="pnlInaccessible" runat="server" CssClass="ClubSys_Tip" Visible="false">
            <label>
                您不是该球会会长或干事，不得进入此页面。
            </label>
        </asp:Panel>
        <asp:PlaceHolder ID="phContent" runat="server"></asp:PlaceHolder>
        <asp:GridView ID="gvClubMemberList" runat="server" AllowPaging="true" PageSize="5"
                      GridLines="None" AlternatingRowStyle-CssClass="AlternatingRow" RowStyle-CssClass="Row"
                      HeaderStyle-CssClass="Header" OnRowDataBound="gvClubMemberList_RowDataBound"
                      OnPageIndexChanging="gvClubMemberList_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="ID" Visible="false"/>
                <asp:TemplateField HeaderStyle-Width="40px" HeaderText="头像">
                    <ItemTemplate>
                        <asp:Image ID="imgAvatar" runat="server" Width="30" Height="30"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="用户名">
                    <ItemTemplate>
                        <a href="MyPlayerProfile.aspx?userID=<%#DataBinder.Eval(Container.DataItem, "UserID") %>"
                           target="_blank">
                            <%#DataBinder.Eval(Container.DataItem, "UserName") %>
                        </a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="组别">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlUserGroup" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="积分">
                    <ItemTemplate>
                        <em>
                            <asp:Literal ID="ltrlUserCredit" runat="server"></asp:Literal>
                        </em>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="金钱">
                    <ItemTemplate>
                        <em>
                            <asp:Literal ID="ltrlUserFortune" runat="server"></asp:Literal>
                        </em>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="发帖数">
                    <ItemTemplate>
                        <em>
                            <asp:Literal ID="ltrlUserPosts" runat="server"></asp:Literal>
                        </em>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="审核">
                    <ItemTemplate>
                        <a class="LinkBtn JoinBtn" href="javascript:ActionClicked(<%#DataBinder.Eval(Container.DataItem, "ID") %>, true);">
                            通过
                        </a> <a class="LinkBtn BackBtn" href="javascript:ActionClicked(<%#DataBinder.Eval(Container.DataItem, "ID") %>, false);">
                            驳回
                        </a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>