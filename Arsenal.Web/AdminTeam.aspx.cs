using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Arsenal.Entity;

namespace Arsenal.Web
{
    public partial class AdminTeam : AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = this.Username;
            ctrlCustomPagerInfo.PageChanged += new Control.CustomPagerInfo.PageChangedEventHandler(ctrlCustomPagerInfo_PageChanged);

            if (!IsPostBack)
            {
                #region Bind ddlLeague
                List<League> list = League.Cache.LeagueList;

                ddlLeague.DataSource = list;
                ddlLeague.DataTextField = "LeagueNameInfo";
                ddlLeague.DataValueField = "LeagueGuid";
                ddlLeague.DataBind();

                ddlLeague.Items.Insert(0, new ListItem("--请选择比赛分类--", string.Empty));
                #endregion

                BindData();
            }
        }

        private Guid? _teamGuid = null;
        private Guid? TeamGuid
        {
            get
            {
                if (_teamGuid.HasValue && _teamGuid == Guid.Empty)
                    return _teamGuid;
                else if (!string.IsNullOrEmpty(Request.QueryString["TeamGuid"]))
                {
                    try { return new Guid(Request.QueryString["TeamGuid"]); }
                    catch { return Guid.Empty; }
                }
                else
                    return null;
            }
            set { _teamGuid = value; }
        }

        private void BindData()
        {
            List<Team> list = Team.GetTeams().FindAll(delegate(Team t)
            {
                Boolean returnValue = true;
                string tmpString = string.Empty;

                if (ViewState["LeagueGuid"] != null)
                {
                    tmpString = ViewState["LeagueGuid"].ToString();
                    if (!string.IsNullOrEmpty(tmpString))
                        returnValue = returnValue && RelationLeagueTeam.Exist(t.TeamGuid, new Guid(tmpString));
                }

                if (ViewState["TeamName"] != null)
                {
                    tmpString = ViewState["TeamName"].ToString();
                    if (!string.IsNullOrEmpty(tmpString))
                        returnValue = returnValue && (t.TeamDisplayName.Contains(tmpString) || t.TeamEnglishName.Contains(tmpString));
                }

                //if (ViewState["DisplayName"] != null)
                //{
                //    tmpString = ViewState["DisplayName"].ToString();
                //    if (!string.IsNullOrEmpty(tmpString) && tmpString != "--球员姓名--")
                //        returnValue = returnValue && t.DisplayName.ToLower().Contains(tmpString.ToLower());
                //}

                return returnValue;
            });

            #region set GridView Selected PageIndex
            if (TeamGuid.HasValue && TeamGuid != Guid.Empty)
            {
                int i = list.FindIndex(delegate(Team p) { return p.TeamGuid == TeamGuid; });
                if (i >= 0)
                {
                    gvTeam.PageIndex = i / gvTeam.PageSize;
                    gvTeam.SelectedIndex = i % gvTeam.PageSize;
                }
                else
                {
                    gvTeam.PageIndex = 0;
                    gvTeam.SelectedIndex = -1;
                }
            }
            else
            {
                gvTeam.SelectedIndex = -1;
            }
            #endregion

            gvTeam.DataSource = list;
            gvTeam.DataBind();

            #region set Control Custom Pager
            if (gvTeam.BottomPagerRow != null)
            {
                gvTeam.BottomPagerRow.Visible = true;
                ctrlCustomPagerInfo.Visible = true;

                ctrlCustomPagerInfo.PageIndex = gvTeam.PageIndex;
                ctrlCustomPagerInfo.PageCount = gvTeam.PageCount;
                ctrlCustomPagerInfo.RowCount = list.Count;
                ctrlCustomPagerInfo.InitComponent();
            }
            else
            {
                ctrlCustomPagerInfo.Visible = false;
            }
            #endregion
        }

        protected void gvTeam_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTeam.PageIndex = e.NewPageIndex;
            TeamGuid = Guid.Empty;

            BindData();
        }

        protected void ctrlCustomPagerInfo_PageChanged(object sender, Control.CustomPagerInfo.DataNavigatorEventArgs e)
        {
            if (e.PageIndex > 0)
            {
                gvTeam.PageIndex = e.PageIndex;
                TeamGuid = Guid.Empty;
            }

            BindData();
        }

        protected void gvTeam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvTeam.SelectedIndex != -1)
            {
                Response.Redirect(string.Format("AdminTeamView.aspx?TeamGuid={0}", gvTeam.DataKeys[gvTeam.SelectedIndex].Value.ToString()));
            }
        }

        protected void gvTeam_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Guid teamGuid = (Guid)gvTeam.DataKeys[e.RowIndex].Value;
            Guid leagueGuid = Guid.Empty;

            try
            {
                if (!string.IsNullOrEmpty(ddlLeague.SelectedValue))
                    leagueGuid = new Guid(ddlLeague.SelectedValue);
                else
                    throw new Exception("未选择比赛分类");

                if (RelationLeagueTeam.GetRelationLeagueTeams().Exists(delegate(RelationLeagueTeam rlt) { return rlt.TeamGuid.Equals(teamGuid) && rlt.LeagueGuid.Equals(leagueGuid); }))
                {
                    Team t = new Team();
                    t.TeamGuid = teamGuid;
                    t.Select();

                    if (t.LeagueCountInfo <= 1)
                        throw new Exception("该球队仅属于此分类，不能移除");
                    else
                    {
                        RelationLeagueTeam rlt = new RelationLeagueTeam();
                        rlt.TeamGuid = teamGuid;
                        rlt.LeagueGuid = leagueGuid;
                        rlt.Select();

                        rlt.Delete();
                    }
                }
                else
                {
                    throw new Exception("该球队未在此分类中");
                }
            }
            catch (Exception ex)
            {
                this.ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}');", ex.Message.ToString()), true);
            }

            BindData();
        }

        protected void btnRefreshCache_Click(object sender, EventArgs e)
        {
            Team.Cache.RefreshCache();

            ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('更新缓存成功');window.location.href=window.location.href", true);
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbTeamName.Text.Trim()))
                ViewState["TeamName"] = tbTeamName.Text.Trim();
            else
                ViewState["TeamName"] = string.Empty;

            TeamGuid = Guid.Empty;
            gvTeam.PageIndex = 0;

            BindData();
        }

        protected void ddlLeague_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlLeague.SelectedValue))
                ViewState["LeagueGuid"] = ddlLeague.SelectedValue;
            else
                ViewState["LeagueGuid"] = string.Empty;

            TeamGuid = Guid.Empty;
            gvTeam.PageIndex = 0;

            BindData();
        }
    }
}
