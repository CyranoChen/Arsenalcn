using System;
using System.Data;
using System.Web.UI.WebControls;
using Arsenalcn.CasinoSys.Entity;
using Arsenalcn.CasinoSys.Web.Common;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class AdminGroup : AdminBasePage
    {
        private Guid SelectedLeague
        {
            get { return new Guid(ddlLeague.SelectedValue); }
        }

        private Guid SelectedGroup
        {
            get
            {
                if (gvGroup.SelectedIndex == -1)
                    return Guid.Empty;
                return (Guid) gvGroup.DataKeys[gvGroup.SelectedIndex].Value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = username;

            if (!IsPostBack)
            {
                var list = League.Cache.LeagueList_Active;

                var item = new ListItem("--请选择分类--", Guid.Empty.ToString());

                ddlLeague.DataSource = list;
                ddlLeague.DataTextField = "LeagueNameInfo";
                ddlLeague.DataValueField = "ID";
                ddlLeague.DataBind();
                ddlLeague.Items.Insert(0, item);

                ddlGroupLeague.DataSource = list;
                ddlGroupLeague.DataTextField = "LeagueNameInfo";
                ddlGroupLeague.DataValueField = "ID";
                ddlGroupLeague.DataBind();
                ddlGroupLeague.Items.Insert(0, item);

                if (ddlGroupLeague.Items[0].Value == Guid.Empty.ToString())
                    lbLeagueTeam.Visible = false;

                BindGroupData();

                ResetForm();
            }
        }

        private void BindGroupData()
        {
            var dtGroup = Group.GetGroupByLeague(SelectedLeague);
            DataTable dtGroupTeam;

            if (dtGroup != null)
            {
                dtGroup.Columns.Add("LeagueName", typeof (string));
                dtGroup.Columns.Add("LeagueSeason", typeof (string));

                dtGroup.Columns.Add("GroupTeamCount", typeof (int));
                dtGroup.Columns.Add("GroupTeamList", typeof (string));
                dtGroup.Columns.Add("GroupMatchCount", typeof (string));
                dtGroup.Columns.Add("GroupAllMatchCount", typeof (int));

                foreach (DataRow dr in dtGroup.Rows)
                {
                    var l = League.Cache.Load((Guid) dr["LeagueGuid"]);

                    dr["LeagueName"] = l.LeagueName;
                    dr["LeagueSeason"] = l.LeagueSeason;

                    var groupTeamCount = 0;
                    dtGroupTeam = Group.GetRelationGroupTeam((Guid) dr["GroupGuid"]);

                    if (dtGroupTeam != null)
                    {
                        foreach (DataRow drTeam in dtGroupTeam.Rows)
                        {
                            dr["GroupTeamList"] += $"{Team.Cache.Load((Guid) drTeam["TeamGuid"]).TeamDisplayName},";
                            groupTeamCount++;
                        }
                        dr["GroupTeamCount"] = groupTeamCount;
                    }
                    else
                        dr["GroupTeamCount"] = 0;

                    var allMatchCount = Group.GetAllMatchCount((Guid) dr["GroupGuid"]);
                    var resultMatchCount = Group.GetResultMatchCount((Guid) dr["GroupGuid"]);

                    dr["GroupAllMatchCount"] = allMatchCount;
                    dr["GroupMatchCount"] = $"{allMatchCount}({resultMatchCount})";
                }
            }

            gvGroup.DataSource = dtGroup;
            gvGroup.DataBind();


            pnlGroupView.Visible = false;
            gvGroup.Visible = true;
        }

        private void ResetForm()
        {
            lblGroupGuid.Text = Guid.NewGuid().ToString();
            tbGroupName.Text = string.Empty;
            tbGroupOrder.Text = "0";
            cbIsTable.Checked = false;
            ddlGroupLeague.SelectedIndex = 0;
            lbLeagueTeam.Items.Clear();
            lbLeagueTeam.Visible = false;
            pnlGroupView.Visible = true;
        }

        private void BindGroupTeam()
        {
            var list = Team.Cache.GetTeamsByLeagueGuid(new Guid(ddlGroupLeague.SelectedValue));

            lbLeagueTeam.DataSource = list;
            lbLeagueTeam.DataTextField = "TeamDisplayName";
            lbLeagueTeam.DataValueField = "ID";
            lbLeagueTeam.DataBind();

            #region Set lbLeagueTeam Selected Items

            if (SelectedGroup != Guid.Empty)
            {
                foreach (ListItem item in lbLeagueTeam.Items)
                {
                    if (GroupTeam.IsExistRelationGroupTeam(SelectedGroup, new Guid(item.Value)))
                        item.Selected = true;
                }
            }

            #endregion
        }

        protected void gvGroup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var drv = e.Row.DataItem as DataRowView;

                var btnResetGroupTable = e.Row.FindControl("btnResetGroupTable") as LinkButton;
                var btnResetGroupMatch = e.Row.FindControl("btnResetGroupMatch") as LinkButton;

                if (drv != null && btnResetGroupTable != null && btnResetGroupMatch != null)
                {
                    btnResetGroupTable.CommandArgument = drv["GroupGuid"].ToString();

                    if (!(bool) drv["IsTable"])
                        btnResetGroupMatch.CommandArgument = drv["GroupGuid"].ToString();
                    else
                        btnResetGroupMatch.Visible = false;
                }
            }
        }

        protected void ddlLeague_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvGroup.SelectedIndex = -1;

            ResetForm();

            BindGroupData();
        }

        protected void ddlGroupLeague_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlGroupLeague.SelectedValue != Guid.Empty.ToString())
            {
                BindGroupTeam();
                lbLeagueTeam.Visible = true;
            }
            else
            {
                lbLeagueTeam.Items.Clear();
                lbLeagueTeam.Visible = false;
            }
        }

        protected void gvGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvGroup.SelectedIndex != -1)
            {
                var group = new Group(SelectedGroup);

                lblGroupGuid.Text = group.GroupGuid.ToString();
                tbGroupName.Text = @group.GroupName;
                tbGroupOrder.Text = group.GroupOrder.ToString();
                cbIsTable.Checked = group.IsTable;

                ddlGroupLeague.SelectedValue = group.LeagueGuid.ToString();
                BindGroupTeam();

                lbLeagueTeam.Visible = true;
                pnlGroupView.Visible = true;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var groupGuid = new Guid(lblGroupGuid.Text);
                Guid leagueGuid;
                Guid teamGuid;

                #region ListBox Multiple Value for RelationGroupTeam

                if (ddlGroupLeague.SelectedValue != Guid.Empty.ToString())
                {
                    Group.RemoveRelationGroupAllTeam(groupGuid);

                    leagueGuid = new Guid(ddlGroupLeague.SelectedValue);

                    foreach (ListItem item in lbLeagueTeam.Items)
                    {
                        teamGuid = new Guid(item.Value);
                        if ((item.Selected) && (!GroupTeam.IsExistRelationGroupTeam(groupGuid, teamGuid)))
                        {
                            var gt = new GroupTeam();
                            gt.GroupGuid = groupGuid;
                            gt.TeamGuid = teamGuid;
                            gt.Insert();
                        }
                    }

                    ClientScript.RegisterClientScriptBlock(typeof (string), "updated", "alert('保存分组球队列表');", true);
                }
                else
                    leagueGuid = SelectedLeague;

                #endregion

                if (leagueGuid == Guid.Empty)
                    throw new Exception("No Selected League Guid");

                var group = new Group(groupGuid);
                group.GroupGuid = groupGuid;
                group.GroupName = tbGroupName.Text;
                group.GroupOrder = Convert.ToInt16(tbGroupOrder.Text);
                group.LeagueGuid = leagueGuid;
                group.IsTable = cbIsTable.Checked;

                if (gvGroup.SelectedIndex != -1)
                {
                    group.Update();
                    ClientScript.RegisterClientScriptBlock(typeof (string), "success", "alert('更新分组成功');", true);
                }
                else
                {
                    group.Insert();
                    ClientScript.RegisterClientScriptBlock(typeof (string), "success", "alert('添加分组成功');", true);
                }

                gvGroup.SelectedIndex = -1;

                ResetForm();

                BindGroupData();
            }
            catch
            {
                if (SelectedLeague == Guid.Empty)
                    ClientScript.RegisterClientScriptBlock(typeof (string), "failed", "alert('没有选择当前分类');", true);
                else
                    ClientScript.RegisterClientScriptBlock(typeof (string), "failed", "alert('添加/更新分组失败');", true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            BindGroupData();
            pnlGroupView.Visible = false;
        }

        protected void btnTeamAdd_Click(object sender, EventArgs e)
        {
            ResetForm();
            gvGroup.Visible = false;
        }

        protected void gvGroup_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlLeague.SelectedValue))
            {
                try
                {
                    var groupGuid = (Guid) gvGroup.DataKeys[e.RowIndex].Value;
                    //Guid leagueGuid = new Guid(ddlLeague.SelectedValue);

                    Group.RemoveGroup(groupGuid);

                    ClientScript.RegisterClientScriptBlock(typeof (string), "success", "alert('删除分组成功');", true);
                }
                catch
                {
                    ClientScript.RegisterClientScriptBlock(typeof (string), "failed", "alert('删除分组失败');", true);
                }
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(typeof (string), "failed", "alert('没有选择当前分类');", true);
            }

            BindGroupData();
        }

        protected void gvGroup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ResetGroupTable")
            {
                var groupGuid = new Guid(e.CommandArgument.ToString());
                //Guid groupGuid = new Guid(gvGroup.SelectedDataKey.Value.ToString());
                try
                {
                    Group.GroupTableStatistics(groupGuid);

                    ClientScript.RegisterClientScriptBlock(typeof (string), "success", "alert('统计积分榜成功');", true);
                }
                catch
                {
                    ClientScript.RegisterClientScriptBlock(typeof (string), "failed", "alert('统计积分榜失败');", true);
                }
            }

            if (e.CommandName == "ResetGroupMatch")
            {
                var groupGuid = new Guid(e.CommandArgument.ToString());

                try
                {
                    Group.SetGroupMatch(groupGuid);

                    ClientScript.RegisterClientScriptBlock(typeof (string), "success", "alert('绑定比赛成功');", true);
                }
                catch
                {
                    ClientScript.RegisterClientScriptBlock(typeof (string), "failed", "alert('绑定比赛失败');", true);
                }
            }
        }

        protected void gvGroup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvGroup.PageIndex = e.NewPageIndex;

            BindGroupData();
        }
    }
}