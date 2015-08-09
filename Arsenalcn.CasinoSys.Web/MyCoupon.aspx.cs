using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

using Arsenalcn.CasinoSys.Entity;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class MyCoupon : Common.BasePage
    {
        protected override void OnInit(EventArgs e)
        {
            AnonymousRedirect = true;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            #region Assign Control Property

            ctrlLeftPanel.UserID = userid;
            ctrlLeftPanel.UserName = username;

            ctrlFieldTooBar.UserID = userid;

            ctrlMenuTabBar.CurrentMenu = Arsenalcn.CasinoSys.Web.Control.CasinoMenuType.CasinoPortal;

            ctrlGamblerHeader.UserID = userid;
            ctrlGamblerHeader.UserName = username;

            #endregion

            if (!IsPostBack)
            {
                DataTable dtMatch = Entity.CasinoItem.GetMatchCasinoItemView(true);

                //if (dtMatch != null)
                //{
                //    dtMatch.Columns.Add("League", typeof(string));

                //    foreach (DataRow dr in dtMatch.Rows)
                //    {
                //        string league = dr["LeagueName"].ToString();

                //        //if (!string.IsNullOrEmpty(dr["LeagueSeason"].ToString()))
                //        //    league += dr["LeagueSeason"].ToString();

                //        if (!Convert.IsDBNull(dr["Round"]))
                //            league += string.Format("赛季 第{0}轮", dr["Round"]);

                //        dr["League"] = league;
                //    }
                //}

                gvMatch.DataSource = dtMatch;
                gvMatch.DataBind();

                if (itemAvailable)
                    btnSubmit.Visible = true;
                else
                    btnSubmit.Visible = false;
            }
        }

        private bool itemAvailable = false;

        protected void gvMatch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = e.Row.DataItem as DataRowView;

                Match m = new Match((Guid)drv["MatchGuid"]);

                Literal ltrlLeagueInfo = e.Row.FindControl("ltrlLeagueInfo") as Literal;

                if (ltrlLeagueInfo != null)
                {
                    string _strLeague = "<a href=\"CasinoGame.aspx?League={0}\" title=\"{1}\"><img src=\"{2}\" alt=\"{1}\" class=\"CasinoSys_CategoryImg\" /></a>";

                    string _strLeagueName = string.Format("{0}{1}", m.LeagueName, m.Round.HasValue ?
                        string.Format(" 第{0}轮", m.Round.ToString()) : string.Empty);

                    ltrlLeagueInfo.Text = string.Format(_strLeague, m.LeagueGuid.ToString(), _strLeagueName,
                        League.Cache.Load(m.LeagueGuid).LeagueLogo);
                }

                Label lblHome = e.Row.FindControl("lblHome") as Label;
                Label lblAway = e.Row.FindControl("lblAway") as Label;
                HyperLink hlVersus = e.Row.FindControl("hlVersus") as HyperLink;

                if (lblHome != null && lblAway != null && hlVersus != null)
                {
                    Team tHome = Team.Cache.Load(m.Home);
                    Team tAway = Team.Cache.Load(m.Away);

                    string _strTeamName = "<a class=\"StrongLink\" href=\"CasinoTeam.aspx?Team={0}\"  title=\"{1}\">{2}</a> ";
                    string _strTeamLogo = "<img src=\"{3}\" alt=\"{1}\" /> ";

                    lblHome.Text = string.Format(_strTeamName + _strTeamLogo,
                        tHome.ID.ToString(), tHome.TeamEnglishName, tHome.TeamDisplayName, tHome.TeamLogo);
                    lblAway.Text = string.Format(_strTeamLogo + _strTeamName,
                        tAway.ID.ToString(), tAway.TeamEnglishName, tAway.TeamDisplayName, tAway.TeamLogo);

                    hlVersus.NavigateUrl = string.Format("CasinoTeam.aspx?Match={0}", m.MatchGuid.ToString());
                    hlVersus.Text = string.Format("<em title=\"{0}{1}\">vs</em>", tHome.Ground,
                        tHome.Capacity.HasValue ? ("(" + tHome.Capacity.Value.ToString() + ")") : string.Empty);
                }

                Guid? guid = Entity.CasinoItem.GetCasinoItemGuidByMatch(m.MatchGuid, CasinoType.SingleChoice);

                if (guid.HasValue)
                {
                    Entity.CasinoItem item = Entity.CasinoItem.GetCasinoItem(guid.Value);

                    if (item != null)
                    {
                        List<Entity.ChoiceOption> options = ((Entity.SingleChoice)item).Options;

                        ChoiceOption winOption = options.Find(delegate(ChoiceOption option) { return option.OptionValue == Entity.MatchChoiceOption.HomeWinValue; });
                        ChoiceOption drawOption = options.Find(delegate(ChoiceOption option) { return option.OptionValue == Entity.MatchChoiceOption.DrawValue; });
                        ChoiceOption loseOption = options.Find(delegate(ChoiceOption option) { return option.OptionValue == Entity.MatchChoiceOption.AwayWinValue; });

                        if (!string.IsNullOrEmpty(winOption.OptionValue) && !string.IsNullOrEmpty(drawOption.OptionValue) && !string.IsNullOrEmpty(loseOption.OptionValue))
                        {
                            Literal ltrlWinRate = e.Row.FindControl("ltrlWinRate") as Literal;
                            Literal ltrlDrawRate = e.Row.FindControl("ltrlDrawRate") as Literal;
                            Literal ltrlLoseRate = e.Row.FindControl("ltrlLoseRate") as Literal;

                            ltrlWinRate.Text = Convert.ToSingle(winOption.OptionRate.Value).ToString("f2");
                            ltrlDrawRate.Text = Convert.ToSingle(drawOption.OptionRate.Value).ToString("f2");
                            ltrlLoseRate.Text = Convert.ToSingle(loseOption.OptionRate.Value).ToString("f2");
                        }
                    }
                }

                //bet for match result

                guid = Entity.CasinoItem.GetCasinoItemGuidByMatch(m.MatchGuid, CasinoType.MatchResult);

                if (guid.HasValue)
                {
                    Entity.CasinoItem item = Entity.CasinoItem.GetCasinoItem(guid.Value);

                    if (item != null)
                    {
                        List<Entity.Bet> bets = Entity.Bet.GetUserCasinoItemAllBet(this.userid, item.ItemGuid.Value);

                        if (bets.Count != 0)
                        {
                            MatchResultBetDetail betDetail = new MatchResultBetDetail(Entity.BetDetail.GetBetDetailByBetID(bets[0].ID));

                            TextBox tbHomeScore = e.Row.FindControl("tbHomeScore") as TextBox;
                            TextBox tbAwayScore = e.Row.FindControl("tbAwayScore") as TextBox;

                            tbHomeScore.Text = betDetail.Home.ToString();
                            tbHomeScore.ReadOnly = true;
                            tbHomeScore.Style.Add("border", "none");
                            tbHomeScore.Style.Add("font-weight", "bold");
                            tbHomeScore.Style.Add("background", "none");
                            tbHomeScore.Style.Add("color", "#aa0000");

                            tbAwayScore.Text = betDetail.Away.ToString();
                            tbAwayScore.ReadOnly = true;
                            tbAwayScore.Style.Add("border", "none");
                            tbAwayScore.Style.Add("font-weight", "bold");
                            tbAwayScore.Style.Add("background", "none");
                            tbAwayScore.Style.Add("color", "#aa0000");
                        }
                        else
                        {
                            itemAvailable = true;
                        }
                    }
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvMatch.Rows)
            {
                TextBox tbHomeScore = row.FindControl("tbHomeScore") as TextBox;
                TextBox tbAwayScore = row.FindControl("tbAwayScore") as TextBox;

                if (tbHomeScore != null && tbAwayScore != null)
                {
                    Guid matchGuid = (Guid)gvMatch.DataKeys[row.RowIndex].Value;

                    Guid? guid = Entity.CasinoItem.GetCasinoItemGuidByMatch(matchGuid, CasinoType.MatchResult);

                    if (guid.HasValue)
                    {
                        Entity.CasinoItem item = Entity.CasinoItem.GetCasinoItem(guid.Value);

                        if (item != null)
                        {
                            List<Entity.Bet> bets = Entity.Bet.GetUserCasinoItemAllBet(this.userid, item.ItemGuid.Value);

                            if (bets.Count != 0)
                            {
                                //already bet
                                continue;
                            }
                            else
                            {
                                short homeScore, awayScore;
                                if (short.TryParse(tbHomeScore.Text, out homeScore) && short.TryParse(tbAwayScore.Text, out awayScore))
                                {
                                    //save
                                    if (Entity.CasinoItem.GetCasinoItem(guid.Value).CloseTime < DateTime.Now)
                                    {
                                        continue;
                                    }

                                    if (homeScore < 0 || awayScore < 0)
                                    {
                                        continue;
                                    }

                                    try
                                    {
                                        Bet bet = new Bet();
                                        bet.BetAmount = null;
                                        bet.BetRate = null;
                                        bet.CasinoItemGuid = guid.Value;
                                        bet.UserID = this.userid;
                                        bet.UserName = this.username;

                                        if (bet.BetCheck())
                                        {
                                            MatchResultBetDetail matchResult = new Entity.MatchResultBetDetail();
                                            matchResult.Home = homeScore;
                                            matchResult.Away = awayScore;

                                            bet.Insert(matchResult);
                                        }
                                        else
                                            continue;
                                    }
                                    catch
                                    {
                                        this.ClientScript.RegisterClientScriptBlock(typeof(string), "failed", "alert('投注失败'); window.location.href = window.location.href;", true);
                                    }
                                }
                                else
                                    continue;
                            }
                        }
                    }
                }
            }

            this.ClientScript.RegisterClientScriptBlock(typeof(string), "failed", "alert('您的投注单已提交'); window.location.href = window.location.href;", true);
        }
    }
}
