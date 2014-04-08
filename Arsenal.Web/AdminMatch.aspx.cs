using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Arsenal.Entity;

namespace Arsenal.Web
{
    public partial class AdminMatch : Common.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = this.username;
            ctrlCustomPagerInfo.PageChanged += new Control.CustomPagerInfo.PageChangedEventHandler(ctrlCustomPagerInfo_PageChanged);

            if (!IsPostBack)
            {
                #region Bind ddlLeague
                List<League> list = League.Cache.LeagueList.FindAll(delegate(League l) { return Match.Cache.MatchList.Exists(delegate(Match m) { return m.LeagueGuid.Equals(l.LeagueGuid); }); });

                ddlLeague.DataSource = list;
                ddlLeague.DataTextField = "LeagueNameInfo";
                ddlLeague.DataValueField = "LeagueGuid";
                ddlLeague.DataBind();

                ddlLeague.Items.Insert(0, new ListItem("--请选择比赛分类--", string.Empty));
                #endregion

                BindData();
            }
        }

        private Guid? _matchGuid = null;
        private Guid? MatchGuid
        {
            get
            {
                if (_matchGuid.HasValue && _matchGuid == Guid.Empty)
                    return _matchGuid;
                else if (!string.IsNullOrEmpty(Request.QueryString["MatchGuid"]))
                {
                    try { return new Guid(Request.QueryString["MatchGuid"]); }
                    catch { return Guid.Empty; }
                }
                else
                    return null;
            }
            set { _matchGuid = value; }
        }

        private void BindData()
        {
            List<Match> list = Match.GetMatchs().FindAll(delegate(Match m)
            {
                Boolean returnValue = true;
                string tmpString = string.Empty;

                if (ViewState["LeagueGuid"] != null)
                {
                    tmpString = ViewState["LeagueGuid"].ToString();
                    if (!string.IsNullOrEmpty(tmpString))
                        returnValue = returnValue && m.LeagueGuid.Equals(new Guid(tmpString));
                }

                if (ViewState["TeamGuid"] != null)
                {
                    tmpString = ViewState["TeamGuid"].ToString();
                    if (!string.IsNullOrEmpty(tmpString))
                        returnValue = returnValue && m.TeamGuid.Equals(new Guid(tmpString));
                }

                if (ViewState["IsHome"] != null)
                {
                    tmpString = ViewState["IsHome"].ToString();
                    if (!string.IsNullOrEmpty(tmpString))
                        returnValue = returnValue && m.IsHome.Equals(Convert.ToBoolean(tmpString));
                }

                return returnValue;
            });

            #region set GridView Selected PageIndex
            if (MatchGuid.HasValue && MatchGuid != Guid.Empty)
            {
                int i = list.FindIndex(delegate(Match m) { return m.MatchGuid == MatchGuid; });
                if (i >= 0)
                {
                    gvMatch.PageIndex = i / gvMatch.PageSize;
                    gvMatch.SelectedIndex = i % gvMatch.PageSize;
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
            MatchGuid = Guid.Empty;

            BindData();
        }

        protected void ctrlCustomPagerInfo_PageChanged(object sender, Control.CustomPagerInfo.DataNavigatorEventArgs e)
        {
            if (e.PageIndex > 0)
            {
                gvMatch.PageIndex = e.PageIndex;
                MatchGuid = Guid.Empty;
            }

            BindData();
        }

        protected void gvMatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvMatch.SelectedIndex != -1)
            {
                Response.Redirect(string.Format("AdminMatchView.aspx?MatchGuid={0}", gvMatch.DataKeys[gvMatch.SelectedIndex].Value.ToString()));
            }
        }

        protected void btnRefreshCache_Click(object sender, EventArgs e)
        {
            Match.Cache.RefreshCache();

            ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('更新缓存成功');window.location.href = window.location.href", true);
        }

        private void BindTeamData(Guid LeagueGuid)
        {
            List<RelationLeagueTeam> list = RelationLeagueTeam.GetRelationLeagueTeams().FindAll(delegate(RelationLeagueTeam rlt) { return rlt.LeagueGuid.Equals(LeagueGuid); });
            List<Team> lstTeam = new List<Team>();

            if (list != null && list.Count > 0)
            {
                foreach (RelationLeagueTeam rlt in list)
                {
                    Team t = Team.Cache.Load(rlt.TeamGuid);

                    if (t != null)
                        lstTeam.Add(t);
                }

                lstTeam.Sort(delegate(Team t1, Team t2) { return Comparer<string>.Default.Compare(t1.TeamEnglishName, t2.TeamEnglishName); });

                ddlTeam.DataSource = lstTeam;
                ddlTeam.DataTextField = "TeamDisplayName";
                ddlTeam.DataValueField = "TeamGuid";
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

            MatchGuid = Guid.Empty;
            gvMatch.PageIndex = 0;

            BindData();
        }

        protected void dllTeam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlTeam.SelectedValue))
                ViewState["TeamGuid"] = ddlTeam.SelectedValue;
            else
                ViewState["TeamGuid"] = string.Empty;

            MatchGuid = Guid.Empty;
            gvMatch.PageIndex = 0;

            BindData();
        }

        protected void ddlIsHome_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlIsHome.SelectedValue))
                ViewState["IsHome"] = ddlIsHome.SelectedValue;
            else
                ViewState["IsHome"] = string.Empty;

            MatchGuid = Guid.Empty;
            gvMatch.PageIndex = 0;

            BindData();
        }
    }
}
