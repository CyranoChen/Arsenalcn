<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" CodeBehind="AdminClubInfo.aspx.cs"
    Inherits="Arsenalcn.ClubSys.Web.AdminClubInfo" Title="后台管理 管理球会信息" EnableViewState="false" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:AdminPanel ID="pnlAdmin" runat="server" />
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server" />
        <asp:GridView ID="gvClubInfo" runat="server" DataSourceID="ClubInfoDataSouce" DataKeyNames="ClubUid"
            OnPageIndexChanging="gvClubInfo_PageIndexChanging" OnRowCancelingEdit="gvClubInfo_RowCancelingEdit"
            OnRowEditing="gvClubInfo_RowEditing">
            <Columns>
                <asp:BoundField DataField="FullName" HeaderText="球会名称" ControlStyle-CssClass="TextBox" />
                <asp:BoundField DataField="ShortName" HeaderText="简称" ControlStyle-CssClass="TextBox" />
                <asp:BoundField DataField="RankLevel" HeaderText="等级" ControlStyle-CssClass="TextBox" />
                <asp:BoundField DataField="RankScore" HeaderText="评价分" ControlStyle-CssClass="TextBox" />
                <asp:BoundField DataField="Fortune" HeaderText="球会财富" ControlStyle-CssClass="TextBox" />
                <asp:BoundField DataField="MemberCredit" HeaderText="会员积分" ControlStyle-CssClass="TextBox" />
                <asp:BoundField DataField="MemberFortune" HeaderText="会员财富" ControlStyle-CssClass="TextBox" />
                <asp:BoundField DataField="MemberRP" HeaderText="会员RP" ControlStyle-CssClass="TextBox" />
                <asp:BoundField DataField="MemberLoyalty" HeaderText="会员忠诚" ControlStyle-CssClass="TextBox" />
                <asp:BoundField DataField="IsActive" HeaderText="开关" ReadOnly="true" />
                <asp:CommandField ShowEditButton="true" EditText="修改" UpdateText="保存" CancelText="取消"
                    ControlStyle-CssClass="LinkBtn" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="ClubInfoDataSouce" SelectCommand="SELECT * From [dbo].[AcnClub_Club]"
            UpdateCommand="UPDATE [dbo].[AcnClub_Club] SET FullName=@FullName, ShortName=@ShortName, RankLevel=@RankLevel, RankScore=@RankScore,
            Fortune=@Fortune, MemberCredit=@MemberCredit, MemberFortune=@MemberFortune, MemberRP=@MemberRP, MemberLoyalty=@MemberLoyalty
	        WHERE ClubUid = @ClubUid" runat="server"></asp:SqlDataSource>
    </div>
</asp:Content>
