using System;
using System.Data;
using System.Collections.Generic;

using Arsenalcn.CasinoSys.Entity;
using ArsenalTeam = Arsenalcn.CasinoSys.Entity.Arsenal.Team;

namespace Arsenalcn.CasinoSys.Web.Control
{
    public partial class CasinoHeader : System.Web.UI.UserControl
    {
        public ArsenalTeam HomeTeam
        { get; set; }

        public ArsenalTeam AwayTeam
        { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (MatchGuid != Guid.Empty)
            {
                Match m = new Match(MatchGuid);

                string leagueDisplay = m.LeagueName.ToString();
                if (m.Round.HasValue)
                    leagueDisplay += string.Format(" 第{0}轮", m.Round.ToString());

                ltrlLeagueSeason.Text = leagueDisplay;

                Guid homeGuid = m.Home;
                Guid awayGuid = m.Away;

                HomeTeam = Arsenal_Team.Cache.Load(homeGuid);
                AwayTeam = Arsenal_Team.Cache.Load(awayGuid);

                ltrlPlayTime.Text = m.PlayTime.ToString("yyyy-MM-dd HH:mm");

                Guid? guid = Entity.CasinoItem.GetCasinoItemGuidByMatch(MatchGuid, CasinoItem.CasinoType.SingleChoice);

                float matchTotalBet = 0f;

                if (guid.HasValue)
                {
                    Entity.CasinoItem item = Entity.CasinoItem.GetCasinoItem(guid.Value);

                    if (item != null)
                    {
                        List<Entity.ChoiceOption> options = ((Entity.SingleChoice)item).Options;

                        ChoiceOption winOption = options.Find(delegate(ChoiceOption option) { return option.OptionValue == MatchChoiceOption.HomeWinValue; });
                        ChoiceOption drawOption = options.Find(delegate(ChoiceOption option) { return option.OptionValue == MatchChoiceOption.DrawValue; });
                        ChoiceOption loseOption = options.Find(delegate(ChoiceOption option) { return option.OptionValue == MatchChoiceOption.AwayWinValue; });

                        if (string.IsNullOrEmpty(winOption.OptionValue) || string.IsNullOrEmpty(drawOption.OptionValue) || string.IsNullOrEmpty(loseOption.OptionValue))
                            throw new Exception("该比赛没有赔率");
                        else
                        {
                            string txtString = "<em title=\"共有{0}注，总计:{1}博彩币\">{2}</em>";

                            string rateWin = Math.Round(winOption.OptionRate.Value, 2).ToString("f2");
                            string rateDraw = Math.Round(drawOption.OptionRate.Value, 2).ToString("f2");
                            string rateLose = Math.Round(loseOption.OptionRate.Value, 2).ToString("f2");

                            string totalBetWin = Entity.ChoiceOption.GetOptionTotalBet(guid.Value, winOption.OptionValue).ToString("N0");
                            string totalBetDraw = Entity.ChoiceOption.GetOptionTotalBet(guid.Value, drawOption.OptionValue).ToString("N0");
                            string totalBetLose = Entity.ChoiceOption.GetOptionTotalBet(guid.Value, loseOption.OptionValue).ToString("N0");

                            string betCountWin = Entity.ChoiceOption.GetOptionTotalCount(guid.Value, winOption.OptionValue).ToString();
                            string betCountDraw = Entity.ChoiceOption.GetOptionTotalCount(guid.Value, drawOption.OptionValue).ToString();
                            string betCountLose = Entity.ChoiceOption.GetOptionTotalCount(guid.Value, loseOption.OptionValue).ToString();

                            ltrlWin.Text = string.Format(txtString, betCountWin, totalBetWin, rateWin);
                            ltrlDraw.Text = string.Format(txtString, betCountDraw, totalBetDraw, rateDraw);
                            ltrlLose.Text = string.Format(txtString, betCountLose, totalBetLose, rateLose);

                            matchTotalBet = Convert.ToSingle(totalBetWin) + Convert.ToSingle(totalBetDraw) + Convert.ToSingle(totalBetLose);
                        }
                    }

                    List<Bet> betList = Entity.Bet.GetMatchAllBet(MatchGuid);

                    if (betList != null && betList.Count > 0)
                        ltrlMatchBetCount.Text = betList.Count.ToString();
                    else
                        ltrlMatchBetCount.Text = "0";

                    ltrlMatchTotalBet.Text = matchTotalBet.ToString("N0");

                    if (!IsHistoryView)
                    {
                        pnlHistoryResult.Visible = false;

                        if (item.Earning != null)
                            ltrlEarning.Text = Convert.ToSingle(item.Earning).ToString("N2");

                        ltrlTopBet.Text = Entity.Bet.GetMatchTopBet(MatchGuid).ToString("N0");
                        ltrlTopEarning.Text = Entity.Bet.GetMatchTopEarning(MatchGuid).ToString("N2");

                        Guid? matchResultGuid = Entity.CasinoItem.GetCasinoItemGuidByMatch(MatchGuid, CasinoItem.CasinoType.MatchResult);

                        if (matchResultGuid.HasValue)
                        {
                            CasinoItem matchResultItem = Entity.CasinoItem.GetCasinoItem(matchResultGuid.Value);

                            if (matchResultItem != null && ((MatchResult)matchResultItem).Home.HasValue && ((MatchResult)matchResultItem).Away.HasValue)
                            {
                                pnlMatchResult.Visible = true;
                                ltrlMatchResult.Text = string.Format("{0}:{1}", ((MatchResult)matchResultItem).Home, ((MatchResult)matchResultItem).Away);
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

                        int[] historyResultArr = Entity.CasinoItem.GetHistoryResultByMatch(MatchGuid);

                        ltrlMatchCount.Text = historyResultArr[0].ToString();
                        ltrlHomeWon.Text = historyResultArr[1].ToString();
                        ltrlHomeDraw.Text = historyResultArr[2].ToString();
                        ltrlHomeLost.Text = historyResultArr[3].ToString();
                    }
                }
            }
        }

        public Guid MatchGuid
        {
            get;
            set;
        }

        public bool IsHistoryView
        {
            get;
            set;
        }

        public int UserID
        { get; set; }
    }
}