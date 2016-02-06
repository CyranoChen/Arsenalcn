using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Arsenalcn.ClubSys.Service;
using UserVideo = Arsenalcn.ClubSys.Entity.UserVideo;

namespace Arsenalcn.ClubSys.Web.Control
{
    public partial class ClubVideo : UserControl
    {
        private int clubID = -1;

        public int ClubID
        {
            set { clubID = value; }
        }

        public int GRank
        {
            get
            {
                int tmp;
                if (int.TryParse(ddlGoalRank.SelectedValue, out tmp))
                    return tmp;
                if (int.TryParse(Request.QueryString["GRank"], out tmp))
                    return tmp;
                Response.Redirect($"ClubVideo.aspx?ClubID={clubID}");

                return 0;
            }
        }

        public int TRank
        {
            get
            {
                int tmp;
                if (int.TryParse(ddlTeamRank.SelectedValue, out tmp))
                    return tmp;
                if (int.TryParse(Request.QueryString["TRank"], out tmp))
                    return tmp;
                return 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (GRank > 0)
                ddlGoalRank.SelectedValue = GRank.ToString();

            if (TRank > 0)
                ddlTeamRank.SelectedValue = TRank.ToString();

            if (clubID > 0)
            {
                var haveVideoCount = 0;
                var totalVideoCount = Video.Cache.GetAvailableVideosByRank(GRank, TRank).Count;

                //load data
                //DataTable dt = UserVideo.GetUserVideoByClubID(clubID);

                var list = UserVideo.GetUserVideosByClubID(clubID);

                if (list != null && list.Count > 0)
                {
                    //string expression = string.Empty;

                    if (GRank > 0 && TRank <= 0)
                    {
                        //expression = string.Format("GoalRank = {0}", GRank.ToString());
                        list =
                            list.FindAll(
                                delegate(UserVideo uv)
                                {
                                    return Video.Cache.Load(uv.VideoGuid).GoalRank.Equals(GRank.ToString());
                                });
                    }
                    else if (GRank > 0 && TRank > 0)
                    {
                        //expression = string.Format("GoalRank = {0} AND TeamworkRank = {1}", GRank.ToString(), TRank.ToString());
                        list = list.FindAll(delegate(UserVideo uv)
                        {
                            var v = Video.Cache.Load(uv.VideoGuid);
                            return v.GoalRank.Equals(GRank.ToString()) && v.TeamworkRank.Equals(TRank.ToString());
                        });
                    }
                    else if (GRank <= 0 && TRank > 0)
                    {
                        //expression = string.Format("TeamworkRank = {0}", TRank.ToString());
                        list =
                            list.FindAll(
                                delegate(UserVideo uv)
                                {
                                    return Video.Cache.Load(uv.VideoGuid).TeamworkRank.Equals(TRank.ToString());
                                });
                    }

                    //DataRow[] dr = dt.Select(expression, "ActiveDate DESC");
                    list.Sort(
                        delegate(UserVideo uv1, UserVideo uv2) { return uv2.ActiveDate.CompareTo(uv1.ActiveDate); });

                    haveVideoCount = list.Count;

                    rptVideo.DataSource = list;
                    rptVideo.DataBind();
                }

                ltlVideoCount.Text =
                    $"<span title=\"GRank:{GRank} | TRank:{TRank}\">已获得(总共)视频:<em>{haveVideoCount}/{totalVideoCount}</em></span>";
            }
        }

        protected void rptVideo_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                //DataRowView drv = e.Item.DataItem as DataRowView;
                var uv = e.Item.DataItem as UserVideo;

                var lblPlayerVideoID = e.Item.FindControl("lblPlayerVideoID") as Label;
                var lblPlayerVideoPath = e.Item.FindControl("lblPlayerVideoPath") as Label;
                var btnSwfView = e.Item.FindControl("btnSwfView") as LinkButton;

                lblPlayerVideoID.Text = uv.UserVideoID.ToString();
                lblPlayerVideoPath.Text =
                    $"swf/PlayerVideoActive.swf?XMLURL=ServerXml.aspx%3FUserVideoID={uv.UserVideoID}";

                btnSwfView.OnClientClick = "GenFlashFrame('swf/ShowVideoRoom.swf?XMLURL=ServerXml.aspx%3FUserVideoID=" +
                                           uv.UserVideoID + "', '480', '300', true); return false";
            }
        }
    }
}