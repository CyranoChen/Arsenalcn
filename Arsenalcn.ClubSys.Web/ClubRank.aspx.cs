using System;
using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Web.Common;
using Arsenalcn.ClubSys.Web.Control;

namespace Arsenalcn.ClubSys.Web
{
    public partial class ClubRank : BasePage
    {
        public int ClubID
        {
            get
            {
                int tmp;
                if (int.TryParse(Request.QueryString["ClubID"], out tmp))
                    return tmp;
                Response.Redirect("ClubPortal.aspx");

                return -1;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            #region SetControlProperty

            ctrlLeftPanel.UserID = userid;
            ctrlLeftPanel.UserName = username;
            ctrlLeftPanel.UserKey = userkey;

            ctrlFieldToolBar.UserID = userid;
            ctrlFieldToolBar.UserName = username;

            ctrlMenuTabBar.CurrentMenu = ClubMenuItem.ClubRank;
            ctrlMenuTabBar.ClubID = ClubID;

            ctrlClubSysHeader.UserID = userid;
            ctrlClubSysHeader.ClubID = ClubID;
            ctrlClubSysHeader.UserName = username;

            #endregion

            var currentClub = ClubLogic.GetClubInfo(ClubID);

            if (currentClub != null && Title.IndexOf("{0}") >= 0)
                Title = string.Format(Title, currentClub.FullName);

            if (currentClub != null)
            {
                var memberCount = ClubLogic.GetClubMemberCount(ClubID);
                var memberQuota = ClubLogic.GetClubMemberQuota(ClubID);

                var ra = new RankAlgorithm(currentClub);

                ltrlMemberCount.Text =
                    $"<cite class=\"RankLevel\"><a style=\"width: {(ra.MemberCountRank*2)}px\">{ra.MemberCountRank}%</a></cite><em>{memberCount}//{memberQuota}</em>";

                ltrlClubFortune.Text =
                    $"<cite class=\"RankLevel\"><a style=\"width: {(ra.ClubFortuneRank*2)}px\">{ra.ClubFortuneRank}%</a></cite><em>{Convert.ToInt32(currentClub.Fortune).ToString("N0")}</em>";

                ltrlMemberCredit.Text =
                    $"<cite class=\"RankLevel\"><a style=\"width: {(ra.MemberCreditRank*2)}px\">{ra.MemberCreditRank}%</a></cite><em>{Convert.ToInt32(currentClub.MemberCredit).ToString("N0")}</em>";

                ltrlMemberRP.Text =
                    $"<cite class=\"RankLevel\"><a style=\"width: {(ra.MemberRPRank*2)}px\">{ra.MemberRPRank}%</a></cite><em>{Convert.ToInt32(currentClub.MemberRP).ToString("N0")}</em>";

                ltrlEquipmentCount.Text =
                    $"<cite class=\"RankLevel\"><a style=\"width: {(ra.MemberEquipmentRank*2)}px\">{ra.MemberEquipmentRank}%</a></cite><em>C:{PlayerStrip.GetClubMemberCardCount(currentClub.ID.Value)}|V:{PlayerStrip.GetClubMemberVideoCount(currentClub.ID.Value)}</em>";

                ltrlRankScore.Text = currentClub.RankScore.ToString();
            }
        }
    }
}