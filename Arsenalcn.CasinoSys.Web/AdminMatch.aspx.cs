using System;
using System.Data;
using System.Web.UI.WebControls;
using Arsenalcn.CasinoSys.Entity;
using Arsenalcn.CasinoSys.Web.Common;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class AdminMatch : AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = username;

            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            var dt = CasinoItem.GetMatchCasinoItemView(false);

            dt.Columns.Add("HomeDisplay", typeof(string));
            dt.Columns.Add("AwayDisplay", typeof(string));

            foreach (DataRow dr in dt.Rows)
            {
                var m = new Match((Guid)dr["MatchGuid"]);

                dr["HomeDisplay"] = Team.Cache.Load(m.Home).TeamDisplayName;
                dr["AwayDisplay"] = Team.Cache.Load(m.Away).TeamDisplayName;
            }

            gvMatch.DataSource = dt;
            gvMatch.DataBind();
        }

        protected void gvMatch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Match m = e.Row.DataItem as Match;
                var drv = e.Row.DataItem as DataRowView;

                if (drv != null)
                {
                    var m = new Match((Guid)drv["MatchGuid"]);

                    var matchResult = e.Row.FindControl("ltrlMatchResult") as Literal;
                    var btnCalcBonus = e.Row.FindControl("btnCalcBonus") as LinkButton;
                    var btnReturnBet = e.Row.FindControl("btnReturnBet") as LinkButton;
                    var lblBonus = e.Row.FindControl("lblBonus") as Label;
                    var tbPlayTime = e.Row.FindControl("tbPlayTime") as TextBox;
                    var tbRound = e.Row.FindControl("tbRound") as TextBox;
                    var tbHome = e.Row.FindControl("tbHome") as TextBox;
                    var tbAway = e.Row.FindControl("tbAway") as TextBox;

                    if (tbPlayTime != null)
                        tbPlayTime.Text = m.PlayTime.ToString("yyyy-MM-dd HH:mm");

                    if (tbRound != null)
                        tbRound.Text = m.Round.ToString();

                    if (tbHome != null && tbAway != null && m.ResultHome.HasValue && m.ResultAway.HasValue)
                    {
                        tbHome.Text = m.ResultHome.ToString();
                        tbAway.Text = m.ResultAway.ToString();
                    }

                    if (btnCalcBonus != null && btnReturnBet != null && lblBonus != null)
                    {
                        if (matchResult != null && m.ResultHome.HasValue && m.ResultAway.HasValue)
                        {
                            matchResult.Text = $"{m.ResultHome}：{m.ResultAway}";

                            var matchGuid = m.MatchGuid;

                            var itemGuid = CasinoItem.GetCasinoItemGuidByMatch(matchGuid, CasinoType.SingleChoice);

                            if (itemGuid.HasValue)
                            {
                                var item = CasinoItem.GetCasinoItem(itemGuid.Value);

                                if (item.Earning.HasValue)
                                {
                                    //hide button calc bonus
                                    btnCalcBonus.Visible = false;
                                    btnReturnBet.Visible = false;
                                    lblBonus.Text = item.Earning.Value.ToString("N2");
                                    lblBonus.Visible = true;
                                }
                                else
                                {
                                    //show button calc bonus                        
                                    btnCalcBonus.Visible = true;
                                    btnReturnBet.Visible = false;
                                    lblBonus.Visible = false;

                                    //set button command argument
                                    btnCalcBonus.CommandArgument = m.MatchGuid.ToString();
                                }
                            }
                        }
                        else
                        {
                            var betList = Bet.GetMatchAllBet(m.MatchGuid);

                            var betCount = 0;
                            if (betList != null && betList.Count > 0)
                                betCount = betList.Count;

                            btnReturnBet.Text = $"退还:{betCount}注";
                            btnReturnBet.Visible = true;

                            btnCalcBonus.Visible = false;
                            lblBonus.Visible = false;

                            btnReturnBet.CommandArgument = m.MatchGuid.ToString();

                            if (betCount <= 0)
                            {
                                btnReturnBet.Enabled = false;
                            }
                        }
                    }
                }
            }
        }

        protected void gvMatch_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var tbHome = gvMatch.Rows[gvMatch.EditIndex].FindControl("tbHome") as TextBox;
            var tbAway = gvMatch.Rows[gvMatch.EditIndex].FindControl("tbAway") as TextBox;
            var tbPlayTime = gvMatch.Rows[gvMatch.EditIndex].FindControl("tbPlayTime") as TextBox;
            var tbRound = gvMatch.Rows[gvMatch.EditIndex].FindControl("tbRound") as TextBox;

            var key = gvMatch.DataKeys[gvMatch.EditIndex];


            if (tbHome != null && tbAway != null && tbPlayTime != null && tbRound != null && key != null)
            {
                try
                {
                    var guid = (Guid)key.Value;

                    var m = new Match(guid);

                    short rHome;
                    short rAway;
                    if (short.TryParse(tbHome.Text, out rHome) && short.TryParse(tbAway.Text, out rAway))
                    {
                        m.ResultHome = rHome;
                        m.ResultAway = rAway;
                    }
                    else if (string.IsNullOrEmpty(tbHome.Text) && string.IsNullOrEmpty(tbAway.Text))
                    {
                        m.ResultHome = null;
                        m.ResultAway = null;
                    }

                    m.PlayTime = Convert.ToDateTime(tbPlayTime.Text);

                    if (!string.IsNullOrEmpty(tbRound.Text))
                        m.Round = Convert.ToInt16(tbRound.Text);

                    m.Update();

                    var casinoItemGuid = CasinoItem.GetCasinoItemGuidByMatch(guid, CasinoType.MatchResult);

                    if (casinoItemGuid.HasValue && m.ResultHome.HasValue && m.ResultAway.HasValue)
                    {
                        Match.UpdateMatchResult(casinoItemGuid.Value, m.ResultHome.Value, m.ResultAway.Value);
                    }

                    if (casinoItemGuid.HasValue)
                        CasinoItem.UpdateCasinoItemCloseTime(guid, m.PlayTime);
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}');", true);
                }
            }

            gvMatch.EditIndex = -1;

            BindData();
        }

        protected void gvMatch_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var key = gvMatch.DataKeys[e.RowIndex];

            if (key != null)
            {
                try
                {
                    var m = new Match((Guid)key.Value);
                    m.Delete();
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}');", true);
                }
            }

            BindData();
        }


        protected void gvMatch_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvMatch.EditIndex = e.NewEditIndex;

            BindData();
        }

        protected void gvMatch_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvMatch.EditIndex = -1;

            BindData();
        }

        protected void gvMatch_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ReturnBet")
                {
                    var guid = new Guid(e.CommandArgument.ToString());

                    var m = new Match(guid);
                    m.ReturnBet();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "success", "alert('投注退还成功');", true);

                    BindData();
                }
                else if (e.CommandName == "CalcBonus")
                {
                    var guid = new Guid(e.CommandArgument.ToString());

                    var m = new Match(guid);
                    m.CalcBonus();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "success", "alert('奖金发放成功');", true);

                    BindData();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}');", true);
            }
        }

        protected void gvMatch_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMatch.PageIndex = e.NewPageIndex;

            BindData();
        }
    }
}