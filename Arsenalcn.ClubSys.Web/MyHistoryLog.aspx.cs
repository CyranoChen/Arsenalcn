using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Arsenalcn.ClubSys.Entity;
using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Web.Common;

namespace Arsenalcn.ClubSys.Web
{
    public partial class MyApplyLog : BasePage
    {
        private List<Entity.ClubHistory> history;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            AnonymousRedirect = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            #region SetControlProperty

            ctrlLeftPanel.UserID = userid;
            ctrlLeftPanel.UserName = username;
            ctrlLeftPanel.UserKey = userkey;

            ctrlFieldToolBar.UserID = userid;
            ctrlFieldToolBar.UserName = username;

            #endregion

            if (!IsPostBack)
            {
                BindClubHistory();
            }
        }

        private void BindClubHistory()
        {
            if (history == null)
            {
                history = ClubLogic.GetUserClubHistory(username);

                foreach (var ch in history)
                {
                    var actionType = (ClubHistoryActionType) Enum.Parse(typeof (ClubHistoryActionType), ch.ActionType);
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
                            ch.ActionDescription = $"<em>{ch.ActionDescription}</em>";
                            break;
                        case ClubHistoryActionType.TransferExtcredit:
                            ch.AdditionalData = "ClubSys_Agree";
                            ch.ActionDescription = $"<em>{ch.ActionDescription}</em>";
                            break;
                        default:
                            ch.AdditionalData = "ClubSys_Agree";
                            break;
                    }

                    ch.AdditionalData2 = ClubLogic.TranslateClubHistoryActionType(actionType);
                }

                var bingoHistory = PlayerStrip.GetUserBingoHistory(userid);

                foreach (var bh in bingoHistory)
                {
                    var current = new Entity.ClubHistory();

                    var br = new BingoResult(bh.Result, bh.ResultDetail);

                    current.OperatorUserName = username;
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
                                current.ActionDescription =
                                    $"获得球员卡: {Player.Cache.Load(new Guid(br.ResultDetail)).DisplayName}";
                            break;
                        case BingoResultType.Cash:
                            current.AdditionalData = "ClubSys_Agree";
                            current.ActionDescription = "获得枪手币: " + br.ResultDetail;
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
                        current.ActionDescription = $"<em>{current.ActionDescription}</em>";
                    }

                    history.Add(current);
                }

                var playerHistory = PlayerLog.GetUserPlayerHistory(userid);

                foreach (var ph in playerHistory)
                {
                    var current = new Entity.ClubHistory();

                    current.OperatorUserName = username;
                    current.ClubID = ClubLogic.GetActiveUserClubs(userid)[0].ID.Value;
                    current.AdditionalData = "ClubSys_Star";
                    current.ActionDescription = $"<em>{ph.TypeDesc}</em>";
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
                var ch = (Entity.ClubHistory) e.Row.DataItem;

                var club = ClubLogic.GetClubInfo(ch.ClubID);

                if (club != null)
                {
                    var ltrlClubName = e.Row.FindControl("ltrlClubName") as Literal;

                    if (ltrlClubName != null)
                    {
                        ltrlClubName.Text = club.FullName;
                    }
                }
            }
        }
    }
}