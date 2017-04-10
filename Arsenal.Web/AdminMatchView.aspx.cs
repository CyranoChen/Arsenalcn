using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Arsenal.Service;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;

namespace Arsenal.Web
{
    public partial class AdminMatchView : AdminPageBase
    {
        private readonly IRepository _repo = new Repository();

        private Guid MatchGuid
        {
            get
            {
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
                return Guid.Empty;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = Username;

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

                InitForm();
            }
        }

        private void InitForm()
        {
            if (MatchGuid != Guid.Empty)
            {
                var m = _repo.Single<Match>(MatchGuid);

                tbMatchGuid.Text = m.ID.ToString();

                if (m.LeagueGuid.HasValue)
                {
                    ddlLeague.SelectedValue = m.LeagueGuid.Value.ToString();
                    BindTeamData(m.LeagueGuid.Value);
                    ddlTeam.SelectedValue = m.TeamGuid.ToString();
                }
                else
                {
                    ddlLeague.SelectedValue = string.Empty;
                }

                cbIsHome.Checked = m.IsHome;

                if (m.Round.HasValue)
                    tbRound.Text = m.Round.Value.ToString();
                else
                    tbRound.Text = string.Empty;


                tbPlayTime.Text = m.PlayTime.ToString("yyyy-MM-dd HH:mm");
                cbIsActive.Checked = m.IsActive;

                if (m.ResultHome.HasValue)
                    tbResultHome.Text = m.ResultHome.Value.ToString();
                else
                    tbResultHome.Text = string.Empty;

                if (m.ResultAway.HasValue)
                    tbResultAway.Text = m.ResultAway.Value.ToString();
                else
                    tbResultAway.Text = string.Empty;

                if (m.CasinoMatchGuid.HasValue)
                    tbCasinoMatchGuid.Text = m.CasinoMatchGuid.Value.ToString();
                else
                    tbCasinoMatchGuid.Text = string.Empty;

                if (m.GroupGuid.HasValue)
                    tbGroupGuid.Text = m.GroupGuid.Value.ToString();
                else
                    tbGroupGuid.Text = string.Empty;

                tbReportImageURL.Text = m.ReportImageURL;
                tbReportURL.Text = m.ReportURL;
                tbTopicURL.Text = m.TopicURL;
                tbRemark.Text = m.Remark;
            }
            else
            {
                tbMatchGuid.Text = Guid.NewGuid().ToString();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var m = new Match();

                if (!MatchGuid.Equals(Guid.Empty))
                {
                    m = _repo.Single<Match>(MatchGuid);
                }

                if (!string.IsNullOrEmpty(ddlTeam.SelectedValue))
                {
                    m.TeamGuid = new Guid(ddlTeam.SelectedValue);
                    m.TeamName = ddlTeam.SelectedItem.Text.Trim();
                }
                else
                    throw new Exception("TeamGuid can't be NULL");

                m.IsHome = cbIsHome.Checked;

                if (!string.IsNullOrEmpty(tbResultHome.Text.Trim()))
                    m.ResultHome = Convert.ToInt16(tbResultHome.Text.Trim());
                else
                    m.ResultHome = null;

                if (!string.IsNullOrEmpty(tbResultAway.Text.Trim()))
                    m.ResultAway = Convert.ToInt16(tbResultAway.Text.Trim());
                else
                    m.ResultAway = null;

                m.PlayTime = Convert.ToDateTime(tbPlayTime.Text.Trim());

                if (!string.IsNullOrEmpty(ddlLeague.SelectedValue))
                {
                    m.LeagueGuid = new Guid(ddlLeague.SelectedValue);
                    m.LeagueName = ddlLeague.SelectedItem.Text.Trim();
                }
                else
                {
                    m.LeagueGuid = null;
                    m.LeagueName = string.Empty;
                }

                if (!string.IsNullOrEmpty(tbRound.Text.Trim()))
                    m.Round = Convert.ToInt16(tbRound.Text.Trim());
                else
                    m.Round = null;

                if (!string.IsNullOrEmpty(tbGroupGuid.Text.Trim()))
                    m.GroupGuid = new Guid(tbGroupGuid.Text.Trim());
                else
                    m.GroupGuid = null;

                if (!string.IsNullOrEmpty(tbCasinoMatchGuid.Text.Trim()))
                    m.CasinoMatchGuid = new Guid(tbCasinoMatchGuid.Text.Trim());
                else
                    m.CasinoMatchGuid = null;

                m.ReportImageURL = tbReportImageURL.Text.Trim();
                m.ReportURL = tbReportURL.Text.Trim();
                m.TopicURL = tbTopicURL.Text.Trim();
                m.IsActive = cbIsActive.Checked;
                m.Remark = tbRemark.Text.Trim();

                if (MatchGuid != Guid.Empty)
                {
                    _repo.Update(m);
                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                        "alert('更新成功');window.location.href = window.location.href", true);
                }
                else
                {
                    _repo.Insert(m);
                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                        "alert('添加成功');window.location.href = 'AdminMatch.aspx'", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}')", true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (MatchGuid != Guid.Empty)
            {
                Response.Redirect("AdminMatch.aspx?MatchGuid=" + MatchGuid);
            }
            else
            {
                Response.Redirect("AdminMatch.aspx");
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (MatchGuid != Guid.Empty)
                {
                    _repo.Delete<Match>(MatchGuid);

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                        "alert('删除成功');window.location.href='AdminMatch.aspx'", true);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", "alert('删除失败')", true);
            }
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
            }
            else
            {
                ddlTeam.Items.Clear();
            }

            ddlTeam.Items.Insert(0, new ListItem("--请选择对阵球队--", string.Empty));
        }

        protected void ddlLeague_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlLeague.SelectedValue))
            { BindTeamData(new Guid(ddlLeague.SelectedValue)); }
        }
    }
}