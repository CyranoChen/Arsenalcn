using System;
using System.Linq;
using System.Web.UI.WebControls;
using Arsenal.Service;
using Arsenal.Web.Control;
using Arsenalcn.Core;

namespace Arsenal.Web
{
    public partial class AdminTeam : AdminPageBase
    {
        private readonly IRepository _repo = new Repository();

        private Guid? _teamGuid;

        private Guid? TeamGuid
        {
            get
            {
                if (_teamGuid.HasValue && _teamGuid == Guid.Empty)
                    return _teamGuid;
                if (!string.IsNullOrEmpty(Request.QueryString["TeamGuid"]))
                {
                    try
                    {
                        return new Guid(Request.QueryString["TeamGuid"]);
                    }
                    catch
                    {
                        return Guid.Empty;
                    }
                }
                return null;
            }
            set { _teamGuid = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = Username;
            ctrlCustomPagerInfo.PageChanged += ctrlCustomPagerInfo_PageChanged;

            if (!IsPostBack)
            {
                #region Bind ddlLeague

                var list = League.Cache.LeagueList;

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
            var list = _repo.All<Team>().ToList().FindAll(x =>
            {
                var returnValue = true;
                string tmpString;

                if (ViewState["LeagueGuid"] != null)
                {
                    tmpString = ViewState["LeagueGuid"].ToString();
                    if (!string.IsNullOrEmpty(tmpString))
                        returnValue = new RelationLeagueTeam { TeamGuid = x.ID, LeagueGuid = new Guid(tmpString) }.Any();
                }

                if (ViewState["TeamName"] != null)
                {
                    tmpString = ViewState["TeamName"].ToString();
                    if (!string.IsNullOrEmpty(tmpString))
                        returnValue = returnValue &&
                                      (x.TeamDisplayName.Contains(tmpString) || x.TeamEnglishName.Contains(tmpString));
                }

                return returnValue;
            });

            #region set GridView Selected PageIndex

            if (TeamGuid.HasValue && !TeamGuid.Equals(Guid.Empty))
            {
                var i = list.FindIndex(x => x.ID.Equals(TeamGuid));
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
            TeamGuid = null;

            BindData();
        }

        protected void ctrlCustomPagerInfo_PageChanged(object sender, CustomPagerInfo.DataNavigatorEventArgs e)
        {
            if (e.PageIndex > 0)
            {
                gvTeam.PageIndex = e.PageIndex;
                TeamGuid = null;
            }

            BindData();
        }

        protected void gvTeam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvTeam.SelectedIndex != -1)
            {
                var key = gvTeam.DataKeys[gvTeam.SelectedIndex];
                if (key != null)
                    Response.Redirect($"AdminTeamView.aspx?TeamGuid={key.Value}");
            }
        }

        protected void gvTeam_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var dataKey = gvTeam.DataKeys[e.RowIndex];
            if (dataKey != null)
            {
                var teamGuid = (Guid)dataKey.Value;

                try
                {
                    Guid leagueGuid;

                    if (!string.IsNullOrEmpty(ddlLeague.SelectedValue))
                        leagueGuid = new Guid(ddlLeague.SelectedValue);
                    else
                        throw new Exception("未选择比赛分类");

                    var rlt = new RelationLeagueTeam { TeamGuid = teamGuid, LeagueGuid = leagueGuid };

                    if (rlt.Any())
                    {
                        if (RelationLeagueTeam.QueryByTeamGuid(teamGuid).Count <= 1)
                        {
                            throw new Exception("该球队仅属于此分类，不能移除");
                        }
                        rlt.Delete();
                    }
                    else
                    {
                        throw new Exception("该球队未在此分类中");
                    }
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}');", true);
                }
            }

            BindData();
        }

        protected void btnRefreshCache_Click(object sender, EventArgs e)
        {
            Team.Cache.RefreshCache();

            ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                "alert('更新缓存成功');window.location.href=window.location.href", true);
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbTeamName.Text.Trim()))
                ViewState["TeamName"] = tbTeamName.Text.Trim();
            else
                ViewState["TeamName"] = string.Empty;

            TeamGuid = null;
            gvTeam.PageIndex = 0;

            BindData();
        }

        protected void ddlLeague_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlLeague.SelectedValue))
                ViewState["LeagueGuid"] = ddlLeague.SelectedValue;
            else
                ViewState["LeagueGuid"] = string.Empty;

            TeamGuid = null;
            gvTeam.PageIndex = 0;

            BindData();
        }
    }
}