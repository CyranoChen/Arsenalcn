using System;
using System.Web.UI;
using Arsenalcn.CasinoSys.Entity;

namespace Arsenalcn.CasinoSys.Web.Control
{
    public partial class CasinoHeader : UserControl
    {
        public Team HomeTeam { get; set; }

        public Team AwayTeam { get; set; }

        public Guid MatchGuid { get; set; }

        public bool IsHistoryView { get; set; }

        public int UserId { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (MatchGuid != Guid.Empty)
            {
                var m = new Match(MatchGuid);

                var leagueDisplay = m.LeagueName;
                if (m.Round.HasValue)
                    leagueDisplay += $" 第{m.Round}轮";

                ltrlLeagueSeason.Text = leagueDisplay;

                var homeGuid = m.Home;
                var awayGuid = m.Away;

                HomeTeam = Team.Cache.Load(homeGuid);
                AwayTeam = Team.Cache.Load(awayGuid);

                ltrlPlayTime.Text = m.PlayTime.ToString("yyyy-MM-dd HH:mm");

                var guid = CasinoItem.GetCasinoItemGuidByMatch(MatchGuid, CasinoType.SingleChoice);

                var matchTotalBet = 0f;

                if (guid.HasValue)
                {
                    var item = CasinoItem.GetCasinoItem(guid.Value);

                    if (item != null)
                    {
                        var options = ((SingleChoice) item).Options;

                        var winOption =
                            options.Find(
                                delegate(ChoiceOption option)
                                {
                                    return option.OptionValue == MatchChoiceOption.HomeWinValue;
                                });
                        var drawOption =
                            options.Find(
                                delegate(ChoiceOption option)
                                {
                                    return option.OptionValue == MatchChoiceOption.DrawValue;
                                });
                        var loseOption =
                            options.Find(
                                delegate(ChoiceOption option)
                                {
                                    return option.OptionValue == MatchChoiceOption.AwayWinValue;
                                });

                        if (string.IsNullOrEmpty(winOption.OptionValue) || string.IsNullOrEmpty(drawOption.OptionValue) ||
                            string.IsNullOrEmpty(loseOption.OptionValue))
                            throw new Exception("该比赛没有赔率");
                        var txtString = "<em title=\"共有{0}注，总计:{1}博彩币\">{2}</em>";

                        var rateWin = Math.Round(winOption.OptionRate.Value, 2).ToString("f2");
                        var rateDraw = Math.Round(drawOption.OptionRate.Value, 2).ToString("f2");
                        var rateLose = Math.Round(loseOption.OptionRate.Value, 2).ToString("f2");

                        var totalBetWin =
                            ChoiceOption.GetOptionTotalBet(guid.Value, winOption.OptionValue).ToString("N0");
                        var totalBetDraw =
                            ChoiceOption.GetOptionTotalBet(guid.Value, drawOption.OptionValue).ToString("N0");
                        var totalBetLose =
                            ChoiceOption.GetOptionTotalBet(guid.Value, loseOption.OptionValue).ToString("N0");

                        var betCountWin = ChoiceOption.GetOptionTotalCount(guid.Value, winOption.OptionValue).ToString();
                        var betCountDraw =
                            ChoiceOption.GetOptionTotalCount(guid.Value, drawOption.OptionValue).ToString();
                        var betCountLose =
                            ChoiceOption.GetOptionTotalCount(guid.Value, loseOption.OptionValue).ToString();

                        ltrlWin.Text = string.Format(txtString, betCountWin, totalBetWin, rateWin);
                        ltrlDraw.Text = string.Format(txtString, betCountDraw, totalBetDraw, rateDraw);
                        ltrlLose.Text = string.Format(txtString, betCountLose, totalBetLose, rateLose);

                        matchTotalBet = Convert.ToSingle(totalBetWin) + Convert.ToSingle(totalBetDraw) +
                                        Convert.ToSingle(totalBetLose);
                    }

                    var betList = Bet.GetMatchAllBet(MatchGuid);

                    if (betList != null && betList.Count > 0)
                        ltrlMatchBetCount.Text = betList.Count.ToString();
                    else
                        ltrlMatchBetCount.Text = "0";

                    ltrlMatchTotalBet.Text = matchTotalBet.ToString("N0");

                    if (!IsHistoryView)
                    {
                        pnlHistoryResult.Visible = false;

                        if (item?.Earning != null)
                            ltrlEarning.Text = Convert.ToSingle(item.Earning).ToString("N2");

                        ltrlTopBet.Text = Bet.GetMatchTopBet(MatchGuid).ToString("N0");
                        ltrlTopEarning.Text = Bet.GetMatchTopEarning(MatchGuid).ToString("N2");

                        var matchResultGuid = CasinoItem.GetCasinoItemGuidByMatch(MatchGuid, CasinoType.MatchResult);

                        if (matchResultGuid.HasValue)
                        {
                            var matchResultItem = CasinoItem.GetCasinoItem(matchResultGuid.Value);

                            if (((MatchResult) matchResultItem)?.Home != null &&
                                ((MatchResult) matchResultItem).Away.HasValue)
                            {
                                pnlMatchResult.Visible = true;
                                ltrlMatchResult.Text =
                                    $"{((MatchResult) matchResultItem).Home}:{((MatchResult) matchResultItem).Away}";
                            }
                            else
                            {
                                pnlMatchResult.Visible = false;
                                ltrlMatchResult.Text = "暂无结果";
                                ltrlEarning.Text = "暂无结果";
                            }
                        }
                    }
                    else
                    {
                        pnlHistoryResult.Visible = true;
                        pnlMatchResult.Visible = false;

                        var historyResultArr = CasinoItem.GetHistoryResultByMatch(MatchGuid);

                        ltrlMatchCount.Text = historyResultArr[0].ToString();
                        ltrlHomeWon.Text = historyResultArr[1].ToString();
                        ltrlHomeDraw.Text = historyResultArr[2].ToString();
                        ltrlHomeLost.Text = historyResultArr[3].ToString();
                    }
                }
            }
        }
    }
}