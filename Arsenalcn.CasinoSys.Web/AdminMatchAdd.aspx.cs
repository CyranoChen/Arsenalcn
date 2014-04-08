using System;
using System.Data;
using System.Web.UI.WebControls;

using Arsenalcn.CasinoSys.Entity;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class AdminMatchAdd : Common.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = this.username;

            if (!IsPostBack)
            {
                DataTable dtLeague = Entity.League.GetLeague(true);
                if (dtLeague != null)
                {
                    dtLeague.Columns.Add("LeagueDisplay", typeof(string));
                    foreach (DataRow dr in dtLeague.Rows)
                    {
                        dr["LeagueDisplay"] = string.Format("{0} {1}", dr["LeagueName"], dr["LeagueSeason"]);
                    }
                }

                ddlLeague.DataSource = dtLeague;
                ddlLeague.DataTextField = "LeagueDisplay";
                ddlLeague.DataValueField = "LeagueGuid";
                ddlLeague.DataBind();

                ListItem itemLeague = new ListItem("--请选择比赛分类--", Guid.Empty.ToString());
                ddlLeague.Items.Insert(0, itemLeague);

                ListItem itemGroup = new ListItem("--请选择比赛组别--", Guid.Empty.ToString());
                ddlLeagueGroup.Items.Insert(0, itemGroup);
            }
        }

        protected void ddlLeague_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtTeams = Entity.Team.GetTeamByLeague(new Guid(ddlLeague.SelectedValue));

            if (dtTeams != null)
            {
                ddlHomeTeam.DataSource = dtTeams;
                ddlHomeTeam.DataTextField = "TeamDisplayName";
                ddlHomeTeam.DataValueField = "TeamGuid";
                ddlHomeTeam.DataBind();

                ddlAwayTeam.DataSource = dtTeams;
                ddlAwayTeam.DataTextField = "TeamDisplayName";
                ddlAwayTeam.DataValueField = "TeamGuid";
                ddlAwayTeam.DataBind();
            }
            else
            {
                ddlHomeTeam.Items.Clear();
                ddlAwayTeam.Items.Clear();
            }


            DataTable dtGroups = Entity.Group.GetGroupByLeague(new Guid(ddlLeague.SelectedValue), false);

            if (dtGroups != null)
            {
                ddlLeagueGroup.DataSource = dtGroups;
                ddlLeagueGroup.DataTextField = "GroupName";
                ddlLeagueGroup.DataValueField = "GroupGuid";
                ddlLeagueGroup.DataBind();
            }
            else
                ddlLeagueGroup.Items.Clear();


            ListItem itemGroup = new ListItem("--请选择比赛组别--", Guid.Empty.ToString());
            ddlLeagueGroup.Items.Insert(0, itemGroup);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Match m = new Match();
                m.MatchGuid = Guid.NewGuid();
                m.Home = new Guid(ddlHomeTeam.SelectedValue);
                m.Away = new Guid(ddlAwayTeam.SelectedValue);
                m.PlayTime = Convert.ToDateTime(tbPlayTime.Text);
                m.LeagueGuid = new Guid(ddlLeague.SelectedValue);

                League l = new League(m.LeagueGuid);
                m.LeagueName = l.LeagueName + l.LeagueSeason;

                if (!string.IsNullOrEmpty(tbRound.Text))
                    m.Round = Convert.ToInt16(tbRound.Text);
                else
                    m.Round = null;

                if (ddlLeagueGroup.SelectedValue != Guid.Empty.ToString())
                    m.GroupGuid = new Guid(ddlLeagueGroup.SelectedValue);
                else
                    m.GroupGuid = null;

                float winRate = Convert.ToSingle(tbWinRate.Text);
                float drawRate = Convert.ToSingle(tbDrawRate.Text);
                float loseRate = Convert.ToSingle(tbLoseRate.Text);

                m.Insert(this.userid, this.username, Convert.ToSingle(tbWinRate.Text), Convert.ToSingle(tbDrawRate.Text), Convert.ToSingle(tbLoseRate.Text));

                this.ClientScript.RegisterClientScriptBlock(typeof(string), "save", "alert('比赛添加成功');", true);
            }
            catch (Exception ex)
            {
                this.ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}');", ex.Message.ToString()), true);
            }
        }
    }
}
