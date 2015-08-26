using System;

using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Entity;

namespace Arsenalcn.ClubSys.Web
{
    public partial class ClubVideoView : Common.BasePage
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

        //public int GRank
        //{
        //    get
        //    {
        //        int tmp;
        //        if (int.TryParse(ddlGoalRank.SelectedValue, out tmp))
        //            return tmp;
        //        else if (int.TryParse(Request.QueryString["GRank"], out tmp))
        //            return tmp;
        //        else
        //        {
        //            Response.Redirect(string.Format("ClubVideo.aspx?ClubID={0}", ClubID.ToString()));

        //            return 0;
        //        }
        //    }
        //}

        //public int TRank
        //{
        //    get
        //    {
        //        int tmp;
        //        if (int.TryParse(ddlTeamRank.SelectedValue, out tmp))
        //            return tmp;
        //        else if (int.TryParse(Request.QueryString["TRank"], out tmp))
        //            return tmp;
        //        else
        //            return 0;
        //    }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            var club = ClubLogic.GetClubInfo(ClubID);

            if (club != null && this.Title.IndexOf("{0}") >= 0)
                this.Title = string.Format(this.Title, club.FullName);

            #region SetControlProperty

            ctrlLeftPanel.UserID = this.userid;
            ctrlLeftPanel.UserName = this.username;
            ctrlLeftPanel.UserKey = this.userkey;

            ctrlFieldToolBar.UserID = this.userid;
            ctrlFieldToolBar.UserName = this.username;

            ctrlMenuTabBar.CurrentMenu = Arsenalcn.ClubSys.Web.Control.ClubMenuItem.ClubVideo;
            ctrlMenuTabBar.ClubID = ClubID;

            ctrlClubSysHeader.UserID = this.userid;
            ctrlClubSysHeader.ClubID = ClubID;
            ctrlClubSysHeader.UserName = this.username;

            #endregion

            ctrlClubVideo.ClubID = ClubID;
        }
    }
}
