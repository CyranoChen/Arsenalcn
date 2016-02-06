using System;
using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Web.Common;
using Arsenalcn.ClubSys.Web.Control;

namespace Arsenalcn.ClubSys.Web
{
    public partial class ClubVideoView : BasePage
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

            if (club != null && Title.IndexOf("{0}") >= 0)
                Title = string.Format(Title, club.FullName);

            #region SetControlProperty

            ctrlLeftPanel.UserID = userid;
            ctrlLeftPanel.UserName = username;
            ctrlLeftPanel.UserKey = userkey;

            ctrlFieldToolBar.UserID = userid;
            ctrlFieldToolBar.UserName = username;

            ctrlMenuTabBar.CurrentMenu = ClubMenuItem.ClubVideo;
            ctrlMenuTabBar.ClubID = ClubID;

            ctrlClubSysHeader.UserID = userid;
            ctrlClubSysHeader.ClubID = ClubID;
            ctrlClubSysHeader.UserName = username;

            #endregion

            ctrlClubVideo.ClubID = ClubID;
        }
    }
}