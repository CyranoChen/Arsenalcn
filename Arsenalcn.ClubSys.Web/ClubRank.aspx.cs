using System;
using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Entity;

namespace Arsenalcn.ClubSys.Web
{
    public partial class ClubRank : Common.BasePage
    {
        public int ClubID
        {
            get
            {
                int tmp;
                if (int.TryParse(Request.QueryString["ClubID"], out tmp))
                    return tmp;
                else
                {
                    Response.Redirect("ClubPortal.aspx");

                    return -1;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            #region SetControlProperty

            ctrlLeftPanel.UserID = this.userid;
            ctrlLeftPanel.UserName = this.username;
            ctrlLeftPanel.UserKey = this.userkey;

            ctrlFieldToolBar.UserID = this.userid;
            ctrlFieldToolBar.UserName = this.username;

            ctrlMenuTabBar.CurrentMenu = Arsenalcn.ClubSys.Web.Control.ClubMenuItem.ClubRank;
            ctrlMenuTabBar.ClubID = ClubID;

            ctrlClubSysHeader.UserID = this.userid;
            ctrlClubSysHeader.ClubID = ClubID;
            ctrlClubSysHeader.UserName = this.username;

            #endregion

            var currentClub = ClubLogic.GetClubInfo(ClubID);

            if (currentClub != null && this.Title.IndexOf("{0}") >= 0)
                this.Title = string.Format(this.Title, currentClub.FullName);

            if (currentClub != null)
            {
                var memberCount = ClubLogic.GetClubMemberCount(ClubID);
                var memberQuota = ClubLogic.GetClubMemberQuota(ClubID);

                var ra = new RankAlgorithm(currentClub);

                ltrlMemberCount.Text =
                    $"<cite class=\"RankLevel\"><a style=\"width: {(ra.MemberCountRank*2).ToString()}px\">{ra.MemberCountRank.ToString()}%</a></cite><em>{memberCount.ToString()}//{memberQuota.ToString()}</em>";

                ltrlClubFortune.Text =
                    $"<cite class=\"RankLevel\"><a style=\"width: {(ra.ClubFortuneRank*2).ToString()}px\">{ra.ClubFortuneRank.ToString()}%</a></cite><em>{Convert.ToInt32(currentClub.Fortune).ToString("N0")}</em>";

                ltrlMemberCredit.Text =
                    $"<cite class=\"RankLevel\"><a style=\"width: {(ra.MemberCreditRank*2).ToString()}px\">{ra.MemberCreditRank.ToString()}%</a></cite><em>{Convert.ToInt32(currentClub.MemberCredit).ToString("N0")}</em>";

                ltrlMemberRP.Text =
                    $"<cite class=\"RankLevel\"><a style=\"width: {(ra.MemberRPRank*2).ToString()}px\">{ra.MemberRPRank.ToString()}%</a></cite><em>{Convert.ToInt32(currentClub.MemberRP).ToString("N0")}</em>";

                ltrlEquipmentCount.Text =
                    $"<cite class=\"RankLevel\"><a style=\"width: {(ra.MemberEquipmentRank*2).ToString()}px\">{ra.MemberEquipmentRank.ToString()}%</a></cite><em>C:{PlayerStrip.GetClubMemberCardCount(currentClub.ID.Value).ToString()}|V:{PlayerStrip.GetClubMemberVideoCount(currentClub.ID.Value).ToString()}</em>";

                ltrlRankScore.Text = currentClub.RankScore.ToString();
            }
        }
    }
}
