using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

using Arsenalcn.CasinoSys.Entity;
using ArsenalLeauge = Arsenalcn.CasinoSys.Entity.Arsenal.League;
using ArsenalTeam = Arsenalcn.CasinoSys.Entity.Arsenal.Team;

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

            Match match = new Match(CurrentMatch);

            if (match != null)
            {
                DateTime dtPlay = match.PlayTime;

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

                Guid? guid = Entity.CasinoItem.GetCasinoItemGuidByMatch(CurrentMatch, CasinoItem.CasinoType.SingleChoice);

                if (!IsPostBack)
                {
                    #region 设置其他可投注比赛的下拉框
                    DataTable dtMatch = Entity.CasinoItem.GetMatchCasinoItemView(true);

                    if (dtMatch != null)
                    {
                        dtMatch.Columns.Add("MatchTeamDisplay", typeof(string));

                        foreach (DataRow dr_m in dtMatch.Rows)
                        {
                            Match m = new Match((Guid)dr_m["MatchGuid"]);

                            dr_m["MatchTeamDisplay"] = string.Format("{0} vs {1}",
                                Arsenal_Team.Cache.Load((Guid)m.Home).TeamDisplayName,
                                Arsenal_Team.Cache.Load((Guid)m.Away).TeamDisplayName);
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
                        Entity.CasinoItem item = Entity.CasinoItem.GetCasinoItem(guid.Value);

                        if (item != null)
                        {
                            List<Entity.ChoiceOption> options = ((Entity.SingleChoice)item).Options;

                            Entity.ChoiceOption winOption = options.Find(delegate(Entity.ChoiceOption option) { return option.OptionValue == Entity.MatchChoiceOption.HomeWinValue; });
                            Entity.ChoiceOption drawOption = options.Find(delegate(Entity.ChoiceOption option) { return option.OptionValue == Entity.MatchChoiceOption.DrawValue; });
                            Entity.ChoiceOption loseOption = options.Find(delegate(Entity.ChoiceOption option) { return option.OptionValue == Entity.MatchChoiceOption.AwayWinValue; });

                            if (string.IsNullOrEmpty(winOption.OptionValue) || string.IsNullOrEmpty(drawOption.OptionValue) || string.IsNullOrEmpty(loseOption.OptionValue))
                                throw new Exception();
                            else
                            {
                                ListItem liWin = rblSingleChoice.Items[0];
                                liWin.Text = string.Format("<em>主队胜({0})</em>", Convert.ToSingle(winOption.OptionRate).ToString("f2"));
                                liWin.Value = Entity.MatchChoiceOption.HomeWinValue;

                                ListItem liDraw = rblSingleChoice.Items[1];
                                liDraw.Text = string.Format("<em>双方平({0})</em>", Convert.ToSingle(drawOption.OptionRate).ToString("f2"));
                                liDraw.Value = Entity.MatchChoiceOption.DrawValue;

                                ListItem liLose = rblSingleChoice.Items[2];
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
            List<Bet> betList = Entity.Bet.GetUserMatchAllBet(userid, CurrentMatch);

            Guid? matchResultGuid = Entity.CasinoItem.GetCasinoItemGuidByMatch(CurrentMatch, CasinoItem.CasinoType.MatchResult);

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

            DataTable dtHistory = Entity.CasinoItem.GetHistoryViewByMatch(CurrentMatch);

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
                Guid? guid = Entity.CasinoItem.GetCasinoItemGuidByMatch(CurrentMatch, CasinoItem.CasinoType.MatchResult);

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

                    Bet bet = new Bet();
                    bet.BetAmount = null;
                    bet.BetRate = null;
                    bet.CasinoItemGuid = guid.Value;
                    bet.UserID = this.userid;
                    bet.UserName = this.username;

                    MatchResultBetDetail matchResult = new MatchResultBetDetail();
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
                Guid? guid = Entity.CasinoItem.GetCasinoItemGuidByMatch(CurrentMatch, CasinoItem.CasinoType.SingleChoice);

                if (guid.HasValue)
                {
                    if (Entity.CasinoItem.GetCasinoItem(guid.Value).CloseTime < DateTime.Now)
                    { throw new Exception("已超出投注截止时间"); }

                    //Gambler in Lower could not bet above the SingleBetLimit of DefaultLeague (Contest)
                    Match m = new Match(CurrentMatch);

                    if (m.LeagueGuid.Equals(ConfigGlobal.DefaultLeagueID))
                    {
                        if (Entity.Gambler.GetGamblerTotalBetByUserID(this.userid, m.LeagueGuid) < ConfigGlobal.TotalBetStandard)
                        {
                            float _alreadyMatchBet = Entity.Bet.GetUserMatchTotalBet(this.userid, CurrentMatch);
                            float _currentMatchBet = 0f;

                            if (!string.IsNullOrEmpty(tbBet.Text.Trim()) && Single.TryParse(tbBet.Text.Trim(), out _currentMatchBet))
                            {
                                if (_alreadyMatchBet + _currentMatchBet > ConfigGlobal.SingleBetLimit)
                                { throw new Exception(string.Format("下半赛区博彩玩家单场投注不能超过{0}博彩币", ConfigGlobal.SingleBetLimit.ToString("f2"))); }
                            }
                        }
                    }


                    //get selected option
                    Entity.SingleChoice item = (Entity.SingleChoice)Entity.CasinoItem.GetCasinoItem(guid.Value);
                    Entity.ChoiceOption seletedOption = item.Options.Find(delegate(Entity.ChoiceOption option) { return option.OptionValue == rblSingleChoice.SelectedValue; });

                    Entity.Bet bet = new Arsenalcn.CasinoSys.Entity.Bet();
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
                Literal ltrlResult = e.Row.FindControl("ltrlResult") as Literal;
                Literal ltrlBetRate = e.Row.FindControl("ltrlBetRate") as Literal;
                Literal ltrlBonusCalc = e.Row.FindControl("ltrlBonusCalc") as Literal;

                if (ltrlResult != null)
                {
                    Entity.Bet bet = e.Row.DataItem as Entity.Bet;

                    Entity.CasinoItem item = Entity.CasinoItem.GetCasinoItem(bet.CasinoItemGuid);

                    switch (item.ItemType)
                    {
                        case CasinoItem.CasinoType.SingleChoice:
                            DataTable dt = Entity.BetDetail.GetBetDetailByBetID(bet.ID);
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
                        case CasinoItem.CasinoType.MatchResult:
                            MatchResultBetDetail matchResult = new MatchResultBetDetail(Entity.BetDetail.GetBetDetailByBetID(bet.ID));
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
                DataRowView drv = e.Row.DataItem as DataRowView;

                Match m = new Match((Guid)drv["MatchGuid"]);

                Literal ltrlLeagueInfo = e.Row.FindControl("ltrlLeagueInfo") as Literal;

                if (ltrlLeagueInfo != null)
                {
                    string _strLeague = "<a href=\"CasinoGame.aspx?League={0}\" title=\"{1}\"><img src=\"{2}\" alt=\"{1}\" class=\"CasinoSys_CategoryImg\" /></a>";

                    string _strLeagueName = string.Format("{0}{1}", m.LeagueName, m.Round.HasValue ?
                        string.Format(" 第{0}轮", m.Round.ToString()) : string.Empty);

                    ltrlLeagueInfo.Text = string.Format(_strLeague, m.LeagueGuid.ToString(), _strLeagueName,
                        Arsenal_League.Cache.Load(m.LeagueGuid).LeagueLogo);
                }

                Label lblHome = e.Row.FindControl("lblHome") as Label;
                Label lblAway = e.Row.FindControl("lblAway") as Label;
                HyperLink hlVersus = e.Row.FindControl("hlVersus") as HyperLink;
                Literal ltrlResult = e.Row.FindControl("ltrlResult") as Literal;

                if (lblHome != null && lblAway != null && hlVersus != null)
                {
                    ArsenalTeam tHome = Arsenal_Team.Cache.Load(m.Home);
                    ArsenalTeam tAway = Arsenal_Team.Cache.Load(m.Away);

                    string _strTeamName = "<a class=\"StrongLink\" href=\"CasinoTeam.aspx?Team={0}\"  title=\"{1}\">{2}</a> ";
                    string _strTeamLogo = "<img src=\"{3}\" alt=\"{1}\" /> ";

                    lblHome.Text = string.Format(_strTeamName + _strTeamLogo,
                        tHome.TeamGuid.ToString(), tHome.TeamEnglishName, tHome.TeamDisplayName, tHome.TeamLogo);
                    lblAway.Text = string.Format(_strTeamLogo + _strTeamName,
                        tAway.TeamGuid.ToString(), tAway.TeamEnglishName, tAway.TeamDisplayName, tAway.TeamLogo);

                    hlVersus.NavigateUrl = string.Format("CasinoTeam.aspx?Match={0}", m.MatchGuid.ToString());
                    hlVersus.Text = string.Format("<em title=\"{0}{1}\">vs</em>", tHome.Ground,
                        tHome.Capacity.HasValue ? ("(" + tHome.Capacity.Value.ToString() + ")") : string.Empty);
                }

                if (ltrlResult != null)
                {
                    ltrlResult.Text = string.Format("{0} : {1}", m.ResultHome.Value.ToString(), m.ResultAway.Value.ToString());
                }

                Literal ltrlTotalBetCount = e.Row.FindControl("ltrlTotalBetCount") as Literal;
                Literal ltrlTotalBetCash = e.Row.FindControl("ltrlTotalBetCash") as Literal;
                Literal ltrlTotalWin = e.Row.FindControl("ltrlTotalWin") as Literal;

                List<Entity.Bet> betList = Entity.Bet.GetMatchAllBet(m.MatchGuid);

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
