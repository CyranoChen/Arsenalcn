using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

using Arsenalcn.CasinoSys.Entity;

using Discuz.Forum;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class CasinoPortal : Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Assign Control Property

            ctrlLeftPanel.UserID = userid;
            ctrlLeftPanel.UserName = username;

            ctrlFieldTooBar.UserID = userid;

            ctrlMenuTabBar.CurrentMenu = Arsenalcn.CasinoSys.Web.Control.CasinoMenuType.CasinoPortal;

            #endregion

            if (userid == -1)
            {
                gvMatch.Columns[gvMatch.Columns.Count - 1].Visible = false;
            }
            else if (CurrentGambler.Cash <= 0)
            {
                gvMatch.Columns[gvMatch.Columns.Count - 1].Visible = false;
            }

            DataTable dtMatch = Entity.CasinoItem.GetMatchCasinoItemView(true);

            if (dtMatch != null)
            {
                dtMatch.Columns.Add("League", typeof(string));
                foreach (DataRow dr in dtMatch.Rows)
                {
                    string league = dr["LeagueName"].ToString();

                    if (!string.IsNullOrEmpty(dr["LeagueSeason"].ToString()))
                        league += dr["LeagueSeason"].ToString();

                    if (!Convert.IsDBNull(dr["Round"]))
                        league += string.Format("赛季 第{0}轮", dr["Round"]);

                    dr["League"] = league;
                }
            }

            gvMatch.DataSource = dtMatch;
            gvMatch.DataBind();
        }

        protected void gvMatch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = e.Row.DataItem as DataRowView;

                Guid matchGuid = (Guid)drv["MatchGuid"];

                Guid? guid = Entity.CasinoItem.GetCasinoItemGuidByMatch(matchGuid, CasinoItem.CasinoType.SingleChoice);

                if (guid.HasValue)
                {
                    CasinoItem item = Entity.CasinoItem.GetCasinoItem(guid.Value);

                    if (item != null)
                    {
                        List<ChoiceOption> options = ((SingleChoice)item).Options;

                        ChoiceOption winOption = options.Find(delegate(ChoiceOption option) { return option.OptionValue == MatchChoiceOption.HomeWinValue; });
                        ChoiceOption drawOption = options.Find(delegate(ChoiceOption option) { return option.OptionValue == MatchChoiceOption.DrawValue; });
                        ChoiceOption loseOption = options.Find(delegate(ChoiceOption option) { return option.OptionValue == MatchChoiceOption.AwayWinValue; });

                        if (string.IsNullOrEmpty(winOption.OptionValue) || string.IsNullOrEmpty(drawOption.OptionValue) || string.IsNullOrEmpty(loseOption.OptionValue))
                            throw new Exception();
                        else
                        {
                            Literal ltrlWinRate = e.Row.FindControl("ltrlWinRate") as Literal;
                            Literal ltrlDrawRate = e.Row.FindControl("ltrlDrawRate") as Literal;
                            Literal ltrlLoseRate = e.Row.FindControl("ltrlLoseRate") as Literal;

                            ltrlWinRate.Text = string.Format("<em title=\"主队胜赔率\">{0}</em>", Convert.ToSingle(winOption.OptionRate.Value).ToString("f2"));
                            ltrlDrawRate.Text = string.Format("<em title=\"双方平赔率\">{0}</em>", Convert.ToSingle(drawOption.OptionRate.Value).ToString("f2"));
                            ltrlLoseRate.Text = string.Format("<em title=\"客队胜赔率\">{0}</em>", Convert.ToSingle(loseOption.OptionRate.Value).ToString("f2"));

                            Label lbWinInfo = e.Row.FindControl("lbWinInfo") as Label;
                            Label lbDrawInfo = e.Row.FindControl("lbDrawInfo") as Label;
                            Label lbLoseInfo = e.Row.FindControl("lbLoseInfo") as Label;

                            lbWinInfo.Text = string.Format("{0} | {1}", ChoiceOption.GetOptionTotalCount(guid.Value, winOption.OptionValue).ToString(), ChoiceOption.GetOptionTotalBet(guid.Value, winOption.OptionValue).ToString("N0"));
                            lbDrawInfo.Text = string.Format("{0} | {1}", ChoiceOption.GetOptionTotalCount(guid.Value, drawOption.OptionValue).ToString(), ChoiceOption.GetOptionTotalBet(guid.Value, drawOption.OptionValue).ToString("N0"));
                            lbLoseInfo.Text = string.Format("{0} | {1}", ChoiceOption.GetOptionTotalCount(guid.Value, loseOption.OptionValue).ToString(), ChoiceOption.GetOptionTotalBet(guid.Value, loseOption.OptionValue).ToString("N0"));

                            //Literal ltrlWinTotalBet = e.Row.FindControl("ltrlWinTotalBet") as Literal;
                            //Literal ltrlDrawTotalBet = e.Row.FindControl("ltrlDrawTotalBet") as Literal;
                            //Literal ltrlLoseTotalBet = e.Row.FindControl("ltrlLoseTotalBet") as Literal;

                            //ltrlWinTotalBet.Text = Convert.ToSingle(Entity.ChoiceOption.GetOptionTotalBet(guid.Value, winOption.OptionValue)).ToString("N0");
                            //ltrlDrawTotalBet.Text = Convert.ToSingle(Entity.ChoiceOption.GetOptionTotalBet(guid.Value, drawOption.OptionValue)).ToString("N0");
                            //ltrlLoseTotalBet.Text = Convert.ToSingle(Entity.ChoiceOption.GetOptionTotalBet(guid.Value, loseOption.OptionValue)).ToString("N0");

                            //Literal ltrlWinBetCount = e.Row.FindControl("ltrlWinBetCount") as Literal;
                            //Literal ltrlDrawBetCount = e.Row.FindControl("ltrlDrawBetCount") as Literal;
                            //Literal ltrlLoseBetCount = e.Row.FindControl("ltrlLoseBetCount") as Literal;

                            //ltrlWinBetCount.Text = Entity.ChoiceOption.GetOptionTotalCount(guid.Value, winOption.OptionValue).ToString();
                            //ltrlDrawBetCount.Text = Entity.ChoiceOption.GetOptionTotalCount(guid.Value, drawOption.OptionValue).ToString();
                            //ltrlLoseBetCount.Text = Entity.ChoiceOption.GetOptionTotalCount(guid.Value, loseOption.OptionValue).ToString();

                            HyperLink btnBet = e.Row.FindControl("btnBet") as HyperLink;

                            if (btnBet != null)
                            {
                                List<Bet> betList = Entity.Bet.GetUserMatchAllBet(this.userid, matchGuid);
                                int betCount = int.MinValue;

                                if (betList != null && betList.Count > 0)
                                    betCount = betList.Count;
                                else
                                    betCount = 0;

                                btnBet.Text = string.Format("投注 <span class=\"CasinoSys_BetInfo\">{0} | {1}</span>", betCount.ToString(), Bet.GetUserMatchTotalBet(this.userid, matchGuid).ToString("N0"));
                                btnBet.NavigateUrl = string.Format("CasinoGameBet.aspx?Match={0}", matchGuid.ToString());
                            }
                            else
                            {
                                btnBet.Visible = false;
                            }

                            // Adv Bodog Bet Button

                            HyperLink btnBetBodog = e.Row.FindControl("btnBet_Bodog") as HyperLink;

                            //管理人员或100积分以上会员看不到广告
                            if (btnBetBodog == null || this.useradminid > 0 || this.usergroupid > 12)
                            {
                                btnBetBodog.Visible = false;
                            }

                            if (btnBetBodog != null && this.CurrentGambler != null)
                            {
                                btnBetBodog.Text = "博狗投注 <span class=\"CasinoSys_BetInfo\">Bodog</span>";
                                btnBetBodog.NavigateUrl = "http://record.slk61.com/_S9AEnQlJCqtPt_LV3gWenWNd7ZgqdRLk/68/?tc=olm36230";
                                btnBetBodog.Target = "_blank";
                            }

                        }
                    }
                }
            }
        }
    }
}
