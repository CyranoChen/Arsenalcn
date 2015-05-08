using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Arsenal.Service;
using Arsenalcn.Core;

namespace Arsenal.Web
{
    public partial class AdminMatchView : AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = this.Username;

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

                InitForm();
            }
        }

        private Guid MatchGuid
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["MatchGuid"]))
                {
                    try { return new Guid(Request.QueryString["MatchGuid"]); }
                    catch { return Guid.Empty; }
                }
                else
                    return Guid.Empty;
            }
        }

        private void InitForm()
        {
            if (MatchGuid != Guid.Empty)
            {
                IEntity entity = new Entity();
                Match m = entity.Single<Match>(MatchGuid);

                tbMatchGuid.Text = m.MatchGuid.ToString();

                if (m.LeagueGuid.HasValue)
                {
                    ddlLeague.SelectedValue = m.LeagueGuid.Value.ToString();
                    BindTeamData(m.LeagueGuid.Value);
                    ddlTeam.SelectedValue = m.TeamGuid.ToString();
                }
                else
                    ddlLeague.SelectedValue = string.Empty;

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

                tbReportImageURL.Text = m.ReportImageURL.ToString();
                tbReportURL.Text = m.ReportURL.ToString();
                tbTopicURL.Text = m.TopicURL.ToString();
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
                Match m = new Match();

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
                    m.MatchGuid = MatchGuid;
                    m.Update<Match>(m);
                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('更新成功');window.location.href = window.location.href", true);
                }
                else
                {
                    m.MatchGuid = new Guid(tbMatchGuid.Text.Trim());

                    m.Create<Match>(m);
                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('添加成功');window.location.href = 'AdminMatch.aspx'", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}')", ex.Message.ToString()), true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (MatchGuid != Guid.Empty)
            {
                Response.Redirect("AdminMatch.aspx?MatchGuid=" + MatchGuid.ToString());
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
                    IEntity entity = new Entity();
                    entity.Delete<Match>(MatchGuid);

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('删除成功');window.location.href='AdminMatch.aspx'", true);
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

        private void BindTeamData(Guid LeagueGuid)
        {
            IEntity entity = new Entity();
            List<RelationLeagueTeam> list = entity.All<RelationLeagueTeam>().FindAll(delegate(RelationLeagueTeam rlt) { return rlt.LeagueGuid.Equals(LeagueGuid); });
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
                BindTeamData(new Guid(ddlLeague.SelectedValue));
        }
    }
}