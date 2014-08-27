using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Entity;

using ArsenalPlayer = Arsenalcn.ClubSys.Service.Arsenal.Player;

namespace Arsenalcn.ClubSys.Web
{
    public partial class MyApplyLog : Common.BasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            AnonymousRedirect = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            #region SetControlProperty

            ctrlLeftPanel.UserID = this.userid;
            ctrlLeftPanel.UserName = this.username;
            ctrlLeftPanel.UserKey = this.userkey;

            ctrlFieldToolBar.UserID = this.userid;
            ctrlFieldToolBar.UserName = this.username;

            #endregion

            if (!IsPostBack)
            {
                BindClubHistory();
            }
        }

        private List<Arsenalcn.ClubSys.Entity.ClubHistory> history = null;
        private void BindClubHistory()
        {
            if (history == null)
            {
                history = ClubLogic.GetUserClubHistory(username);

                foreach (Arsenalcn.ClubSys.Entity.ClubHistory ch in history)
                {
                    ClubHistoryActionType actionType = (ClubHistoryActionType)Enum.Parse(typeof(ClubHistoryActionType), ch.ActionType);
                    switch (actionType)
                    {
                        case ClubHistoryActionType.JoinClub:
                            ch.AdditionalData = "ClubSys_Agree";
                            break;
                        case ClubHistoryActionType.RejectJoinClub:
                            ch.AdditionalData = "ClubSys_Disagree";
                            break;
                        case ClubHistoryActionType.LeaveClub:
                            ch.AdditionalData = "ClubSys_Disagree";
                            break;
                        case ClubHistoryActionType.MandatoryLeaveClub:
                            ch.AdditionalData = "ClubSys_Disagree";
                            break;
                        case ClubHistoryActionType.Nominated:
                            ch.AdditionalData = "ClubSys_Agree";
                            break;
                        case ClubHistoryActionType.Dismiss:
                            ch.AdditionalData = "ClubSys_Disagree";
                            break;
                        case ClubHistoryActionType.LuckyPlayer:
                            ch.AdditionalData = "ClubSys_Agree";
                            ch.ActionDescription = string.Format("<em>{0}</em>", ch.ActionDescription);
                            break;
                        case ClubHistoryActionType.TransferExtcredit:
                            ch.AdditionalData = "ClubSys_Agree";
                            ch.ActionDescription = string.Format("<em>{0}</em>", ch.ActionDescription);
                            break;
                        default:
                            ch.AdditionalData = "ClubSys_Agree";
                            break;
                    }

                    ch.AdditionalData2 = ClubLogic.TranslateClubHistoryActionType(actionType);
                }

                List<BingoHistory> bingoHistory = PlayerStrip.GetUserBingoHistory(this.userid);

                foreach (BingoHistory bh in bingoHistory)
                {
                    Arsenalcn.ClubSys.Entity.ClubHistory current = new Arsenalcn.ClubSys.Entity.ClubHistory();

                    BingoResult br = new BingoResult(bh.Result, bh.ResultDetail);

                    current.OperatorUserName = this.username;
                    current.ClubID = bh.ClubID;
                    current.AdditionalData2 = string.Empty;
                    current.ActionDate = bh.ActionDate;

                    switch (br.Result)
                    {
                        case BingoResultType.Strip:
                            current.AdditionalData = "ClubSys_Agree";
                            if (br.ResultDetail == "strip")
                                current.ActionDescription = "获得一套球衣装备";
                            else if (br.ResultDetail == "strips")
                                current.ActionDescription = "获得五套球衣装备";
                            else
                                current.ActionDescription = BingoUtil.ShowBothBingoDetail("获得一件{0}", br);
                            break;
                        case BingoResultType.Card:
                            current.AdditionalData = "ClubSys_Agree";
                            if (br.ResultDetail == "legend")
                                current.ActionDescription = "获得一张视频卡";
                            else
                                current.ActionDescription = string.Format("获得球员卡: {0}", Arsenal_Player.Cache.Load(new Guid(br.ResultDetail)).DisplayName);
                            break;
                        case BingoResultType.Cash:
                            current.AdditionalData = "ClubSys_Agree";
                            current.ActionDescription = "获得枪手币: " + br.ResultDetail.ToString();
                            break;
                        case BingoResultType.Both:
                            current.AdditionalData = "ClubSys_Agree";
                            current.ActionDescription = BingoUtil.ShowBothBingoDetail("获得一件{0}和枪手币: {1}", br);
                            break;
                        default:
                            current.AdditionalData = "ClubSys_Disagree";
                            current.ActionDescription = "什么都没获得";
                            break;
                    }

                    if (br.Result != BingoResultType.Null)
                    {
                        current.ActionDescription = string.Format("<em>{0}</em>", current.ActionDescription);
                    }

                    history.Add(current);
                }

                List<PlayerHistory> playerHistory = PlayerLog.GetUserPlayerHistory(this.userid);

                foreach (PlayerHistory ph in playerHistory)
                {
                    Arsenalcn.ClubSys.Entity.ClubHistory current = new Arsenalcn.ClubSys.Entity.ClubHistory();

                    current.OperatorUserName = this.username;
                    current.ClubID = ClubLogic.GetActiveUserClubs(this.userid)[0].ID.Value;
                    current.AdditionalData = "ClubSys_Star";
                    current.ActionDescription = string.Format("<em>{0}</em>", ph.TypeDesc);
                    current.ActionDate = ph.ActionDate;

                    history.Add(current);
                }

                history.Sort(new ClubHistoryComparer());
            }

            gvHistoryLog.DataSource = history;
            gvHistoryLog.DataBind();
        }

        protected void gvHistoryLog_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvHistoryLog.PageIndex = e.NewPageIndex;

            BindClubHistory();
        }

        protected void gvHistoryLog_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Arsenalcn.ClubSys.Entity.ClubHistory ch = (Arsenalcn.ClubSys.Entity.ClubHistory)e.Row.DataItem;

                Club club = ClubLogic.GetClubInfo(ch.ClubID);

                if (club != null)
                {
                    Literal ltrlClubName = e.Row.FindControl("ltrlClubName") as Literal;

                    if (ltrlClubName != null)
                    {
                        ltrlClubName.Text = club.FullName;
                    }
                }
            }
        }
    }
}
