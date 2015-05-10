using System;
using System.Web.UI.WebControls;
using System.Linq;

using Arsenal.Service;

namespace Arsenal.Web
{
    public partial class AdminVideo : AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = this.Username;
            ctrlCustomPagerInfo.PageChanged += new Control.CustomPagerInfo.PageChangedEventHandler(ctrlCustomPagerInfo_PageChanged);

            if (!IsPostBack)
            {
                #region Bind ddlGoalYear

                ddlGoalYear.DataSource = Video.Cache.ColList_GoalYear;
                ddlGoalYear.DataBind();

                ddlGoalYear.Items.Insert(0, new ListItem("--进球年份--", string.Empty));
                #endregion

                BindData();
            }
        }

        private Guid? _videoGuid = null;
        private Guid? VideoGuid
        {
            get
            {
                if (_videoGuid.HasValue && _videoGuid == Guid.Empty)
                    return _videoGuid;
                else if (!string.IsNullOrEmpty(Request.QueryString["VideoGuid"]))
                {
                    try { return new Guid(Request.QueryString["VideoGuid"]); }
                    catch { return Guid.Empty; }
                }
                else
                    return null;
            }
            set { _videoGuid = value; }
        }

        private void BindData()
        {
            var list = new Video().All<Video>().ToList().FindAll(x =>
                 {
                     Boolean returnValue = true;
                     string tmpString = string.Empty;

                     if (ViewState["GoalYear"] != null)
                     {
                         tmpString = ViewState["GoalYear"].ToString();
                         if (!string.IsNullOrEmpty(tmpString))
                             returnValue = returnValue && x.GoalYear.Equals(tmpString);
                     }

                     if (ViewState["GoalRank"] != null)
                     {
                         tmpString = ViewState["GoalRank"].ToString();
                         if (!string.IsNullOrEmpty(tmpString))
                             returnValue = returnValue && x.GoalRank.Equals(tmpString);
                     }

                     if (ViewState["TeamworkRank"] != null)
                     {
                         tmpString = ViewState["TeamworkRank"].ToString();
                         if (!string.IsNullOrEmpty(tmpString))
                             returnValue = returnValue && x.TeamworkRank.Equals(tmpString);
                     }

                     return returnValue;
                 });

            #region set GridView Selected PageIndex
            if (VideoGuid.HasValue && !VideoGuid.Equals(Guid.Empty))
            {
                int i = list.FindIndex(x => x.VideoGuid.Equals(VideoGuid));
                if (i >= 0)
                {
                    gvVideo.PageIndex = i / gvVideo.PageSize;
                    gvVideo.SelectedIndex = i % gvVideo.PageSize;
                }
                else
                {
                    gvVideo.PageIndex = 0;
                    gvVideo.SelectedIndex = -1;
                }
            }
            else
            {
                gvVideo.SelectedIndex = -1;
            }
            #endregion

            gvVideo.DataSource = list;
            gvVideo.DataBind();

            #region set Control Custom Pager
            if (gvVideo.BottomPagerRow != null)
            {
                gvVideo.BottomPagerRow.Visible = true;
                ctrlCustomPagerInfo.Visible = true;

                ctrlCustomPagerInfo.PageIndex = gvVideo.PageIndex;
                ctrlCustomPagerInfo.PageCount = gvVideo.PageCount;
                ctrlCustomPagerInfo.RowCount = list.Count;
                ctrlCustomPagerInfo.InitComponent();
            }
            else
            {
                ctrlCustomPagerInfo.Visible = false;
            }
            #endregion
        }

        protected void gvVideo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvVideo.PageIndex = e.NewPageIndex;
            VideoGuid = Guid.Empty;

            BindData();
        }

        protected void ctrlCustomPagerInfo_PageChanged(object sender, Control.CustomPagerInfo.DataNavigatorEventArgs e)
        {
            if (e.PageIndex > 0)
            {
                gvVideo.PageIndex = e.PageIndex;
                VideoGuid = Guid.Empty;
            }

            BindData();
        }

        protected void gvVideo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvVideo.SelectedIndex != -1)
            {
                Response.Redirect(string.Format("AdminVideoView.aspx?VideoGuid={0}", gvVideo.DataKeys[gvVideo.SelectedIndex].Value.ToString()));
            }
        }

        protected void gvVideo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Video v = e.Row.DataItem as Video;
                Literal ltrlMatchOpponentInfo = e.Row.FindControl("ltrlMatchOpponentInfo") as Literal;

                if (v.ArsenalMatchGuid.HasValue)
                {
                    Match m = Match.Cache.Load(v.ArsenalMatchGuid.Value);

                    if (m != null)
                    {
                        ltrlMatchOpponentInfo.Text = string.Format("<a href=\"AdminMatchView.aspx?MatchGuid={0}\" target=\"_blank\"><em>{1}</em></a>", m.MatchGuid.ToString(), m.TeamName);
                    }
                    else
                    {
                        ltrlMatchOpponentInfo.Text = v.Opponent;
                    }
                }
                else
                {
                    ltrlMatchOpponentInfo.Text = v.Opponent;
                }
            }
        }


        protected void btnRefreshCache_Click(object sender, EventArgs e)
        {
            Video.Cache.RefreshCache();

            ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('更新缓存成功');window.location.href=window.location.href", true);
        }

        protected void ddlGoalYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlGoalYear.SelectedValue))
                ViewState["GoalYear"] = ddlGoalYear.SelectedValue;
            else
                ViewState["GoalYear"] = string.Empty;

            VideoGuid = Guid.Empty;
            gvVideo.PageIndex = 0;

            BindData();
        }

        protected void ddlGoalRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlGoalRank.SelectedValue))
                ViewState["GoalRank"] = ddlGoalRank.SelectedValue;
            else
                ViewState["GoalRank"] = string.Empty;

            VideoGuid = Guid.Empty;
            gvVideo.PageIndex = 0;

            BindData();
        }

        protected void ddlTeamworkRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlTeamworkRank.SelectedValue))
                ViewState["TeamworkRank"] = ddlTeamworkRank.SelectedValue;
            else
                ViewState["TeamworkRank"] = string.Empty;

            VideoGuid = Guid.Empty;
            gvVideo.PageIndex = 0;

            BindData();
        }
    }
}