using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

using Arsenalcn.CasinoSys.Entity;
using ArsenalLeauge = Arsenalcn.CasinoSys.Entity.Arsenal.League;
using ArsenalTeam = Arsenalcn.CasinoSys.Entity.Arsenal.Team;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class AdminMatchAdd : Common.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = this.username;

            if (!IsPostBack)
            {
                List<ArsenalLeauge> list = Entity.League.Cache.LeagueList_Active;

                ddlLeague.DataSource = list;
                ddlLeague.DataTextField = "LeagueNameInfo";
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
            List<ArsenalTeam> list = Entity.Team.Cache.GetTeamsByLeagueGuid(new Guid(ddlLeague.SelectedValue));

            if (list != null && list.Count > 0)
            {
                ddlHomeTeam.DataSource = list;
                ddlHomeTeam.DataTextField = "TeamDisplayName";
                ddlHomeTeam.DataValueField = "TeamGuid";
                ddlHomeTeam.DataBind();

                ddlAwayTeam.DataSource = list;
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

                ArsenalLeauge l = League.Cache.Load(m.LeagueGuid);
                m.LeagueName = l.LeagueNameInfo;

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
