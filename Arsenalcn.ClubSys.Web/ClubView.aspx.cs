using System;
using System.Web;
using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Web.Common;
using Arsenalcn.ClubSys.Web.Control;

namespace Arsenalcn.ClubSys.Web
{
    public partial class ClubView : BasePage
    {
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
            #region SetControlProperty

            ctrlLeftPanel.UserID = userid;
            ctrlLeftPanel.UserName = username;
            ctrlLeftPanel.UserKey = userkey;

            ctrlFieldToolBar.UserID = userid;
            ctrlFieldToolBar.UserName = username;

            ctrlMenuTabBar.CurrentMenu = ClubMenuItem.ClubInfo;
            ctrlMenuTabBar.ClubID = ClubID;

            ctrlClubSysHeader.UserID = userid;
            ctrlClubSysHeader.ClubID = ClubID;
            ctrlClubSysHeader.UserName = username;

            #endregion

            var currentClub = ClubLogic.GetClubInfo(ClubID);

            if (currentClub != null && Title.IndexOf("{0}", StringComparison.Ordinal) >= 0)
                Title = string.Format(Title, currentClub.FullName);

            if (currentClub != null)
            {
                ltrlShortName.Text = currentClub.ShortName;
                ltrlCreatorName.Text =
                    $"<a href=\"MyPlayerProfile.aspx?userid={currentClub.CreatorUid.Value}\" target = \"_blank\" >{currentClub.CreatorUserName}</ a >";
                //ltrlCreatorUid.Text = currentClub.CreatorUid.Value.ToString();

                ltrlSlogan.Text = HttpUtility.HtmlEncode(currentClub.Slogan);
                ltrlFortune.Text = Convert.ToInt32(currentClub.Fortune).ToString("N0");
                ltrlMemberCredit.Text = Convert.ToInt32(currentClub.MemberCredit).ToString("N0");
                ltrlMemberFortune.Text = Convert.ToInt32(currentClub.MemberFortune).ToString("N0");
                //ltrlEquipmentCount.Text = Convert.ToInt32(PlayerStrip.GetClubMemberEquipmentCount(currentClub.ID.Value)).ToString("N0");
                ltrlEquipmentCount.Text =
                    $"C:{PlayerStrip.GetClubMemberCardCount(currentClub.ID.Value)} | V:{PlayerStrip.GetClubMemberVideoCount(currentClub.ID.Value)}";
                ltrlCreateDate.Text = currentClub.CreateDate.ToString("yyyy年MM月dd日");
                ltrlDays.Text = (DateTime.Now - currentClub.CreateDate).Days.ToString();
                ltrlMemeberCount.Text = ClubLogic.GetClubMemberCount(ClubID).ToString();
                ltrlMemberQuota.Text = ClubLogic.GetClubMemberQuota(ClubID).ToString();

                if (currentClub.IsAppliable.Value)
                {
                    ltrlAppliable.Text = "开放中";
                }
                else
                {
                    ltrlAppliable.Text = "已关闭";
                }

                var uc = ClubLogic.GetClubLeads(ClubID);

                foreach (var userClub in uc)
                {
                    userClub.AdditionalData = ClubLogic.TranslateResponsibility(userClub.Responsibility.Value);

                    //temp usage of username for li class
                    if (userClub.Responsibility.Value == (int) Responsibility.Manager)
                        userClub.AdditionalData2 = " class=\"Manager\"";
                    else
                        userClub.AdditionalData2 = string.Empty;
                }

                rptClubLeads.DataSource = uc;
                rptClubLeads.DataBind();
            }
        }
    }
}