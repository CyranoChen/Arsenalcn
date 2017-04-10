using System;
using System.Linq;
using System.Web.UI.WebControls;
using Arsenal.Service;
using Arsenal.Web.Control;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;

namespace Arsenal.Web
{
    public partial class AdminVideo : AdminPageBase
    {
        private readonly IRepository _repo = new Repository();

        private Guid? _videoGuid;

        private Guid? VideoGuid
        {
            get
            {
                if (_videoGuid.HasValue && _videoGuid == Guid.Empty)
                    return _videoGuid;
                if (!string.IsNullOrEmpty(Request.QueryString["VideoGuid"]))
                {
                    try
                    {
                        return new Guid(Request.QueryString["VideoGuid"]);
                    }
                    catch
                    {
                        return Guid.Empty;
                    }
                }
                return null;
            }
            set { _videoGuid = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = Username;
            ctrlCustomPagerInfo.PageChanged += ctrlCustomPagerInfo_PageChanged;

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

        private void BindData()
        {
            var list = _repo.All<Video>().ToList().FindAll(x =>
            {
                var returnValue = true;
                string tmpString;

                if (ViewState["GoalYear"] != null)
                {
                    tmpString = ViewState["GoalYear"].ToString();
                    if (!string.IsNullOrEmpty(tmpString))
                        returnValue = x.GoalYear.Equals(tmpString);
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
                var i = list.FindIndex(x => x.ID.Equals(VideoGuid));
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

        protected void ctrlCustomPagerInfo_PageChanged(object sender, CustomPagerInfo.DataNavigatorEventArgs e)
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
                var key = gvVideo.DataKeys[gvVideo.SelectedIndex];
                if (key != null)
                    Response.Redirect($"AdminVideoView.aspx?VideoGuid={key.Value}");
            }
        }

        protected void gvVideo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var v = e.Row.DataItem as Video;
                var ltrlMatchOpponentInfo = e.Row.FindControl("ltrlMatchOpponentInfo") as Literal;

                if (ltrlMatchOpponentInfo != null && v?.ArsenalMatchGuid != null)
                {
                    var m = Match.Cache.Load(v.ArsenalMatchGuid.Value);

                    if (m != null)
                    {
                        ltrlMatchOpponentInfo.Text =
                            $"<a href=\"AdminMatchView.aspx?MatchGuid={m.ID}\" target=\"_blank\"><em>{m.TeamName}</em></a>";
                    }
                    else
                    {
                        ltrlMatchOpponentInfo.Text = v.Opponent;
                    }
                }
            }
        }


        protected void btnRefreshCache_Click(object sender, EventArgs e)
        {
            Video.Cache.RefreshCache();

            ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                "alert('更新缓存成功');window.location.href=window.location.href", true);
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