using System;
using System.Web.UI.WebControls;
using Arsenal.Service;
using Arsenalcn.Core.Dapper;

namespace Arsenal.Web
{
    public partial class AdminLeagueTeamView : AdminPageBase
    {
        private readonly IRepository _repo = new Repository();

        private Guid LeagueGuid
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["LeagueGuid"]))
                {
                    try
                    {
                        return new Guid(Request.QueryString["LeagueGuid"]);
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
                InitForm();
            }
        }

        private void InitForm()
        {
            if (LeagueGuid != Guid.Empty)
            {
                var l = _repo.Single<League>(LeagueGuid);

                lblLeagueGuid.Text = LeagueGuid.ToString();
                lblLeagueName.Text = l.LeagueNameInfo;

                var teams = Team.Cache.GetTeamsByLeagueGuid(LeagueGuid);

                if (teams.Count > 0)
                {
                    lbLeagueTeam.DataSource = teams;
                    lbLeagueTeam.DataTextField = "TeamNameInfo";
                    lbLeagueTeam.DataValueField = "ID";
                    lbLeagueTeam.DataBind();
                }
            }
            else
            {
                Response.Redirect("AdminLeague.aspx");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (LeagueGuid != Guid.Empty)
            {
                Response.Redirect("AdminLeagueView.aspx?LeagueGuid=" + LeagueGuid);
            }
            else
            {
                Response.Redirect("AdminLeague.aspx");
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (LeagueGuid != Guid.Empty)
                {
                    var rlts = _repo.Query<RelationLeagueTeam>(x => x.LeagueGuid == LeagueGuid);

                    if (rlts != null && rlts.Count > 0)
                    {
                        rlts.ForEach(x => x.Delete());

                        Team.Cache.RefreshCache();
                        RelationLeagueTeam.Cache.RefreshCache();

                        ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", 
                            $"alert('删除成功(包括{rlts.Count}个所属球队)');window.location.href='AdminLeagueTeamView.aspx?LeagueGuid={LeagueGuid}'", true);
                    }
                }
                else
                {
                    Response.Redirect("AdminLeague.aspx");
                }
            }
            catch
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", "alert('删除失败')", true);
            }
        }

        protected void btnAddTeam_Click(object sender, EventArgs e)
        {
            Guid teamGuid;

            if (!string.IsNullOrEmpty(tbTeamSelected.Text.Trim())
                && Guid.TryParse(tbTeamSelected.Text.Trim(), out teamGuid))
            {
                var rlt = new RelationLeagueTeam { LeagueGuid = LeagueGuid, TeamGuid = teamGuid };

                if (!rlt.Any())
                {
                    _repo.Insert(rlt);
                }

                Team.Cache.RefreshCache();
                RelationLeagueTeam.Cache.RefreshCache();
            }

            InitForm();
        }

        protected void btnRemoveTeam_Click(object sender, EventArgs e)
        {
            foreach (ListItem item in lbLeagueTeam.Items)
            {
                Guid teamGuid;

                if (item.Selected && Guid.TryParse(item.Value, out teamGuid))
                {
                    var rlt = new RelationLeagueTeam { LeagueGuid = LeagueGuid, TeamGuid = teamGuid };

                    rlt.Delete();
                }
            }

            Team.Cache.RefreshCache();
            RelationLeagueTeam.Cache.RefreshCache();

            InitForm();
        }
    }
}