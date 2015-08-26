using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

using Arsenalcn.CasinoSys.Entity;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class CasinoGameBet : Common.BasePage
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

            ctrlCasinoHeader.UserID = userid;
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

                var guid = Entity.CasinoItem.GetCasinoItemGuidByMatch(CurrentMatch, CasinoType.SingleChoice);

                if (!IsPostBack)
                {
                    #region 设置其他可投注比赛的下拉框
                    var dtMatch = Entity.CasinoItem.GetMatchCasinoItemView(true);

                    if (dtMatch != null)
                    {
                        dtMatch.Columns.Add("MatchTeamDisplay", typeof(string));

                        foreach (DataRow dr_m in dtMatch.Rows)
                        {
                            var m = new Match((Guid)dr_m["MatchGuid"]);

                            dr_m["MatchTeamDisplay"] = string.Format("{0} vs {1}",
                                Team.Cache.Load((Guid)m.Home).TeamDisplayName,
                                Team.Cache.Load((Guid)m.Away).TeamDisplayName);
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
                        var item = Entity.CasinoItem.GetCasinoItem(guid.Value);

                        if (item != null)
                        {
                            var options = ((Entity.SingleChoice)item).Options;

                            var winOption = options.Find(delegate(Entity.ChoiceOption option) { return option.OptionValue == Entity.MatchChoiceOption.HomeWinValue; });
                            var drawOption = options.Find(delegate(Entity.ChoiceOption option) { return option.OptionValue == Entity.MatchChoiceOption.DrawValue; });
                            var loseOption = options.Find(delegate(Entity.ChoiceOption option) { return option.OptionValue == Entity.MatchChoiceOption.AwayWinValue; });

                            if (string.IsNullOrEmpty(winOption.OptionValue) || string.IsNullOrEmpty(drawOption.OptionValue) || string.IsNullOrEmpty(loseOption.OptionValue))
                                throw new Exception();
                            else
                            {
                                var liWin = rblSingleChoice.Items[0];
                                liWin.Text = string.Format("<em>主队胜({0})</em>", Convert.ToSingle(winOption.OptionRate).ToString("f2"));
                                liWin.Value = Entity.MatchChoiceOption.HomeWinValue;

                                var liDraw = rblSingleChoice.Items[1];
                                liDraw.Text = string.Format("<em>双方平({0})</em>", Convert.ToSingle(drawOption.OptionRate).ToString("f2"));
                                liDraw.Value = Entity.MatchChoiceOption.DrawValue;

                                var liLose = rblSingleChoice.Items[2];
                                liLose.Text = string.Format("<em>客队胜({0})</em>", Convert.ToSingle(loseOption.OptionRate).ToString("f2"));
                                liLose.Value = Entity.MatchChoiceOption.AwayWinValue;
                            }
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
            var betList = Entity.Bet.GetUserMatchAllBet(userid, CurrentMatch);

            var matchResultGuid = Entity.CasinoItem.GetCasinoItemGuidByMatch(CurrentMatch, CasinoType.MatchResult);

            if (matchResultGuid.HasValue && betList.Exists(delegate(Bet bet) { return bet.CasinoItemGuid == matchResultGuid.Value; }))
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

            var dtHistory = Entity.CasinoItem.GetHistoryViewByMatch(CurrentMatch);

            gvMatch.DataSource = dtHistory;
            gvMatch.DataBind();
        }

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

        protected void ddlCasinoGame_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((ddlCasinoGame.SelectedIndex != 0) && (ddlCasinoGame.SelectedValue != Guid.Empty.ToString()))
            {
                Response.Redirect("CasinoGameBet.aspx?Match=" + ddlCasinoGame.SelectedValue.ToString());
            }
        }

        protected void btnMatchResult_Click(object sender, EventArgs e)
        {
            try
            {
                var guid = Entity.CasinoItem.GetCasinoItemGuidByMatch(CurrentMatch, CasinoType.MatchResult);

                if (guid.HasValue)
                {
                    if (Entity.CasinoItem.GetCasinoItem(guid.Value).CloseTime < DateTime.Now)
                    {
                        throw new Exception("已超出投注截止时间");
                    }

                    if (Entity.Bet.GetUserCasinoItemAllBet(this.userid, guid.Value).Count > 0)
                    {
                        throw new Exception("已经投过此注，不能重复猜比分");
                    }

                    var bet = new Bet();
                    bet.BetAmount = null;
                    bet.BetRate = null;
                    bet.CasinoItemGuid = guid.Value;
                    bet.UserID = this.userid;
                    bet.UserName = this.username;

                    var matchResult = new MatchResultBetDetail();
                    matchResult.Home = Convert.ToInt16(tbHome.Text);
                    matchResult.Away = Convert.ToInt16(tbAway.Text);

                    bet.Insert(matchResult);

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('投注成功'); window.location.href = window.location.href;", true);
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}')", ex.Message.ToString()), true);
            }
        }

        protected void btnSingleChoice_Click(object sender, EventArgs e)
        {
            try
            {
                var guid = Entity.CasinoItem.GetCasinoItemGuidByMatch(CurrentMatch, CasinoType.SingleChoice);

                if (guid.HasValue)
                {
                    if (Entity.CasinoItem.GetCasinoItem(guid.Value).CloseTime < DateTime.Now)
                    { throw new Exception("已超出投注截止时间"); }

                    //Gambler in Lower could not bet above the SingleBetLimit of DefaultLeague (Contest)
                    var m = new Match(CurrentMatch);

                    if (m.LeagueGuid.Equals(ConfigGlobal.DefaultLeagueID))
                    {
                        if (Entity.Gambler.GetGamblerTotalBetByUserID(this.userid, m.LeagueGuid) < ConfigGlobal.TotalBetStandard)
                        {
                            var _alreadyMatchBet = Entity.Bet.GetUserMatchTotalBet(this.userid, CurrentMatch);
                            var _currentMatchBet = 0f;

                            if (!string.IsNullOrEmpty(tbBet.Text.Trim()) && Single.TryParse(tbBet.Text.Trim(), out _currentMatchBet))
                            {
                                if (_alreadyMatchBet + _currentMatchBet > ConfigGlobal.SingleBetLimit)
                                { throw new Exception(string.Format("下半赛区博彩玩家单场投注不能超过{0}博彩币", ConfigGlobal.SingleBetLimit.ToString("f2"))); }
                            }
                        }
                    }


                    //get selected option
                    var item = (Entity.SingleChoice)Entity.CasinoItem.GetCasinoItem(guid.Value);
                    var seletedOption = item.Options.Find(delegate(Entity.ChoiceOption option) { return option.OptionValue == rblSingleChoice.SelectedValue; });

                    var bet = new Arsenalcn.CasinoSys.Entity.Bet();
                    bet.BetAmount = Convert.ToSingle(tbBet.Text.Trim());
                    bet.BetRate = seletedOption.OptionRate;
                    bet.CasinoItemGuid = guid.Value;
                    bet.UserID = this.userid;
                    bet.UserName = this.username;

                    bet.Insert(seletedOption.OptionValue);

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('投注成功'); window.location.href = window.location.href;", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}')", ex.Message.ToString()), true);
            }
        }

        protected void gvBet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var ltrlResult = e.Row.FindControl("ltrlResult") as Literal;
                var ltrlBetRate = e.Row.FindControl("ltrlBetRate") as Literal;
                var ltrlBonusCalc = e.Row.FindControl("ltrlBonusCalc") as Literal;

                if (ltrlResult != null)
                {
                    var bet = e.Row.DataItem as Entity.Bet;

                    var item = Entity.CasinoItem.GetCasinoItem(bet.CasinoItemGuid);

                    switch (item.ItemType)
                    {
                        case CasinoType.SingleChoice:
                            var dt = Entity.BetDetail.GetBetDetailByBetID(bet.ID);
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
                            var matchResult = new MatchResultBetDetail(Entity.BetDetail.GetBetDetailByBetID(bet.ID));
                            ltrlResult.Text = string.Format("{0} : {1}", matchResult.Home, matchResult.Away);
                            break;
                        default:
                            break;
                    }

                    if (bet.BetRate.HasValue)
                    {
                        ltrlBetRate.Text = Convert.ToSingle(bet.BetRate).ToString("f2");
                        ltrlBonusCalc.Text = "+" + ((Convert.ToSingle(bet.BetRate) - 1) * Convert.ToSingle(bet.BetAmount)).ToString("N2");
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

                var m = new Match((Guid)drv["MatchGuid"]);

                var ltrlLeagueInfo = e.Row.FindControl("ltrlLeagueInfo") as Literal;

                if (ltrlLeagueInfo != null)
                {
                    var _strLeague = "<a href=\"CasinoGame.aspx?League={0}\" title=\"{1}\"><img src=\"{2}\" alt=\"{1}\" class=\"CasinoSys_CategoryImg\" /></a>";

                    var _strLeagueName = string.Format("{0}{1}", m.LeagueName, m.Round.HasValue ?
                        string.Format(" 第{0}轮", m.Round.ToString()) : string.Empty);

                    ltrlLeagueInfo.Text = string.Format(_strLeague, m.LeagueGuid.ToString(), _strLeagueName,
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

                    var _strTeamName = "<a class=\"StrongLink\" href=\"CasinoTeam.aspx?Team={0}\"  title=\"{1}\">{2}</a> ";
                    var _strTeamLogo = "<img src=\"{3}\" alt=\"{1}\" /> ";

                    lblHome.Text = string.Format(_strTeamName + _strTeamLogo,
                        tHome.ID.ToString(), tHome.TeamEnglishName, tHome.TeamDisplayName, tHome.TeamLogo);
                    lblAway.Text = string.Format(_strTeamLogo + _strTeamName,
                        tAway.ID.ToString(), tAway.TeamEnglishName, tAway.TeamDisplayName, tAway.TeamLogo);

                    hlVersus.NavigateUrl = string.Format("CasinoTeam.aspx?Match={0}", m.MatchGuid.ToString());
                    hlVersus.Text = string.Format("<em title=\"{0}{1}\">vs</em>", tHome.Ground,
                        tHome.Capacity.HasValue ? ("(" + tHome.Capacity.Value.ToString() + ")") : string.Empty);
                }

                if (ltrlResult != null)
                {
                    ltrlResult.Text = string.Format("{0} : {1}", m.ResultHome.Value.ToString(), m.ResultAway.Value.ToString());
                }

                var ltrlTotalBetCount = e.Row.FindControl("ltrlTotalBetCount") as Literal;
                var ltrlTotalBetCash = e.Row.FindControl("ltrlTotalBetCash") as Literal;
                var ltrlTotalWin = e.Row.FindControl("ltrlTotalWin") as Literal;

                var betList = Entity.Bet.GetMatchAllBet(m.MatchGuid);

                if (ltrlTotalBetCount != null)
                { ltrlTotalBetCount.Text = betList.Count.ToString(); }

                if (ltrlTotalBetCash != null)
                {
                    float _totalbetCash = 0;
                    betList.ForEach(delegate(Entity.Bet bet) { _totalbetCash += bet.BetAmount.GetValueOrDefault(0f); });
                    ltrlTotalBetCash.Text = _totalbetCash.ToString("N0");
                }

                if (ltrlTotalWin != null)
                {
                    float _totalWin = 0;
                    betList.ForEach(delegate(Entity.Bet bet) { _totalWin += (bet.BetAmount.GetValueOrDefault(0f) - bet.Earning.GetValueOrDefault(0f)); });
                    ltrlTotalWin.Text = _totalWin.ToString("N2");
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
