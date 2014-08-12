using System;
using System.Data;
using System.Web.UI.WebControls;

using Arsenalcn.ClubSys.DataAccess;
using ArsenalVideo = Arsenal.Entity.Video;

namespace Arsenalcn.ClubSys.Web.Control
{
    public partial class ClubVideo : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (GRank > 0)
                ddlGoalRank.SelectedValue = GRank.ToString();

            if (TRank > 0)
                ddlTeamRank.SelectedValue = TRank.ToString();

            if (clubID > 0)
            {
                int haveVideoCount = 0;
                int totalVideoCount = ArsenalVideo.Cache.GetAvailableVideosByRank(GRank, TRank).Count;

                //load data
                DataTable dt = UserVideo.GetUserVideoByClubID(clubID);

                if (dt != null)
                {
                    string expression = string.Empty;

                    if (GRank > 0 && TRank <= 0 )
                        expression = string.Format("GoalRank = {0}", GRank.ToString());
                    else if (GRank > 0 && TRank > 0)
                        expression = string.Format("GoalRank = {0} AND TeamworkRank = {1}", GRank.ToString(), TRank.ToString());
                    else if (GRank <= 0 && TRank > 0)
                        expression = string.Format("TeamworkRank = {0}", TRank.ToString());

                    DataRow[] dr = dt.Select(expression, "ActiveDate DESC");
                    DataTable dtFilter = dt.Clone();

                    if (dr != null && dr.Length > 0)
                    {
                        for (int i = 0; i < dr.Length; i++)
                        {
                            dtFilter.ImportRow((DataRow)dr[i]);
                        }
                    }
                    dt = dtFilter;
                    haveVideoCount = dt.Rows.Count;
                }

                rptVideo.DataSource = dt;
                rptVideo.DataBind();

                ltlVideoCount.Text = string.Format("<span title=\"GRank:{0} | TRank:{1}\">已获得(总共)视频:<em>{2}/{3}</em></span>", GRank.ToString(), TRank.ToString(), haveVideoCount.ToString(), totalVideoCount.ToString());
            }
        }

        private int clubID = -1;
        public int ClubID
        {
            set
            {
                clubID = value;
            }
        }

        public int GRank
        {
            get
            {
                int tmp;
                if (int.TryParse(ddlGoalRank.SelectedValue, out tmp))
                    return tmp;
                else if (int.TryParse(Request.QueryString["GRank"], out tmp))
                    return tmp;
                else
                {
                    Response.Redirect(string.Format("ClubVideo.aspx?ClubID={0}", clubID.ToString()));

                    return 0;
                }
            }
        }

        public int TRank
        {
            get
            {
                int tmp;
                if (int.TryParse(ddlTeamRank.SelectedValue, out tmp))
                    return tmp;
                else if (int.TryParse(Request.QueryString["TRank"], out tmp))
                    return tmp;
                else
                    return 0;
            }
        }

        protected void rptVideo_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                DataRowView drv = e.Item.DataItem as DataRowView;

                Label lblPlayerVideoID = e.Item.FindControl("lblPlayerVideoID") as Label;
                Label lblPlayerVideoPath = e.Item.FindControl("lblPlayerVideoPath") as Label;
                LinkButton btnSwfView = e.Item.FindControl("btnSwfView") as LinkButton;

                lblPlayerVideoID.Text = drv["ID"].ToString();
                lblPlayerVideoPath.Text = string.Format("swf/PlayerVideoActive.swf?XMLURL=ServerXml.aspx%3FUserVideoID={0}", drv["ID"].ToString());

                btnSwfView.OnClientClick = "GenFlashFrame('swf/ShowVideoRoom.swf?XMLURL=ServerXml.aspx%3FUserVideoID=" + drv["ID"].ToString() + "', '480', '300', true); return false";
            }
        }
    }
}