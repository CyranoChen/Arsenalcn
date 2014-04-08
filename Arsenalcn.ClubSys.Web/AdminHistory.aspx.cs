using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Arsenalcn.ClubSys.DataAccess;
using Arsenalcn.ClubSys.Entity;

namespace Arsenalcn.ClubSys.Web
{
    public partial class AdminHistory : Common.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ListItem item = new ListItem("--请选择球会--", Guid.Empty.ToString());

            ddlClub.DataSource = ClubLogic.GetActiveClubs();
            ddlClub.DataTextField = "FullName";
            ddlClub.DataValueField = "ID";
            ddlClub.DataBind();

            ddlClub.Items.Insert(0, item);

            if( ClubID != -1 )
                ddlClub.SelectedValue = ClubID.ToString();

            ctrlAdminFieldToolBar.AdminUserName = this.username;

            BindClubHistory();
        }

        private int ClubID
        {
            get
            {
                int clubID = -1;

                string ddlKey = string.Empty;
                foreach (string key in Request.Form.AllKeys)
                {
                    if (key.IndexOf("ddlClub") >= 0)
                        ddlKey = key;
                }

                if (ddlKey != string.Empty)
                {
                    int.TryParse(Request.Form[ddlKey], out clubID);
                }
                else
                {
                    int.TryParse(ddlClub.SelectedValue, out clubID);
                }
                
                return clubID;
            }
        }

        private List<Arsenalcn.ClubSys.Entity.ClubHistory> history = null;
        private void BindClubHistory()
        {
            if (history == null)
            {
                if (ClubID > 0)
                    history = ClubLogic.GetClubHistory(ClubID);
                else
                    history = ClubLogic.GetClubHistory();

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
                            ch.AdditionalData = "ClubSys_Disagree";
                            break;
                        default:
                            ch.AdditionalData = "ClubSys_Agree";
                            break;
                    }

                    ch.AdditionalData2 = ClubLogic.TranslateClubHistoryActionType(actionType);
                }
            }

            gvHistory.DataSource = history;
            gvHistory.DataBind();
        }

        protected void gvHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvHistory.PageIndex = e.NewPageIndex;

            BindClubHistory();
        }
    }
}
