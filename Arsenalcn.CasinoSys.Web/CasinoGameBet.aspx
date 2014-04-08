<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" CodeBehind="CasinoGameBet.aspx.cs"
    Inherits="Arsenalcn.CasinoSys.Web.CasinoGameBet" Title="比赛投注" %>

<%@ Register Src="Control/LeftPanel.ascx" TagName="LeftPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/FieldTooBar.ascx" TagName="FieldTooBar" TagPrefix="uc2" %>
<%@ Register Src="Control/MenuTabBar.ascx" TagName="MenuTabBar" TagPrefix="uc3" %>
<%@ Register Src="Control/CasinoHeader.ascx" TagName="CasinoHeader" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <script type="text/javascript">
        function isNumber(oNum) {
            if (!oNum) return false;
            var strP = /^\d+(\.\d+)?$/;
            if (!strP.test(oNum)) return false;
            try {
                if (parseFloat(oNum) != oNum) return false;
            }
            catch (ex) {
                return false;
            }
            return true;
        }

        function BetConfirm() {
            var betCount = $("input.BetCount").val();
            var selected = $("span.SelectedChoice > input:radio:checked").next("label").text();

            var str = "下注" + betCount + "博彩币，投" + selected + "?";

            if (isNumber(betCount))
                return window.confirm(str);
            else {
                alert("投注金额必须为数字");
                return false;
            }
        }

        function BetMatchResultConfirm() {
            var homeRes = $("input.HomeRes").val();
            var awayRes = $("input.AwayRes").val();

            if (isNumber(homeRes) && isNumber(awayRes)) {
                var str = "猜比分为" + homeRes + " : " + awayRes + "?";

                return window.confirm(str);
            }
            else {
                alert("比分必须为数字");

                return false;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:LeftPanel ID="ctrlLeftPanel" runat="server" />
    <div id="MainPanel">
        <uc2:FieldTooBar ID="ctrlFieldTooBar" runat="server" />
        <uc3:MenuTabBar ID="ctrlMenuTabBar" runat="server" />
        <uc4:CasinoHeader ID="ctrlCasinoHeader" runat="server" />
        <asp:Panel ID="pnlClose" runat="server" CssClass="CasinoSys_Tip">
            <span>比赛投注已截至</span>
        </asp:Panel>
        <asp:PlaceHolder ID="phBet" runat="server">
            <div class="FunctionBar">
                <div class="DivFloatLeft CasinoSys_Tip" style="border: none">
                    <span>开放猜输赢(可重复投注)、猜比分(单场只能一次下注)；您现有博彩币<em><%=CurrentGambler.Cash.ToString("N2") %></em></span></div>
                <div class="DivFloatRight">
                    <asp:DropDownList ID="ddlCasinoGame" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCasinoGame_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
                <div class="Clear">
                </div>
            </div>
            <div class="CasinoSys_MainInfo">
                <table class="DataView" cellspacing="0" cellpadding="5">
                    <tbody>
                        <tr class="Row">
                            <td class="FieldHeader">
                                猜输赢:
                            </td>
                            <td align="left">
                                <asp:RadioButtonList ID="rblSingleChoice" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="Flow" CssClass="SelectedChoice">
                                    <asp:ListItem Text="胜" Value="Home" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="负" Value="Draw"></asp:ListItem>
                                    <asp:ListItem Text="平" Value="Away"></asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:TextBox ID="tbBet" runat="server" Text="请输入下注博彩币" Width="100" CssClass="TextBox BetCount"></asp:TextBox>
                                <asp:LinkButton ID="btnSingleChoice" runat="server" Text="投注" CssClass="LinkBtn CashBtn"
                                    OnClick="btnSingleChoice_Click" OnClientClick="return BetConfirm();"></asp:LinkButton>
                            </td>
                        </tr>
                        <tr class="Row" runat="server" id="trMatchResult">
                            <td class="FieldHeader">
                                猜比分:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="tbHome" runat="server" Text="主队" CssClass="TextBox HomeRes" Width="30"></asp:TextBox><em>vs</em><asp:TextBox
                                    ID="tbAway" runat="server" Text="客队" CssClass="TextBox AwayRes" Width="30"></asp:TextBox>
                                <asp:LinkButton ID="btnMatchResult" runat="server" Text="投注" CssClass="LinkBtn CashBtn"
                                    OnClick="btnMatchResult_Click" OnClientClick="return BetMatchResultConfirm();"></asp:LinkButton>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <script type="text/javascript">
                var oriStr = $("input.BetCount").val();
                $("input.BetCount").focus(function () { if ($(this).val() == oriStr) { $(this).val(""); } });
                $("input.BetCount").blur(function () { if ($(this).val() == "") { $(this).val(oriStr); } });

                var home = $("input.HomeRes").val();
                $("input.HomeRes").focus(function () { if ($(this).val() == home) { $(this).val(""); } });
                $("input.HomeRes").blur(function () { if ($(this).val() == "") { $(this).val(home); } });

                var away = $("input.AwayRes").val();
                $("input.AwayRes").focus(function () { if ($(this).val() == away) { $(this).val(""); } });
                $("input.AwayRes").blur(function () { if ($(this).val() == "") { $(this).val(away); } });
            </script>
        </asp:PlaceHolder>
        <asp:GridView ID="gvBet" runat="server" OnRowDataBound="gvBet_RowDataBound">
            <Columns>
                <asp:BoundField DataField="BetTime" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" HeaderText="我的投注记录" />
                <asp:TemplateField HeaderText="投注结果">
                    <ItemTemplate>
                        <em>
                            <asp:Literal ID="ltrlResult" runat="server"></asp:Literal></em>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="BetAmount" HeaderText="投注金额" DataFormatString="{0:N0}"
                    NullDisplayText="/" />
                <asp:TemplateField HeaderText="投注赔率">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlBetRate" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="收益预测">
                    <ItemTemplate>
                        <em>
                            <asp:Literal ID="ltrlBonusCalc" runat="server"></asp:Literal></em>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:GridView ID="gvMatch" runat="server" OnPageIndexChanging="gvMatch_PageIndexChanging"
            OnRowDataBound="gvMatch_RowDataBound" PageSize="10">
            <Columns>
                <asp:TemplateField HeaderText="<em>历史记录</em>">
                    <ItemTemplate>
                        <a href="CasinoGame.aspx?League=<%#DataBinder.Eval(Container.DataItem, "LeagueGuid") %>"
                            title="<%#DataBinder.Eval(Container.DataItem, "LeagueDisplayName") %>">
                            <img src="<%#DataBinder.Eval(Container.DataItem, "LeagueLogo") %>" alt="<%#DataBinder.Eval(Container.DataItem, "LeagueDisplayName") %>"
                                class="CasinoSys_CategoryImg" /></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="PlayTime" DataFormatString="{0:yyyy-MM-dd HH:mm}" HeaderText="比赛时间" />
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
                            <em title="<%# DataBinder.Eval(Container.DataItem, "Ground") %>(<%# DataBinder.Eval(Container.DataItem, "Capacity") %>)">
                                vs</em></a>
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
                <asp:TemplateField>
                    <HeaderTemplate>
                        结果</HeaderTemplate>
                    <ItemTemplate>
                        <%#DataBinder.Eval(Container.DataItem, "ResultHome") %>：<%#DataBinder.Eval(Container.DataItem, "ResultAway") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        注数</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Literal ID="ltrlTotalBetCount" runat="server"></asp:Literal></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        投注总量</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Literal ID="ltrlTotalBetCash" runat="server"></asp:Literal></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        比赛盈亏</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Literal ID="ltrlTotalWin" runat="server"></asp:Literal></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        中奖查询</HeaderTemplate>
                    <ItemTemplate>
                        <a class="LinkBtn SelectBtn" href="CasinoBetLog.aspx?Match=<%#DataBinder.Eval(Container.DataItem, "MatchGuid") %>">
                            中奖查询</a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
