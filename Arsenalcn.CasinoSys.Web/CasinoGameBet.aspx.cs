using System;
using System.Data;
using System.Web.UI.WebControls;
using Arsenalcn.CasinoSys.Entity;
using Arsenalcn.CasinoSys.Web.Common;
using Arsenalcn.CasinoSys.Web.Control;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class CasinoGameBet : BasePage
    {
        public Guid CurrentMatch
        {
            get
            {
                try
                {
                    return new Guid(Request.QueryString["Match"]);
                }
                catch
                {
                    Response.Redirect("CasinoPortal.aspx");

                    return new Guid();
                }
            }
        }

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

            ctrlCasinoHeader.UserId = userid;
            ctrlCasinoHeader.MatchGuid = CurrentMatch;
            ctrlCasinoHeader.IsHistoryView = true;

            #endregion

            var match = new Match(CurrentMatch);

            if (match != null)
            {
                var dtPlay = match.PlayTime;

                if (DateTime.Now >= dtPlay)
                {
                    phBet.Visible = false;
                    pnlClose.Visible = true;
                }
                else
                {
                    phBet.Visible = true;
                    pnlClose.Visible = false;
                }

                var guid = CasinoItem.GetCasinoItemGuidByMatch(CurrentMatch, CasinoType.SingleChoice);

                if (!IsPostBack)
                {
                    #region 设置其他可投注比赛的下拉框

                    var dtMatch = CasinoItem.GetMatchCasinoItemView(true);

                    if (dtMatch != null)
                    {
                        dtMatch.Columns.Add("MatchTeamDisplay", typeof (string));

                        foreach (DataRow drM in dtMatch.Rows)
                        {
                            var m = new Match((Guid) drM["MatchGuid"]);

                            drM["MatchTeamDisplay"] =
                                $"{Team.Cache.Load(m.Home).TeamDisplayName} vs {Team.Cache.Load(m.Away).TeamDisplayName}";
                        }
                    }

                    ddlCasinoGame.DataSource = dtMatch;
                    ddlCasinoGame.DataTextField = "MatchTeamDisplay";
                    ddlCasinoGame.DataValueField = "MatchGuid";
                    ddlCasinoGame.DataBind();

                    ddlCasinoGame.Items.Insert(0, new ListItem("--选择其他比赛场次--", Guid.Empty.ToString()));

                    #endregion

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

                            if (string.IsNullOrEmpty(winOption.OptionValue) ||
                                string.IsNullOrEmpty(drawOption.OptionValue) ||
                                string.IsNullOrEmpty(loseOption.OptionValue))
                                throw new Exception();
                            var liWin = rblSingleChoice.Items[0];
                            liWin.Text = $"<em>主队胜({Convert.ToSingle(winOption.OptionRate).ToString("f2")})</em>";
                            liWin.Value = MatchChoiceOption.HomeWinValue;

                            var liDraw = rblSingleChoice.Items[1];
                            liDraw.Text = $"<em>双方平({Convert.ToSingle(drawOption.OptionRate).ToString("f2")})</em>";
                            liDraw.Value = MatchChoiceOption.DrawValue;

                            var liLose = rblSingleChoice.Items[2];
                            liLose.Text = $"<em>客队胜({Convert.ToSingle(loseOption.OptionRate).ToString("f2")})</em>";
                            liLose.Value = MatchChoiceOption.AwayWinValue;
                        }
                    }
                }

                BindData();
            }
            else
                Response.Redirect("CasinoPortal.aspx");
        }

        private void BindData()
        {
            var betList = Bet.GetUserMatchAllBet(userid, CurrentMatch);

            var matchResultGuid = CasinoItem.GetCasinoItemGuidByMatch(CurrentMatch, CasinoType.MatchResult);

            if (matchResultGuid.HasValue &&
                betList.Exists(delegate(Bet bet) { return bet.CasinoItemGuid == matchResultGuid.Value; }))
            {
                trMatchResult.Visible = false;
            }
            else if (!matchResultGuid.HasValue)
            {
                trMatchResult.Visible = false;
            }
            else
            {
                trMatchResult.Visible = true;
            }

            gvBet.DataSource = betList;
            gvBet.DataBind();

            var dtHistory = CasinoItem.GetHistoryViewByMatch(CurrentMatch);

            gvMatch.DataSource = dtHistory;
            gvMatch.DataBind();
        }

        protected void ddlCasinoGame_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((ddlCasinoGame.SelectedIndex != 0) && (ddlCasinoGame.SelectedValue != Guid.Empty.ToString()))
            {
                Response.Redirect("CasinoGameBet.aspx?Match=" + ddlCasinoGame.SelectedValue);
            }
        }

        protected void btnMatchResult_Click(object sender, EventArgs e)
        {
            try
            {
                var guid = CasinoItem.GetCasinoItemGuidByMatch(CurrentMatch, CasinoType.MatchResult);

                if (guid.HasValue)
                {
                    if (CasinoItem.GetCasinoItem(guid.Value).CloseTime < DateTime.Now)
                    {
                        throw new Exception("已超出投注截止时间");
                    }

                    if (Bet.GetUserCasinoItemAllBet(userid, guid.Value).Count > 0)
                    {
                        throw new Exception("已经投过此注，不能重复猜比分");
                    }

                    var bet = new Bet();
                    bet.BetAmount = null;
                    bet.BetRate = null;
                    bet.CasinoItemGuid = guid.Value;
                    bet.UserID = userid;
                    bet.UserName = username;

                    var matchResult = new MatchResultBetDetail();
                    matchResult.Home = Convert.ToInt16(tbHome.Text);
                    matchResult.Away = Convert.ToInt16(tbAway.Text);

                    bet.Insert(matchResult);

                    ClientScript.RegisterClientScriptBlock(typeof (string), "succeed",
                        "alert('投注成功'); window.location.href = window.location.href;", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof (string), "failed", $"alert('{ex.Message}')", true);
            }
        }

        protected void btnSingleChoice_Click(object sender, EventArgs e)
        {
            try
            {
                var guid = CasinoItem.GetCasinoItemGuidByMatch(CurrentMatch, CasinoType.SingleChoice);

                if (guid.HasValue)
                {
                    if (CasinoItem.GetCasinoItem(guid.Value).CloseTime < DateTime.Now)
                    {
                        throw new Exception("已超出投注截止时间");
                    }

                    //Gambler in Lower could not bet above the SingleBetLimit of DefaultLeague (Contest)
                    var m = new Match(CurrentMatch);

                    if (m.LeagueGuid.Equals(ConfigGlobal.DefaultLeagueID))
                    {
                        if (Gambler.GetGamblerTotalBetByUserID(userid, m.LeagueGuid) < ConfigGlobal.TotalBetStandard)
                        {
                            var alreadyMatchBet = Bet.GetUserMatchTotalBet(userid, CurrentMatch);
                            float currentMatchBet;

                            if (!string.IsNullOrEmpty(tbBet.Text.Trim()) &&
                                float.TryParse(tbBet.Text.Trim(), out currentMatchBet))
                            {
                                if (alreadyMatchBet + currentMatchBet > ConfigGlobal.SingleBetLimit)
                                {
                                    throw new Exception(
                                        $"下半赛区博彩玩家单场投注不能超过{ConfigGlobal.SingleBetLimit.ToString("f2")}博彩币");
                                }
                            }
                        }
                    }


                    //get selected option
                    var item = (SingleChoice) CasinoItem.GetCasinoItem(guid.Value);
                    var seletedOption =
                        item.Options.Find(
                            delegate(ChoiceOption option)
                            {
                                return option.OptionValue == rblSingleChoice.SelectedValue;
                            });

                    var bet = new Bet();
                    bet.BetAmount = Convert.ToSingle(tbBet.Text.Trim());
                    bet.BetRate = seletedOption.OptionRate;
                    bet.CasinoItemGuid = guid.Value;
                    bet.UserID = userid;
                    bet.UserName = username;

                    bet.Insert(seletedOption.OptionValue);

                    ClientScript.RegisterClientScriptBlock(typeof (string), "succeed",
                        "alert('投注成功'); window.location.href = window.location.href;", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof (string), "failed", $"alert('{ex.Message}')", true);
            }
        }

        protected void gvBet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var ltrlResult = e.Row.FindControl("ltrlResult") as Literal;
                var ltrlBetRate = e.Row.FindControl("ltrlBetRate") as Literal;
                var ltrlBonusCalc = e.Row.FindControl("ltrlBonusCalc") as Literal;

                if (ltrlResult != null && ltrlBetRate != null && ltrlBonusCalc != null)
                {
                    var bet = e.Row.DataItem as Bet;

                    if (bet != null)
                    {
                        var item = CasinoItem.GetCasinoItem(bet.CasinoItemGuid);

                        switch (item.ItemType)
                        {
                            case CasinoType.SingleChoice:
                                var dt = BetDetail.GetBetDetailByBetID(bet.ID);
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (dr["DetailName"].ToString() == MatchChoiceOption.HomeWinValue)
                                        ltrlResult.Text = "主队胜";
                                    else if (dr["DetailName"].ToString() == MatchChoiceOption.DrawValue)
                                        ltrlResult.Text = "双方平";
                                    else if (dr["DetailName"].ToString() == MatchChoiceOption.AwayWinValue)
                                        ltrlResult.Text = "客队胜";
                                }
                                break;
                            case CasinoType.MatchResult:
                                var matchResult = new MatchResultBetDetail(BetDetail.GetBetDetailByBetID(bet.ID));
                                ltrlResult.Text = $"{matchResult.Home} : {matchResult.Away}";
                                break;
                        }
                    }

                    if (bet?.BetRate != null)
                    {
                        ltrlBetRate.Text = Convert.ToSingle(bet.BetRate).ToString("f2");
                        ltrlBonusCalc.Text = "+" +
                                             ((Convert.ToSingle(bet.BetRate) - 1)*Convert.ToSingle(bet.BetAmount))
                                                 .ToString("N2");
                    }
                    else
                    {
                        ltrlBetRate.Text = "/";
                        ltrlBonusCalc.Text = "RP+1";
                    }
                }
            }
        }

        protected void gvMatch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var drv = e.Row.DataItem as DataRowView;

                if (drv != null)
                {
                    var m = new Match((Guid) drv["MatchGuid"]);

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
                    var ltrlResult = e.Row.FindControl("ltrlResult") as Literal;

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

                    if (ltrlResult != null && m.ResultHome.HasValue && m.ResultAway.HasValue)
                    {
                        ltrlResult.Text = $"{m.ResultHome.Value} : {m.ResultAway.Value}";
                    }

                    var ltrlTotalBetCount = e.Row.FindControl("ltrlTotalBetCount") as Literal;
                    var ltrlTotalBetCash = e.Row.FindControl("ltrlTotalBetCash") as Literal;
                    var ltrlTotalWin = e.Row.FindControl("ltrlTotalWin") as Literal;

                    var betList = Bet.GetMatchAllBet(m.MatchGuid);

                    if (ltrlTotalBetCount != null)
                    {
                        ltrlTotalBetCount.Text = betList.Count.ToString();
                    }

                    if (ltrlTotalBetCash != null)
                    {
                        float totalbetCash = 0;
                        betList.ForEach(delegate(Bet bet) { totalbetCash += bet.BetAmount.GetValueOrDefault(0f); });
                        ltrlTotalBetCash.Text = totalbetCash.ToString("N0");
                    }

                    if (ltrlTotalWin != null)
                    {
                        float totalWin = 0;
                        betList.ForEach(
                            delegate(Bet bet)
                            {
                                totalWin += (bet.BetAmount.GetValueOrDefault(0f) - bet.Earning.GetValueOrDefault(0f));
                            });
                        ltrlTotalWin.Text = totalWin.ToString("N2");
                    }
                }
            }
        }

        protected void gvMatch_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMatch.PageIndex = e.NewPageIndex;

            BindData();
        }
    }
}