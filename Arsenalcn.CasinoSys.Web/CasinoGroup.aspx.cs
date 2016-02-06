using System;
using System.Data;
using System.Web.UI.WebControls;
using Arsenalcn.CasinoSys.Entity;
using Arsenalcn.CasinoSys.Web.Common;
using Arsenalcn.CasinoSys.Web.Control;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class CasinoGroup : BasePage
    {
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
                        return ConfigGlobal.DefaultLeagueID;
                    }
                }
                if (CurrentGroup != Guid.Empty)
                {
                    try
                    {
                        return new Group(CurrentGroup).LeagueGuid;
                    }
                    catch
                    {
                        return ConfigGlobal.DefaultLeagueID;
                    }
                }
                return ConfigGlobal.DefaultLeagueID;
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
                return Guid.Empty;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            #region Assign Control Property

            ctrlLeftPanel.UserId = userid;
            ctrlLeftPanel.UserName = username;

            ctrlFieldTooBar.UserId = userid;

            ctrlMenuTabBar.CurrentMenu = CasinoMenuType.CasinoGroup;

            ctrlLeagueHeader.CurrLeagueGuid = CurrentLeague;
            ctrlLeagueHeader.PageUrl = "CasinoGroup.aspx";

            #endregion

            if (!IsPostBack)
            {
                if (CurrentLeague != Guid.Empty)
                {
                    //Bind ddlSeason
                    var list = League.Cache.GetSeasonsByLeagueGuid(CurrentLeague);

                    ddlSeason.DataSource = list;
                    ddlSeason.DataTextField = "LeagueSeason";
                    ddlSeason.DataValueField = "ID";
                    ddlSeason.DataBind();
                    ddlSeason.SelectedValue = CurrentLeague.ToString();

                    // Bind ddlGroup
                    var dtGroup = Group.GetGroupByLeague(CurrentLeague);

                    if (dtGroup != null)
                    {
                        ddlGroup.DataSource = dtGroup;
                        ddlGroup.DataTextField = "GroupName";
                        ddlGroup.DataValueField = "GroupGuid";
                        ddlGroup.DataBind();

                        var item = new ListItem("所有分组", Guid.Empty.ToString());
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

                var group = new Group(CurrentGroup);

                btnGroupMatch.Visible = !group.IsTable;
                pnlGroupList.Visible = false;
            }
            else
            {
                if (Group.IsExistGroupByLeague(CurrentLeague, false))
                {
                    var dtGroup = Group.GetGroupByLeague(CurrentLeague);
                    rptGroup.DataSource = dtGroup;
                    rptGroup.DataBind();

                    pnlGroupList.Visible = true;
                    gvGroupTable.Visible = false;
                    btnGroupMatch.Visible = false;
                }
                else if (Group.IsExistGroupByLeague(CurrentLeague, true))
                {
                    var dtTable = Group.GetGroupByLeague(CurrentLeague);
                    var drTable = dtTable.Rows[0];

                    BindGroupData(gvGroupTable, (Guid) drTable["GroupGuid"]);

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
            var dt = Group.GetTableGroupTeam(groupGuid);

            gv.Columns[1].HeaderText =
                $"<a href=\"CasinoGroup.aspx?Group={groupGuid}\">&lt;{new Group(groupGuid).GroupName}&gt;</a>";

            gv.DataSource = dt;
            gv.DataBind();
        }

        protected void ddlSeason_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect($"CasinoGroup.aspx?League={ddlSeason.SelectedValue}");
        }

        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlGroup.SelectedValue != Guid.Empty.ToString())
                Response.Redirect($"CasinoGroup.aspx?Group={ddlGroup.SelectedValue}");
            else
                Response.Redirect($"CasinoGroup.aspx?League={ddlSeason.SelectedValue}");
        }

        protected void rptGroup_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                var dr = e.Item.DataItem as DataRowView;
                var gvGroupTeam = e.Item.FindControl("gvGroupTeam") as GridView;

                if (dr != null) BindGroupData(gvGroupTeam, (Guid) dr["GroupGuid"]);
            }
        }

        protected void btnGroupMatch_Click(object sender, EventArgs e)
        {
            Response.Redirect("CasinoGame.aspx?Group=" + CurrentGroup);
        }

        protected void gvGroupTable_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var drv = e.Row.DataItem as DataRowView;

                var ltrlTeamLogo = e.Row.FindControl("ltrlTeamLogo") as Literal;
                var hlTeamInfo = e.Row.FindControl("hlTeamInfo") as HyperLink;
                var ltrlGoalDiff = e.Row.FindControl("ltrlGoalDiff") as Literal;

                if (drv != null)
                {
                    if (ltrlTeamLogo != null && hlTeamInfo != null)
                    {
                        var t = Team.Cache.Load((Guid) drv["TeamGuid"]);

                        ltrlTeamLogo.Text =
                            string.Format(
                                "<span class=\"CasinoSys_GameName\" title=\"{0}\"><img src=\"{1}\" alt=\"{0}\" /></span>",
                                t.TeamEnglishName, t.TeamLogo);

                        hlTeamInfo.Text = $"<em>{t.TeamDisplayName}</em>";
                        hlTeamInfo.NavigateUrl = $"CasinoTeam.aspx?Team={t.ID}";
                    }

                    if (ltrlGoalDiff != null)
                    {
                        var goalDiff = (short) drv["TotalGoalDiff"];

                        ltrlGoalDiff.Text = $"<em>{(goalDiff > 0 ? ("+" + goalDiff) : goalDiff.ToString())}</em>";
                    }
                }
            }
        }

        protected void gvGroupTeam_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var drv = e.Row.DataItem as DataRowView;

                var ltrlTeamLogo = e.Row.FindControl("ltrlTeamLogo") as Literal;
                var hlTeamInfo = e.Row.FindControl("hlTeamInfo") as HyperLink;
                //var ltrlGoalDiff = e.Row.FindControl("ltrlGoalDiff") as Literal;

                if (drv != null && ltrlTeamLogo != null && hlTeamInfo != null)
                {
                    var t = Team.Cache.Load((Guid) drv["TeamGuid"]);

                    ltrlTeamLogo.Text =
                        string.Format(
                            "<span class=\"CasinoSys_GameName\" title=\"{0}\"><img src=\"{1}\" alt=\"{0}\" /></span>",
                            t.TeamEnglishName, t.TeamLogo);

                    hlTeamInfo.Text = $"<em>{t.TeamDisplayName}</em>";
                    hlTeamInfo.NavigateUrl = $"CasinoTeam.aspx?Team={t.ID}";
                }
            }
        }
    }
}