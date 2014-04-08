<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PortalClubList.ascx.cs"
    Inherits="Arsenalcn.ClubSys.Web.Control.PortalClubList" %>
<%@ OutputCache Duration="3600" VaryByParam="none" %>
<asp:GridView ID="gvClubList" runat="server" OnRowDataBound="gvClubList_RowDataBound"
    OnPageIndexChanging="gvClubList_PageIndexChanging">
    <Columns>
        <asp:TemplateField HeaderStyle-Width="90" HeaderText="球会标志">
            <ItemTemplate>
                <asp:Literal ID="ltrlClubLogo" runat="server"></asp:Literal>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="球会名称">
            <ItemTemplate>
                <asp:Literal ID="ltrlClubName" runat="server"></asp:Literal>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="总评价分">
            <ItemTemplate>
                <asp:Literal ID="ltrlClubRank" runat="server"></asp:Literal>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="状况">
            <ItemTemplate>

                <script type="text/javascript">
                    GenSwfObject('RankChart<%#DataBinder.Eval(Container.DataItem, "ID") %>', 'swf/RankChart_s.swf?XMLURL=ServerXml.aspx%3FClubID=<%#DataBinder.Eval(Container.DataItem, "ID") %>', '110', '110');
                </script>

            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="指标">
            <ItemTemplate>
                <div class="ClubSys_DVRankItemPanel">
                    <ul>
                        <li>1.会员数：<em><asp:Literal ID="ltrlMemberCount" runat="server"></asp:Literal></em><asp:Literal
                            ID="ltrlIsAppliable" runat="server" Text="(已关闭)"></asp:Literal></li>
                        <li>2.财富数：<em><%#Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Fortune")).ToString("N0") %></em></li>
                        <li>3.总积分：<em><%#Convert.ToInt32(DataBinder.Eval(Container.DataItem, "MemberCredit")).ToString("N0") %></em></li>
                        <li>4.总RP值：<em><%#Convert.ToInt32(DataBinder.Eval(Container.DataItem, "MemberRP")).ToString("N0") %></em></li>
                        <li>5.装备数：<asp:Literal ID="ltrlEquipmentCount" runat="server"></asp:Literal></li></ul>
                </div>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="球会干部">
            <ItemTemplate>
                <div class="ClubSys_AdminPanel">
                    <ul>
                        <asp:Repeater ID="rptClubLeads" runat="server">
                            <ItemTemplate>
                                <li<%# DataBinder.Eval(Container.DataItem, "AdditionalData2") %>><%# DataBinder.Eval(Container.DataItem, "AdditionalData") %>: 
                                        <a href="MyPlayerProfile.aspx?userid=<%#DataBinder.Eval(Container.DataItem, "Userid") %>" target="_blank">
                                        <%#DataBinder.Eval(Container.DataItem, "UserName") %></a>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
