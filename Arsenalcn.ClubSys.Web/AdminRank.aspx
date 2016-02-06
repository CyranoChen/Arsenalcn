<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" CodeBehind="AdminRank.aspx.cs"
Inherits="Arsenalcn.ClubSys.Web.AdminRank" Title="后台管理 积分评价" EnableViewState="false" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:AdminPanel ID="pnlAdmin" runat="server"/>
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server"/>
        <asp:GridView ID="gvRanks" runat="server" DataSourceID="RanksConfigSqlDataSource"
                      DataKeyNames="RankLevelID" OnRowCancelingEdit="gvRanks_RowCancelingEdit" OnRowEditing="gvRanks_RowEditing">
            <Columns>
                <asp:BoundField DataField="RankLevelID" HeaderText="等级" ReadOnly="true"/>
                <asp:BoundField DataField="MaxClubFortune" HeaderText="最大球会财富" ItemStyle-HorizontalAlign="Right"
                                ControlStyle-CssClass="TextBox"/>
                <asp:BoundField DataField="MaxMember" HeaderText="最大球会人数" ItemStyle-HorizontalAlign="Right"
                                ControlStyle-CssClass="TextBox"/>
                <asp:BoundField DataField="MaxExecutor" HeaderText="最大球会干事数" ControlStyle-CssClass="TextBox"/>
                <asp:BoundField DataField="MemberCreditRankEvaluateValue" HeaderText="成员积分评价系数" ItemStyle-HorizontalAlign="Right"
                                ControlStyle-CssClass="TextBox"/>
                <asp:BoundField DataField="MemberFortuneRankEvaluateValue" HeaderText="成员财富评价系数"
                                ItemStyle-HorizontalAlign="Right" ControlStyle-CssClass="TextBox"/>
                <asp:BoundField DataField="MemberRPRankEvaluateValue" HeaderText="成员RP评价系数" ItemStyle-HorizontalAlign="Right"
                                ControlStyle-CssClass="TextBox"/>
                <asp:BoundField DataField="MemberLoyaltyRankEvaluateValue" HeaderText="成员忠诚评价系数"
                                ItemStyle-HorizontalAlign="Right" ControlStyle-CssClass="TextBox"/>
                <asp:CommandField ShowEditButton="true" EditText="修改" HeaderText="修改" UpdateText="保存"
                                  CancelText="取消" ControlStyle-CssClass="LinkBtn"/>
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="RanksConfigSqlDataSource" SelectCommand="SELECT * From [dbo].[AcnClub_ConfigRank]"
                           UpdateCommand="UPDATE [dbo].[AcnClub_ConfigRank] SET MaxClubFortune = @MaxClubFortune, MaxMember = @MaxMember, 
            MaxExecutor = @MaxExecutor, MemberCreditRankEvaluateValue = @MemberCreditRankEvaluateValue,	MemberFortuneRankEvaluateValue = @MemberFortuneRankEvaluateValue, 
            MemberRPRankEvaluateValue = @MemberRPRankEvaluateValue, MemberLoyaltyRankEvaluateValue = @MemberLoyaltyRankEvaluateValue WHERE RankLevelID = @RankLevelID"
                           runat="server">
        </asp:SqlDataSource>
    </div>
</asp:Content>