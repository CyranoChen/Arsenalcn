using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

using Arsenalcn.CasinoSys.Entity;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class AdminMatch : Common.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = this.username;

            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            DataTable dt = Entity.CasinoItem.GetMatchCasinoItemView(false);
            DataView dv = dt.DefaultView;

            gvMatch.DataSource = dv;
            gvMatch.DataBind();
        }

        protected void gvMatch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Match m = e.Row.DataItem as Match;
                DataRowView drv = e.Row.DataItem as DataRowView;
                Match m = new Match((Guid)drv["MatchGuid"]);

                Literal matchResult = e.Row.FindControl("ltrlMatchResult") as Literal;
                LinkButton btnCalcBonus = e.Row.FindControl("btnCalcBonus") as LinkButton;
                LinkButton btnReturnBet = e.Row.FindControl("btnReturnBet") as LinkButton;
                Label lblBonus = e.Row.FindControl("lblBonus") as Label;
                TextBox tbPlayTime = e.Row.FindControl("tbPlayTime") as TextBox;
                TextBox tbRound = e.Row.FindControl("tbRound") as TextBox;
                TextBox tbHome = e.Row.FindControl("tbHome") as TextBox;
                TextBox tbAway = e.Row.FindControl("tbAway") as TextBox;

                if (tbPlayTime != null)
                    tbPlayTime.Text = m.PlayTime.ToString("yyyy-MM-dd HH:mm");

                if (tbRound != null)
                    tbRound.Text = m.Round.ToString();

                if (tbHome != null && tbAway != null && m.ResultHome.HasValue && m.ResultAway.HasValue)
                {
                    tbHome.Text = m.ResultHome.ToString();
                    tbAway.Text = m.ResultAway.ToString();
                }

                if (matchResult != null && m.ResultHome.HasValue && m.ResultAway.HasValue)
                {
                    matchResult.Text = string.Format("{0}：{1}", m.ResultHome.ToString(), m.ResultAway.ToString());

                    Guid matchGuid = m.MatchGuid;

                    Guid? itemGuid = Entity.CasinoItem.GetCasinoItemGuidByMatch(matchGuid, CasinoItem.CasinoType.SingleChoice);

                    if (itemGuid.HasValue)
                    {
                        Entity.CasinoItem item = Entity.CasinoItem.GetCasinoItem(itemGuid.Value);

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
                            if (btnCalcBonus != null)
                                btnCalcBonus.CommandArgument = m.MatchGuid.ToString();
                        }
                    }
                }
                else
                {
                    List<Entity.Bet> betList = Entity.Bet.GetMatchAllBet(m.MatchGuid);

                    int betCount = 0;
                    if (betList != null && betList.Count > 0)
                        betCount = betList.Count;

                    btnReturnBet.Text = string.Format("退还:{0}注", betCount.ToString());
                    btnReturnBet.Visible = true;

                    btnCalcBonus.Visible = false;
                    lblBonus.Visible = false;

                    if (btnReturnBet != null)
                        btnReturnBet.CommandArgument = m.MatchGuid.ToString();

                    if (betCount <= 0)
                    {
                        btnReturnBet.Enabled = false;
                    }
                }
            }
        }

        protected void gvMatch_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            TextBox tbHome = gvMatch.Rows[gvMatch.EditIndex].FindControl("tbHome") as TextBox;
            TextBox tbAway = gvMatch.Rows[gvMatch.EditIndex].FindControl("tbAway") as TextBox;
            TextBox tbPlayTime = gvMatch.Rows[gvMatch.EditIndex].FindControl("tbPlayTime") as TextBox;
            TextBox tbRound = gvMatch.Rows[gvMatch.EditIndex].FindControl("tbRound") as TextBox;

            if (tbHome != null && tbAway != null && tbPlayTime != null && tbRound != null)
            {
                try
                {
                    Guid guid = (Guid)gvMatch.DataKeys[gvMatch.EditIndex].Value;

                    Match m = new Match(guid);

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

                    Guid? casinoItemGuid = Entity.CasinoItem.GetCasinoItemGuidByMatch(guid, CasinoItem.CasinoType.MatchResult);

                    if (casinoItemGuid.HasValue && m.ResultHome.HasValue && m.ResultAway.HasValue)
                    {
                        Entity.Match.UpdateMatchResult(casinoItemGuid.Value, m.ResultHome.Value, m.ResultAway.Value);
                    }

                    if (casinoItemGuid.HasValue && !string.IsNullOrEmpty(m.PlayTime.ToString()))
                        Entity.CasinoItem.UpdateCasinoItemCloseTime(guid, m.PlayTime);
                }
                catch (Exception ex)
                {
                    this.ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}');", ex.Message.ToString()), true);
                }
            }

            gvMatch.EditIndex = -1;

            BindData();
        }

        protected void gvMatch_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Guid guid = (Guid)gvMatch.DataKeys[e.RowIndex].Value;
            try
            {
                Match m = new Match(guid);
                m.Delete();
            }
            catch (Exception ex)
            {
                this.ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}');", ex.Message.ToString()), true);
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
                    Guid guid = new Guid(e.CommandArgument.ToString());

                    Match m = new Match(guid);
                    m.ReturnBet();

                    this.ClientScript.RegisterClientScriptBlock(typeof(string), "success", "alert('投注退还成功');", true);

                }
                else if (e.CommandName == "CalcBonus")
                {
                    Guid guid = new Guid(e.CommandArgument.ToString());

                    Match m = new Match(guid);
                    m.CalcBonus();

                    this.ClientScript.RegisterClientScriptBlock(typeof(string), "success", "alert('奖金发放成功');", true);
                }
            }
            catch (Exception ex)
            {
                this.ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}');", ex.Message.ToString()), true);
            }
            finally
            {
                BindData();
            }
        }

        protected void gvMatch_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMatch.PageIndex = e.NewPageIndex;

            BindData();
        }
    }
}
