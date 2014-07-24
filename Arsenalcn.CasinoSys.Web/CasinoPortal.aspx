<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="CasinoPortal.aspx.cs" Inherits="Arsenalcn.CasinoSys.Web.CasinoPortal"
    Title="ACN CasinoSys 博彩系统" %>

<%@ Register Src="Control/LeftPanel.ascx" TagName="LeftPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/FieldTooBar.ascx" TagName="FieldTooBar" TagPrefix="uc2" %>
<%@ Register Src="Control/MenuTabBar.ascx" TagName="MenuTabBar" TagPrefix="uc3" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
    <script type="text/javascript">
        var p = "http", d = "static";
        if (document.location.protocol == "https:")
        { p += "s"; d = "engine"; }
        var z = document.createElement("script");
        z.type = "text/javascript";
        z.async = true;
        z.src = p + "://" + d + ".adzerk.net/ados.js";
        var s = document.getElementsByTagName("script")[0];
        s.parentNode.insertBefore(z, s);
    </script>
    <script type="text/javascript">
        var ados = ados || {};
        ados.run = ados.run || [];
        ados.run.push(function () {
            /* load placement for account: Bodog, site: Arsenalcntaobao, size: 180x30 - 180 x 30*/
            ados_add_placement(6641, 63640, "azk22313", 243);
            ados_load();
        });
    </script>
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
            <div class="DivFloatRight">
                <div id="azk22313"></div>
            </div>
            <div class="Clear">
            </div>
        </div>
        <asp:GridView ID="gvMatch" runat="server" PageSize="100" OnRowDataBound="gvMatch_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="分类">
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
                <asp:TemplateField HeaderText="vs" ItemStyle-Width="20">
                    <ItemTemplate>
                        <a href="CasinoTeam.aspx?Match=<%# DataBinder.Eval(Container.DataItem, "MatchGuid") %>">
                            <em title="<%# DataBinder.Eval(Container.DataItem, "Ground") %>(<%# DataBinder.Eval(Container.DataItem, "Capacity") %>)">vs</em></a>
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
                        <asp:HyperLink ID="BtnBet_Bodog" runat="server" CssClass="LinkBtn" ToolTip="博狗投注"></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
