using System;
using System.Web.UI.WebControls;

using Arsenalcn.CasinoSys.Entity;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class AdminMatchAdd : Common.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = username;

            if (!IsPostBack)
            {
                var list = League.Cache.LeagueList_Active;

                ddlLeague.DataSource = list;
                ddlLeague.DataTextField = "LeagueNameInfo";
                ddlLeague.DataValueField = "ID";
                ddlLeague.DataBind();

                var itemLeague = new ListItem("--请选择比赛分类--", Guid.Empty.ToString());
                ddlLeague.Items.Insert(0, itemLeague);

                var itemGroup = new ListItem("--请选择比赛组别--", Guid.Empty.ToString());
                ddlLeagueGroup.Items.Insert(0, itemGroup);
            }
        }

        protected void ddlLeague_SelectedIndexChanged(object sender, EventArgs e)
        {
            var list = Team.Cache.GetTeamsByLeagueGuid(new Guid(ddlLeague.SelectedValue));

            if (list != null && list.Count > 0)
            {
                ddlHomeTeam.DataSource = list;
                ddlHomeTeam.DataTextField = "TeamDisplayName";
                ddlHomeTeam.DataValueField = "ID";
                ddlHomeTeam.DataBind();

                ddlAwayTeam.DataSource = list;
                ddlAwayTeam.DataTextField = "TeamDisplayName";
                ddlAwayTeam.DataValueField = "ID";
                ddlAwayTeam.DataBind();
            }
            else
            {
                ddlHomeTeam.Items.Clear();
                ddlAwayTeam.Items.Clear();
            }


            var dtGroups = Group.GetGroupByLeague(new Guid(ddlLeague.SelectedValue), false);

            if (dtGroups != null)
            {
                ddlLeagueGroup.DataSource = dtGroups;
                ddlLeagueGroup.DataTextField = "GroupName";
                ddlLeagueGroup.DataValueField = "GroupGuid";
                ddlLeagueGroup.DataBind();
            }
            else
                ddlLeagueGroup.Items.Clear();


            var itemGroup = new ListItem("--请选择比赛组别--", Guid.Empty.ToString());
            ddlLeagueGroup.Items.Insert(0, itemGroup);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var m = new Match
                {
                    MatchGuid = Guid.NewGuid(),
                    Home = new Guid(ddlHomeTeam.SelectedValue),
                    Away = new Guid(ddlAwayTeam.SelectedValue),
                    PlayTime = Convert.ToDateTime(tbPlayTime.Text),
                    LeagueGuid = new Guid(ddlLeague.SelectedValue)
                };

                var l = League.Cache.Load(m.LeagueGuid);
                m.LeagueName = l.LeagueNameInfo;

                if (!string.IsNullOrEmpty(tbRound.Text))
                    m.Round = Convert.ToInt16(tbRound.Text);
                else
                    m.Round = null;

                if (ddlLeagueGroup.SelectedValue != Guid.Empty.ToString())
                    m.GroupGuid = new Guid(ddlLeagueGroup.SelectedValue);
                else
                    m.GroupGuid = null;

                //var winRate = Convert.ToSingle(tbWinRate.Text);
                //var drawRate = Convert.ToSingle(tbDrawRate.Text);
                //var loseRate = Convert.ToSingle(tbLoseRate.Text);

                m.Insert(userid, username, Convert.ToSingle(tbWinRate.Text), Convert.ToSingle(tbDrawRate.Text), Convert.ToSingle(tbLoseRate.Text));

                ClientScript.RegisterClientScriptBlock(typeof(string), "save", "alert('比赛添加成功');", true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}');", true);
            }
        }
    }
}
