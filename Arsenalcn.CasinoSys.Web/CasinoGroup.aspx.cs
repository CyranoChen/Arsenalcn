using System;
using System.Data;
using System.Web.UI.WebControls;

using Arsenalcn.CasinoSys.Entity;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class CasinoGroup : Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Assign Control Property

            ctrlLeftPanel.UserID = userid;
            ctrlLeftPanel.UserName = username;

            ctrlFieldTooBar.UserID = userid;

            ctrlMenuTabBar.CurrentMenu = Arsenalcn.CasinoSys.Web.Control.CasinoMenuType.CasinoGroup;

            ctrlLeagueHeader.CurrLeagueGuid = CurrentLeague;
            ctrlLeagueHeader.PageURL = "CasinoGroup.aspx";

            #endregion

            if (!IsPostBack)
            {
                if (CurrentLeague != Guid.Empty)
                {
                    //Bind ddlSeason
                    DataTable dt = Entity.League.GetLeagueAllSeason(CurrentLeague);
                    ddlSeason.DataSource = dt;
                    ddlSeason.DataTextField = "LeagueSeason";
                    ddlSeason.DataValueField = "LeagueGuid";
                    ddlSeason.DataBind();
                    ddlSeason.SelectedValue = CurrentLeague.ToString();

                    // Bind ddlGroup
                    DataTable dtGroup = Entity.Group.GetGroupByLeague(CurrentLeague);
                    if (dtGroup != null)
                    {
                        ddlGroup.DataSource = dtGroup;
                        ddlGroup.DataTextField = "GroupName";
                        ddlGroup.DataValueField = "GroupGuid";
                        ddlGroup.DataBind();

                        ListItem item = new ListItem("所有分组", Guid.Empty.ToString());
                        ddlGroup.Items.Insert(0, item);

                        ddlGroup.SelectedValue = CurrentGroup.ToString();
                    }
                    else
                        ddlGroup.Visible = false;
                }
                else
                    ddlSeason.Visible = false;

                BindData();
            }
        }

        private void BindData()
        {
            if (CurrentGroup != Guid.Empty)
            {
                BindGroupData(gvGroupTable, CurrentGroup);

                Group group = new Group(CurrentGroup);

                btnGroupMatch.Visible = !group.IsTable;
                pnlGroupList.Visible = false;
            }
            else
            {
                if (Entity.Group.IsExistGroupByLeague(CurrentLeague, false))
                {
                    DataTable dtGroup = Entity.Group.GetGroupByLeague(CurrentLeague);
                    rptGroup.DataSource = dtGroup;
                    rptGroup.DataBind();

                    pnlGroupList.Visible = true;
                    gvGroupTable.Visible = false;
                    btnGroupMatch.Visible = false;
                }
                else if (Entity.Group.IsExistGroupByLeague(CurrentLeague, true))
                {
                    DataTable dtTable = Entity.Group.GetGroupByLeague(CurrentLeague);
                    DataRow drTable = dtTable.Rows[0];

                    BindGroupData(gvGroupTable, (Guid)drTable["GroupGuid"]);

                    pnlGroupList.Visible = false;
                    btnGroupMatch.Visible = false;
                }
                else
                {
                    pnlGroupList.Visible = false;
                    gvGroupTable.Visible = false;
                    btnGroupMatch.Visible = false;
                }
            }
        }

        private void BindGroupData(GridView gv, Guid groupGuid)
        {
            DataTable dt = Entity.Group.GetTableGroupTeam(groupGuid);

            if (dt != null)
            {
                dt.Columns.Add("GoalDiff", typeof(string));
                foreach (DataRow dr in dt.Rows)
                {
                    if ((short)dr["TotalGoalDiff"] > 0)
                        dr["GoalDiff"] = string.Format("+{0}", dr["TotalGoalDiff"].ToString());
                    else
                        dr["GoalDiff"] = dr["TotalGoalDiff"].ToString();
                }
            }

            gv.Columns[1].HeaderText = string.Format("<a href=\"CasinoGroup.aspx?Group={0}\">&lt;{1}&gt;</a>", groupGuid.ToString(), new Group(groupGuid).GroupName);

            gv.DataSource = dt;
            gv.DataBind();
        }

        public Guid CurrentLeague
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["League"]))
                {
                    try
                    {
                        return new Guid(Request.QueryString["League"]);
                    }
                    catch
                    {
                        return Entity.ConfigGlobal.DefaultLeagueID;
                    }
                }
                else if (CurrentGroup != Guid.Empty)
                {
                    try
                    {
                        return new Group(CurrentGroup).LeagueGuid;
                    }
                    catch
                    {
                        return Entity.ConfigGlobal.DefaultLeagueID;
                    }
                }
                else
                    return Entity.ConfigGlobal.DefaultLeagueID;
            }
        }

        public Guid CurrentGroup
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["Group"]))
                {
                    try
                    {
                        return new Guid(Request.QueryString["Group"]);
                    }
                    catch
                    {
                        return Guid.Empty;
                    }
                }
                else
                    return Guid.Empty;
            }
        }

        protected void ddlSeason_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("CasinoGroup.aspx?League={0}", ddlSeason.SelectedValue.ToString()));
        }

        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlGroup.SelectedValue != Guid.Empty.ToString())
                Response.Redirect(string.Format("CasinoGroup.aspx?Group={0}", ddlGroup.SelectedValue.ToString()));
            else
                Response.Redirect(string.Format("CasinoGroup.aspx?League={0}", ddlSeason.SelectedValue.ToString()));
        }

        protected void rptGroup_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                DataRowView dr = e.Item.DataItem as DataRowView;
                GridView gvGroupTeam = e.Item.FindControl("gvGroupTeam") as GridView;

                BindGroupData(gvGroupTeam, (Guid)dr["GroupGuid"]);
            }
        }

        protected void btnGroupMatch_Click(object sender, EventArgs e)
        {
            Response.Redirect("CasinoGame.aspx?Group=" + CurrentGroup.ToString());
        }
    }
}