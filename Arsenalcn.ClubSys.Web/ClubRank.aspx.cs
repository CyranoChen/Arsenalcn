using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

using Arsenalcn.ClubSys.DataAccess;
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

            Club currentClub = ClubLogic.GetClubInfo(ClubID);

            if (currentClub != null && this.Title.IndexOf("{0}") >= 0)
                this.Title = string.Format(this.Title, currentClub.FullName);

            if (currentClub != null)
            {
                int memberCount = ClubLogic.GetClubMemberCount(ClubID);
                int memberQuota = ClubLogic.GetClubMemberQuota(ClubID);

                RankAlgorithm ra = new RankAlgorithm(currentClub);

                ltrlMemberCount.Text = string.Format("<cite class=\"RankLevel\"><a style=\"width: {0}px\">{1}%</a></cite><em>{2}//{3}</em>", (ra.MemberCountRank * 2).ToString(), ra.MemberCountRank.ToString(), memberCount.ToString(), memberQuota.ToString());

                ltrlClubFortune.Text = string.Format("<cite class=\"RankLevel\"><a style=\"width: {0}px\">{1}%</a></cite><em>{2}</em>", (ra.ClubFortuneRank * 2).ToString(), ra.ClubFortuneRank.ToString(), Convert.ToInt32(currentClub.Fortune).ToString("N0"));

                ltrlMemberCredit.Text = string.Format("<cite class=\"RankLevel\"><a style=\"width: {0}px\">{1}%</a></cite><em>{2}</em>", (ra.MemberCreditRank * 2).ToString(), ra.MemberCreditRank.ToString(), Convert.ToInt32(currentClub.MemberCredit).ToString("N0"));

                ltrlMemberRP.Text = string.Format("<cite class=\"RankLevel\"><a style=\"width: {0}px\">{1}%</a></cite><em>{2}</em>", (ra.MemberRPRank * 2).ToString(), ra.MemberRPRank.ToString(), Convert.ToInt32(currentClub.MemberRP).ToString("N0"));

                ltrlEquipmentCount.Text = string.Format("<cite class=\"RankLevel\"><a style=\"width: {0}px\">{1}%</a></cite><em>C:{2}|V:{3}</em>", (ra.MemberEquipmentRank * 2).ToString(), ra.MemberEquipmentRank.ToString(), PlayerStrip.GetClubMemberCardCount(currentClub.ID.Value).ToString(), PlayerStrip.GetClubMemberVideoCount(currentClub.ID.Value).ToString());

                ltrlRankScore.Text = currentClub.RankScore.ToString();
            }
        }
    }
}
