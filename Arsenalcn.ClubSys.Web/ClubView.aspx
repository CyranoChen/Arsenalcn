<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" CodeBehind="ClubView.aspx.cs"
    Inherits="Arsenalcn.ClubSys.Web.ClubView" Title="{0} 基本信息" EnableViewState="false" %>

<%@ Register Src="Control/LeftPanel.ascx" TagName="LeftPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/FieldToolBar.ascx" TagName="FieldToolBar" TagPrefix="uc2" %>
<%@ Register Src="Control/ClubSysHeader.ascx" TagName="ClubSysHeader" TagPrefix="uc3" %>
<%@ Register Src="Control/MenuTabBar.ascx" TagName="MenuTabBar" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:LeftPanel ID="ctrlLeftPanel" runat="server" />
    <div id="MainPanel">
        <uc2:FieldToolBar ID="ctrlFieldToolBar" runat="server" />
        <uc4:MenuTabBar ID="ctrlMenuTabBar" runat="server" />
        <uc3:ClubSysHeader ID="ctrlClubSysHeader" runat="server" />
        <div class="ClubSys_MainInfo">
            <div class="BaseInfoPanel">
                <dl>
                    <dt>
                        <label>
                            简称:</label><asp:Literal ID="ltrlShortName" runat="server"></asp:Literal></dt>
                    <dd>
                        <label>
                            创建人:</label><asp:Literal ID="ltrlCreatorName" runat="server"></asp:Literal></dd>
                    <dt>
                        <label>
                            球会口号:</label><asp:Literal ID="ltrlSlogan" runat="server"></asp:Literal></dt>
                </dl>
                <dl>
                    <dd title="全部会员数">
                        <label>
                            会员数:</label><em><asp:Literal ID="ltrlMemeberCount" runat="server"></asp:Literal>/<asp:Literal
                                ID="ltrlMemberQuota" runat="server"></asp:Literal></em>(<asp:Literal ID="ltrlAppliable"
                                    runat="server"></asp:Literal>)</dd>
                    <dd title="球会积累的财富值">
                        <label>
                            财富数:</label><em><asp:Literal ID="ltrlFortune" runat="server"></asp:Literal></em>枪手币
                    </dd>
                    <dd title="全部会员的积分总和">
                        <label>
                            总积分:</label><em><asp:Literal ID="ltrlMemberCredit" runat="server"></asp:Literal></em>分
                    </dd>
                    <dd title="全部会员的金钱总和">
                        <label>
                            总金钱:</label><em><asp:Literal ID="ltrlMemberFortune" runat="server"></asp:Literal></em>枪手币
                    </dd>
                    <dd title="全部球员的卡片数C与视频数V">
                        <label>
                            装备数:</label><em><asp:Literal ID="ltrlEquipmentCount" runat="server"></asp:Literal></em>
                    </dd>
                </dl>
            </div>
            <div class="AdminPanel">
                <ul>
                    <asp:Repeater ID="rptClubLeads" runat="server">
                        <ItemTemplate>
                            <li<%# DataBinder.Eval(Container.DataItem, "AdditionalData2") %>><%# DataBinder.Eval(Container.DataItem, "AdditionalData") %>: <a href="MyPlayerProfile.aspx?userid=<%#DataBinder.Eval(Container.DataItem, "Userid") %>" target="_blank"><%#DataBinder.Eval(Container.DataItem, "UserName") %></a>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
                <div class="CreateDate">
                    创建于<asp:Literal ID="ltrlCreateDate" runat="server"></asp:Literal>,有<em><asp:Literal
                        ID="ltrlDays" runat="server"></asp:Literal></em>天历史</div>
            </div>
            <div class="Clear">
            </div>
        </div>
    </div>
</asp:Content>
