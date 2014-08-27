using System;
using System.Data;
using System.Web.UI.WebControls;

using Arsenalcn.ClubSys.Entity;
using Arsenalcn.ClubSys.Service;
using ArsenalVideo = Arsenalcn.ClubSys.Service.Arsenal.Video;

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
                Response.Redirect(string.Format("ClubVideoView.aspx?ClubID={0}&GRank={1}", ClubID.ToString(), gRank));

            Club club = ClubLogic.GetClubInfo(ClubID);

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
            DataTable dt = UserVideo.GetUserVideoByClubID(ClubID);

            if (dt != null)
            {
                dt.Columns.Add("AdditionalData", typeof(int));

                foreach (DataRow dr in dt.Rows)
                {
                    dr["AdditionalData"] = (int)(Convert.ToInt16(dr["GoalRank"]) * 20);
                }
            }

            gvVideo.DataSource = dt;
            gvVideo.DataBind();

            this.ltlVideoCount.Text = string.Format("<span title=\"同一视频计为一个,仅计算可获得视频\">已获得(总共)视频:<em>{0}/{1}</em></span>", dt.Rows.Count.ToString(), Arsenal_Video.Cache.VideoList_Legend.Count.ToString());
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
                DataRowView drv = e.Row.DataItem as DataRowView;

                Literal ltrlVideo = e.Row.FindControl("ltrlVideo") as Literal;
                LinkButton btnSwfView = e.Row.FindControl("btnSwfView") as LinkButton;

                String StrSwfContent = "<div class=\"ClubSys_ItemPH\">";
                StrSwfContent += string.Format("<script type=\"text/javascript\">GenSwfObject('PlayerVideoActive{0}', 'swf/PlayerVideoActive.swf?XMLURL=ServerXml.aspx%3FUserVideoID={0}', '80', '100');</script>", drv["ID"].ToString());
                StrSwfContent += "</div>";

                ltrlVideo.Text = StrSwfContent;

                btnSwfView.OnClientClick = string.Format("ShowVideoPreview('{0}'); return false", drv["VideoGuid"].ToString());
            }
        }
    }
}
