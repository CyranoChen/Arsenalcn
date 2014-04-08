<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="CasinoRank.aspx.cs" Inherits="Arsenalcn.CasinoSys.Web.CasinoRank"
    Title="博彩排行" %>

<%@ Register Src="Control/LeftPanel.ascx" TagName="LeftPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/FieldTooBar.ascx" TagName="FieldTooBar" TagPrefix="uc2" %>
<%@ Register Src="Control/MenuTabBar.ascx" TagName="MenuTabBar" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:LeftPanel ID="ctrlLeftPanel" runat="server" />
    <div id="MainPanel">
        <uc2:FieldTooBar ID="ctrlFieldTooBar" runat="server" />
        <uc3:MenuTabBar ID="ctrlMenuTabBar" runat="server" />
        <asp:GridView ID="gvRank" runat="server" PageSize="24" OnPageIndexChanging="gvRank_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="RankDate" HeaderText="排行年月" DataFormatString="<em>{0}</em>"
                    HtmlEncode="false" />
                <asp:TemplateField HeaderText="当月菜王">
                    <ItemTemplate>
                        <a href="MyBonusLog.aspx?UserID=<%#DataBinder.Eval(Container.DataItem, "WinnerUserID") %>"
                            target="_blank" class="StrongLink">
                            <%#DataBinder.Eval(Container.DataItem, "WinnerUserName") %></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="WinnerProfit" DataFormatString="<em style='color:#aa0000'>{0:N2}</em>"
                    ItemStyle-HorizontalAlign="Right" HeaderText="盈亏状况" HtmlEncode="false"></asp:BoundField>
                <asp:BoundField DataField="WinnerProfitRate" DataFormatString="<em style='color:#aa0000'>{0:N2}%</em>"
                    ItemStyle-HorizontalAlign="Right" HeaderText="盈亏率" HtmlEncode="false"></asp:BoundField>
                <asp:TemplateField HeaderText="当月菜虫">
                    <ItemTemplate>
                        <a href="MyBonusLog.aspx?UserID=<%#DataBinder.Eval(Container.DataItem, "LoserUserID") %>"
                            target="_blank" class="StrongLink">
                            <%#DataBinder.Eval(Container.DataItem, "LoserUserName") %></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="LoserProfit" DataFormatString="<em style='color:#00853e'>{0:N2}</em>"
                    ItemStyle-HorizontalAlign="Right" HeaderText="盈亏状况" HtmlEncode="false"></asp:BoundField>
                <asp:BoundField DataField="LoserProfitRate" DataFormatString="<em style='color:#00853e'>{0:N2}%</em>"
                    ItemStyle-HorizontalAlign="Right" HeaderText="盈亏率" HtmlEncode="false"></asp:BoundField>
                <asp:TemplateField HeaderText="当月RP王">
                    <ItemTemplate>
                        <a href="MyBonusLog.aspx?UserID=<%#DataBinder.Eval(Container.DataItem, "RPUserID") %>"
                            target="_blank" class="StrongLink">
                            <%#DataBinder.Eval(Container.DataItem, "RPUserName") %></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="RPAmount" DataFormatString="<em>RP+{0}</em>" HeaderText="RP" HtmlEncode="false" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
