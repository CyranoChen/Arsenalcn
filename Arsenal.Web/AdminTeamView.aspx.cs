using System;
using System.Web.UI.WebControls;
using Arsenal.Service;
using Arsenalcn.Core;

namespace Arsenal.Web
{
    public partial class AdminTeamView : AdminPageBase
    {
        private readonly IRepository _repo = new Repository();

        private Guid TeamGuid
        {
            get
            {
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

                ddlTeamLeague.DataSource = list;
                ddlTeamLeague.DataTextField = "LeagueNameInfo";
                ddlTeamLeague.DataValueField = "ID";
                ddlTeamLeague.DataBind();

                ddlTeamLeague.Items.Insert(0, new ListItem("--请选择比赛分类--", string.Empty));

                #endregion

                InitForm();
            }
        }

        private void InitForm()
        {
            if (TeamGuid != Guid.Empty)
            {
                var t = _repo.Single<Team>(TeamGuid);

                tbTeamGuid.Text = t.ID.ToString();
                tbTeamEnglishName.Text = t.TeamEnglishName;
                tbTeamDisplayName.Text = t.TeamDisplayName;
                tbTeamLogo.Text = t.TeamLogo;
                tbTeamNickName.Text = t.TeamNickName;
                tbTeamFounded.Text = t.Founded;
                tbGround.Text = t.Ground;

                if (t.Capacity != null)
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
                var t = new Team();

                if (!TeamGuid.Equals(Guid.Empty))
                {
                    t = _repo.Single<Team>(TeamGuid);
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
                    var leagueGuid = new Guid(ddlTeamLeague.SelectedValue);

                    var rlt = new RelationLeagueTeam { TeamGuid = TeamGuid, LeagueGuid = leagueGuid };

                    if (!rlt.Any())
                    {
                        rlt.Insert();
                    }
                }

                if (TeamGuid != Guid.Empty)
                {
                    _repo.Update(t);
                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                        "alert('更新成功');window.location.href = window.location.href", true);
                }
                else
                {
                    _repo.Insert(t);
                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                        "alert('添加成功');window.location.href = 'AdminTeam.aspx'", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}')", true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (TeamGuid != Guid.Empty)
            {
                Response.Redirect("AdminTeam.aspx?TeamGuid=" + TeamGuid);
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
                    var list = RelationLeagueTeam.QueryByTeamGuid(TeamGuid);

                    if (list != null && list.Count > 0)
                    {
                        var num = list.Delete();

                        _repo.Delete<Team>(TeamGuid);

                        ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", $"alert('删除成功(包括{num}个分类关联)');window.location.href='AdminTeam.aspx'", true);
                    }
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