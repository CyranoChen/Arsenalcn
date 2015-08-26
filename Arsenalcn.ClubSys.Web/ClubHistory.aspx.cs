using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Entity;

namespace Arsenalcn.ClubSys.Web
{
    public partial class ClubHistory : Common.BasePage
    {
        private int ClubID
        {
            get
            {
                int tmp;
                if (int.TryParse(Request.QueryString["ClubID"], out tmp))
                    return tmp;
                else
                {
                    Response.Redirect("ClubPortal.aspx");

                    return -1;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var club = ClubLogic.GetClubInfo(ClubID);

            if( club != null && this.Title.IndexOf("{0}") >= 0 )
                this.Title = string.Format(this.Title, club.FullName);

            #region SetControlProperty

            ctrlLeftPanel.UserID = this.userid;
            ctrlLeftPanel.UserName = this.username;
            ctrlLeftPanel.UserKey = this.userkey;

            ctrlFieldToolBar.UserID = this.userid;
            ctrlFieldToolBar.UserName = this.username;

            ctrlMenuTabBar.CurrentMenu = Arsenalcn.ClubSys.Web.Control.ClubMenuItem.ClubHistory;
            ctrlMenuTabBar.ClubID = ClubID;

            ctrlClubSysHeader.UserID = this.userid;
            ctrlClubSysHeader.ClubID = ClubID;
            ctrlClubSysHeader.UserName = this.username;

            #endregion

            BindClubHistory();
        }

        private List<Arsenalcn.ClubSys.Entity.ClubHistory> history = null;
        private void BindClubHistory()
        {
            if (history == null)
            {
                history = ClubLogic.GetClubHistory(ClubID);

                foreach (var ch in history)
                {
                    var actionType = (ClubHistoryActionType)Enum.Parse(typeof(ClubHistoryActionType), ch.ActionType);
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
