using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Arsenalcn.ClubSys.Entity;
using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Web.Common;
using Arsenalcn.ClubSys.Web.Control;

namespace Arsenalcn.ClubSys.Web
{
    public partial class ClubHistory : BasePage
    {
        private List<Entity.ClubHistory> history;

        private int ClubID
        {
            get
            {
                int tmp;
                if (int.TryParse(Request.QueryString["ClubID"], out tmp))
                    return tmp;
                Response.Redirect("ClubPortal.aspx");

                return -1;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var club = ClubLogic.GetClubInfo(ClubID);

            if (club != null && Title.IndexOf("{0}") >= 0)
                Title = string.Format(Title, club.FullName);

            #region SetControlProperty

            ctrlLeftPanel.UserID = userid;
            ctrlLeftPanel.UserName = username;
            ctrlLeftPanel.UserKey = userkey;

            ctrlFieldToolBar.UserID = userid;
            ctrlFieldToolBar.UserName = username;

            ctrlMenuTabBar.CurrentMenu = ClubMenuItem.ClubHistory;
            ctrlMenuTabBar.ClubID = ClubID;

            ctrlClubSysHeader.UserID = userid;
            ctrlClubSysHeader.ClubID = ClubID;
            ctrlClubSysHeader.UserName = username;

            #endregion

            BindClubHistory();
        }

        private void BindClubHistory()
        {
            if (history == null)
            {
                history = ClubLogic.GetClubHistory(ClubID);

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
                            ch.AdditionalData = "ClubSys_Cash";
                            break;
                        case ClubHistoryActionType.TransferExtcredit:
                            ch.AdditionalData = "ClubSys_Cash";
                            break;
                        default:
                            ch.AdditionalData = "ClubSys_Agree";
                            break;
                    }

                    ch.AdditionalData2 = ClubLogic.TranslateClubHistoryActionType(actionType);
                }
            }

            gvClubHistory.DataSource = history;
            gvClubHistory.DataBind();
        }

        protected void gvClubHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvClubHistory.PageIndex = e.NewPageIndex;

            BindClubHistory();
        }
    }
}