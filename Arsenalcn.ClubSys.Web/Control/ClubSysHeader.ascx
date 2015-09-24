<%@ Control Language="C#" CodeBehind="ClubSysHeader.ascx.cs" Inherits="Arsenalcn.ClubSys.Web.Control.ClubSysHeader"
    EnableViewState="false" %>
<div class="ClubSys_Header">
    <table>
        <tr>
            <td rowspan="2" class="ClubLogo">
                <asp:Image ID="imgClubLogo" runat="server" Width="80" />
            </td>
            <td class="ClubTitle">
                <asp:Literal ID="ltrlClubFullName" runat="server"></asp:Literal>
            </td>
            <td class="ClubBtnGroup">
                <asp:LinkButton ID="btnGetStrip" runat="server" CssClass="LinkBtn GetBtn" Text="获取装备"
                    PostBackUrl="../ClubBingo.aspx"></asp:LinkButton>
                <asp:LinkButton ID="btnJoinClub" runat="server" CssClass="LinkBtn JoinBtn" Text="加入球会(审核)"
                    OnClick="btnJoinClub_Click" OnClientClick="return window.confirm('您是否要加入该球会？');"></asp:LinkButton>
                <asp:LinkButton ID="btnCancelApply" runat="server" CssClass="LinkBtn CancelBtn" Text="取消申请"
                    OnClick="btnCancelApply_Click" OnClientClick="return window.confirm('您是否要取消加入球会申请？');"></asp:LinkButton>
                <asp:LinkButton ID="btnLeaveClub" runat="server" CssClass="LinkBtn BackBtn" Text="退出球会"
                    OnClick="btnLeaveClub_Click" OnClientClick="return window.confirm('您是否要退出该球会？');"></asp:LinkButton>
                <!-- 会长/干事专用显示 -->
                <a runat="server" id="aManageClub" class="LinkBtn ManageBtn" href="../ManageApplication.aspx?ClubID={0}">
                    球会管理</a>
            </td>
        </tr>
        <tr>
            <td class="ClubDesc">
                <asp:Literal ID="ltrlClubDesc" runat="server"></asp:Literal>
            </td>
            <td>
                <div runat="server" id="divClubRank" class="ClubSys_Rank" title="球会等级">
                </div>
            </td>
        </tr>
    </table>
</div>
