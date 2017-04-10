using System;
using System.Linq;
using System.Web.UI.WebControls;
using Arsenal.Service;
using Arsenalcn.Core.Dapper;

namespace Arsenal.Web
{
    public partial class AdminGroupView : AdminPageBase
    {
        private readonly IRepository _repo = new Repository();

        private Guid GroupGuid
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["GroupGuid"]))
                {
                    try
                    {
                        return new Guid(Request.QueryString["GroupGuid"]);
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

                var list = League.Cache.LeagueList_Active;

                ddlGroupLeague.DataSource = list;
                ddlGroupLeague.DataTextField = "LeagueNameInfo";
                ddlGroupLeague.DataValueField = "ID";
                ddlGroupLeague.DataBind();
                ddlGroupLeague.Items.Insert(0, new ListItem("--请选择分类--", string.Empty));

                #endregion

                InitForm();
            }
        }

        private void InitForm()
        {
            if (GroupGuid != Guid.Empty)
            {
                var g = _repo.Single<Group>(GroupGuid);

                tbGroupGuid.Text = g.ID.ToString();
                tbGroupName.Text = g.GroupName;
                tbGroupOrder.Text = g.GroupOrder.ToString();
                rblRankMethod.SelectedValue = ((int)g.RankMethod).ToString();
                cbIsTable.Checked = g.IsTable;

                ddlGroupLeague.SelectedValue = g.LeagueGuid.ToString();
                BindGroupTeam(g.LeagueGuid);
            }
            else
            {
                tbGroupGuid.Text = Guid.NewGuid().ToString();
                ddlGroupLeague.SelectedIndex = 0;
                lbLeagueTeam.Items.Clear();
                lbLeagueTeam.Visible = false;
            }
        }

        protected void ddlGroupLeague_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlGroupLeague.SelectedValue))
            {
                BindGroupTeam(new Guid(ddlGroupLeague.SelectedValue));
                lbLeagueTeam.Visible = true;
            }
            else
            {
                lbLeagueTeam.Items.Clear();
                lbLeagueTeam.Visible = false;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            using (var dapper = DapperHelper.GetInstance())
            {
                var trans = dapper.BeginTransaction();

                try
                {
                    if (string.IsNullOrEmpty(ddlGroupLeague.SelectedValue) ||
                        !lbLeagueTeam.Items.Cast<ListItem>().Any(x => x.Selected))
                    { throw new Exception("请选择联赛分类与分组球队"); }

                    var g = new Group();

                    if (GroupGuid != Guid.Empty)
                    {
                        g = _repo.Single<Group>(GroupGuid);
                    }
                    else
                    {
                        g.ID = new Guid(tbGroupGuid.Text);
                    }

                    g.GroupName = tbGroupName.Text;
                    g.GroupOrder = Convert.ToInt16(tbGroupOrder.Text);
                    g.RankMethod = (RankMethodType)Enum.Parse(typeof(RankMethodType), rblRankMethod.SelectedValue);
                    g.IsTable = cbIsTable.Checked;

                    g.LeagueGuid = new Guid(ddlGroupLeague.SelectedValue);

                    #region ListBox Multiple Value for RelationGroupTeam

                    if (GroupGuid != Guid.Empty)
                    {
                        var rgts = _repo.Query<RelationGroupTeam>(x => x.GroupGuid == GroupGuid);

                        if (rgts.Count > 0)
                        {
                            foreach (var rgt in rgts)
                            {
                                rgt.Delete();
                            }
                        }
                    }

                    foreach (ListItem item in lbLeagueTeam.Items)
                    {
                        Guid tGuid;
                        if (item.Selected && Guid.TryParse(item.Value, out tGuid))
                        {
                            var gt = new RelationGroupTeam
                            {
                                GroupGuid = g.ID,
                                TeamGuid = tGuid
                            };

                            _repo.Insert(gt);
                        }
                    }

                    #endregion

                    if (GroupGuid != Guid.Empty)
                    {
                        _repo.Update(g);
                        trans.Commit();

                        ClientScript.RegisterClientScriptBlock(typeof(string), "success", "alert('更新分组成功');", true);
                    }
                    else
                    {
                        _repo.Insert(g);
                        trans.Commit();

                        ClientScript.RegisterClientScriptBlock(typeof(string), "success", "alert('添加分组成功');", true);
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}')", true);
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (GroupGuid != Guid.Empty)
            {
                Response.Redirect($"AdminGroup.aspx?LeagueGuid={_repo.Single<Group>(GroupGuid)?.LeagueGuid}");
            }
            else
            {
                Response.Redirect("AdminGroup.aspx");
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (GroupGuid != Guid.Empty)
                {
                    _repo.Delete<Group>(GroupGuid);

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('删除成功');window.location.href='AdminGroup.aspx'", true);
                }
                else
                {
                    throw new Exception("无对应分组");
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}')", true);
            }
        }

        private void BindGroupTeam(Guid leagueGuid)
        {
            var rlts = _repo.Query<RelationLeagueTeam>(x => x.LeagueGuid == leagueGuid);

            var query = from rlt in rlts
                        join t in Team.Cache.TeamList on rlt.TeamGuid equals t.ID
                        orderby t.TeamEnglishName
                        select t;

            lbLeagueTeam.DataSource = query;
            lbLeagueTeam.DataTextField = "TeamDisplayName";
            lbLeagueTeam.DataValueField = "ID";
            lbLeagueTeam.DataBind();

            lbLeagueTeam.Visible = true;

            #region Set lbLeagueTeam Selected Items

            if (GroupGuid != Guid.Empty)
            {
                var rgts = _repo.Query<RelationGroupTeam>(x => x.GroupGuid == GroupGuid);

                foreach (ListItem item in lbLeagueTeam.Items)
                {
                    if (rgts.Exists(x => x.TeamGuid.ToString() == item.Value))
                    { item.Selected = true; }
                }
            }

            #endregion
        }
    }
}