using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Entity;

namespace Arsenalcn.ClubSys.Web.Control
{
    public partial class PortalClubList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //#region Callback Reference
            //string callbackReference = Page.ClientScript.GetCallbackEventReference(this, "clubID", "GetResult", "context");

            //string callbackScript = string.Format("function Action(clubID, context){{ {0} }};", callbackReference);

            //Page.ClientScript.RegisterClientScriptBlock(typeof(string), "action", callbackScript, true);

            //#endregion

            BindClubListData();
        }
        //#region ICallbackEventHandler Members

        //public string GetCallbackResult()
        //{
        //    if (_clubID > 0 && _clientStatus >= 0)
        //    {
        //        UserClubStatus uct = (UserClubStatus)_clientStatus;
        //        UserClubStatus resultStatus = UserClubStatus.No;

        //        if ((int)ClubLogic.GetUserClubStatus(this.userid, _clubID) == _clientStatus)
        //        {
        //            //action = apply, check if club appliable flag
        //            if (uct == UserClubStatus.No)
        //            {
        //                Arsenalcn.ClubSys.Entity.Club club = ClubLogic.GetClubInfo(_clubID);
        //                if (club != null)
        //                {
        //                    if (!club.IsAppliable.Value)
        //                    {
        //                        return "Not Appliable";
        //                    }
        //                }
        //                else
        //                {
        //                    return string.Empty;
        //                }
        //            }

        //            if (UserClubLogic.UserClubAction(this.userid, this.username, _clubID, uct))
        //            {
        //                switch (uct)
        //                {
        //                    case UserClubStatus.Applied:
        //                        resultStatus = UserClubStatus.No;
        //                        break;
        //                    case UserClubStatus.Member:
        //                        resultStatus = UserClubStatus.No;
        //                        break;
        //                    case UserClubStatus.No:
        //                        resultStatus = UserClubStatus.Applied;
        //                        break;
        //                    default:
        //                        break;
        //                }

        //                return string.Format("{0};{1}", _clubID, (int)resultStatus);
        //            }
        //            else
        //                return string.Empty;
        //        }
        //        else
        //            return string.Empty;
        //    }
        //    else
        //        return string.Empty;
        //}

        //private int _clubID = -1;
        //private int _clientStatus = -1;
        //public void RaiseCallbackEvent(string eventArgument)
        //{
        //    string[] param = eventArgument.Split(';');

        //    if (param.Length == 2)
        //    {
        //        _clubID = int.Parse(param[0]);
        //        _clientStatus = int.Parse(param[1]);
        //    }
        //}

        //#endregion

        protected void gvClubList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var club = (Club)e.Row.DataItem;

                var count = ClubLogic.GetClubMembers(club.ID.Value).Count;

                var ltrlMemberCount = e.Row.FindControl("ltrlMemberCount") as Literal;
                ltrlMemberCount.Text = count.ToString();

                var ltrlClubLogo = e.Row.FindControl("ltrlClubLogo") as Literal;
                var ltrlClubName = e.Row.FindControl("ltrlClubName") as Literal;
                var ltrlClubRank = e.Row.FindControl("ltrlClubRank") as Literal;

                var ltrlActionText = e.Row.FindControl("ltrlActionText") as Literal;
                var ltrlStatus = e.Row.FindControl("ltrlStatus") as Literal;
                //Literal ltrlButtonDisplay = e.Row.FindControl("ltrlButtonDisplay") as Literal;
                var ltrlIsAppliable = e.Row.FindControl("ltrlIsAppliable") as Literal;

                var ltrlEquipmentCount = e.Row.FindControl("ltrlEquipmentCount") as Literal;

                ltrlClubLogo.Text = string.Format("<a href=\"ClubView.aspx?ClubID={0}\" title=\"{2}\"><img src=\"UploadFiles/{1}\" alt=\"{2}\" width=\"80\" height=\"80\" /></a>", club.ID.ToString(), club.LogoName.ToString(), club.FullName.ToString());
                ltrlClubName.Text =
                    $"<a href=\"ClubView.aspx?ClubID={club.ID.ToString()}\" class=\"StrongLink\" title=\"{HttpUtility.HtmlEncode(club.Slogan).Replace("'", "\"")}\">{club.FullName.ToString()}</a>";
                ltrlClubRank.Text =
                    $"<a href=\"ClubRank.aspx?ClubID={club.ID.ToString()}\" class=\"StrongLink\">RPos:{club.RankScore.ToString()}</a><div class=\"ClubSys_Rank\" style=\"width: {(club.RankLevel*20).ToString()}px;\"></div>";

                ltrlEquipmentCount.Text =
                    $"<em title=\"卡片数C|视频数V(今日库存)\">{PlayerStrip.GetClubMemberCardCount(club.ID.Value).ToString()}|{PlayerStrip.GetClubMemberVideoCount(club.ID.Value).ToString()}({PlayerStrip.GetClubRemainingEquipment(club.ID.Value)})</em>";

                if (!club.IsAppliable.Value)
                    ltrlIsAppliable.Visible = true;
                else
                    ltrlIsAppliable.Visible = false;

                if (ConfigGlobal.ChampionsClubID > 0 && club.ID == ConfigGlobal.ChampionsClubID)
                {
                    ltrlClubName.Text =
                        $"<div class=\"ClubSys_Crown\" title=\"{ConfigGlobal.ChampionsTitle}\"></div><div>{ltrlClubName.Text}</div>";
                }

                //if (userid == -1 || club.ManagerUid == userid)
                //{
                //    ltrlButtonDisplay.Text = "none";
                //}
                //else
                //{
                //    ltrlButtonDisplay.Text = "inline";
                //}

                // the count of clubs which current user has joined exceed max quota, hide join action
                //if (ClubLogic.GetActiveUserClubs(userid).Count >= Config.SingleUserMaxClubCount && uct != UserClubStatus.Member)
                //{
                //    ltrlButtonDisplay.Text = "none";
                //}

                //hide join btn if club is set to not appliable to join
                //if (uct == UserClubStatus.No && (!club.IsAppliable.Value || count >= ClubLogic.GetClubMemberQuota(club.ID.Value)))
                //{
                //    ltrlButtonDisplay.Text = "none";
                //}

                //ltrlStatus.Text = ((int)uct).ToString();

                //switch (uct)
                //{
                //    case UserClubStatus.Applied:
                //        ltrlActionText.Text = "取消申请";
                //        break;
                //    case UserClubStatus.Member:
                //        ltrlActionText.Text = "退出球会";
                //        break;
                //    case UserClubStatus.No:
                //        ltrlActionText.Text = "申请加入";
                //        break;
                //    default:
                //        ltrlActionText.Text = "申请加入";
                //        break;
                //}

                var rptLeader = e.Row.FindControl("rptClubLeads") as Repeater;
                if (rptLeader != null)
                {
                    var uc = ClubLogic.GetClubLeads(club.ID.Value);

                    foreach (var userClub in uc)
                    {
                        userClub.AdditionalData = ClubLogic.TranslateResponsibility(userClub.Responsibility.Value);

                        //temp usage of username for li class
                        if (userClub.Responsibility.Value == (int)Responsibility.Manager)
                            userClub.AdditionalData2 = " class=\"Manager\"";
                        else
                            userClub.AdditionalData2 = string.Empty;
                    }

                    rptLeader.DataSource = uc;
                    rptLeader.DataBind();
                }
            }
        }

        protected void gvClubList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvClubList.PageIndex = e.NewPageIndex;

            BindClubListData();
        }

        private List<Club> clubList = null;
        private void BindClubListData()
        {
            if (clubList == null)
                clubList = ClubLogic.GetActiveClubs();

            //if (userid == -1)
            //{
            //    //hide func button
            //    gvClubList.Columns[gvClubList.Columns.Count - 1].Visible = false;
            //}
            //else if (ClubLogic.GetActiveUserClubs(userid).Count != 0)
            //{
            //    gvClubList.Columns[gvClubList.Columns.Count - 1].Visible = false;
            //}

            //clubList.Sort(new ClubComparer());

            gvClubList.DataSource = clubList;
            gvClubList.DataBind();

        }
    }
}