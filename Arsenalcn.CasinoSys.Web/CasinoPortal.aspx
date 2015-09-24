<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="CasinoPortal.aspx.cs" Inherits="Arsenalcn.CasinoSys.Web.CasinoPortal"
    Title="ACN CasinoSys 博彩系统" %>

<%@ Register Src="Control/LeftPanel.ascx" TagName="LeftPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/FieldTooBar.ascx" TagName="FieldTooBar" TagPrefix="uc2" %>
<%@ Register Src="Control/MenuTabBar.ascx" TagName="MenuTabBar" TagPrefix="uc3" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:LeftPanel ID="ctrlLeftPanel" runat="server" />
    <div id="MainPanel">
        <uc2:FieldTooBar ID="ctrlFieldTooBar" runat="server" />
        <uc3:MenuTabBar ID="ctrlMenuTabBar" runat="server" />
        <div class="FunctionBar">
            <div class="DivFloatLeft CasinoSys_Tip">
                <span>只使用博彩币；单场可多次投注；赔率固定不变化；博彩按120分钟(如加时)计；</span>
            </div>
            <div class="Clear">
            </div>
        </div>
        <asp:GridView ID="gvMatch" runat="server" PageSize="100" OnRowDataBound="gvMatch_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="分类">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlLeagueInfo" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="主队" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <asp:Label ID="lblHome" runat="server" CssClass="CasinoSys_GameName"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="vs" ItemStyle-Width="20">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlVersus" runat="server"></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="客队" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:Label ID="lblAway" runat="server" CssClass="CasinoSys_GameName"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="PlayTime" HeaderText="比赛时间" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                <asp:TemplateField HeaderText="主队胜">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlWinRate" runat="server"></asp:Literal>
                        <asp:Label ID="lbWinInfo" runat="server" CssClass="CasinoSys_BetInfo" ToolTip="投注次数|金额总计"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="双方平">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlDrawRate" runat="server"></asp:Literal>
                        <asp:Label ID="lbDrawInfo" runat="server" CssClass="CasinoSys_BetInfo" ToolTip="投注次数|金额总计"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="客队胜">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlLoseRate" runat="server"></asp:Literal>
                        <asp:Label ID="lbLoseInfo" runat="server" CssClass="CasinoSys_BetInfo" ToolTip="投注次数|金额总计"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="投注">
                    <ItemTemplate>
                        <asp:HyperLink ID="BtnBet" runat="server" CssClass="LinkBtn" ToolTip="您的投注记录"></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
