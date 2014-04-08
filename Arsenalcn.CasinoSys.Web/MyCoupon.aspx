<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="MyCoupon.aspx.cs" Inherits="Arsenalcn.CasinoSys.Web.MyCoupon" Title="我的投注单" %>

<%@ Register Src="Control/LeftPanel.ascx" TagName="LeftPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/FieldTooBar.ascx" TagName="FieldTooBar" TagPrefix="uc2" %>
<%@ Register Src="Control/MenuTabBar.ascx" TagName="MenuTabBar" TagPrefix="uc3" %>
<%@ Register Src="Control/GamblerHeader.ascx" TagName="GamblerHeader" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:LeftPanel ID="ctrlLeftPanel" runat="server" />
    <div id="MainPanel">
        <uc2:FieldTooBar ID="ctrlFieldTooBar" runat="server" />
        <uc3:MenuTabBar ID="ctrlMenuTabBar" runat="server" />
        <uc4:GamblerHeader ID="ctrlGamblerHeader" runat="server" />
        <div class="FunctionBar">
            <div class="DivFloatLeft CasinoSys_Tip" style="border: none">
                <span>统一提交所有比赛的比分投注，一经提交则不能修改。</span></div>
            <div class="DivFloatRight">
                <asp:Button ID="btnSubmit" runat="server" Text="保存并提交投注单" OnClick="btnSubmit_Click"
                    CssClass="InputBtn" OnClientClick="return confirm('确认保存并提交投注单?')" /></div>
            <div class="Clear">
            </div>
        </div>
        <asp:GridView ID="gvMatch" runat="server" PageSize="100" DataKeyNames="MatchGuid"
            OnRowDataBound="gvMatch_RowDataBound">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        分类</HeaderTemplate>
                    <ItemTemplate>
                        <a href="CasinoGame.aspx?League=<%#DataBinder.Eval(Container.DataItem, "LeagueGuid") %>"
                            title="<%#DataBinder.Eval(Container.DataItem, "League") %>">
                            <img src="<%#DataBinder.Eval(Container.DataItem, "LeagueLogo") %>" alt="<%#DataBinder.Eval(Container.DataItem, "League") %>"
                                class="CasinoSys_CategoryImg" /></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="主队" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <span class="CasinoSys_GameName"><a class="StrongLink" href="CasinoTeam.aspx?Team=<%# DataBinder.Eval(Container.DataItem, "Home") %>"
                            title="<%# DataBinder.Eval(Container.DataItem, "HomeEng") %>">
                            <%# DataBinder.Eval(Container.DataItem, "HomeDisplay") %></a>
                            <img src="<%# DataBinder.Eval(Container.DataItem, "HomeLogo") %>" alt="<%# DataBinder.Eval(Container.DataItem, "HomeEng") %>" />
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="vs" ItemStyle-Width="50">
                    <ItemTemplate>
                        <div style="white-space: nowrap">
                            <asp:TextBox ID="tbHomeScore" runat="server" CssClass="TextBox" Width="10" ToolTip="主队得分"></asp:TextBox>
                            <a href="CasinoTeam.aspx?Match=<%# DataBinder.Eval(Container.DataItem, "MatchGuid") %>">
                                <em title="<%# DataBinder.Eval(Container.DataItem, "Ground") %>(<%# DataBinder.Eval(Container.DataItem, "Capacity") %>)">
                                    vs</em></a>
                            <asp:TextBox ID="tbAwayScore" runat="server" CssClass="TextBox" Width="10" ToolTip="客队得分"></asp:TextBox>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="客队" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <span class="CasinoSys_GameName">
                            <img src="<%# DataBinder.Eval(Container.DataItem, "AwayLogo") %>" alt="<%# DataBinder.Eval(Container.DataItem, "AwayEng") %>" />
                            <a class="StrongLink" href="CasinoTeam.aspx?Team=<%# DataBinder.Eval(Container.DataItem, "Away") %>"
                                title="<%# DataBinder.Eval(Container.DataItem, "AwayEng") %>">
                                <%# DataBinder.Eval(Container.DataItem, "AwayDisplay") %></a></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="PlayTime" HeaderText="比赛时间" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                <asp:TemplateField>
                    <HeaderTemplate>
                        主队胜</HeaderTemplate>
                    <ItemTemplate>
                        <em title="主队胜赔率">
                            <asp:Literal ID="ltrlWinRate" runat="server"></asp:Literal></em>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        双方平</HeaderTemplate>
                    <ItemTemplate>
                        <em title="双方平赔率">
                            <asp:Literal ID="ltrlDrawRate" runat="server"></asp:Literal></em>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        客队胜</HeaderTemplate>
                    <ItemTemplate>
                        <em title="客队胜赔率">
                            <asp:Literal ID="ltrlLoseRate" runat="server"></asp:Literal></em>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
