<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" CodeBehind="ClubRank.aspx.cs"
    Inherits="Arsenalcn.ClubSys.Web.ClubRank" Title="{0} 等级评价" EnableViewState="false" %>

<%@ Register Src="Control/LeftPanel.ascx" TagName="LeftPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/FieldToolBar.ascx" TagName="FieldToolBar" TagPrefix="uc2" %>
<%@ Register Src="Control/MenuTabBar.ascx" TagName="MenuTabBar" TagPrefix="uc3" %>
<%@ Register Src="Control/ClubSysHeader.ascx" TagName="ClubSysHeader" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:LeftPanel ID="ctrlLeftPanel" runat="server" />
    <div id="MainPanel">
        <uc2:FieldToolBar ID="ctrlFieldToolBar" runat="server" />
        <uc3:MenuTabBar ID="ctrlMenuTabBar" runat="server" />
        <uc4:ClubSysHeader ID="ctrlClubSysHeader" runat="server" />
        <div class="ClubSys_RankInfo">
            <div class="RankChartPanel">

                <script type="text/javascript">
                    GenSwfObject('RankChart', 'swf/RankChart.swf?XMLURL=ServerXml.aspx%3FClubID=<%=ClubID %>', '220', '220');
                </script>

            </div>
            <div class="RankItemPanel">
                <dl>
                    <dd title="全部会员数">
                        <label class="ClubSys_FlagTop">
                            1. 会员数:
                        </label>
                        <asp:Literal ID="ltrlMemberCount" runat="server"></asp:Literal>
                    </dd>
                    <dd title="球会积累的财富值">
                        <label>
                            2. 财富数:
                        </label>
                        <asp:Literal ID="ltrlClubFortune" runat="server"></asp:Literal>
                    </dd>
                    <dd title="全部会员的积分总和">
                        <label>
                            3. 总积分:
                        </label>
                        <asp:Literal ID="ltrlMemberCredit" runat="server"></asp:Literal>
                    </dd>
                    <dd title="全部会员的RP总和">
                        <label>
                            4. 总RP值:
                        </label>
                        <asp:Literal ID="ltrlMemberRP" runat="server"></asp:Literal>
                    </dd>
                    <dd title="全部球员的卡片数C与视频数V">
                        <label>
                            5. 装备数:
                        </label>
                        <asp:Literal ID="ltrlEquipmentCount" runat="server"></asp:Literal>
                    </dd>
                    <dt><a onclick="document.getElementById('RankDetailPanel').style.display=''">指标详细说明...</a></dt>
                </dl>
            </div>
            <div class="RankPointPanel">
                <div class="RankPoint">
                    <asp:Literal ID="ltrlRankScore" runat="server"></asp:Literal></div>
            </div>
            <div class="Clear">
            </div>
        </div>
        <div id="RankDetailPanel" class="ClubSys_RankInfo" style="display: none;">
            <div class="ClubSys_RankDetailPanel">
                <p>
                    球会等级LV由球会财富数决定;球会总评价分RP由5个指标的评价分决定(按权重)。5个指标是:</p>
                <ul>
                    <li>1.会员数: 体现一个球会的人丁兴旺程度,等级将决定会员数上限。</li>
                    <li>2.财富数: 体现一个球会的经营能力与财富积累程度,直接影响等级。</li>
                    <li>3.总积分: 体现一个球会中会员的总体势力,由该球会所有会员积分相加而得。</li>
                    <li>4.总RP值: 体现一个球会中会员的活跃程度,由该球会所有会员RP相加而得。</li>
                    <li>5.装备数: 体现一个球会中球员的收藏价值,由该球会现有球员所有卡片与视频数组成。</li>
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
