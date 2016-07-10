using System;
using System.Data;
using System.Web.UI.WebControls;
using Arsenalcn.CasinoSys.Entity;
using Arsenalcn.CasinoSys.Web.Common;
using Arsenalcn.CasinoSys.Web.Control;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class MyCoupon : BasePage
    {
        private bool _itemAvailable;

        protected override void OnInit(EventArgs e)
        {
            AnonymousRedirect = true;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            #region Assign Control Property

            ctrlLeftPanel.UserId = userid;
            ctrlLeftPanel.UserName = username;

            ctrlFieldTooBar.UserId = userid;

            ctrlMenuTabBar.CurrentMenu = CasinoMenuType.CasinoPortal;

            ctrlGamblerHeader.UserId = userid;
            ctrlGamblerHeader.UserName = username;

            #endregion

            if (!IsPostBack)
            {
                var dtMatch = CasinoItem.GetMatchCasinoItemView(true);

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

                btnSubmit.Visible = _itemAvailable;
            }
        }

        protected void gvMatch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var drv = e.Row.DataItem as DataRowView;

                if (drv != null)
                {
                    var m = new Match((Guid)drv["MatchGuid"]);

                    var ltrlLeagueInfo = e.Row.FindControl("ltrlLeagueInfo") as Literal;

                    if (ltrlLeagueInfo != null)
                    {
                        var strLeague =
                            "<a href=\"CasinoGame.aspx?League={0}\" title=\"{1}\"><img src=\"{2}\" alt=\"{1}\" class=\"CasinoSys_CategoryImg\" /></a>";

                        var strLeagueName = $"{m.LeagueName}{(m.Round.HasValue ? $" 第{m.Round}轮" : string.Empty)}";

                        ltrlLeagueInfo.Text = string.Format(strLeague, m.LeagueGuid, strLeagueName,
                            League.Cache.Load(m.LeagueGuid).LeagueLogo);
                    }

                    var lblHome = e.Row.FindControl("lblHome") as Label;
                    var lblAway = e.Row.FindControl("lblAway") as Label;
                    var hlVersus = e.Row.FindControl("hlVersus") as HyperLink;

                    if (lblHome != null && lblAway != null && hlVersus != null)
                    {
                        var tHome = Team.Cache.Load(m.Home);
                        var tAway = Team.Cache.Load(m.Away);

                        var strTeamName =
                            "<a class=\"StrongLink\" href=\"CasinoTeam.aspx?Team={0}\"  title=\"{1}\">{2}</a> ";
                        var strTeamLogo = "<img src=\"{3}\" alt=\"{1}\" /> ";

                        lblHome.Text = string.Format(strTeamName + strTeamLogo,
                            tHome.ID, tHome.TeamEnglishName, tHome.TeamDisplayName, tHome.TeamLogo);
                        lblAway.Text = string.Format(strTeamLogo + strTeamName,
                            tAway.ID, tAway.TeamEnglishName, tAway.TeamDisplayName, tAway.TeamLogo);

                        hlVersus.NavigateUrl = $"CasinoTeam.aspx?Match={m.MatchGuid}";
                        hlVersus.Text =
                            $"<em title=\"{tHome.Ground}{(tHome.Capacity.HasValue ? ("(" + tHome.Capacity.Value + ")") : string.Empty)}\">vs</em>";
                    }

                    var guid = CasinoItem.GetCasinoItemGuidByMatch(m.MatchGuid, CasinoType.SingleChoice);

                    if (guid.HasValue)
                    {
                        var item = CasinoItem.GetCasinoItem(guid.Value);

                        if (item != null)
                        {
                            var options = ((SingleChoice)item).Options;

                            var winOption = options.Find(option => option.OptionValue == MatchChoiceOption.HomeWinValue);
                            var drawOption = options.Find(option => option.OptionValue == MatchChoiceOption.DrawValue);
                            var loseOption = options.Find(option => option.OptionValue == MatchChoiceOption.AwayWinValue);

                            if (!string.IsNullOrEmpty(winOption.OptionValue) &&
                                !string.IsNullOrEmpty(drawOption.OptionValue) &&
                                !string.IsNullOrEmpty(loseOption.OptionValue) &&
                                winOption.OptionRate.HasValue &&
                                drawOption.OptionRate.HasValue &&
                                loseOption.OptionRate.HasValue)
                            {
                                var ltrlWinRate = e.Row.FindControl("ltrlWinRate") as Literal;
                                var ltrlDrawRate = e.Row.FindControl("ltrlDrawRate") as Literal;
                                var ltrlLoseRate = e.Row.FindControl("ltrlLoseRate") as Literal;

                                if (ltrlWinRate != null)
                                    ltrlWinRate.Text = Convert.ToSingle(winOption.OptionRate.Value).ToString("f2");
                                if (ltrlDrawRate != null)
                                    ltrlDrawRate.Text = Convert.ToSingle(drawOption.OptionRate.Value).ToString("f2");
                                if (ltrlLoseRate != null)
                                    ltrlLoseRate.Text = Convert.ToSingle(loseOption.OptionRate.Value).ToString("f2");
                            }
                        }
                    }

                    //bet for match result

                    guid = CasinoItem.GetCasinoItemGuidByMatch(m.MatchGuid, CasinoType.MatchResult);

                    if (guid.HasValue)
                    {
                        var item = CasinoItem.GetCasinoItem(guid.Value);

                        if (item?.ItemGuid != null)
                        {
                            var bets = Bet.GetUserCasinoItemAllBet(userid, item.ItemGuid.Value);

                            if (bets.Count != 0)
                            {
                                var betDetail = new MatchResultBetDetail(BetDetail.GetBetDetailByBetId(bets[0].ID));

                                var tbHomeScore = e.Row.FindControl("tbHomeScore") as TextBox;
                                var tbAwayScore = e.Row.FindControl("tbAwayScore") as TextBox;

                                if (tbHomeScore != null)
                                {
                                    tbHomeScore.Text = betDetail.Home.ToString();
                                    tbHomeScore.ReadOnly = true;
                                    tbHomeScore.Style.Add("border", "none");
                                    tbHomeScore.Style.Add("font-weight", "bold");
                                    tbHomeScore.Style.Add("background", "none");
                                    tbHomeScore.Style.Add("color", "#aa0000");
                                }

                                if (tbAwayScore != null)
                                {
                                    tbAwayScore.Text = betDetail.Away.ToString();
                                    tbAwayScore.ReadOnly = true;
                                    tbAwayScore.Style.Add("border", "none");
                                    tbAwayScore.Style.Add("font-weight", "bold");
                                    tbAwayScore.Style.Add("background", "none");
                                    tbAwayScore.Style.Add("color", "#aa0000");
                                }
                            }
                            else
                            {
                                _itemAvailable = true;
                            }
                        }
                    }
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvMatch.Rows)
            {
                var tbHomeScore = row.FindControl("tbHomeScore") as TextBox;
                var tbAwayScore = row.FindControl("tbAwayScore") as TextBox;

                if (tbHomeScore != null && tbAwayScore != null && gvMatch.DataKeys[row.RowIndex] != null)
                {
                    var matchGuid = (Guid)gvMatch.DataKeys[row.RowIndex].Value;

                    var guid = CasinoItem.GetCasinoItemGuidByMatch(matchGuid, CasinoType.MatchResult);

                    if (guid.HasValue)
                    {
                        var item = CasinoItem.GetCasinoItem(guid.Value);

                        if (item?.ItemGuid != null)
                        {
                            short homeScore, awayScore;

                            if (short.TryParse(tbHomeScore.Text, out homeScore) && short.TryParse(tbAwayScore.Text, out awayScore))
                            {
                                //save
                                if (CasinoItem.GetCasinoItem(guid.Value).CloseTime < DateTime.Now)
                                {
                                    continue;
                                }

                                if (homeScore < 0 || awayScore < 0)
                                {
                                    continue;
                                }

                                var bets = Bet.GetUserCasinoItemAllBet(userid, item.ItemGuid.Value);

                                if (bets.Count > 0)
                                {
                                    //already bet
                                    continue;
                                }

                                try
                                {
                                    var bet = new Bet
                                    {
                                        BetAmount = null,
                                        BetRate = null,
                                        CasinoItemGuid = guid.Value,
                                        UserID = userid,
                                        UserName = username
                                    };

                                    if (bet.BetCheck())
                                    {
                                        var matchResult = new MatchResultBetDetail
                                        {
                                            Home = homeScore,
                                            Away = awayScore
                                        };

                                        bet.Insert(matchResult);
                                    }
                                }
                                catch
                                {
                                    ClientScript.RegisterClientScriptBlock(typeof(string), "failed",
                                        "alert('投注失败'); window.location.href = window.location.href;", true);
                                }
                            }
                        }
                    }
                }
            }

            ClientScript.RegisterClientScriptBlock(typeof(string), "failed",
                "alert('您的投注单已提交'); window.location.href = window.location.href;", true);
        }
    }
}