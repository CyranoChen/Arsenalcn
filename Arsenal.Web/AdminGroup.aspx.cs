using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Arsenal.Service;
using Arsenalcn.Core;
using CasinoMatch = Arsenal.Service.Casino.Match;

namespace Arsenal.Web
{
    public partial class AdminGroup : AdminPageBase
    {
        private readonly IRepository _repo = new Repository();

        private Guid? _leagueGuid;

        private Guid? LeagueGuid
        {
            get
            {
                if (_leagueGuid.HasValue && _leagueGuid == Guid.Empty)
                    return _leagueGuid;
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
                return null;
            }
            set { _leagueGuid = value; }
        }

        //private Guid? _groupGuid;

        //private Guid? GroupGuid
        //{
        //    get
        //    {
        //        if (_groupGuid.HasValue && _groupGuid == Guid.Empty)
        //            return _groupGuid;
        //        if (!string.IsNullOrEmpty(Request.QueryString["GroupGuid"]))
        //        {
        //            try
        //            {
        //                return new Guid(Request.QueryString["GroupGuid"]);
        //            }
        //            catch
        //            {
        //                return Guid.Empty;
        //            }
        //        }
        //        return null;
        //    }
        //    set { _groupGuid = value; }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = Username;

            if (!IsPostBack)
            {
                #region Bind ddlLeague

                var list = League.Cache.LeagueList_Active;

                ddlLeague.DataSource = list;
                ddlLeague.DataTextField = "LeagueNameInfo";
                ddlLeague.DataValueField = "ID";
                ddlLeague.DataBind();

                if (LeagueGuid.HasValue && LeagueGuid != Guid.Empty)
                {
                    ddlLeague.SelectedValue = LeagueGuid.ToString();
                }
                else
                {
                    ddlLeague.SelectedValue = ConfigGlobal_AcnCasino.DefaultLeagueID.ToString();
                }

                ViewState["LeagueGuid"] = ddlLeague.SelectedValue;

                #endregion

                BindData();
            }
        }

        private void BindData()
        {
            var list = _repo.All<Group>().FindAll(x =>
            {
                var returnValue = true;
                string tmpString;

                if (ViewState["LeagueGuid"] != null)
                {
                    tmpString = ViewState["LeagueGuid"].ToString();
                    if (!string.IsNullOrEmpty(tmpString))
                        returnValue = x.LeagueGuid.Equals(new Guid(tmpString));
                }

                if (ViewState["GroupName"] != null)
                {
                    tmpString = ViewState["GroupName"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && tmpString != "--分组名称--")
                        returnValue = returnValue && x.GroupName.ToLower().Contains(tmpString.ToLower());
                }

                return returnValue;
            });

            #region set GridView Selected PageIndex

            //if (GroupGuid.HasValue && !GroupGuid.Value.Equals(Guid.Empty))
            //{
            //    var i = list.FindIndex(x => x.ID.Equals(GroupGuid));

            //    if (i >= 0)
            //    {
            //        gvGroup.PageIndex = i / gvGroup.PageSize;
            //        gvGroup.SelectedIndex = i % gvGroup.PageSize;
            //    }
            //    else
            //    {
            //        gvGroup.PageIndex = 0;
            //        gvGroup.SelectedIndex = -1;
            //    }
            //}
            //else
            //{
            //    gvGroup.SelectedIndex = -1;
            //}

            #endregion

            gvGroup.DataSource = list;
            gvGroup.DataBind();
        }

        protected void gvGroup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var g = e.Row.DataItem as Group;

                var btnResetGroupTable = e.Row.FindControl("btnResetGroupTable") as LinkButton;
                var btnResetGroupMatch = e.Row.FindControl("btnResetGroupMatch") as LinkButton;

                if (g != null && btnResetGroupTable != null && btnResetGroupMatch != null)
                {
                    btnResetGroupTable.CommandArgument = g.ID.ToString();

                    if (!g.IsTable)
                        btnResetGroupMatch.CommandArgument = g.ID.ToString();
                    else
                        btnResetGroupMatch.Visible = false;
                }

                var ltrlGroupTeamInfo = e.Row.FindControl("ltrlGroupTeamInfo") as Literal;
                var ltrlGroupMatchInfo = e.Row.FindControl("ltrlGroupMatchInfo") as Literal;

                if (g != null && ltrlGroupTeamInfo != null && ltrlGroupMatchInfo != null)
                {
                    ltrlGroupTeamInfo.Text = $"<em>{_repo.Query<RelationGroupTeam>(x => x.GroupGuid == g.ID).Count}</em>";

                    List<CasinoMatch> matches;

                    if (!g.IsTable)
                    {
                        matches = _repo.Query<CasinoMatch>(x => x.GroupGuid == g.ID);
                    }
                    else
                    {
                        matches = _repo.Query<CasinoMatch>(x => x.LeagueGuid == g.LeagueGuid);
                    }

                    ltrlGroupMatchInfo.Text = $"<em>{matches.Count(x => x.ResultHome.HasValue && x.ResultAway.HasValue)} / {matches.Count}<em>";
                }
            }
        }

        protected void ddlLeague_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlLeague.SelectedValue))
                ViewState["LeagueGuid"] = ddlLeague.SelectedValue;
            else
                ViewState["LeagueGuid"] = string.Empty;

            LeagueGuid = null;
            gvGroup.PageIndex = 0;

            BindData();
        }

        protected void gvGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvGroup.SelectedIndex != -1)
            {
                var key = gvGroup.DataKeys[gvGroup.SelectedIndex];
                if (key != null)
                    Response.Redirect($"AdminGroupView.aspx?GroupGuid={key.Value}");
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbGroupName.Text.Trim()))
                ViewState["GroupName"] = tbGroupName.Text.Trim();
            else
                ViewState["GroupName"] = string.Empty;

            LeagueGuid = null;
            gvGroup.PageIndex = 0;

            BindData();
        }

        protected void gvGroup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ResetGroupTable")
            {
                try
                {
                    var id = new Guid(e.CommandArgument.ToString());

                    var g = _repo.Single<Group>(id);

                    if (g != null)
                    {
                        g.Statistic();

                        ClientScript.RegisterClientScriptBlock(typeof(string), "success", "alert('统计积分榜成功');", true);
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

            if (e.CommandName == "ResetGroupMatch")
            {
                try
                {
                    var id = new Guid(e.CommandArgument.ToString());

                    var g = _repo.Single<Group>(id);

                    if (g != null)
                    {
                        g.BindMatches();

                        ClientScript.RegisterClientScriptBlock(typeof(string), "success", "alert('绑定比赛成功');", true);
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
        }
    }
}