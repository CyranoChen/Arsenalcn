using System;
using System.Data;
using System.Web.UI.WebControls;

using Arsenalcn.ClubSys.Entity;
using Arsenalcn.ClubSys.Service;

namespace Arsenalcn.ClubSys.Web
{
    public partial class ClubVideo : Common.BasePage
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
            int gRank;
            if (int.TryParse(ddlGoalRank.SelectedValue, out gRank))
                Response.Redirect($"ClubVideoView.aspx?ClubID={ClubID.ToString()}&GRank={gRank}");

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

            BindVideo();
        }

        private void BindVideo()
        {
            var dt = Service.UserVideo.GetUserVideoByClubID(ClubID);

            if (dt != null)
            {
                dt.Columns.Add("GoalPlayerName", typeof(string));
                dt.Columns.Add("GoalRank", typeof(int));

                foreach (DataRow dr in dt.Rows)
                {
                    var v = Video.Cache.Load((Guid)dr["VideoGuid"]);

                    dr["GoalPlayerName"] = v.GoalPlayerName;
                    dr["GoalRank"] = Convert.ToInt16(v.GoalRank);

                    //dr["AdditionalData"] = (int)(Convert.ToInt16(dr["GoalRank"]) * 20);
                }
            }

            gvVideo.DataSource = dt;
            gvVideo.DataBind();

            this.ltlVideoCount.Text =
                $"<span title=\"同一视频计为一个,仅计算可获得视频\">已获得(总共)视频:<em>{dt.Rows.Count.ToString()}/{Video.Cache.VideoList_Legend.Count.ToString()}</em></span>";
        }

        protected void gvVideo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvVideo.PageIndex = e.NewPageIndex;

            BindVideo();
        }

        protected void gvVideo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var drv = e.Row.DataItem as DataRowView;

                var ltrlVideo = e.Row.FindControl("ltrlVideo") as Literal;
                var ltrlGoalRankInfo = e.Row.FindControl("ltrlGoalRankInfo") as Literal;
                var btnSwfView = e.Row.FindControl("btnSwfView") as LinkButton;

                if (ltrlVideo != null)
                {
                    var StrSwfContent = "<div class=\"ClubSys_ItemPH\">";
                    StrSwfContent += string.Format("<script type=\"text/javascript\">GenSwfObject('PlayerVideoActive{0}', 'swf/PlayerVideoActive.swf?XMLURL=ServerXml.aspx%3FUserVideoID={0}', '80', '100');</script>", drv["ID"].ToString());
                    StrSwfContent += "</div>";

                    ltrlVideo.Text = StrSwfContent;
                }

                if (ltrlGoalRankInfo != null)
                {
                    ltrlGoalRankInfo.Text =
                        $"<div class=\"ClubSys_PlayerLV\" style=\"width: {((int) (Convert.ToInt16(drv["GoalRank"])*20)).ToString()}px;\" title=\"视频等级\"></div>";
                }

                if (btnSwfView != null)
                {
                    btnSwfView.OnClientClick = $"ShowVideoPreview('{drv["VideoGuid"].ToString()}'); return false";
                }
            }
        }
    }
}
