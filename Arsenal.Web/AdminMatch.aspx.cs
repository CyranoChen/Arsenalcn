using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Arsenal.Service;
using Arsenal.Web.Control;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;

namespace Arsenal.Web
{
    public partial class AdminMatch : AdminPageBase
    {
        private readonly IRepository _repo = new Repository();

        private Guid? _matchGuid;

        private Guid? MatchGuid
        {
            get
            {
                if (_matchGuid.HasValue && _matchGuid == Guid.Empty)
                    return _matchGuid;
                if (!string.IsNullOrEmpty(Request.QueryString["MatchGuid"]))
                {
                    try
                    {
                        return new Guid(Request.QueryString["MatchGuid"]);
                    }
                    catch
                    {
                        return Guid.Empty;
                    }
                }
                return null;
            }
            set { _matchGuid = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = Username;
            ctrlCustomPagerInfo.PageChanged += ctrlCustomPagerInfo_PageChanged;

            if (!IsPostBack)
            {
                #region Bind ddlLeague

                var list = League.Cache.LeagueList.FindAll(l =>
                    Match.Cache.MatchList.Exists(m => m.LeagueGuid.Equals(l.ID)));

                ddlLeague.DataSource = list;
                ddlLeague.DataTextField = "LeagueNameInfo";
                ddlLeague.DataValueField = "ID";
                ddlLeague.DataBind();

                ddlLeague.Items.Insert(0, new ListItem("--请选择比赛分类--", string.Empty));

                #endregion

                BindData();
            }
        }

        private void BindData()
        {
            var list = _repo.All<Match>().FindAll(x =>
            {
                var returnValue = true;
                string tmpString;

                if (ViewState["LeagueGuid"] != null)
                {
                    tmpString = ViewState["LeagueGuid"].ToString();
                    if (!string.IsNullOrEmpty(tmpString))
                        returnValue = x.LeagueGuid.Equals(new Guid(tmpString));
                }

                if (ViewState["TeamGuid"] != null)
                {
                    tmpString = ViewState["TeamGuid"].ToString();
                    if (!string.IsNullOrEmpty(tmpString))
                        returnValue = returnValue && x.TeamGuid.Equals(new Guid(tmpString));
                }

                if (ViewState["IsHome"] != null)
                {
                    tmpString = ViewState["IsHome"].ToString();
                    if (!string.IsNullOrEmpty(tmpString))
                        returnValue = returnValue && x.IsHome.Equals(Convert.ToBoolean(tmpString));
                }

                return returnValue;
            });

            #region set GridView Selected PageIndex

            if (MatchGuid.HasValue && !MatchGuid.Value.Equals(Guid.Empty))
            {
                var i = list.FindIndex(x => x.ID.Equals(MatchGuid));
                if (i >= 0)
                {
                    gvMatch.PageIndex = i/gvMatch.PageSize;
                    gvMatch.SelectedIndex = i%gvMatch.PageSize;
                }
                else
                {
                    gvMatch.PageIndex = 0;
                    gvMatch.SelectedIndex = -1;
                }
            }
            else
            {
                gvMatch.SelectedIndex = -1;
            }

            #endregion

            gvMatch.DataSource = list;
            gvMatch.DataBind();

            #region set Control Custom Pager

            if (gvMatch.BottomPagerRow != null)
            {
                gvMatch.BottomPagerRow.Visible = true;
                ctrlCustomPagerInfo.Visible = true;

                ctrlCustomPagerInfo.PageIndex = gvMatch.PageIndex;
                ctrlCustomPagerInfo.PageCount = gvMatch.PageCount;
                ctrlCustomPagerInfo.RowCount = list.Count;
                ctrlCustomPagerInfo.InitComponent();
            }
            else
            {
                ctrlCustomPagerInfo.Visible = false;
            }

            #endregion
        }

        protected void gvMatch_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMatch.PageIndex = e.NewPageIndex;
            MatchGuid = null;

            BindData();
        }

        protected void ctrlCustomPagerInfo_PageChanged(object sender, CustomPagerInfo.DataNavigatorEventArgs e)
        {
            if (e.PageIndex > 0)
            {
                gvMatch.PageIndex = e.PageIndex;
                MatchGuid = null;
            }

            BindData();
        }

        protected void gvMatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvMatch.SelectedIndex != -1)
            {
                var key = gvMatch.DataKeys[gvMatch.SelectedIndex];
                if (key != null)
                    Response.Redirect($"AdminMatchView.aspx?MatchGuid={key.Value}");
            }
        }

        protected void btnRefreshCache_Click(object sender, EventArgs e)
        {
            Match.Cache.RefreshCache();

            ClientScript.RegisterClientScriptBlock(typeof (string), "succeed",
                "alert('更新缓存成功');window.location.href = window.location.href", true);
        }

        private void BindTeamData(Guid guid)
        {
            var rltList = _repo.Query<RelationLeagueTeam>(x => x.LeagueGuid == guid);

            var list = new List<Team>();

            if (rltList != null && rltList.Count > 0)
            {
                list.AddRange(rltList.Select(rlt => Team.Cache.Load(rlt.TeamGuid)).Where(t => t != null));

                ddlTeam.DataSource = list.OrderBy(x => x.TeamEnglishName);
                ddlTeam.DataTextField = "TeamDisplayName";
                ddlTeam.DataValueField = "ID";
                ddlTeam.DataBind();

                ddlTeam.Visible = true;
            }
            else
            {
                ddlTeam.Items.Clear();
                ddlTeam.Visible = false;
            }

            ddlTeam.Items.Insert(0, new ListItem("--请选择对阵球队--", string.Empty));
        }

        protected void ddlLeague_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlLeague.SelectedValue))
            {
                ViewState["LeagueGuid"] = ddlLeague.SelectedValue;
                BindTeamData(new Guid(ddlLeague.SelectedValue));
            }
            else
                ViewState["LeagueGuid"] = string.Empty;

            MatchGuid = null;
            gvMatch.PageIndex = 0;

            BindData();
        }

        protected void dllTeam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlTeam.SelectedValue))
                ViewState["TeamGuid"] = ddlTeam.SelectedValue;
            else
                ViewState["TeamGuid"] = string.Empty;

            MatchGuid = null;
            gvMatch.PageIndex = 0;

            BindData();
        }

        protected void ddlIsHome_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlIsHome.SelectedValue))
                ViewState["IsHome"] = ddlIsHome.SelectedValue;
            else
                ViewState["IsHome"] = string.Empty;

            MatchGuid = null;
            gvMatch.PageIndex = 0;

            BindData();
        }
    }
}