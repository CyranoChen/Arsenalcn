using System;
using System.Data;
using System.Web.UI.WebControls;

using Arsenalcn.CasinoSys.Entity;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class AdminGroup : Common.AdminBasePage
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
                        if (!Convert.IsDBNull(dr["LeagueSeason"]) && !string.IsNullOrEmpty(dr["LeagueSeason"].ToString()))
                        {
                            dr["LeagueDisplay"] = string.Format("{0}{1}", dr["LeagueName"], dr["LeagueSeason"]);
                        }
                    }
                }

                ListItem item = new ListItem("--请选择分类--", Guid.Empty.ToString());

                ddlLeague.DataSource = dtLeague;
                ddlLeague.DataTextField = "LeagueDisplay";
                ddlLeague.DataValueField = "LeagueGuid";
                ddlLeague.DataBind();
                ddlLeague.Items.Insert(0, item);

                ddlGroupLeague.DataSource = dtLeague;
                ddlGroupLeague.DataTextField = "LeagueDisplay";
                ddlGroupLeague.DataValueField = "LeagueGuid";
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
            DataTable dtGroup = Entity.Group.GetGroupByLeague(SelectedLeague);
            DataTable dtGroupTeam;

            if (dtGroup != null)
            {
                dtGroup.Columns.Add("GroupTeamCount", typeof(int));
                dtGroup.Columns.Add("GroupTeamList", typeof(string));
                dtGroup.Columns.Add("GroupMatchCount", typeof(string));
                dtGroup.Columns.Add("GroupAllMatchCount", typeof(int));

                foreach (DataRow dr in dtGroup.Rows)
                {
                    int groupTeamCount = 0;
                    dtGroupTeam = Entity.Group.GetRelationGroupTeam((Guid)dr["GroupGuid"]);

                    if (dtGroupTeam != null)
                    {
                        foreach (DataRow drTeam in dtGroupTeam.Rows)
                        {
                            dr["GroupTeamList"] += string.Format("{0},", new Team((Guid)drTeam["TeamGuid"]).TeamDisplayName.ToString());
                            groupTeamCount++;
                        }
                        dr["GroupTeamCount"] = groupTeamCount;
                    }
                    else
                        dr["GroupTeamCount"] = 0;

                    int allMatchCount = Entity.Group.GetAllMatchCount((Guid)dr["GroupGuid"]);
                    int resultMatchCount = Entity.Group.GetResultMatchCount((Guid)dr["GroupGuid"]);

                    dr["GroupAllMatchCount"] = allMatchCount;
                    dr["GroupMatchCount"] = string.Format("{0}({1})", allMatchCount.ToString(), resultMatchCount.ToString());
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
            DataTable dtTeam = Entity.Team.GetTeamByLeague(new Guid(ddlGroupLeague.SelectedValue));

            if (dtTeam != null)
            {
                dtTeam.Columns.Add("TeamDisplay", typeof(string));

                foreach (DataRow dr in dtTeam.Rows)
                {
                    if (!Convert.IsDBNull(dr["TeamDisplayName"]) && !Convert.IsDBNull(dr["TeamEnglishName"]))
                        dr["TeamDisplay"] = string.Format("{0} ({1})", dr["TeamDisplayName"].ToString(), dr["TeamEnglishName"].ToString());
                    else
                        dr["TeamDisplay"] = dr["TeamDisplayName"].ToString();
                }
            }

            lbLeagueTeam.DataSource = dtTeam;
            lbLeagueTeam.DataTextField = "TeamDisplay";
            lbLeagueTeam.DataValueField = "TeamGuid";
            lbLeagueTeam.DataBind();

            #region Set lbLeagueTeam Selected Items
            if (SelectedGroup != Guid.Empty)
            {
                foreach (ListItem item in lbLeagueTeam.Items)
                {
                    if (Entity.GroupTeam.IsExistRelationGroupTeam(SelectedGroup, new Guid(item.Value)))
                        item.Selected = true;
                }
            }
            #endregion
        }

        protected void gvGroup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = e.Row.DataItem as DataRowView;

                LinkButton btnResetGroupTable = e.Row.FindControl("btnResetGroupTable") as LinkButton;
                LinkButton btnResetGroupMatch = e.Row.FindControl("btnResetGroupMatch") as LinkButton;

                if (btnResetGroupTable != null)
                    btnResetGroupTable.CommandArgument = drv["GroupGuid"].ToString();

                if (btnResetGroupMatch != null && !(bool)drv["IsTable"])
                    btnResetGroupMatch.CommandArgument = drv["GroupGuid"].ToString();
                else
                    btnResetGroupMatch.Visible = false;
            }
        }

        private Guid SelectedLeague
        {
            get
            {
                return new Guid(ddlLeague.SelectedValue);
            }
        }

        private Guid SelectedGroup
        {
            get
            {
                if (gvGroup.SelectedIndex == -1)
                    return Guid.Empty;
                else
                {
                    return (Guid)gvGroup.DataKeys[gvGroup.SelectedIndex].Value;
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
                Group group = new Group(SelectedGroup);

                lblGroupGuid.Text = group.GroupGuid.ToString();
                tbGroupName.Text = group.GroupName.ToString();
                tbGroupOrder.Text = group.GroupOrder.ToString();
                cbIsTable.Checked = (bool)group.IsTable;

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
                Guid groupGuid = new Guid(lblGroupGuid.Text);
                Guid leagueGuid = Guid.Empty;
                Guid teamGuid = Guid.Empty;

                #region ListBox Multiple Value for RelationGroupTeam
                if (ddlGroupLeague.SelectedValue != Guid.Empty.ToString())
                {
                    Entity.Group.RemoveRelationGroupAllTeam(groupGuid);

                    leagueGuid = new Guid(ddlGroupLeague.SelectedValue.ToString());

                    foreach (ListItem item in lbLeagueTeam.Items)
                    {
                        teamGuid = new Guid(item.Value);
                        if ((item.Selected) && (!Entity.GroupTeam.IsExistRelationGroupTeam(groupGuid, teamGuid)))
                        {
                            GroupTeam gt = new GroupTeam();
                            gt.GroupGuid = groupGuid;
                            gt.TeamGuid = teamGuid;
                            gt.Insert();
                        }
                    }

                    this.ClientScript.RegisterClientScriptBlock(typeof(string), "updated", "alert('保存分组球队列表');", true);
                }
                else
                    leagueGuid = SelectedLeague;
                #endregion

                if (leagueGuid == Guid.Empty)
                    throw new Exception("No Selected League Guid");

                Group group = new Group(groupGuid);
                group.GroupGuid = groupGuid;
                group.GroupName = tbGroupName.Text;
                group.GroupOrder = Convert.ToInt16(tbGroupOrder.Text);
                group.LeagueGuid = leagueGuid;
                group.IsTable = cbIsTable.Checked;

                if (gvGroup.SelectedIndex != -1)
                {
                    group.Update();
                    this.ClientScript.RegisterClientScriptBlock(typeof(string), "success", "alert('更新分组成功');", true);
                }
                else
                {
                    group.Insert();
                    this.ClientScript.RegisterClientScriptBlock(typeof(string), "success", "alert('添加分组成功');", true);
                }

                gvGroup.SelectedIndex = -1;

                ResetForm();

                BindGroupData();
            }
            catch
            {
                if (SelectedLeague == Guid.Empty)
                    this.ClientScript.RegisterClientScriptBlock(typeof(string), "failed", "alert('没有选择当前分类');", true);
                else
                    this.ClientScript.RegisterClientScriptBlock(typeof(string), "failed", "alert('添加/更新分组失败');", true);
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
                    Guid groupGuid = (Guid)gvGroup.DataKeys[e.RowIndex].Value;
                    //Guid leagueGuid = new Guid(ddlLeague.SelectedValue);

                    Entity.Group.RemoveGroup(groupGuid);

                    this.ClientScript.RegisterClientScriptBlock(typeof(string), "success", "alert('删除分组成功');", true);
                }
                catch
                {
                    this.ClientScript.RegisterClientScriptBlock(typeof(string), "failed", "alert('删除分组失败');", true);
                }
            }
            else
            {
                this.ClientScript.RegisterClientScriptBlock(typeof(string), "failed", "alert('没有选择当前分类');", true);
            }

            BindGroupData();
        }

        protected void gvGroup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ResetGroupTable")
            {
                Guid groupGuid = new Guid(e.CommandArgument.ToString());
                //Guid groupGuid = new Guid(gvGroup.SelectedDataKey.Value.ToString());
                try
                {
                    Entity.Group.GroupTableStatistics(groupGuid);

                    this.ClientScript.RegisterClientScriptBlock(typeof(string), "success", "alert('统计积分榜成功');", true);
                }
                catch (Exception ex)
                {
                    this.ClientScript.RegisterClientScriptBlock(typeof(string), "failed", "alert('统计积分榜失败');", true);
                    throw ex;
                }
            }

            if (e.CommandName == "ResetGroupMatch")
            {
                Guid groupGuid = new Guid(e.CommandArgument.ToString());

                try
                {
                    Entity.Group.SetGroupMatch(groupGuid);

                    this.ClientScript.RegisterClientScriptBlock(typeof(string), "success", "alert('绑定比赛成功');", true);
                }
                catch
                {
                    this.ClientScript.RegisterClientScriptBlock(typeof(string), "failed", "alert('绑定比赛失败');", true);
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
