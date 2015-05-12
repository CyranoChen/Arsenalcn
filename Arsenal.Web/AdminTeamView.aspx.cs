using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

using Arsenal.Service;
using Arsenalcn.Core;

namespace Arsenal.Web
{
    public partial class AdminTeamView : AdminPageBase
    {
        private readonly IRepository repo = new Repository();
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = this.Username;

            if (!IsPostBack)
            {
                #region Bind ddlLeague
                List<League> list = League.Cache.LeagueList;

                ddlTeamLeague.DataSource = list;
                ddlTeamLeague.DataTextField = "LeagueNameInfo";
                ddlTeamLeague.DataValueField = "ID";
                ddlTeamLeague.DataBind();

                ddlTeamLeague.Items.Insert(0, new ListItem("--请选择比赛分类--", string.Empty));
                #endregion

                InitForm();
            }
        }

        private Guid TeamGuid
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["TeamGuid"]))
                {
                    try { return new Guid(Request.QueryString["TeamGuid"]); }
                    catch { return Guid.Empty; }
                }
                else
                    return Guid.Empty;
            }
        }

        private void InitForm()
        {
            if (TeamGuid != Guid.Empty)
            {
                Team t = repo.Single<Team>(TeamGuid);

                tbTeamGuid.Text = t.ID.ToString();
                tbTeamEnglishName.Text = t.TeamEnglishName;
                tbTeamDisplayName.Text = t.TeamDisplayName;
                tbTeamLogo.Text = t.TeamLogo;
                tbTeamNickName.Text = t.TeamNickName;
                tbTeamFounded.Text = t.Founded;
                tbGround.Text = t.Ground;
                tbCapacity.Text = t.Capacity.Value.ToString();
                tbChairMan.Text = t.Chairman;
                tbManager.Text = t.Manager;
                ddlTeamLeague.SelectedValue = t.LeagueGuid.ToString();
            }
            else
            {
                tbTeamGuid.Text = Guid.NewGuid().ToString();
                //ddlTeamLeague.SelectedValue = SelectedLeague.ToString();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Team t = new Team();

                if (!TeamGuid.Equals(Guid.Empty))
                {
                    t = repo.Single<Team>(TeamGuid);
                }

                t.TeamEnglishName = tbTeamEnglishName.Text.Trim();
                t.TeamDisplayName = tbTeamDisplayName.Text.Trim();
                t.TeamLogo = tbTeamLogo.Text.Trim();
                t.TeamNickName = tbTeamNickName.Text.Trim();
                t.Founded = tbTeamFounded.Text.Trim();
                t.Ground = tbGround.Text.Trim();
                t.Capacity = int.Parse(tbCapacity.Text.Trim());
                t.Chairman = tbChairMan.Text.Trim();
                t.Manager = tbManager.Text.Trim();

                // Insert Relation Team League
                if (!string.IsNullOrEmpty(ddlTeamLeague.SelectedValue))
                {
                    Guid leagueGuid = new Guid(ddlTeamLeague.SelectedValue);

                    var rlt = new RelationLeagueTeam() { TeamGuid = TeamGuid, LeagueGuid = leagueGuid };

                    if (!rlt.Any())
                    {
                        rlt.Create();
                    }
                }

                if (TeamGuid != Guid.Empty)
                {
                    repo.Update(t);
                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('更新成功');window.location.href = window.location.href", true);
                }
                else
                {
                    repo.Insert(t);
                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('添加成功');window.location.href = 'AdminTeam.aspx'", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}')", ex.Message.ToString()), true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (TeamGuid != Guid.Empty)
            {
                Response.Redirect("AdminTeam.aspx?TeamGuid=" + TeamGuid.ToString());
            }
            else
            {
                Response.Redirect("AdminTeam.aspx");
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (TeamGuid != Guid.Empty)
                {
                    int countRelationLeagueTeam = int.MinValue;

                    IRelationLeagueTeam instance = new RelationLeagueTeam();

                    // TODO
                    var query = instance.Query(x => x.TeamGuid.Equals(TeamGuid));

                    if (query != null && query.Count() > 0)
                    {
                        countRelationLeagueTeam = query.Count();

                        instance.Delete(x => x.TeamGuid.Equals(TeamGuid));
                    }

                    repo.Delete<Team>(TeamGuid);

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", string.Format("alert('删除成功(包括{0}个分类关联)');window.location.href='AdminTeam.aspx'", countRelationLeagueTeam.ToString()), true);
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
    }
}
