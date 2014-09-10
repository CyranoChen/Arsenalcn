<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="MyBonusLog.aspx.cs" Inherits="Arsenalcn.CasinoSys.Web.MyBonusLog"
    Title="我的盈亏情况" %>

<%@ Register Src="Control/LeftPanel.ascx" TagName="LeftPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/FieldTooBar.ascx" TagName="FieldTooBar" TagPrefix="uc2" %>
<%@ Register Src="Control/MenuTabBar.ascx" TagName="MenuTabBar" TagPrefix="uc3" %>
<%@ Register Src="Control/GamblerHeader.ascx" TagName="GamblerHeader" TagPrefix="uc4" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:LeftPanel ID="ctrlLeftPanel" runat="server" />
    <div id="MainPanel">
        <uc2:FieldTooBar ID="ctrlFieldTooBar" runat="server" />
        <uc3:MenuTabBar ID="ctrlMenuTabBar" runat="server" />
        <uc4:GamblerHeader ID="ctrlGamblerHeader" runat="server" />
        <asp:GridView ID="gvBonusLog" runat="server" OnPageIndexChanging="gvBonusLog_PageIndexChanging"
            OnRowDataBound="gvBonusLog_RowDataBound" PageSize="20">
            <Columns>
                <asp:TemplateField HeaderText="标识">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlResult" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="PlayTime" DataFormatString="{0:yyyy-MM-dd HH:mm}" HeaderText="比赛时间" />
                <asp:TemplateField HeaderText="主队" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlHome" runat="server" class="StrongLink"></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-Width="20">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlVersus" runat="server"></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="客队" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlAway" runat="server" class="StrongLink"></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="投注金额" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlBetCount" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="盈亏" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <asp:Literal ID="ltrWinLose" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="额外奖励" >
                    <ItemTemplate>
                        <asp:Literal ID="ltrlExtraBonus" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
